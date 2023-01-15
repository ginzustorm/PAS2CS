using System;

namespace Translator
{
    partial class FirstForm
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
            this.FileLocation = new System.Windows.Forms.TextBox();
            this.ChosePath = new System.Windows.Forms.Button();
            this.RUN = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FileLocation
            // 
            this.FileLocation.Location = new System.Drawing.Point(15, 30);
            this.FileLocation.Name = "FileLocation";
            this.FileLocation.ReadOnly = true;
            this.FileLocation.Size = new System.Drawing.Size(348, 20);
            this.FileLocation.TabIndex = 0;
            this.FileLocation.TextChanged += new System.EventHandler(this.FileLocation_TextChanged);
            // 
            // ChosePath
            // 
            this.ChosePath.Location = new System.Drawing.Point(369, 28);
            this.ChosePath.Name = "ChosePath";
            this.ChosePath.Size = new System.Drawing.Size(75, 23);
            this.ChosePath.TabIndex = 1;
            this.ChosePath.Text = "Обзор";
            this.ChosePath.UseVisualStyleBackColor = true;
            this.ChosePath.Click += new System.EventHandler(this.ChosePath_Click);
            // 
            // RUN
            // 
            this.RUN.Location = new System.Drawing.Point(268, 56);
            this.RUN.Name = "RUN";
            this.RUN.Size = new System.Drawing.Size(176, 23);
            this.RUN.TabIndex = 2;
            this.RUN.Text = "Провести трансляцию";
            this.RUN.UseVisualStyleBackColor = true;
            this.RUN.Click += new System.EventHandler(this.RUN_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "pas";
            this.openFileDialog.FileName = "*.pas";
            this.openFileDialog.Filter = "Код на языке Pascal (.pas)|*.pas";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Путь к файлу для трансляции";
            // 
            // FirstForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 91);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RUN);
            this.Controls.Add(this.ChosePath);
            this.Controls.Add(this.FileLocation);
            this.Name = "FirstForm";
            this.Text = "Pascal to C#";
            this.Load += new System.EventHandler(this.FirstForm_Load);
            this.Shown += new System.EventHandler(this.FirstForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void FileLocation_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion

        private System.Windows.Forms.TextBox FileLocation;
        private System.Windows.Forms.Button ChosePath;
        private System.Windows.Forms.Button RUN;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label1;
    }
}

