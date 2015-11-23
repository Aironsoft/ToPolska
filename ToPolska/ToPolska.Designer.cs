namespace ToPolska
{
    partial class ToPolska
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbExpression = new System.Windows.Forms.TextBox();
            this.btOK = new System.Windows.Forms.Button();
            this.lbPolska = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tbExpression
            // 
            this.tbExpression.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbExpression.Location = new System.Drawing.Point(12, 12);
            this.tbExpression.Name = "tbExpression";
            this.tbExpression.Size = new System.Drawing.Size(533, 22);
            this.tbExpression.TabIndex = 0;
            this.tbExpression.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbExpression_KeyDown);
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(551, 11);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(37, 23);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // lbPolska
            // 
            this.lbPolska.AutoSize = true;
            this.lbPolska.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbPolska.ForeColor = System.Drawing.Color.White;
            this.lbPolska.Location = new System.Drawing.Point(12, 52);
            this.lbPolska.Name = "lbPolska";
            this.lbPolska.Size = new System.Drawing.Size(23, 17);
            this.lbPolska.TabIndex = 2;
            this.lbPolska.Text = "...";
            // 
            // ToPolska
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(593, 124);
            this.Controls.Add(this.lbPolska);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.tbExpression);
            this.Name = "ToPolska";
            this.Text = "ToPolska";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbExpression;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Label lbPolska;
    }
}

