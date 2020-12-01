namespace SharePortfolioManager.ChartForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChart));
            this.chartDailyValues = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblNoDataMessage = new System.Windows.Forms.Label();
            this.toolStripChart = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelMessage = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.chartDailyValues)).BeginInit();
            this.toolStripChart.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartDailyValues
            // 
            this.chartDailyValues.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Dash;
            chartArea1.Name = "ChartArea1";
            this.chartDailyValues.ChartAreas.Add(chartArea1);
            this.chartDailyValues.Dock = System.Windows.Forms.DockStyle.Top;
            this.chartDailyValues.Location = new System.Drawing.Point(0, 0);
            this.chartDailyValues.Name = "chartDailyValues";
            this.chartDailyValues.Size = new System.Drawing.Size(700, 325);
            this.chartDailyValues.TabIndex = 1;
            this.chartDailyValues.Text = "Daily values";
            this.chartDailyValues.CustomizeLegend += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CustomizeLegendEventArgs>(this.OnChartDailyValues_CustomizeLegend);
            this.chartDailyValues.MouseLeave += new System.EventHandler(this.OnChartDailyValues_MouseLeave);
            // 
            // lblNoDataMessage
            // 
            this.lblNoDataMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNoDataMessage.BackColor = System.Drawing.Color.White;
            this.lblNoDataMessage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoDataMessage.ForeColor = System.Drawing.Color.Red;
            this.lblNoDataMessage.Location = new System.Drawing.Point(0, 0);
            this.lblNoDataMessage.Name = "lblNoDataMessage";
            this.lblNoDataMessage.Size = new System.Drawing.Size(700, 325);
            this.lblNoDataMessage.TabIndex = 2;
            this.lblNoDataMessage.Text = "_NoDataMessage";
            this.lblNoDataMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblNoDataMessage.Visible = false;
            this.lblNoDataMessage.MouseLeave += new System.EventHandler(this.OnLblNoDataMessage_MouseLeave);
            // 
            // toolStripChart
            // 
            this.toolStripChart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripChart.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelMessage});
            this.toolStripChart.Location = new System.Drawing.Point(0, 325);
            this.toolStripChart.Name = "toolStripChart";
            this.toolStripChart.Size = new System.Drawing.Size(700, 25);
            this.toolStripChart.TabIndex = 3;
            this.toolStripChart.Text = "toolStrip1";
            // 
            // toolStripLabelMessage
            // 
            this.toolStripLabelMessage.Name = "toolStripLabelMessage";
            this.toolStripLabelMessage.Size = new System.Drawing.Size(0, 22);
            // 
            // FrmChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(700, 350);
            this.Controls.Add(this.toolStripChart);
            this.Controls.Add(this.lblNoDataMessage);
            this.Controls.Add(this.chartDailyValues);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 350);
            this.Name = "FrmChart";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Chart";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FrmChart_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.chartDailyValues)).EndInit();
            this.toolStripChart.ResumeLayout(false);
            this.toolStripChart.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartDailyValues;
        private System.Windows.Forms.Label lblNoDataMessage;
        private System.Windows.Forms.ToolStrip toolStripChart;
        private System.Windows.Forms.ToolStripLabel toolStripLabelMessage;
    }
}