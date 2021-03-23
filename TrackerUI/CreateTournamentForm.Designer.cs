
namespace TrackerUI
{
    partial class CreateTournamentForm
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
            this.headerLabel = new System.Windows.Forms.Label();
            this.tournamentNameTextBox = new System.Windows.Forms.TextBox();
            this.tournamentNameLabel = new System.Windows.Forms.Label();
            this.entryValueTextBox = new System.Windows.Forms.TextBox();
            this.entryFeeLabel = new System.Windows.Forms.Label();
            this.selectTeamDropDown = new System.Windows.Forms.ComboBox();
            this.selectTeamLabel = new System.Windows.Forms.Label();
            this.createNewTeamLink = new System.Windows.Forms.LinkLabel();
            this.addTeamButton = new System.Windows.Forms.Button();
            this.createPrizeButton = new System.Windows.Forms.Button();
            this.tournamentTeamsListBox = new System.Windows.Forms.ListBox();
            this.torunamentPlayersLabel = new System.Windows.Forms.Label();
            this.removeSelectedTeamButton = new System.Windows.Forms.Button();
            this.prizeLabel = new System.Windows.Forms.Label();
            this.prizesListBox = new System.Windows.Forms.ListBox();
            this.removeSelectedPrizeButton = new System.Windows.Forms.Button();
            this.createTournamentButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // headerLabel
            // 
            this.headerLabel.AutoSize = true;
            this.headerLabel.Font = new System.Drawing.Font("Segoe UI Historic", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.headerLabel.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.headerLabel.Location = new System.Drawing.Point(27, 28);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(338, 50);
            this.headerLabel.TabIndex = 1;
            this.headerLabel.Text = "Create Tournament";
            // 
            // tournamentNameTextBox
            // 
            this.tournamentNameTextBox.Location = new System.Drawing.Point(27, 126);
            this.tournamentNameTextBox.Name = "tournamentNameTextBox";
            this.tournamentNameTextBox.Size = new System.Drawing.Size(287, 35);
            this.tournamentNameTextBox.TabIndex = 10;
            // 
            // tournamentNameLabel
            // 
            this.tournamentNameLabel.AutoSize = true;
            this.tournamentNameLabel.Font = new System.Drawing.Font("Segoe UI Historic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tournamentNameLabel.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.tournamentNameLabel.Location = new System.Drawing.Point(27, 78);
            this.tournamentNameLabel.Name = "tournamentNameLabel";
            this.tournamentNameLabel.Size = new System.Drawing.Size(287, 45);
            this.tournamentNameLabel.TabIndex = 9;
            this.tournamentNameLabel.Text = "Tournament Name";
            // 
            // entryValueTextBox
            // 
            this.entryValueTextBox.Location = new System.Drawing.Point(183, 193);
            this.entryValueTextBox.Name = "entryValueTextBox";
            this.entryValueTextBox.Size = new System.Drawing.Size(100, 35);
            this.entryValueTextBox.TabIndex = 12;
            this.entryValueTextBox.Text = "20";
            // 
            // entryFeeLabel
            // 
            this.entryFeeLabel.AutoSize = true;
            this.entryFeeLabel.Font = new System.Drawing.Font("Segoe UI Historic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.entryFeeLabel.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.entryFeeLabel.Location = new System.Drawing.Point(27, 183);
            this.entryFeeLabel.Name = "entryFeeLabel";
            this.entryFeeLabel.Size = new System.Drawing.Size(150, 45);
            this.entryFeeLabel.TabIndex = 11;
            this.entryFeeLabel.Text = "Entry Fee";
            // 
            // selectTeamDropDown
            // 
            this.selectTeamDropDown.FormattingEnabled = true;
            this.selectTeamDropDown.Location = new System.Drawing.Point(27, 289);
            this.selectTeamDropDown.Name = "selectTeamDropDown";
            this.selectTeamDropDown.Size = new System.Drawing.Size(287, 38);
            this.selectTeamDropDown.TabIndex = 14;
            // 
            // selectTeamLabel
            // 
            this.selectTeamLabel.AutoSize = true;
            this.selectTeamLabel.Font = new System.Drawing.Font("Segoe UI Historic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.selectTeamLabel.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.selectTeamLabel.Location = new System.Drawing.Point(27, 241);
            this.selectTeamLabel.Name = "selectTeamLabel";
            this.selectTeamLabel.Size = new System.Drawing.Size(192, 45);
            this.selectTeamLabel.TabIndex = 13;
            this.selectTeamLabel.Text = "Select Team";
            // 
            // createNewTeamLink
            // 
            this.createNewTeamLink.AutoSize = true;
            this.createNewTeamLink.Location = new System.Drawing.Point(225, 253);
            this.createNewTeamLink.Name = "createNewTeamLink";
            this.createNewTeamLink.Size = new System.Drawing.Size(117, 30);
            this.createNewTeamLink.TabIndex = 15;
            this.createNewTeamLink.TabStop = true;
            this.createNewTeamLink.Text = "Create new";
            this.createNewTeamLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.createNewTeamLink_LinkClicked);
            // 
            // addTeamButton
            // 
            this.addTeamButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.addTeamButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.addTeamButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.addTeamButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addTeamButton.Font = new System.Drawing.Font("Segoe UI Historic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.addTeamButton.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.addTeamButton.Location = new System.Drawing.Point(92, 333);
            this.addTeamButton.Name = "addTeamButton";
            this.addTeamButton.Size = new System.Drawing.Size(152, 45);
            this.addTeamButton.TabIndex = 16;
            this.addTeamButton.Text = "Add Team";
            this.addTeamButton.UseVisualStyleBackColor = true;
            this.addTeamButton.Click += new System.EventHandler(this.addTeamButton_Click);
            // 
            // createPrizeButton
            // 
            this.createPrizeButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.createPrizeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.createPrizeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.createPrizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createPrizeButton.Font = new System.Drawing.Font("Segoe UI Historic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.createPrizeButton.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.createPrizeButton.Location = new System.Drawing.Point(92, 393);
            this.createPrizeButton.Name = "createPrizeButton";
            this.createPrizeButton.Size = new System.Drawing.Size(152, 45);
            this.createPrizeButton.TabIndex = 17;
            this.createPrizeButton.Text = "Create Prize";
            this.createPrizeButton.UseVisualStyleBackColor = true;
            this.createPrizeButton.Click += new System.EventHandler(this.createPrizeButton_Click);
            // 
            // tournamentTeamsListBox
            // 
            this.tournamentTeamsListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tournamentTeamsListBox.FormattingEnabled = true;
            this.tournamentTeamsListBox.ItemHeight = 30;
            this.tournamentTeamsListBox.Location = new System.Drawing.Point(363, 126);
            this.tournamentTeamsListBox.Name = "tournamentTeamsListBox";
            this.tournamentTeamsListBox.Size = new System.Drawing.Size(297, 122);
            this.tournamentTeamsListBox.TabIndex = 18;
            // 
            // torunamentPlayersLabel
            // 
            this.torunamentPlayersLabel.AutoSize = true;
            this.torunamentPlayersLabel.Font = new System.Drawing.Font("Segoe UI Historic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.torunamentPlayersLabel.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.torunamentPlayersLabel.Location = new System.Drawing.Point(363, 78);
            this.torunamentPlayersLabel.Name = "torunamentPlayersLabel";
            this.torunamentPlayersLabel.Size = new System.Drawing.Size(241, 45);
            this.torunamentPlayersLabel.TabIndex = 19;
            this.torunamentPlayersLabel.Text = "Teams / Players";
            // 
            // removeSelectedTeamButton
            // 
            this.removeSelectedTeamButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.removeSelectedTeamButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.removeSelectedTeamButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.removeSelectedTeamButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeSelectedTeamButton.Font = new System.Drawing.Font("Segoe UI Historic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.removeSelectedTeamButton.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.removeSelectedTeamButton.Location = new System.Drawing.Point(666, 154);
            this.removeSelectedTeamButton.Name = "removeSelectedTeamButton";
            this.removeSelectedTeamButton.Size = new System.Drawing.Size(152, 74);
            this.removeSelectedTeamButton.TabIndex = 20;
            this.removeSelectedTeamButton.Text = "Remove Selected";
            this.removeSelectedTeamButton.UseVisualStyleBackColor = true;
            this.removeSelectedTeamButton.Click += new System.EventHandler(this.removeSelectedTeamButton_Click);
            // 
            // prizeLabel
            // 
            this.prizeLabel.AutoSize = true;
            this.prizeLabel.Font = new System.Drawing.Font("Segoe UI Historic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.prizeLabel.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.prizeLabel.Location = new System.Drawing.Point(363, 268);
            this.prizeLabel.Name = "prizeLabel";
            this.prizeLabel.Size = new System.Drawing.Size(102, 45);
            this.prizeLabel.TabIndex = 22;
            this.prizeLabel.Text = "Prizes";
            // 
            // prizesListBox
            // 
            this.prizesListBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prizesListBox.FormattingEnabled = true;
            this.prizesListBox.ItemHeight = 30;
            this.prizesListBox.Location = new System.Drawing.Point(363, 316);
            this.prizesListBox.Name = "prizesListBox";
            this.prizesListBox.Size = new System.Drawing.Size(297, 122);
            this.prizesListBox.TabIndex = 21;
            // 
            // removeSelectedPrizeButton
            // 
            this.removeSelectedPrizeButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.removeSelectedPrizeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.removeSelectedPrizeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.removeSelectedPrizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.removeSelectedPrizeButton.Font = new System.Drawing.Font("Segoe UI Historic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.removeSelectedPrizeButton.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.removeSelectedPrizeButton.Location = new System.Drawing.Point(666, 344);
            this.removeSelectedPrizeButton.Name = "removeSelectedPrizeButton";
            this.removeSelectedPrizeButton.Size = new System.Drawing.Size(152, 74);
            this.removeSelectedPrizeButton.TabIndex = 23;
            this.removeSelectedPrizeButton.Text = "Remove Selected";
            this.removeSelectedPrizeButton.UseVisualStyleBackColor = true;
            this.removeSelectedPrizeButton.Click += new System.EventHandler(this.removeSelectedPrizeButton_Click);
            // 
            // createTournamentButton
            // 
            this.createTournamentButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.createTournamentButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.createTournamentButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.createTournamentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createTournamentButton.Font = new System.Drawing.Font("Segoe UI Historic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.createTournamentButton.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.createTournamentButton.Location = new System.Drawing.Point(307, 451);
            this.createTournamentButton.Name = "createTournamentButton";
            this.createTournamentButton.Size = new System.Drawing.Size(228, 77);
            this.createTournamentButton.TabIndex = 24;
            this.createTournamentButton.Text = "Create Tournament";
            this.createTournamentButton.UseVisualStyleBackColor = true;
            this.createTournamentButton.Click += new System.EventHandler(this.createTournamentButton_Click);
            // 
            // CreateTournamentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(840, 540);
            this.Controls.Add(this.createTournamentButton);
            this.Controls.Add(this.removeSelectedPrizeButton);
            this.Controls.Add(this.prizeLabel);
            this.Controls.Add(this.prizesListBox);
            this.Controls.Add(this.removeSelectedTeamButton);
            this.Controls.Add(this.torunamentPlayersLabel);
            this.Controls.Add(this.tournamentTeamsListBox);
            this.Controls.Add(this.createPrizeButton);
            this.Controls.Add(this.addTeamButton);
            this.Controls.Add(this.createNewTeamLink);
            this.Controls.Add(this.selectTeamDropDown);
            this.Controls.Add(this.selectTeamLabel);
            this.Controls.Add(this.entryValueTextBox);
            this.Controls.Add(this.entryFeeLabel);
            this.Controls.Add(this.tournamentNameTextBox);
            this.Controls.Add(this.tournamentNameLabel);
            this.Controls.Add(this.headerLabel);
            this.Font = new System.Drawing.Font("Segoe UI Historic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "CreateTournamentForm";
            this.Text = "Create Tournament";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CreateTournamentForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.TextBox tournamentNameTextBox;
        private System.Windows.Forms.Label tournamentNameLabel;
        private System.Windows.Forms.TextBox entryValueTextBox;
        private System.Windows.Forms.Label entryFeeLabel;
        private System.Windows.Forms.ComboBox selectTeamDropDown;
        private System.Windows.Forms.Label selectTeamLabel;
        private System.Windows.Forms.LinkLabel createNewTeamLink;
        private System.Windows.Forms.Button addTeamButton;
        private System.Windows.Forms.Button createPrizeButton;
        private System.Windows.Forms.ListBox tournamentTeamsListBox;
        private System.Windows.Forms.Label torunamentPlayersLabel;
        private System.Windows.Forms.Button removeSelectedTeamButton;
        private System.Windows.Forms.Label prizeLabel;
        private System.Windows.Forms.ListBox prizesListBox;
        private System.Windows.Forms.Button removeSelectedPrizeButton;
        private System.Windows.Forms.Button createTournamentButton;
    }
}