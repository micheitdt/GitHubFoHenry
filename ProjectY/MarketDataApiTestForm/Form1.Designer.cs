namespace MarketDataApiTest
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.IPtxt = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.AdapterCodetxt = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSymbol = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tbMessage = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(22, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Address";
            // 
            // IPtxt
            // 
            this.IPtxt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.IPtxt.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.IPtxt.Location = new System.Drawing.Point(111, 16);
            this.IPtxt.Name = "IPtxt";
            this.IPtxt.Size = new System.Drawing.Size(127, 29);
            this.IPtxt.TabIndex = 1;
            this.IPtxt.Text = "10.214.217.45";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(244, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(22, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Market";
            // 
            // AdapterCodetxt
            // 
            this.AdapterCodetxt.AutoCompleteCustomSource.AddRange(new string[] {
            "0-上市",
            "1-上櫃",
            "2-期貨AM盤",
            "3-期貨PM盤",
            "4-選擇權AM盤",
            "5-選擇權PM盤"});
            this.AdapterCodetxt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AdapterCodetxt.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.AdapterCodetxt.FormattingEnabled = true;
            this.AdapterCodetxt.Items.AddRange(new object[] {
            "0-上市",
            "1-上櫃",
            "2-期貨AM盤",
            "3-選擇權AM盤",
            "4-期貨PM盤",
            "5-選擇權PM盤"});
            this.AdapterCodetxt.Location = new System.Drawing.Point(91, 65);
            this.AdapterCodetxt.Name = "AdapterCodetxt";
            this.AdapterCodetxt.Size = new System.Drawing.Size(121, 28);
            this.AdapterCodetxt.TabIndex = 4;
            this.AdapterCodetxt.Text = "2-期貨AM盤";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(216, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Type";
            // 
            // tbType
            // 
            this.tbType.AutoCompleteCustomSource.AddRange(new string[] {
            "0-上市",
            "1-上櫃",
            "2-期貨AM盤",
            "3-期貨PM盤",
            "4-選擇權AM盤",
            "5-選擇權PM盤"});
            this.tbType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tbType.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbType.FormattingEnabled = true;
            this.tbType.Items.AddRange(new object[] {
            "I020",
            "I080",
            "6"});
            this.tbType.Location = new System.Drawing.Point(264, 65);
            this.tbType.Name = "tbType";
            this.tbType.Size = new System.Drawing.Size(121, 28);
            this.tbType.TabIndex = 6;
            this.tbType.Text = "I080";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(391, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Symbol";
            // 
            // tbSymbol
            // 
            this.tbSymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbSymbol.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbSymbol.Location = new System.Drawing.Point(461, 65);
            this.tbSymbol.Name = "tbSymbol";
            this.tbSymbol.Size = new System.Drawing.Size(127, 29);
            this.tbSymbol.TabIndex = 8;
            this.tbSymbol.Text = "TXFI7";
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button2.Location = new System.Drawing.Point(594, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 29);
            this.button2.TabIndex = 9;
            this.button2.Text = "Subscribe";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button3.Location = new System.Drawing.Point(724, 65);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(124, 29);
            this.button3.TabIndex = 10;
            this.button3.Text = "UnSubscribe";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // tbMessage
            // 
            this.tbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMessage.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbMessage.Location = new System.Drawing.Point(3, 100);
            this.tbMessage.Multiline = true;
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(883, 509);
            this.tbMessage.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 613);
            this.Controls.Add(this.tbMessage);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tbSymbol);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.AdapterCodetxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.IPtxt);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "MarketDataApiTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox IPtxt;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox AdapterCodetxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox tbType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSymbol;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox tbMessage;
    }
}

