using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button_LoadTicker_Click(object sender, EventArgs e)
        {
            openFileDialog_LoadTicker.ShowDialog();

        }

        ///function that returns a list of candlesticks from the given filepath
        private List<Candlestick> ReadCandlesticksFromFile(string filePath)
        {
            var candlesticks = new List<Candlestick>();
            using (StreamReader sr = new StreamReader(filePath, Encoding.UTF8))
            {
                //skip the header line
                var header = sr.ReadLine();
                
                //read file until stream reader is null
                while (!sr.EndOfStream)
                {
                    //create a new candlestick for each line of the file
                    string line = sr.ReadLine();
                    line.Trim();
                    candlesticks.Add(new Candlestick(line));
                }
            }
            //return list of candlesticks
            return candlesticks;
        }

        ///function that binds the candlestick list to the datagrid, and the chart
        public void displayStock(List<Candlestick> candlestickList)
        {

            candlestickBindingSource.DataSource = candlestickList;


            //set padding on chart
            NormalizeChart(candlestickList);

            //bind the candlestick list to the chart
            chart1.DataSource = candlestickList;
            chart1.DataBind();
        }
        private void openFileDialog_LoadTicker_FileOk(object sender, CancelEventArgs e)
        {
            Text = openFileDialog_LoadTicker.FileName;
            
            //get whole list of candle sticks and filter it based on the startDate and endDate
            List<Candlestick> candlestickList = ReadCandlesticksFromFile(openFileDialog_LoadTicker.FileName);
            candlestickList = FilterCandlesticksByDate(candlestickList, dateTimePicker_StartDate.Value, dateTimePicker_EndDate.Value);
            displayStock(candlestickList);

        }

        ///function that sorts the candlestick list so that they are within range of startDate and endDate
        public List<Candlestick> FilterCandlesticksByDate(List<Candlestick> candlestickList, DateTime startDate, DateTime endDate)
        { 
            List<Candlestick> updatedList = new List<Candlestick>();
            foreach (Candlestick candlestick in candlestickList)
            {
                //if candlestick is within range, add it to the filtered list
                if(candlestick.Date >= startDate && candlestick.Date <= endDate)
                {
                    updatedList.Add(candlestick);
                }
            }
            return updatedList;
        }

        ///normalizes chart by setting the padding based on the max and min of the candlestick
        public void NormalizeChart(List<Candlestick> candlestickList)
        {
            //find max high, min lowd
            decimal minValue = candlestickList.Min(c => c.Low);
            decimal maxValue = candlestickList.Max(c => c.High);

            //find the padding and subtract from min, and add to max.
            decimal padding = (maxValue - minValue) * 0.05m;
            decimal minY = minValue - padding;
            decimal maxY = maxValue + padding;

            //set the minY and maxY of the chart area
            chart1.ChartAreas["ChartArea_Candlestick"].AxisY.Minimum = (double)minY;
            chart1.ChartAreas["ChartArea_Candlestick"].AxisY.Maximum = (double)maxY;


        }
    }
}
