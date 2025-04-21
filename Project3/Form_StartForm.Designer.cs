namespace Project2
{
    partial class Form1
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
            this.dateTimePicker_StartDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_EndDate = new System.Windows.Forms.DateTimePicker();
            this.button_LoadTicker = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog_LoadTicker = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // dateTimePicker_StartDate
            // 
            this.dateTimePicker_StartDate.Location = new System.Drawing.Point(66, 55);
            this.dateTimePicker_StartDate.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dateTimePicker_StartDate.Name = "dateTimePicker_StartDate";
            this.dateTimePicker_StartDate.Size = new System.Drawing.Size(151, 20);
            this.dateTimePicker_StartDate.TabIndex = 0;
            this.dateTimePicker_StartDate.Value = new System.DateTime(2024, 2, 1, 0, 0, 0, 0);
            // 
            // dateTimePicker_EndDate
            // 
            this.dateTimePicker_EndDate.Location = new System.Drawing.Point(404, 54);
            this.dateTimePicker_EndDate.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dateTimePicker_EndDate.Name = "dateTimePicker_EndDate";
            this.dateTimePicker_EndDate.Size = new System.Drawing.Size(151, 20);
            this.dateTimePicker_EndDate.TabIndex = 1;
            this.dateTimePicker_EndDate.Value = new System.DateTime(2024, 2, 29, 0, 0, 0, 0);
            // 
            // button_LoadTicker
            // 
            this.button_LoadTicker.Location = new System.Drawing.Point(261, 53);
            this.button_LoadTicker.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.button_LoadTicker.Name = "button_LoadTicker";
            this.button_LoadTicker.Size = new System.Drawing.Size(56, 19);
            this.button_LoadTicker.TabIndex = 0;
            this.button_LoadTicker.Text = "Load";
            this.button_LoadTicker.UseVisualStyleBackColor = true;
            this.button_LoadTicker.Click += new System.EventHandler(this.button_LoadTicker_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 56);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start Date:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(350, 56);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "End Date:";
            // 
            // openFileDialog_LoadTicker
            // 
            this.openFileDialog_LoadTicker.DefaultExt = "CSV";
            this.openFileDialog_LoadTicker.FileName = "ABBV-Day";
            this.openFileDialog_LoadTicker.Filter = "All|*.csv|Month|*-Month.csv|Week|*-Week.csv|Day|*-Day.csv";
            this.openFileDialog_LoadTicker.Multiselect = true;
            this.openFileDialog_LoadTicker.ReadOnlyChecked = true;
            this.openFileDialog_LoadTicker.ShowReadOnly = true;
            this.openFileDialog_LoadTicker.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_LoadTicker_FileOk);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 197);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_LoadTicker);
            this.Controls.Add(this.dateTimePicker_EndDate);
            this.Controls.Add(this.dateTimePicker_StartDate);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Form1";
            this.Text = "Start_Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker_StartDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_EndDate;
        private System.Windows.Forms.Button button_LoadTicker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog_LoadTicker;
    }
}

