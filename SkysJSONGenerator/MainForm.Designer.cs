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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxMaterials = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxMod
            // 
            this.comboBoxMod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMod.FormattingEnabled = true;
            this.comboBoxMod.Location = new System.Drawing.Point(113, 27);
            this.comboBoxMod.Name = "comboBoxMod";
            this.comboBoxMod.Size = new System.Drawing.Size(398, 21);
            this.comboBoxMod.TabIndex = 1;
            this.comboBoxMod.SelectedIndexChanged += new System.EventHandler(this.comboBoxMod_SelectedIndexChanged);
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
            this.comboBoxVersion.Location = new System.Drawing.Point(12, 27);
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
            this.checkedListBoxOutput.Location = new System.Drawing.Point(3, 3);
            this.checkedListBoxOutput.Name = "checkedListBoxOutput";
            this.checkedListBoxOutput.Size = new System.Drawing.Size(272, 139);
            this.checkedListBoxOutput.TabIndex = 3;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGenerate.Location = new System.Drawing.Point(438, 234);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(75, 32);
            this.buttonGenerate.TabIndex = 4;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 54);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listBoxMaterials);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkedListBoxOutput);
            this.splitContainer1.Size = new System.Drawing.Size(499, 157);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 7;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(525, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // listBoxMaterials
            // 
            this.listBoxMaterials.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxMaterials.FormattingEnabled = true;
            this.listBoxMaterials.Location = new System.Drawing.Point(3, 0);
            this.listBoxMaterials.Name = "listBoxMaterials";
            this.listBoxMaterials.Size = new System.Drawing.Size(211, 147);
            this.listBoxMaterials.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 278);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.buttonGenerate);
            this.Controls.Add(this.comboBoxVersion);
            this.Controls.Add(this.comboBoxMod);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Sky\'s JSON Generator";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxMod;
        private System.Windows.Forms.ComboBox comboBoxVersion;
        private System.Windows.Forms.CheckedListBox checkedListBoxOutput;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ListBox listBoxMaterials;
    }
}

