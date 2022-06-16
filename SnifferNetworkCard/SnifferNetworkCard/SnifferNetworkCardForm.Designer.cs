namespace SnifferNetworkCard
{
    partial class SnifferNetworkCardForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NetworkCardComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.InformationListView = new System.Windows.Forms.ListView();
            this.ConnectsListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // NetworkCardComboBox
            // 
            this.NetworkCardComboBox.FormattingEnabled = true;
            this.NetworkCardComboBox.ItemHeight = 23;
            this.NetworkCardComboBox.Location = new System.Drawing.Point(511, 17);
            this.NetworkCardComboBox.Name = "NetworkCardComboBox";
            this.NetworkCardComboBox.Size = new System.Drawing.Size(860, 31);
            this.NetworkCardComboBox.TabIndex = 0;
            this.NetworkCardComboBox.Text = "----請選擇偵測的網路卡----";
            this.NetworkCardComboBox.SelectedIndexChanged += new System.EventHandler(this.NetworkCardComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(407, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "流量歷史表";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1366, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "連線IP表";
            // 
            // InformationListView
            // 
            this.InformationListView.Location = new System.Drawing.Point(17, 106);
            this.InformationListView.Name = "InformationListView";
            this.InformationListView.Size = new System.Drawing.Size(893, 1042);
            this.InformationListView.TabIndex = 3;
            this.InformationListView.UseCompatibleStateImageBehavior = false;
            this.InformationListView.View = System.Windows.Forms.View.Details;
            // 
            // ConnectsListView
            // 
            this.ConnectsListView.Location = new System.Drawing.Point(948, 106);
            this.ConnectsListView.Name = "ConnectsListView";
            this.ConnectsListView.Size = new System.Drawing.Size(893, 1042);
            this.ConnectsListView.TabIndex = 4;
            this.ConnectsListView.UseCompatibleStateImageBehavior = false;
            this.ConnectsListView.View = System.Windows.Forms.View.Details;
            // 
            // SnifferNetworkCardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1861, 1167);
            this.Controls.Add(this.ConnectsListView);
            this.Controls.Add(this.InformationListView);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NetworkCardComboBox);
            this.Name = "SnifferNetworkCardForm";
            this.Text = "網卡流量與封包偵測工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComboBox NetworkCardComboBox;
        private Label label1;
        private Label label2;
        private ListView InformationListView;
        private ListView ConnectsListView;
    }
}