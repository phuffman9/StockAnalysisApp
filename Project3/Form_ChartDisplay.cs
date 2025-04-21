using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
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
            chart1.ChartAreas[0].AxisY.StripLines.Clear();
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
            if (!peak.peak)
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
                    bool up = true;
                    bool down = true;

                    //if down wave
                    if (start.peak && end.valley)
                    {
                        StartPrice = currentFilter[start.index].High;
                        EndPrice = currentFilter[end.index].Low;
                        up = false;
                        down = true;

                        //check if any candlesticks break the validity
                        for (int k = start.index + 1; k < end.index; k++)
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
                    else if (!up && isValid && down)
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



            var startIndex = wave.startIndex + 1;
            var endIndex = wave.endIndex + 1;



            var width = Math.Abs(startIndex - endIndex);
            var height = Math.Abs((double)wave.endPrice - (double)wave.startPrice);
            double x = Math.Min(startIndex, endIndex);
            double y = Math.Min((double)wave.startPrice, (double)wave.endPrice);



            RectangleAnnotation annotation = new RectangleAnnotation
            {
                AxisX = chart1.ChartAreas[0].AxisX,
                AxisY = chart1.ChartAreas[0].AxisY,
                Width = width,
                Height = height,
                X = x,
                Y = y,
                LineWidth = 2,
                LineColor = Color.Black,
                IsSizeAlwaysRelative = false,
            };

            if (wave.up)
            {
                annotation.BackColor = Color.FromArgb(50, Color.Green);
            }
            else
            {
                annotation.BackColor = Color.FromArgb(50, Color.Red);
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
        public void createLineAnnotation(int startIndex, int endIndex, double startprice, double endprice)
        {
            int width = Math.Abs(startIndex + endIndex);
            double height = Math.Abs(startprice - endprice);
            var area = chart1.ChartAreas["ChartArea_Candlestick"];
            var lineAnnotation = new LineAnnotation()
            {
                AxisX = area.AxisX,
                AxisY = area.AxisY,
                Name = "diag",
                X = startIndex + 1,
                Y = startPrice,
                Width = width,
                Height = -height,
                LineColor = Color.Green,
                LineWidth = 2,
                IsSizeAlwaysRelative = true,
                ClipToChartArea = area.Name,
            };
            chart1.Annotations.Add(lineAnnotation);
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
            addWaveAnnotations(wave);
            startPrice = (double)wave.startPrice;
            currentPrice = stepPrice = (double)wave.endPrice;
            display_FibLevels(startPrice, currentPrice, wave.startIndex, wave.endIndex);
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
            var wave = listBox_DownWaves.SelectedItem as Wave;
            addWaveAnnotations(wave);
            startPrice = (double)wave.startPrice;
            currentPrice = stepPrice = (double)wave.endPrice;
            display_FibLevels(startPrice, currentPrice, wave.startIndex, wave.endIndex);

            chart1.Update();
        }


        bool mouseDown = false;

        RectangleAnnotation drawnAnnotation = null;
        int waveStart = -1;
        int waveEnd = -1;

        Point startingPoint;
        bool isPeak = false;
        bool isValley = false;

        double startPrice = 0.0;
        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            //clear peak valley annotations once user places mouse down
            chart1.Annotations.Clear();

            var chart = (System.Windows.Forms.DataVisualization.Charting.Chart)sender;
            var chartArea = chart.ChartAreas["ChartArea_Candlestick"];

            double xValue = chartArea.AxisX.PixelPositionToValue(e.X);
            waveStart = (int)Math.Round(xValue);

            //get proper index for wave start
            waveStart -= 1;
            startingPoint = e.Location;
            
            //get rid of margin offsets from the chart area to make sure location is accurate
            var ia = chartArea.InnerPlotPosition;
            var ca = chartArea.Position;

            var plotX = e.X - chart1.ClientSize.Width * ca.X / 100 - chart1.ClientSize.Width * ia.X / 100;
            var plotY = e.Y - chart1.ClientSize.Height * ca.Y / 100 - chart1.ClientSize.Height * ia.Y / 100;

            var xv = chartArea.AxisX.PixelPositionToValue(plotX);
            var yv = chartArea.AxisY.PixelPositionToValue(plotY);

            //check if the starting candlestick is a peak or a valley
            foreach (var peak in peakValleyList)
            {
                if (peak.index == waveStart)
                {
                    if (peak.peak)
                    {
                        isPeak = true;

                    }
                    else if (peak.valley)
                    {
                        isValley = true;
                    }
                }
            }
            if (!(isPeak || isValley))
            {
                MessageBox.Show($"Wave must start at peak or valley: Start :{waveStart}", "invalid");
                mouseDown = false;
                return;
            }
            //choose start price as either the high or low depending on peak or valley
            startPrice = (isPeak) ? (double)currentFilter[waveStart].High : (double)currentFilter[waveStart].Low;
            yv = startPrice;
            drawnAnnotation = new RectangleAnnotation
            {
                Name = "drawnRectangle",
                AxisX = chartArea.AxisX,
                AxisY = chartArea.AxisY,
                LineColor = Color.Blue,
                LineWidth = 2,
                BackColor = Color.FromArgb(50, Color.Blue),
                ClipToChartArea = chartArea.Name,
                X = xv,
                Y = startPrice,
                Width = 0,
                Height = 0,
                IsSizeAlwaysRelative = false,
            };

            chart1.Annotations.Add(drawnAnnotation);
        }

        double currentPrice = 0.0;
        /// <summary>
        /// draw rectangle as the user drags the mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            //if mouse is not down, or drawn annotation wasnt initialized, return
            if (!mouseDown || drawnAnnotation ==null)
            {
                return;
            }
            var chart = (System.Windows.Forms.DataVisualization.Charting.Chart)sender;
            var chartArea = chart.ChartAreas["ChartArea_Candlestick"];
            var currentPoint = e.Location;
            double x0 = chartArea.AxisX.PixelPositionToValue(startingPoint.X);
            double y0 = startPrice;

            //double y0 = chartArea.AxisY.PixelPositionToValue(startingPoint.Y);
            double x1 = chartArea.AxisX.PixelPositionToValue(currentPoint.X);
            double y1 = chartArea.AxisY.PixelPositionToValue(currentPoint.Y);



            var dy = y1 - y0;
            if (drawnAnnotation != null)
            {
                if (x1 >= x0)
                {
                    drawnAnnotation.X = x0;
                    drawnAnnotation.Width = x1 - x0;
                }
                else
                {
                    drawnAnnotation.X = x1;
                    drawnAnnotation.Width = x0 - x1;
                }
                if (dy >= 0)
                {
                    drawnAnnotation.Y = y0;
                    drawnAnnotation.Height = dy;

                }
                else
                {
                    drawnAnnotation.Y = y1;
                    drawnAnnotation.Height = -(dy);
                }
            }

            //calculate the current end candlestick index
            waveEnd = (!(drawnAnnotation == null)) ? (int)Math.Round(drawnAnnotation.Width + waveStart) : -1;

            //calculate current price depending on up/down wave
            currentPrice = (dy >= 0) ? startPrice + drawnAnnotation.Height : startPrice - drawnAnnotation.Height;

            //clear chart if current wave is not valid
            chart1.Annotations.Clear();
            chartArea.AxisY.StripLines.Clear();

            //draw wave if it is valid
            if (determineValidWave(waveStart, waveEnd))
            {
                display_FibLevels(startPrice, currentPrice, waveStart, waveEnd);

                chart1.Annotations.Add(drawnAnnotation);
            }
        }
        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            var chart = (System.Windows.Forms.DataVisualization.Charting.Chart)sender;
            var chartArea = chart.ChartAreas["ChartArea_Candlestick"];

            waveEnd = (drawnAnnotation != null) ? (int)Math.Round(drawnAnnotation.Width + waveStart) : 0;
            if (!determineValidWave(waveStart, waveEnd))
            {
                MessageBox.Show("Ended on an invalid wave, a valid wave will appear when you are in the right position", "Invalid Wave");
                chart.Annotations.Clear();
                chartArea.AxisY.StripLines.Clear();

            }
            else
            {
                MessageBox.Show($"Wave selected: Start = {waveStart}, End = {waveEnd}, start price: {startPrice}, end price: {currentPrice}", "Debug Info");

                display_FibLevels(startPrice, (double)currentPrice, waveStart, waveEnd);
                createLineAnnotation(waveStart, waveEnd, startPrice, currentPrice);

                chart1.Annotations.Remove(drawnAnnotation);
                chart1.Annotations.Add(drawnAnnotation);
            }
        }


        List<double> levels = new List<double>();
        /// <summary>
        /// calculate fibonacci levels given the current starting and end price, add line annotations to represent them
        /// </summary>
        /// <param name="startprice"></param>
        /// <param name="endPrice"></param>
        private void display_FibLevels(double startprice, double endPrice, int waveStart, int waveEnd)
        {
            //calculate range and margin
            var range = Math.Abs(endPrice - startprice);
            var margin = range * 0.01;
            var area = chart1.ChartAreas["ChartArea_Candlestick"];
            area.AxisY.StripLines.Clear();

            //clear previous line annotations if there were any
            var lines = chart1.Annotations.OfType<LineAnnotation>().ToList();
            if (lines.Count > 0)
            {
                foreach (var line in lines)
                {
                    chart1.Annotations.Remove(line);
                }
            }

            //delete existing text annotations for fib tags
            var existingFibAnn = chart1.Annotations.OfType<TextAnnotation>().Where(a => a.Tag?.ToString() == "fib").ToList();
            if (existingFibAnn.Count > 0)
            {
                foreach (var ann in existingFibAnn) { chart1.Annotations.Remove(ann); }
            }
            double[] fib_percents = { 0d, 0.236d, 0.382d, 0.5d, 0.618d, 0.764d, 1d };

            levels?.Clear();
            int i = fib_percents.Count() -1;
            double x0 = chart1.Series[0].Points[waveStart].XValue;
            double x1 = chart1.Series[0].Points[waveEnd].XValue;
            double width = x1 - x0;   

            //for each fibonacci level, create a horizontal line annotation at that level
            foreach (var percent in fib_percents)
            {
                double level = 0;
                if (startprice <= endPrice)
                {
                    level = startprice + (range * percent);
                }
                else
                {
                    level = startprice - (range * percent);
                }

                var strip = new StripLine
                {
                    IntervalOffset = level,
                    BorderColor = Color.Purple,
                    BorderWidth = 2,
                    BorderDashStyle = ChartDashStyle.Solid,
                    BackColor = Color.Transparent,
                };
                area.AxisY.StripLines.Add(strip);

                //add labels (text annotation) for each fib level
                var textAnn = new TextAnnotation()
                {
                    Name = $"fib {i}",
                    AxisX = area.AxisX,
                    AxisY = area.AxisY,
                    Tag = "fib",
                    Text = $"{fib_percents[i] * 100}%",
                    AnchorX = waveEnd,
                    AnchorY = level,
                };
                chart1.Annotations.Add(textAnn);
                i--;
            }


            double leftX = waveStart;

            foreach (var percent in fib_percents)
            {
                levels.Add(startprice + (range * percent));
            }
            //update confirmation label
            label_Confirmations.Text = calculateConfirmations(margin, levels, waveStart, waveEnd).ToString();

        }

        List<Tuple<int, double>> confirmations;

        private int calculateConfirmations(double margin, List<double> fib_levels, int waveStart, int waveEnd)
        {
            //if list is null create new list, otherwise clear it
            if (confirmations == null)
            {
                confirmations = new List<Tuple<int, double>>();
            }
            else
            {
                confirmations.Clear();
            }
            //iterate through each candlestick and each fibonacci level to see if any values are within range.
            int count = 0;
            for (int i = waveStart; i <= waveEnd; i++)
            {
                double high = (double)currentFilter[i].High;
                double low = (double)currentFilter[i].Low;
                double open = (double)currentFilter[i].Open;
                double close = (double)currentFilter[i].Close;
                foreach (var level in fib_levels)
                {
                    //check OHLC is within 1% of fib level
                    if (high >= level - margin && high <= level + margin) {
                        count++;
                        var conf = Tuple.Create(i, high);
                        confirmations.Add(conf); }

                    else if (low >= level - margin && low <= level + margin) {
                        count++;
                        var conf = Tuple.Create(i, low);
                        confirmations.Add(conf);
                    }

                    else if (close >= level - margin && close <= level + margin) {
                        count++;
                        var conf = Tuple.Create(i, close);
                        confirmations.Add(conf); ; }

                    else if (open >= level - margin && open <= level + margin) {
                        count++;
                        var conf = Tuple.Create(i, open);
                        confirmations.Add(conf);
                    }

                }

            }
            //if there are confirmations, draw them
            if (confirmations != null)
            {
                confirmationAnnotations(confirmations);
            }
            return count;
        }

        /// <summary>
        /// Draw the confirmation annotations 
        /// </summary>
        /// <param name="confirmations"></param>
        private void confirmationAnnotations(List<Tuple<int, double>> confirmations)
        {
            //remove existing annotations
            var existingConfirmations = chart1.Annotations.OfType<TextAnnotation>().Where(a => a.Tag?.ToString() == "confirmation").ToList();
            if (confirmations.Count > 0)
            {
                foreach (var ann in existingConfirmations)
                {
                    chart1.Annotations.Remove(ann);
                }
            }

            //make a text annotation for each confirmation found
            foreach (var value in confirmations)
            {
                var annotation = new TextAnnotation()
                {
                    Tag = "confirmation",
                    AxisX = chart1.ChartAreas["ChartArea_Candlestick"].AxisX,
                    AxisY = chart1.ChartAreas["ChartArea_Candlestick"].AxisY,

                    AnchorX = value.Item1 + 1,
                    AnchorY = value.Item2,
                    Text = "C",

                };
                chart1.Annotations.Add(annotation);
            }

        }
        double stepPrice = 0.0;
        private void button_Simulate_Click(object sender, EventArgs e)
        {
            //turn on timer control
            stepPrice = currentPrice;
            timer_Simulate.Enabled = !timer_Simulate.Enabled;

            if (timer_Simulate.Enabled)
            {
                button_Simulate.Text = "Stop";
            }
            else
            {
                button_Simulate.Text = "Start";
            }
        }
        double stepSize = 0.20;
        bool isInit= false;
        private void button_Plus_Click(object sender, EventArgs e)
        {
            //call simulate tick function with up being true
            if (!isInit)
            {
                stepPrice = currentPrice;
                isInit = true;
            }
            simulateTick(true, chart1.Annotations.OfType<RectangleAnnotation>().ToList()[0]);
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {

        }

        private void timer_Simulate_Tick(object sender, EventArgs e)
        {
            simulateTick(true, chart1.Annotations.OfType<RectangleAnnotation>().ToList()[0]);
        }

        private void button_Minus_Click(object sender, EventArgs e)
        {
            //call simulate tick function with up being false
            if (!isInit)
            {
                stepPrice = currentPrice;
                isInit = true;
            }
            simulateTick(false, chart1.Annotations.OfType<RectangleAnnotation>().ToList()[0]);
        }

        /// <summary>
        /// function that increments a waves end price in ticks
        /// </summary>
        /// <param name="up"></param>
        /// <param name="wave"></param>
        private void simulateTick(bool up, RectangleAnnotation wave)
        {
            //calculate range and increment price
            double range = Math.Abs(currentPrice - startPrice);
            //increment price depending on if it was an + tick or - tick
            if (up)
            {
                stepPrice += stepSize;
            }
            else
            {
                stepPrice -= stepSize;
            }
            double max = currentPrice + (range * 0.20);
            double min = currentPrice - (range * 0.20);

            //if tick goes over max, reset to min
            if (stepPrice > max)
            {
                stepPrice = min;
            }

            //draw new annotations and new fib levels

            wave.Height = Math.Abs(startPrice - stepPrice);
            display_FibLevels(startPrice, stepPrice, waveStart, waveEnd);
        }

        /// <summary>
        /// determine whether there is a valid wave between the two index given
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        private bool determineValidWave(int startIndex, int endIndex)
        {
            bool isValid = true;

            if (isPeak)
            {
                for (int i = startIndex + 1; i < endIndex; i++)
                {
                    if ((double)currentFilter[i].High > startPrice || (double)currentFilter[i].Low < currentPrice) { isValid = false; break; }
                }
            }
            else
            {
                for (int i = startIndex + 1; i < endIndex; i++)
                {
                    if ((double)currentFilter[i].Low < startPrice || (double)currentFilter[i].High > currentPrice) { isValid = false; break; }
                }
            }

            return isValid;
        }

        private void button_StepSizeChanged_Click(object sender, EventArgs e)
        {
            stepSize = double.Parse(textBox_StepSize.Text);
        }
    }
}
