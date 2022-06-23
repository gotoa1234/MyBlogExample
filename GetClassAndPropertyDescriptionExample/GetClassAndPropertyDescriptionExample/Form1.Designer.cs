namespace GetClassAndPropertyDescriptionExample
{
    partial class GetClassAndPropertyDescriptionExampleForm
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
            this.SearchButton = new System.Windows.Forms.Button();
            this.InputCllassName_textBox = new System.Windows.Forms.TextBox();
            this.ClassNamelabel = new System.Windows.Forms.Label();
            this.ConverttextBoxMessage = new System.Windows.Forms.TextBox();
            this.ConvertClass_button = new System.Windows.Forms.Button();
            this.StronglyTypeClassConvertbutton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(164, 45);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(139, 23);
            this.SearchButton.TabIndex = 0;
            this.SearchButton.Text = "1.查詢";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // InputCllassName_textBox
            // 
            this.InputCllassName_textBox.Location = new System.Drawing.Point(164, 12);
            this.InputCllassName_textBox.Name = "InputCllassName_textBox";
            this.InputCllassName_textBox.Size = new System.Drawing.Size(458, 23);
            this.InputCllassName_textBox.TabIndex = 1;
            // 
            // ClassNamelabel
            // 
            this.ClassNamelabel.AutoSize = true;
            this.ClassNamelabel.Location = new System.Drawing.Point(40, 15);
            this.ClassNamelabel.Name = "ClassNamelabel";
            this.ClassNamelabel.Size = new System.Drawing.Size(118, 15);
            this.ClassNamelabel.TabIndex = 2;
            this.ClassNamelabel.Text = "查詢的ClassName：";
            // 
            // ConverttextBoxMessage
            // 
            this.ConverttextBoxMessage.Location = new System.Drawing.Point(28, 74);
            this.ConverttextBoxMessage.Multiline = true;
            this.ConverttextBoxMessage.Name = "ConverttextBoxMessage";
            this.ConverttextBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ConverttextBoxMessage.Size = new System.Drawing.Size(739, 320);
            this.ConverttextBoxMessage.TabIndex = 3;
            // 
            // ConvertClass_button
            // 
            this.ConvertClass_button.Location = new System.Drawing.Point(323, 45);
            this.ConvertClass_button.Name = "ConvertClass_button";
            this.ConvertClass_button.Size = new System.Drawing.Size(139, 23);
            this.ConvertClass_button.TabIndex = 4;
            this.ConvertClass_button.Text = "2.轉換對應物件";
            this.ConvertClass_button.UseVisualStyleBackColor = true;
            this.ConvertClass_button.Click += new System.EventHandler(this.ConvertClass_button_Click);
            // 
            // StronglyTypeClassConvertbutton
            // 
            this.StronglyTypeClassConvertbutton.Location = new System.Drawing.Point(483, 45);
            this.StronglyTypeClassConvertbutton.Name = "StronglyTypeClassConvertbutton";
            this.StronglyTypeClassConvertbutton.Size = new System.Drawing.Size(139, 23);
            this.StronglyTypeClassConvertbutton.TabIndex = 5;
            this.StronglyTypeClassConvertbutton.Text = "3. 轉換已存在物件";
            this.StronglyTypeClassConvertbutton.UseVisualStyleBackColor = true;
            this.StronglyTypeClassConvertbutton.Click += new System.EventHandler(this.StronglyTypeClassConvertbutton_Click);
            // 
            // GetClassAndPropertyDescriptionExampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 414);
            this.Controls.Add(this.StronglyTypeClassConvertbutton);
            this.Controls.Add(this.ConvertClass_button);
            this.Controls.Add(this.ConverttextBoxMessage);
            this.Controls.Add(this.ClassNamelabel);
            this.Controls.Add(this.InputCllassName_textBox);
            this.Controls.Add(this.SearchButton);
            this.Name = "GetClassAndPropertyDescriptionExampleForm";
            this.Text = "取得Class物件的Description範例";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button SearchButton;
        private TextBox InputCllassName_textBox;
        private Label ClassNamelabel;
        private TextBox ConverttextBoxMessage;
        private Button ConvertClass_button;
        private Button StronglyTypeClassConvertbutton;
    }
}