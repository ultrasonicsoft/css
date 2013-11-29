namespace WinClient
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
            this.btnBrowseSDF = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSDFPath = new System.Windows.Forms.TextBox();
            this.txtConStr = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConfigureData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBrowseSDF
            // 
            this.btnBrowseSDF.Location = new System.Drawing.Point(440, 24);
            this.btnBrowseSDF.Name = "btnBrowseSDF";
            this.btnBrowseSDF.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseSDF.TabIndex = 0;
            this.btnBrowseSDF.Text = "Browse";
            this.btnBrowseSDF.UseVisualStyleBackColor = true;
            this.btnBrowseSDF.Click += new System.EventHandler(this.btnBrowseSDF_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter .SDF file path:";
            // 
            // txtSDFPath
            // 
            this.txtSDFPath.Location = new System.Drawing.Point(154, 24);
            this.txtSDFPath.Name = "txtSDFPath";
            this.txtSDFPath.Size = new System.Drawing.Size(258, 20);
            this.txtSDFPath.TabIndex = 2;
            // 
            // txtConStr
            // 
            this.txtConStr.Location = new System.Drawing.Point(154, 62);
            this.txtConStr.Name = "txtConStr";
            this.txtConStr.Size = new System.Drawing.Size(423, 20);
            this.txtConStr.TabIndex = 5;
            this.txtConStr.Text = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=CaseControlDB;Trusted_Connection=T" +
                "rue";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Connection String (optional)";
            // 
            // btnConfigureData
            // 
            this.btnConfigureData.Location = new System.Drawing.Point(264, 97);
            this.btnConfigureData.Name = "btnConfigureData";
            this.btnConfigureData.Size = new System.Drawing.Size(131, 23);
            this.btnConfigureData.TabIndex = 6;
            this.btnConfigureData.Text = "Migrate Data";
            this.btnConfigureData.UseVisualStyleBackColor = true;
            this.btnConfigureData.Click += new System.EventHandler(this.btnConfigureData_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 155);
            this.Controls.Add(this.btnConfigureData);
            this.Controls.Add(this.txtConStr);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSDFPath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnBrowseSDF);
            this.Name = "Form1";
            this.Text = "Data Migation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBrowseSDF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSDFPath;
        private System.Windows.Forms.TextBox txtConStr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConfigureData;
    }
}

