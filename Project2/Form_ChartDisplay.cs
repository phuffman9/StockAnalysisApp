using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;      //for annotations


namespace Project2
{
    public partial class Form_ChartDisplay : Form
    {

        public int count = 0;
        List<Wave> waves;
        List<Wave> upwaves;
        List<Wave> downwaves;
        public String filePath;
        public List<Candlestick> candlesticks;
        public List<Candlestick> currentFilter;
        StockReader reader;
        public List<PeakValley> peakValleyList;
        
        public Form_ChartDisplay()
        {
            InitializeComponent();
        }
        public Form_ChartDisplay(DateTime startDate, DateTime endDate, string fileName)
        {

            //create stock reader class to do the file reading
            reader = new StockReader();

            InitializeComponent();
            dateTimePicker_StartDate.Value = startDate;
            dateTimePicker_EndDate.Value = endDate;
            filePath = fileName;


            loadAndDisplay();
            //peakValleyList = findPeakValley(FilterCandlesticksByDate(candlesticks, startDate, endDate));
            peakValleyList = findPeakValley(currentFilter, 1);


            Show();
            showAnnotations();
            updateWaves();
        }


        /// <summary>
        /// read candlestick from the file and return the list
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private List<Candlestick> LoadTicker(string fileName)
        {
            List<Candlestick> listofCandlesticks = reader.ReadCandlesticksFromFile(fileName);
            //get first and second
            var first = listofCandlesticks[0].Date;
            var second = listofCandlesticks[1].Date;
            //if first > second, reverse list, return reversed list
            if (first > second)
            {
                listofCandlesticks.Reverse();
            }
            return listofCandlesticks;

        }

        //wrapper function
        public void loadAndDisplay()
        {
            //call loadTIcker
            candlesticks = LoadTicker(filePath);
            // filter candlesticks

            //get the chart ready to display
            displayStock();
        }

        /// <summary>
        /// filters candlestick by the current datetime picker values and binds the chart to the filtered list of candlesticks
        /// </summary>
        public void displayStock()
        {
            //filter candlesticks
            var filteredCandlesticks = FilterCandlesticksByDate(candlesticks, dateTimePicker_StartDate.Value, dateTimePicker_EndDate.Value);
            currentFilter = filteredCandlesticks;
            //normalize chart
            NormalizeChart(filteredCandlesticks);
            //bind chart to the list of filtered candlesticks
            chart1.DataSource = filteredCandlesticks;
            chart1.DataBind();
        }
         
        ///filters the candlesticks by the start date and end date
        public List<Candlestick> FilterCandlesticksByDate(List<Candlestick> candlestickList, DateTime startDate, DateTime endDate)
        {
            int innerCount = 0;
            List<Candlestick> updatedList = new List<Candlestick>();
            foreach (Candlestick candlestick in candlestickList)
            {
                count++;
                //if candlestick is within range, add it to the filtered list
                if (candlestick.Date >= startDate && candlestick.Date <= endDate)
                {
                    innerCount++;
                    updatedList.Add(candlestick);
                }
            }
            count = count - innerCount;
            return updatedList;
        }

        /// <summary>
        /// finds the min and max value of the chart to be able to normalize chart
        /// </summary>
        /// <param name="candlestickList"></param>
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

        /// <summary>
        /// refresh the candlesticks, peaks, valleys and waves once the refresh button has been clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Refresh_Click(object sender, EventArgs e)
        {
            //update chart
            displayStock();

            //update peaks and valleys
            currentFilter = FilterCandlesticksByDate(candlesticks, dateTimePicker_StartDate.Value, dateTimePicker_EndDate.Value);
            peakValleyList = findPeakValley(currentFilter, hScrollBar_Margin.Value);

            //clear and update annotations
            chart1.Annotations.Clear();
            showAnnotations();
            isInit1 = true;
            isInit2 = true;
            //update waves
            updateWaves();
            
        }

        /// <summary>
        /// Function that updates the waves list and updates the list boxes showing the waves
        /// </summary>
        public void updateWaves()
        {
            waves = FindValidWaves(peakValleyList);
            upwaves = getUpWaves(waves);
            downwaves = getDownWaves(waves);
            listBox_UpWaves.DataSource = upwaves;
            listBox_DownWaves.DataSource = downwaves;
        }


        /// <summary>
        /// update peakvalley list when margin has been changed, also update the waves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            //update peakvalley list and wave list
            label_Margin.Text = hScrollBar_Margin.Value.ToString();
            peakValleyList = findPeakValley(currentFilter, hScrollBar_Margin.Value);
            chart1.Annotations.Clear();
            showAnnotations();


            updateWaves();
            isInit1 = true;
            isInit2 = true;
        }

        
        ///Find all peaks and valleys using the given margin
        public List<PeakValley> findPeakValley(List<Candlestick> candlesticks, int margin)
        {
            
            //initialize a peak for the first and last candlesticks
            var peakList = new List<PeakValley>();
            var firstPeak = new PeakValley();
            var lastPeak = new PeakValley();
            firstPeak.index = 0;
            lastPeak.index = candlesticks.Count - 1;
            int count = candlesticks.Count - 1;
            firstPeak.peak = true;
            firstPeak.valley = true;

            //check first candlestick
            for (int i = 1; i <= margin; i++)
            {
                if (candlesticks[0].High < candlesticks[i].High)
                {
                    firstPeak.peak = false;
                }
                if (candlesticks[0].Low > candlesticks[i].Low)
                {
                    firstPeak.valley = false;
                }
            }
            if (firstPeak.peak || firstPeak.valley)
            {
                firstPeak.date = candlesticks[0].Date;
                peakList.Add(firstPeak);
            }

            //check regular case
            for (int i = margin; i < candlesticks.Count - margin; i++)
            {
                var newPeak = new PeakValley();
                decimal high = candlesticks[i].High;
                decimal low = candlesticks[i].Low;

                newPeak.valley = true;
                newPeak.peak = true;
                for (int j = i - margin; j <= i + margin; j++)
                {
                    //dont compare the same candlestick
                    if (j == i) { continue; }
                    //if any candlesticks in the margin are higher than it isnt a valley
                    if (candlesticks[j].High >= high) { newPeak.peak = false; }
                    if (candlesticks[j].Low <= low) { newPeak.valley = false; }
                }
                //if candlestick is a peak or valley, add it to the peakList
                if (newPeak.peak || newPeak.valley)
                {
                    newPeak.index = i;
                    newPeak.date = candlesticks[i].Date;
                    peakList.Add(newPeak);
                }
            }
        
            //check last candlestick
            lastPeak.peak = true;
            lastPeak.valley = true;
            for (int i = 1; i <= margin; i++)
            {
                if (candlesticks[count].High < candlesticks[count - i].High)
                {
                    lastPeak.peak = false;
                }
                if (candlesticks[count].Low > candlesticks[count - i].Low)
                {
                    lastPeak.valley = false;
                }
            }
            
            if (lastPeak.peak || lastPeak.valley)
            {
                lastPeak.date = candlesticks[count].Date;
                peakList.Add(lastPeak);
            }
            return peakList;
        }

        /// <summary>
        /// wrapper function to add all peaks and valley annotations to the chart
        /// </summary>
        public void showAnnotations()
        {
            foreach (PeakValley peak in peakValleyList)
            { 
                textAnnotation(peak);
            }
            
        }

        /// <summary>
        /// create text annotations for peaks and valleys
        /// </summary>
        /// <param name="peak"></param>
        private void textAnnotation(PeakValley peak)
        {
            string text;
            if (peak.peak)
            {
                text = "P";
            }
            else
            {
                text = "V";
            }
            TextAnnotation annotation = new TextAnnotation()
            {
                Text = text,
                AnchorDataPoint = chart1.Series[0].Points[peak.index]
            };
            if(!peak.peak)
            {
                annotation.ForeColor = Color.Green;
            }
            else
            {
                annotation.ForeColor = Color.Red;
            }

            chart1.Annotations.Add(annotation);

        }

        /// <summary>
        /// find all valid waves out of the peakvalley list, indicate if they are up or down
        /// </summary>
        /// <param name="peakList"></param>
        /// <returns></returns>
        public List<Wave> FindValidWaves(List<PeakValley> peakList)
        {
            var waves = new List<Wave>();
            for (int i = 0; i < peakList.Count; i++)
            {
                var start = peakList[i];

                //test every other peak and valley to see if its possible valid wave
                for (int j = i + 1; j < peakList.Count; j++)
                {
                    bool isValid = true;
                    decimal StartPrice = 0;
                    decimal EndPrice = 0;
                    var end = peakList[j];
                    bool up =true;
                    bool down = true;

                    //if down wave
                    if (start.peak && end.valley)
                    {
                        StartPrice = currentFilter[start.index].High;
                        EndPrice = currentFilter[end.index].Low;
                        up = false;
                        down = true;

                        //check if any candlesticks break the validity
                        for(int k = start.index+1; k < end.index; k++)
                        {
                            if (currentFilter[k].High > StartPrice || currentFilter[k].Low < EndPrice)
                            {
                                isValid = false;
                            }
                        }
                    }
                    
                    //if upwave
                    else if (start.valley && end.peak)
                    {
                        StartPrice = currentFilter[start.index].Low;
                        EndPrice = currentFilter[end.index].High;
                        up = true;
                        down = false;
                        
                        //check validity
                        for (int k = start.index + 1; k < end.index; k++)
                        {
                            if (currentFilter[k].High > EndPrice || currentFilter[k].Low < StartPrice)
                            {
                                isValid = false;
                            }
                        }
                    }

                    //if there is a valid up wave add it to the list
                    if (up && isValid && !down)
                    {
                        waves.Add(new Wave
                        {
                            startPrice = StartPrice,
                            endPrice = EndPrice,
                            startIndex = start.index,
                            endIndex = end.index,
                            startDate = start.date,
                            endDate = end.date,
                            up = true,
                            down = false,
                            displayDate = start.date.ToShortDateString() + "-" + end.date.ToShortDateString()

                        });
                    }
                    //if there is a valid down wave add it to the list
                    else if(!up && isValid && down)
                    {
                        waves.Add(new Wave
                        {
                            startPrice = StartPrice,
                            endPrice = EndPrice,
                            startIndex = start.index,
                            endIndex = end.index,
                            startDate = start.date,
                            endDate = end.date,
                            up = false,
                            down = true,
                            displayDate = start.date.ToShortDateString() + "-" + end.date.ToShortDateString()
                        });
                    }
                }
            }
            return waves;
        }

      


        /// <summary>
        /// calculate area for rectangle annotation and add it to the chart
        /// </summary>
        /// <param name="wave"></param>
        public void addWaveAnnotations(Wave wave)
        {
            chart1.Annotations.Clear();
            //RectangleAnnotation annotation = new RectangleAnnotation();
            //annotation.AxisX = chart1.ChartAreas[0].AxisX;
            //annotation.AxisY = chart1.ChartAreas[0].AxisY;
            var startXvalue = chart1.Series[0].Points[wave.startIndex].XValue;
            var endXvalue = chart1.Series[0].Points[wave.endIndex].XValue;

            var startIndex = chart1.Series[0].Points[wave.startIndex];
            var endIndex = chart1.Series[0].Points[wave.endIndex];

            //annotation.X = chart1.Series[0].Points[wave.startIndex].XValue;
            //annotation.Y = Math.Max((double)wave.startPrice, (double)wave.endPrice);

            var width = Math.Abs(endXvalue - startXvalue);
            var height = Math.Abs((double)wave.endPrice - (double)wave.startPrice);
            double x = Math.Min(startXvalue, endXvalue);
            double y = Math.Max((double)wave.startPrice, (double)wave.endPrice);

            //var size = Pixels2Percent(chart1.ChartAreas[0], width, height);

            RectangleAnnotation annotation = new RectangleAnnotation
            {
                AxisX = chart1.ChartAreas[0].AxisX,
                AxisY = chart1.ChartAreas[0].AxisY,
                Width=width,
                Height=height,
                X = x - count,
                Y = y,
                LineWidth = 2,
                LineColor = Color.Black,
                IsSizeAlwaysRelative = false,
            };

            if (wave.up)
            {
                annotation.BackColor = Color.Green;
            }
            else
            {
                annotation.BackColor = Color.Red;
            }
            annotation.ClipToChartArea = chart1.ChartAreas[0].Name;
            annotation.Visible = true;
            chart1.Annotations.Add(annotation);
        }
       

        //get all upwaves from the list of all waves
        public List<Wave> getUpWaves(List<Wave> waves)
        {
            List<Wave> upWaves = new List<Wave>();
            foreach (Wave wave in waves)
            {
                if (wave.up)
                {
                    upWaves.Add(wave);
                }
            }
            return upWaves;
        }

        //get all downwaves from list of all waves
        public List<Wave> getDownWaves(List<Wave> waves)
        {
            List<Wave> downWaves = new List<Wave>();
            foreach (Wave wave in waves)
            {
                if (wave.down)
                {
                    downWaves.Add(wave);
                }
            }
            return downWaves;
        }


        /// <summary>
        /// create a line annotation starting at the start of the wave to the end of the wave
        /// </summary>
        /// <param name="wave"></param>
        public void createLineAnnotation(Wave wave)
        {
            chart1.Annotations.Clear();
            var lineAnnotation = new LineAnnotation();

            //set x and y axis
            lineAnnotation.AxisX = chart1.ChartAreas[0].AxisX;
            lineAnnotation.AxisY = chart1.ChartAreas[0].AxisY;
            var startX = chart1.Series[0].Points[wave.startIndex].XValue;
            var endX = chart1.Series[0].Points[wave.endIndex].XValue;

            //set x, y, width, height
            lineAnnotation.X = startX;
            lineAnnotation.Y =(double) wave.startPrice;
            lineAnnotation.Width = Math.Abs(endX - startX);
            lineAnnotation.Height = (double)Math.Abs(wave.endPrice - wave.startPrice);
            lineAnnotation.LineColor = Color.Black;
            lineAnnotation.LineWidth = 2;

            chart1.Annotations.Add(lineAnnotation);
            chart1.Invalidate();
        }

        //flag for initial load of listbox
        private bool isInit1 = true;
        private bool isInit2 = true;


        private void listBox_UpWaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit1)
            {
                isInit1 = false;
                return;
            }
            chart1.Invalidate();
            var wave = listBox_UpWaves.SelectedItem as Wave;
            //addWaveAnnotations(upwaves[0]);
            createLineAnnotation(upwaves[0]);
            chart1.Update();

        }

        private void listBox_DownWaves_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInit2)
            {
                isInit2 = false;
                return;
            }
            chart1.Invalidate();
            var wave = listBox_UpWaves.SelectedItem as Wave;
            //addWaveAnnotations(upwaves[0]);
            createLineAnnotation(upwaves[0]);
            chart1.Update();
        }
    }
}
