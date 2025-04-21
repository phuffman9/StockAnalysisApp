using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project2
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
            //var fileList = openFileDialog_LoadTicker.FileNames;

        }

        private void openFileDialog_LoadTicker_FileOk(object sender, CancelEventArgs e)
        {
            var fileNames = new List<string>(openFileDialog_LoadTicker.FileNames);
            foreach (var file in fileNames)
            {
                var display = new Form_ChartDisplay(dateTimePicker_StartDate.Value, dateTimePicker_EndDate.Value, file);
                display.Text = file;
                display.Show();
                
            }
        }
    }
}
//static class - class that doesnt have objects that you instantiate, but  you can invoke methods through that class.

//pass file name to constructor, two datetimes, and a list of candlesticks.
//constructor needs initializecomponent()