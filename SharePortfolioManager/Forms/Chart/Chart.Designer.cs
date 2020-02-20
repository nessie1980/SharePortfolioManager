namespace SharePortfolioManager.Chart
{
    partial class FrmChart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.chartDailyValues = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblNoDataMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartDailyValues)).BeginInit();
            this.SuspendLayout();
            // 
            // chartDailyValues
            // 
            this.chartDailyValues.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.Name = "ChartArea1";
            this.chartDailyValues.ChartAreas.Add(chartArea1);
            this.chartDailyValues.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartDailyValues.Location = new System.Drawing.Point(0, 0);
            this.chartDailyValues.Name = "chartDailyValues";
            this.chartDailyValues.Size = new System.Drawing.Size(684, 334);
            this.chartDailyValues.TabIndex = 1;
            this.chartDailyValues.Text = "Daily values";
            this.chartDailyValues.CustomizeLegend += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CustomizeLegendEventArgs>(this.OnChartDailyValues_CustomizeLegend);
            this.chartDailyValues.Click += new System.EventHandler(this.OnChartDailyValues_Click);
            // 
            // lblNoDataMessage
            // 
            this.lblNoDataMessage.BackColor = System.Drawing.Color.White;
            this.lblNoDataMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNoDataMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoDataMessage.ForeColor = System.Drawing.Color.Red;
            this.lblNoDataMessage.Location = new System.Drawing.Point(0, 0);
            this.lblNoDataMessage.Name = "lblNoDataMessage";
            this.lblNoDataMessage.Size = new System.Drawing.Size(684, 334);
            this.lblNoDataMessage.TabIndex = 2;
            this.lblNoDataMessage.Text = "_NoDataMessage";
            this.lblNoDataMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNoDataMessage.Visible = false;
            this.lblNoDataMessage.Click += new System.EventHandler(this.OnLblNoDataMessage_Click);
            // 
            // FrmChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(684, 334);
            this.Controls.Add(this.lblNoDataMessage);
            this.Controls.Add(this.chartDailyValues);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 350);
            this.Name = "FrmChart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Chart";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.chartDailyValues)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartDailyValues;
        private System.Windows.Forms.Label lblNoDataMessage;
    }
}