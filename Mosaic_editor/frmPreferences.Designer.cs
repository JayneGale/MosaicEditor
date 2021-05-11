
namespace Mosaic_editor
{
    partial class frmPreferences

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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPuzzlesFolder = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnBrowsePuzzleFolders = new System.Windows.Forms.Button();
            this.btnBrowseEngineFolder = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPuzzleEngineFolder = new System.Windows.Forms.TextBox();
            this.chkShowCoords = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(409, 130);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 31);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(516, 130);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 31);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Puzzle folder:";
            // 
            // cmbPuzzlesFolder
            // 
            this.cmbPuzzlesFolder.FormattingEnabled = true;
            this.cmbPuzzlesFolder.Location = new System.Drawing.Point(116, 32);
            this.cmbPuzzlesFolder.Name = "cmbPuzzlesFolder";
            this.cmbPuzzlesFolder.Size = new System.Drawing.Size(400, 21);
            this.cmbPuzzlesFolder.TabIndex = 1;
            this.cmbPuzzlesFolder.SelectedIndexChanged += new System.EventHandler(this.cmbPuzzlesFolder_SelectedIndexChanged);
            // 
            // btnBrowsePuzzleFolders
            // 
            this.btnBrowsePuzzleFolders.Location = new System.Drawing.Point(532, 30);
            this.btnBrowsePuzzleFolders.Name = "btnBrowsePuzzleFolders";
            this.btnBrowsePuzzleFolders.Size = new System.Drawing.Size(75, 23);
            this.btnBrowsePuzzleFolders.TabIndex = 2;
            this.btnBrowsePuzzleFolders.Text = "Browse..";
            this.btnBrowsePuzzleFolders.UseVisualStyleBackColor = true;
            this.btnBrowsePuzzleFolders.Click += new System.EventHandler(this.btnBrowsePuzzleFolders_Click);
            // 
            // btnBrowseEngineFolder
            // 
            this.btnBrowseEngineFolder.Location = new System.Drawing.Point(532, 73);
            this.btnBrowseEngineFolder.Name = "btnBrowseEngineFolder";
            this.btnBrowseEngineFolder.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseEngineFolder.TabIndex = 5;
            this.btnBrowseEngineFolder.Text = "Browse..";
            this.btnBrowseEngineFolder.UseVisualStyleBackColor = true;
            this.btnBrowseEngineFolder.Click += new System.EventHandler(this.btnBrowseEngineFolder_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mosaic Engine folder:";
            // 
            // txtPuzzleEngineFolder
            // 
            this.txtPuzzleEngineFolder.Location = new System.Drawing.Point(155, 75);
            this.txtPuzzleEngineFolder.Name = "txtPuzzleEngineFolder";
            this.txtPuzzleEngineFolder.Size = new System.Drawing.Size(361, 20);
            this.txtPuzzleEngineFolder.TabIndex = 4;
            // 
            // chkShowCoords
            // 
            this.chkShowCoords.AutoSize = true;
            this.chkShowCoords.Location = new System.Drawing.Point(116, 138);
            this.chkShowCoords.Name = "chkShowCoords";
            this.chkShowCoords.Size = new System.Drawing.Size(127, 17);
            this.chkShowCoords.TabIndex = 8;
            this.chkShowCoords.Text = "Show tile coordinates";
            this.chkShowCoords.UseVisualStyleBackColor = true;
            // 
            // frmPreferences
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(638, 200);
            this.Controls.Add(this.chkShowCoords);
            this.Controls.Add(this.txtPuzzleEngineFolder);
            this.Controls.Add(this.btnBrowseEngineFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBrowsePuzzleFolders);
            this.Controls.Add(this.cmbPuzzlesFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPreferences";
            this.Text = "Mosaic Editor Preferences";
            this.Load += new System.EventHandler(this.frmPreferences_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPuzzlesFolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnBrowsePuzzleFolders;
        private System.Windows.Forms.Button btnBrowseEngineFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPuzzleEngineFolder;
        private System.Windows.Forms.CheckBox chkShowCoords;
    }
}