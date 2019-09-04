namespace SkysJSONGenerator
{
    partial class MainForm
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
            this.comboBoxMod = new System.Windows.Forms.ComboBox();
            this.comboBoxVersion = new System.Windows.Forms.ComboBox();
            this.checkedListBoxOutput = new System.Windows.Forms.CheckedListBox();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.linkLabelEmail = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // comboBoxMod
            // 
            this.comboBoxMod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMod.FormattingEnabled = true;
            this.comboBoxMod.Location = new System.Drawing.Point(113, 12);
            this.comboBoxMod.Name = "comboBoxMod";
            this.comboBoxMod.Size = new System.Drawing.Size(178, 21);
            this.comboBoxMod.TabIndex = 1;
            // 
            // comboBoxVersion
            // 
            this.comboBoxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVersion.FormattingEnabled = true;
            this.comboBoxVersion.Items.AddRange(new object[] {
            "All",
            "1.10",
            "1.11",
            "1.12",
            "1.13",
            "1.14"});
            this.comboBoxVersion.Location = new System.Drawing.Point(12, 12);
            this.comboBoxVersion.Name = "comboBoxVersion";
            this.comboBoxVersion.Size = new System.Drawing.Size(95, 21);
            this.comboBoxVersion.TabIndex = 0;
            this.comboBoxVersion.SelectedIndexChanged += new System.EventHandler(this.comboBoxVersion_SelectedIndexChanged);
            // 
            // checkedListBoxOutput
            // 
            this.checkedListBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxOutput.FormattingEnabled = true;
            this.checkedListBoxOutput.Items.AddRange(new object[] {
            "Blocks",
            "Stairs",
            "Walls",
            "Slabs",
            "Polished Variants"});
            this.checkedListBoxOutput.Location = new System.Drawing.Point(12, 39);
            this.checkedListBoxOutput.Name = "checkedListBoxOutput";
            this.checkedListBoxOutput.Size = new System.Drawing.Size(279, 184);
            this.checkedListBoxOutput.TabIndex = 3;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGenerate.Location = new System.Drawing.Point(218, 229);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(75, 46);
            this.buttonGenerate.TabIndex = 4;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // linkLabelEmail
            // 
            this.linkLabelEmail.AutoSize = true;
            this.linkLabelEmail.Location = new System.Drawing.Point(21, 262);
            this.linkLabelEmail.Name = "linkLabelEmail";
            this.linkLabelEmail.Size = new System.Drawing.Size(161, 13);
            this.linkLabelEmail.TabIndex = 5;
            this.linkLabelEmail.TabStop = true;
            this.linkLabelEmail.Text = "JohnBraham1978@Outlook.com";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 287);
            this.Controls.Add(this.linkLabelEmail);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.checkedListBoxOutput);
            this.Controls.Add(this.comboBoxVersion);
            this.Controls.Add(this.comboBoxMod);
            this.Name = "MainForm";
            this.Text = "Sky\'s JSON Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxMod;
        private System.Windows.Forms.ComboBox comboBoxVersion;
        private System.Windows.Forms.CheckedListBox checkedListBoxOutput;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.LinkLabel linkLabelEmail;
    }
}

