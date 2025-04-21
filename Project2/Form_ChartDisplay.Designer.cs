namespace Project2
{
    partial class Form_ChartDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_Refresh = new System.Windows.Forms.Button();
            this.dateTimePicker_StartDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_EndDate = new System.Windows.Forms.DateTimePicker();
            this.hScrollBar_Margin = new System.Windows.Forms.HScrollBar();
            this.label_Margin = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox_UpWaves = new System.Windows.Forms.ListBox();
            this.waveBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.listBox_DownWaves = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.waveBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea_Candlestick";
            chartArea2.Name = "ChartArea_Volume";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ChartAreas.Add(chartArea2);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Top;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea_Candlestick";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.CustomProperties = "PriceDownColor=Red, PriceUpColor=Green";
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.Name = "Candlesticks";
            series1.XValueMember = "Date";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series1.YValueMembers = "High, Low, Open, Close";
            series1.YValuesPerPoint = 4;
            series2.ChartArea = "ChartArea_Volume";
            series2.Legend = "Legend1";
            series2.Name = "Series_Volume";
            series2.XValueMember = "Date";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series2.YValueMembers = "Volume";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(877, 305);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // button_Refresh
            // 
            this.button_Refresh.Location = new System.Drawing.Point(254, 390);
            this.button_Refresh.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_Refresh.Name = "button_Refresh";
            this.button_Refresh.Size = new System.Drawing.Size(56, 19);
            this.button_Refresh.TabIndex = 1;
            this.button_Refresh.Text = "Refresh";
            this.button_Refresh.UseVisualStyleBackColor = true;
            this.button_Refresh.Click += new System.EventHandler(this.button_Refresh_Click);
            // 
            // dateTimePicker_StartDate
            // 
            this.dateTimePicker_StartDate.Location = new System.Drawing.Point(53, 390);
            this.dateTimePicker_StartDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dateTimePicker_StartDate.Name = "dateTimePicker_StartDate";
            this.dateTimePicker_StartDate.Size = new System.Drawing.Size(151, 20);
            this.dateTimePicker_StartDate.TabIndex = 2;
            // 
            // dateTimePicker_EndDate
            // 
            this.dateTimePicker_EndDate.Location = new System.Drawing.Point(380, 390);
            this.dateTimePicker_EndDate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dateTimePicker_EndDate.Name = "dateTimePicker_EndDate";
            this.dateTimePicker_EndDate.Size = new System.Drawing.Size(151, 20);
            this.dateTimePicker_EndDate.TabIndex = 3;
            // 
            // hScrollBar_Margin
            // 
            this.hScrollBar_Margin.LargeChange = 1;
            this.hScrollBar_Margin.Location = new System.Drawing.Point(45, 335);
            this.hScrollBar_Margin.Maximum = 4;
            this.hScrollBar_Margin.Minimum = 1;
            this.hScrollBar_Margin.Name = "hScrollBar_Margin";
            this.hScrollBar_Margin.Size = new System.Drawing.Size(512, 21);
            this.hScrollBar_Margin.TabIndex = 4;
            this.hScrollBar_Margin.Value = 1;
            this.hScrollBar_Margin.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // label_Margin
            // 
            this.label_Margin.AutoSize = true;
            this.label_Margin.Location = new System.Drawing.Point(214, 322);
            this.label_Margin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Margin.Name = "label_Margin";
            this.label_Margin.Size = new System.Drawing.Size(13, 13);
            this.label_Margin.TabIndex = 5;
            this.label_Margin.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(587, 322);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Up Waves";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(770, 322);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Down Waves";
            // 
            // listBox_UpWaves
            // 
            this.listBox_UpWaves.DataSource = this.waveBindingSource;
            this.listBox_UpWaves.DisplayMember = "displayDate";
            this.listBox_UpWaves.FormattingEnabled = true;
            this.listBox_UpWaves.Location = new System.Drawing.Point(559, 337);
            this.listBox_UpWaves.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBox_UpWaves.Name = "listBox_UpWaves";
            this.listBox_UpWaves.Size = new System.Drawing.Size(122, 69);
            this.listBox_UpWaves.TabIndex = 10;
            this.listBox_UpWaves.SelectedIndexChanged += new System.EventHandler(this.listBox_UpWaves_SelectedIndexChanged);
            // 
            // waveBindingSource
            // 
            this.waveBindingSource.DataSource = typeof(Project2.Wave);
            // 
            // listBox_DownWaves
            // 
            this.listBox_DownWaves.DataSource = this.waveBindingSource;
            this.listBox_DownWaves.DisplayMember = "displayDate";
            this.listBox_DownWaves.FormattingEnabled = true;
            this.listBox_DownWaves.Location = new System.Drawing.Point(736, 338);
            this.listBox_DownWaves.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.listBox_DownWaves.Name = "listBox_DownWaves";
            this.listBox_DownWaves.Size = new System.Drawing.Size(126, 69);
            this.listBox_DownWaves.TabIndex = 11;
            this.listBox_DownWaves.SelectedIndexChanged += new System.EventHandler(this.listBox_DownWaves_SelectedIndexChanged);
            // 
            // Form_ChartDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(877, 422);
            this.Controls.Add(this.listBox_DownWaves);
            this.Controls.Add(this.listBox_UpWaves);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_Margin);
            this.Controls.Add(this.hScrollBar_Margin);
            this.Controls.Add(this.dateTimePicker_EndDate);
            this.Controls.Add(this.dateTimePicker_StartDate);
            this.Controls.Add(this.button_Refresh);
            this.Controls.Add(this.chart1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form_ChartDisplay";
            this.Text = "Form_ChartDisplay";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.waveBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button button_Refresh;
        private System.Windows.Forms.DateTimePicker dateTimePicker_StartDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_EndDate;
        private System.Windows.Forms.HScrollBar hScrollBar_Margin;
        private System.Windows.Forms.Label label_Margin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox_DownWaves;
        private System.Windows.Forms.BindingSource waveBindingSource;
        private System.Windows.Forms.ListBox listBox_UpWaves;
    }
}