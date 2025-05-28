namespace GameOfLifeExample
{
    partial class MainForm
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
            buttonExecute = new Button();
            SuspendLayout();
            // 
            // buttonExecute
            // 
            buttonExecute.Location = new Point(12, 12);
            buttonExecute.Name = "buttonExecute";
            buttonExecute.Size = new Size(75, 23);
            buttonExecute.TabIndex = 0;
            buttonExecute.Text = "Execute";
            buttonExecute.UseVisualStyleBackColor = true;
            buttonExecute.Click += buttonExecute_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(272, 48);
            Controls.Add(buttonExecute);
            Margin = new Padding(2);
            Name = "MainForm";
            Text = "Game Of Life Demo";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button buttonExecute;
    }
}
