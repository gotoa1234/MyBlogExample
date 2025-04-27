namespace NethereumUtilLibraryIntroduce_4_2_9_Example
{
    partial class Form1
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
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            textBoxInput = new TextBox();
            textBoxOutput = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(20, 35);
            button1.Margin = new Padding(5, 5, 5, 5);
            button1.Name = "button1";
            button1.Size = new Size(473, 35);
            button1.TabIndex = 0;
            button1.Text = "Keccak 256 雜湊產生";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 89);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(96, 23);
            label1.TabIndex = 1;
            label1.Text = "輸入 Input";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(25, 259);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(113, 23);
            label2.TabIndex = 2;
            label2.Text = "輸出 Output";
            // 
            // textBoxInput
            // 
            textBoxInput.Location = new Point(22, 115);
            textBoxInput.Margin = new Padding(5, 5, 5, 5);
            textBoxInput.Multiline = true;
            textBoxInput.Name = "textBoxInput";
            textBoxInput.Size = new Size(469, 113);
            textBoxInput.TabIndex = 3;
            // 
            // textBoxOutput
            // 
            textBoxOutput.Location = new Point(22, 287);
            textBoxOutput.Margin = new Padding(5, 5, 5, 5);
            textBoxOutput.Multiline = true;
            textBoxOutput.Name = "textBoxOutput";
            textBoxOutput.Size = new Size(469, 113);
            textBoxOutput.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(528, 435);
            Controls.Add(textBoxOutput);
            Controls.Add(textBoxInput);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button1);
            Margin = new Padding(5, 5, 5, 5);
            Name = "Form1";
            Text = "Keccak256 介紹 Nethereum.Util";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Label label1;
        private Label label2;
        private TextBox textBoxInput;
        private TextBox textBoxOutput;
    }
}
