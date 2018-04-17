namespace Client.Forms
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
            this.button1 = new System.Windows.Forms.Button();
            this.AllVegetablesListView = new System.Windows.Forms.ListView();
            this.label4 = new System.Windows.Forms.Label();
            this.SelectedVegetablesListView = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.BillLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.GoToPayButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(365, 206);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 52);
            this.button1.TabIndex = 0;
            this.button1.Text = ">>";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // AllVegetablesListView
            // 
            this.AllVegetablesListView.Location = new System.Drawing.Point(12, 40);
            this.AllVegetablesListView.Name = "AllVegetablesListView";
            this.AllVegetablesListView.Size = new System.Drawing.Size(280, 325);
            this.AllVegetablesListView.TabIndex = 7;
            this.AllVegetablesListView.UseCompatibleStateImageBehavior = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Warzywa";
            // 
            // SelectedVegetablesListView
            // 
            this.SelectedVegetablesListView.Location = new System.Drawing.Point(546, 40);
            this.SelectedVegetablesListView.Name = "SelectedVegetablesListView";
            this.SelectedVegetablesListView.Size = new System.Drawing.Size(286, 325);
            this.SelectedVegetablesListView.TabIndex = 9;
            this.SelectedVegetablesListView.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(543, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Koszyk";
            // 
            // BillLabel
            // 
            this.BillLabel.AutoSize = true;
            this.BillLabel.Location = new System.Drawing.Point(401, 40);
            this.BillLabel.Name = "BillLabel";
            this.BillLabel.Size = new System.Drawing.Size(58, 13);
            this.BillLabel.TabIndex = 11;
            this.BillLabel.Text = "asdasdasd";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(401, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Do zaplaty";
            // 
            // GoToPayButton
            // 
            this.GoToPayButton.Location = new System.Drawing.Point(365, 451);
            this.GoToPayButton.Name = "GoToPayButton";
            this.GoToPayButton.Size = new System.Drawing.Size(149, 42);
            this.GoToPayButton.TabIndex = 14;
            this.GoToPayButton.Text = "Idz do kasy";
            this.GoToPayButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 514);
            this.Controls.Add(this.GoToPayButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BillLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SelectedVegetablesListView);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AllVegetablesListView);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView AllVegetablesListView;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListView SelectedVegetablesListView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label BillLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button GoToPayButton;
    }
}

