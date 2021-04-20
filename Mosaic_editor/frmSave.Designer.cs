
namespace Mosaic_editor
{
    partial class frmSave

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
            this.grpPuzzleType = new System.Windows.Forms.GroupBox();
            this.rbMatch3 = new System.Windows.Forms.RadioButton();
            this.rbMatch1 = new System.Windows.Forms.RadioButton();
            this.rbMatch5 = new System.Windows.Forms.RadioButton();
            this.chkDontMangle = new System.Windows.Forms.CheckBox();
            this.chkFixedColors = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtSwaps = new System.Windows.Forms.NumericUpDown();
            this.txtPlays = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTier = new System.Windows.Forms.NumericUpDown();
            this.grpPuzzleType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSwaps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTier)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(254, 254);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 31);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSave.Location = new System.Drawing.Point(361, 254);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 31);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grpPuzzleType
            // 
            this.grpPuzzleType.Controls.Add(this.rbMatch3);
            this.grpPuzzleType.Controls.Add(this.rbMatch1);
            this.grpPuzzleType.Controls.Add(this.rbMatch5);
            this.grpPuzzleType.Location = new System.Drawing.Point(41, 58);
            this.grpPuzzleType.Name = "grpPuzzleType";
            this.grpPuzzleType.Size = new System.Drawing.Size(179, 113);
            this.grpPuzzleType.TabIndex = 2;
            this.grpPuzzleType.TabStop = false;
            this.grpPuzzleType.Text = "Puzzle Type";
            // 
            // rbMatch3
            // 
            this.rbMatch3.AutoSize = true;
            this.rbMatch3.Checked = true;
            this.rbMatch3.Location = new System.Drawing.Point(34, 53);
            this.rbMatch3.Name = "rbMatch3";
            this.rbMatch3.Size = new System.Drawing.Size(106, 17);
            this.rbMatch3.TabIndex = 1;
            this.rbMatch3.TabStop = true;
            this.rbMatch3.Text = "Match 3 triangles";
            this.rbMatch3.UseVisualStyleBackColor = true;
            // 
            // rbMatch1
            // 
            this.rbMatch1.AutoSize = true;
            this.rbMatch1.Location = new System.Drawing.Point(34, 76);
            this.rbMatch1.Name = "rbMatch1";
            this.rbMatch1.Size = new System.Drawing.Size(101, 17);
            this.rbMatch1.TabIndex = 2;
            this.rbMatch1.Text = "Match 1 triangle";
            this.rbMatch1.UseVisualStyleBackColor = true;
            // 
            // rbMatch5
            // 
            this.rbMatch5.AutoSize = true;
            this.rbMatch5.Location = new System.Drawing.Point(34, 30);
            this.rbMatch5.Name = "rbMatch5";
            this.rbMatch5.Size = new System.Drawing.Size(106, 17);
            this.rbMatch5.TabIndex = 0;
            this.rbMatch5.Text = "Match 5 triangles";
            this.rbMatch5.UseVisualStyleBackColor = true;
            // 
            // chkDontMangle
            // 
            this.chkDontMangle.AutoSize = true;
            this.chkDontMangle.Location = new System.Drawing.Point(75, 186);
            this.chkDontMangle.Name = "chkDontMangle";
            this.chkDontMangle.Size = new System.Drawing.Size(154, 17);
            this.chkDontMangle.TabIndex = 4;
            this.chkDontMangle.Text = "Don\'t randomise this puzzle";
            this.chkDontMangle.UseVisualStyleBackColor = true;
            // 
            // chkFixedColors
            // 
            this.chkFixedColors.AutoSize = true;
            this.chkFixedColors.Location = new System.Drawing.Point(75, 209);
            this.chkFixedColors.Name = "chkFixedColors";
            this.chkFixedColors.Size = new System.Drawing.Size(221, 17);
            this.chkFixedColors.TabIndex = 5;
            this.chkFixedColors.Text = "Don\'t randomise the colours of this puzzle";
            this.chkFixedColors.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(77, 22);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(375, 20);
            this.txtName.TabIndex = 1;
            // 
            // txtSwaps
            // 
            this.txtSwaps.Location = new System.Drawing.Point(246, 108);
            this.txtSwaps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtSwaps.Name = "txtSwaps";
            this.txtSwaps.Size = new System.Drawing.Size(50, 20);
            this.txtSwaps.TabIndex = 6;
            this.txtSwaps.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSwaps.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // txtPlays
            // 
            this.txtPlays.Location = new System.Drawing.Point(246, 139);
            this.txtPlays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtPlays.Name = "txtPlays";
            this.txtPlays.Size = new System.Drawing.Size(50, 20);
            this.txtPlays.TabIndex = 8;
            this.txtPlays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPlays.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(302, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Number of plays for this puzzle";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(302, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Maximum number of tile swaps";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(302, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Tier";
            // 
            // txtTier
            // 
            this.txtTier.Location = new System.Drawing.Point(246, 75);
            this.txtTier.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtTier.Name = "txtTier";
            this.txtTier.Size = new System.Drawing.Size(50, 20);
            this.txtTier.TabIndex = 12;
            this.txtTier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTier.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // frmSave
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(500, 309);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTier);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPlays);
            this.Controls.Add(this.txtSwaps);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkFixedColors);
            this.Controls.Add(this.chkDontMangle);
            this.Controls.Add(this.grpPuzzleType);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSave";
            this.Text = "Save File";
            this.Load += new System.EventHandler(this.frmSave_Load);
            this.grpPuzzleType.ResumeLayout(false);
            this.grpPuzzleType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSwaps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPlays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTier)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox grpPuzzleType;
        private System.Windows.Forms.RadioButton rbMatch3;
        private System.Windows.Forms.RadioButton rbMatch1;
        private System.Windows.Forms.RadioButton rbMatch5;
        private System.Windows.Forms.CheckBox chkDontMangle;
        private System.Windows.Forms.CheckBox chkFixedColors;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.NumericUpDown txtSwaps;
        private System.Windows.Forms.NumericUpDown txtPlays;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown txtTier;
    }
}