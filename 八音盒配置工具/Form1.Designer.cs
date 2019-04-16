namespace 八音盒配置工具
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.XlsxFileText = new System.Windows.Forms.TextBox();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.btnGetXlsxFile = new System.Windows.Forms.Button();
            this.btnGetXmlOne = new System.Windows.Forms.Button();
            this.btnGetXmlTwo = new System.Windows.Forms.Button();
            this.XmlOneText = new System.Windows.Forms.TextBox();
            this.XmlTwoText = new System.Windows.Forms.TextBox();
            this.GenerateXmlOne = new System.Windows.Forms.Button();
            this.GenerateXmlTwo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // XlsxFileText
            // 
            this.XlsxFileText.AllowDrop = true;
            this.XlsxFileText.Location = new System.Drawing.Point(285, 75);
            this.XlsxFileText.Name = "XlsxFileText";
            this.XlsxFileText.Size = new System.Drawing.Size(239, 21);
            this.XlsxFileText.TabIndex = 0;
            this.XlsxFileText.DragDrop += new System.Windows.Forms.DragEventHandler(this.XlsxFileTextDragDrop);
            this.XlsxFileText.DragEnter += new System.Windows.Forms.DragEventHandler(this.XlsxFileTextDragEnter);
            // 
            // LogBox
            // 
            this.LogBox.Location = new System.Drawing.Point(256, 291);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(308, 76);
            this.LogBox.TabIndex = 1;
            // 
            // btnGetXlsxFile
            // 
            this.btnGetXlsxFile.Location = new System.Drawing.Point(84, 73);
            this.btnGetXlsxFile.Name = "btnGetXlsxFile";
            this.btnGetXlsxFile.Size = new System.Drawing.Size(145, 23);
            this.btnGetXlsxFile.TabIndex = 2;
            this.btnGetXlsxFile.Text = "选择配置文件";
            this.btnGetXlsxFile.UseVisualStyleBackColor = true;
            this.btnGetXlsxFile.Click += new System.EventHandler(this.btnGetXlsxFile_Click);
            // 
            // btnGetXmlOne
            // 
            this.btnGetXmlOne.Location = new System.Drawing.Point(84, 132);
            this.btnGetXmlOne.Name = "btnGetXmlOne";
            this.btnGetXmlOne.Size = new System.Drawing.Size(145, 23);
            this.btnGetXmlOne.TabIndex = 3;
            this.btnGetXmlOne.Text = "选择八音盒xml文件";
            this.btnGetXmlOne.UseVisualStyleBackColor = true;
            this.btnGetXmlOne.Click += new System.EventHandler(this.btnGetXmlOne_Click);
            // 
            // btnGetXmlTwo
            // 
            this.btnGetXmlTwo.Location = new System.Drawing.Point(84, 192);
            this.btnGetXmlTwo.Name = "btnGetXmlTwo";
            this.btnGetXmlTwo.Size = new System.Drawing.Size(145, 23);
            this.btnGetXmlTwo.TabIndex = 4;
            this.btnGetXmlTwo.Text = "选择刮刮乐xml文件";
            this.btnGetXmlTwo.UseVisualStyleBackColor = true;
            this.btnGetXmlTwo.Click += new System.EventHandler(this.btnGetXmlTwo_Click);
            // 
            // XmlOneText
            // 
            this.XmlOneText.AllowDrop = true;
            this.XmlOneText.Location = new System.Drawing.Point(285, 134);
            this.XmlOneText.Name = "XmlOneText";
            this.XmlOneText.Size = new System.Drawing.Size(239, 21);
            this.XmlOneText.TabIndex = 5;
            this.XmlOneText.DragDrop += new System.Windows.Forms.DragEventHandler(this.XmlOneTextDragDrop);
            this.XmlOneText.DragEnter += new System.Windows.Forms.DragEventHandler(this.XmlOneTextDragEnter);
            // 
            // XmlTwoText
            // 
            this.XmlTwoText.AllowDrop = true;
            this.XmlTwoText.Location = new System.Drawing.Point(285, 194);
            this.XmlTwoText.Name = "XmlTwoText";
            this.XmlTwoText.Size = new System.Drawing.Size(239, 21);
            this.XmlTwoText.TabIndex = 6;
            this.XmlTwoText.DragDrop += new System.Windows.Forms.DragEventHandler(this.XmlTwoTextDragDrop);
            this.XmlTwoText.DragEnter += new System.Windows.Forms.DragEventHandler(this.XmlTwoTextDragEnter);
            // 
            // GenerateXmlOne
            // 
            this.GenerateXmlOne.Location = new System.Drawing.Point(579, 132);
            this.GenerateXmlOne.Name = "GenerateXmlOne";
            this.GenerateXmlOne.Size = new System.Drawing.Size(145, 23);
            this.GenerateXmlOne.TabIndex = 7;
            this.GenerateXmlOne.Text = "导出";
            this.GenerateXmlOne.UseVisualStyleBackColor = true;
            this.GenerateXmlOne.Click += new System.EventHandler(this.btnGenerateXmlOne_Click);
            // 
            // GenerateXmlTwo
            // 
            this.GenerateXmlTwo.Location = new System.Drawing.Point(579, 192);
            this.GenerateXmlTwo.Name = "GenerateXmlTwo";
            this.GenerateXmlTwo.Size = new System.Drawing.Size(145, 23);
            this.GenerateXmlTwo.TabIndex = 8;
            this.GenerateXmlTwo.Text = "导出";
            this.GenerateXmlTwo.UseVisualStyleBackColor = true;
            this.GenerateXmlTwo.Click += new System.EventHandler(this.btnGenerateXmlTwo_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.GenerateXmlTwo);
            this.Controls.Add(this.GenerateXmlOne);
            this.Controls.Add(this.XmlTwoText);
            this.Controls.Add(this.XmlOneText);
            this.Controls.Add(this.btnGetXmlTwo);
            this.Controls.Add(this.btnGetXmlOne);
            this.Controls.Add(this.btnGetXlsxFile);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.XlsxFileText);
            this.Name = "Form1";
            this.Text = "八音盒&刮刮乐配置工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox XlsxFileText;
        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.Button btnGetXlsxFile;
        private System.Windows.Forms.Button btnGetXmlOne;
        private System.Windows.Forms.Button btnGetXmlTwo;
        private System.Windows.Forms.TextBox XmlOneText;
        private System.Windows.Forms.TextBox XmlTwoText;
        private System.Windows.Forms.Button GenerateXmlOne;
        private System.Windows.Forms.Button GenerateXmlTwo;
    }
}

