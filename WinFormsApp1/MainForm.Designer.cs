namespace Aplikacja_Projektowa
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
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newProjectToolStripMenuItem = new ToolStripMenuItem();
            newFileToolStripMenuItem = new ToolStripMenuItem();
            designToolStripMenuItem = new ToolStripMenuItem();
            measurementsToolStripMenuItem = new ToolStripMenuItem();
            approvalToolStripMenuItem = new ToolStripMenuItem();
            textBox1 = new TextBox();
            button1 = new Button();
            comboBox1 = new ComboBox();
            Results = new DataGridView();
            EditItem = new Button();
            button2 = new Button();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Results).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(434, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newProjectToolStripMenuItem, newFileToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 20);
            fileToolStripMenuItem.Text = "Main";
            // 
            // newProjectToolStripMenuItem
            // 
            newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            newProjectToolStripMenuItem.Size = new Size(180, 22);
            newProjectToolStripMenuItem.Text = "New Project";
            newProjectToolStripMenuItem.Click += newProjectToolStripMenuItem_Click;
            // 
            // newFileToolStripMenuItem
            // 
            newFileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { designToolStripMenuItem, measurementsToolStripMenuItem, approvalToolStripMenuItem });
            newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            newFileToolStripMenuItem.Size = new Size(180, 22);
            newFileToolStripMenuItem.Text = "New Item";
            // 
            // designToolStripMenuItem
            // 
            designToolStripMenuItem.Name = "designToolStripMenuItem";
            designToolStripMenuItem.Size = new Size(180, 22);
            designToolStripMenuItem.Text = "Design";
            designToolStripMenuItem.Click += designToolStripMenuItem_Click;
            // 
            // measurementsToolStripMenuItem
            // 
            measurementsToolStripMenuItem.Name = "measurementsToolStripMenuItem";
            measurementsToolStripMenuItem.Size = new Size(180, 22);
            measurementsToolStripMenuItem.Text = "Measurement";
            measurementsToolStripMenuItem.Click += measurementsToolStripMenuItem_Click;
            // 
            // approvalToolStripMenuItem
            // 
            approvalToolStripMenuItem.Name = "approvalToolStripMenuItem";
            approvalToolStripMenuItem.Size = new Size(180, 22);
            approvalToolStripMenuItem.Text = "Approval";
            approvalToolStripMenuItem.Click += approvalToolStripMenuItem_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 58);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(186, 23);
            textBox1.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(204, 58);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Search";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "All files (*.*)", "Design (*.prt)", "Measurement (*.txt)", "Approval (*.pdf)", "Projects ()" });
            comboBox1.Location = new Point(285, 59);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(137, 23);
            comboBox1.TabIndex = 3;
            // 
            // Results
            // 
            Results.AccessibleName = "Results";
            Results.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Results.BackgroundColor = SystemColors.ButtonHighlight;
            Results.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Results.Location = new Point(12, 111);
            Results.Name = "Results";
            Results.ReadOnly = true;
            Results.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            Results.Size = new Size(410, 247);
            Results.TabIndex = 4;
            // 
            // EditItem
            // 
            EditItem.AccessibleName = "EditItem";
            EditItem.Location = new Point(347, 376);
            EditItem.Name = "EditItem";
            EditItem.Size = new Size(75, 23);
            EditItem.TabIndex = 5;
            EditItem.Text = "Edit Item";
            EditItem.UseVisualStyleBackColor = true;
            EditItem.Click += EditItem_Click;
            // 
            // button2
            // 
            button2.Location = new Point(12, 376);
            button2.Name = "button2";
            button2.Size = new Size(136, 23);
            button2.TabIndex = 6;
            button2.Text = "View Project Details";
            button2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 411);
            Controls.Add(button2);
            Controls.Add(EditItem);
            Controls.Add(Results);
            Controls.Add(comboBox1);
            Controls.Add(button1);
            Controls.Add(textBox1);
            Controls.Add(menuStrip1);
            Name = "MainForm";
            Text = "MainForm";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Results).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newProjectToolStripMenuItem;
        private ToolStripMenuItem newFileToolStripMenuItem;
        private ToolStripMenuItem designToolStripMenuItem;
        private ToolStripMenuItem measurementsToolStripMenuItem;
        private ToolStripMenuItem approvalToolStripMenuItem;
        private TextBox textBox1;
        private Button button1;
        private ComboBox comboBox1;
        private DataGridView Results;
        private Button EditItem;
        private Button button2;
    }
}