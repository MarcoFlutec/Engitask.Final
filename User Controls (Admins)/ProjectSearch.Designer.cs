namespace Engitask.User_Controls__Admins_
{
    partial class ProjectSearch
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            guna2GradientButton4 = new Guna.UI2.WinForms.Guna2GradientButton();
            guna2TextBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            guna2DataGridView111 = new Guna.UI2.WinForms.Guna2DataGridView();
            NoProyecto = new DataGridViewTextBoxColumn();
            NombreDelProyecto = new DataGridViewTextBoxColumn();
            EstatusActual = new DataGridViewComboBoxColumn();
            Empresa = new DataGridViewComboBoxColumn();
            ((System.ComponentModel.ISupportInitialize)guna2DataGridView111).BeginInit();
            SuspendLayout();
            // 
            // guna2GradientButton4
            // 
            guna2GradientButton4.BackColor = Color.Transparent;
            guna2GradientButton4.BorderRadius = 10;
            guna2GradientButton4.BorderThickness = 1;
            guna2GradientButton4.CustomizableEdges = customizableEdges1;
            guna2GradientButton4.DisabledState.BorderColor = Color.DarkGray;
            guna2GradientButton4.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2GradientButton4.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2GradientButton4.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
            guna2GradientButton4.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2GradientButton4.FillColor = Color.FromArgb(234, 216, 217);
            guna2GradientButton4.FillColor2 = Color.Ivory;
            guna2GradientButton4.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2GradientButton4.ForeColor = Color.Black;
            guna2GradientButton4.Image = Properties.Resources.icons8_signing_a_document;
            guna2GradientButton4.Location = new Point(1089, 39);
            guna2GradientButton4.Margin = new Padding(3, 4, 3, 4);
            guna2GradientButton4.Name = "guna2GradientButton4";
            guna2GradientButton4.ShadowDecoration.CustomizableEdges = customizableEdges2;
            guna2GradientButton4.Size = new Size(206, 63);
            guna2GradientButton4.TabIndex = 17;
            guna2GradientButton4.Text = "Save Projects";
            guna2GradientButton4.Click += guna2GradientButton4_Click;
            // 
            // guna2TextBox1
            // 
            guna2TextBox1.BackColor = Color.Transparent;
            guna2TextBox1.BorderRadius = 10;
            guna2TextBox1.CustomizableEdges = customizableEdges3;
            guna2TextBox1.DefaultText = "";
            guna2TextBox1.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            guna2TextBox1.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            guna2TextBox1.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox1.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            guna2TextBox1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox1.Font = new Font("Segoe UI", 9F);
            guna2TextBox1.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            guna2TextBox1.IconLeft = Properties.Resources.icons8_search_in_list;
            guna2TextBox1.Location = new Point(246, 42);
            guna2TextBox1.Margin = new Padding(3, 4, 3, 4);
            guna2TextBox1.Name = "guna2TextBox1";
            guna2TextBox1.PasswordChar = '\0';
            guna2TextBox1.PlaceholderText = "";
            guna2TextBox1.SelectedText = "";
            guna2TextBox1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2TextBox1.Size = new Size(738, 60);
            guna2TextBox1.TabIndex = 18;
            guna2TextBox1.TextChanged += guna2TextBox1_TextChanged_1;
            // 
            // guna2DataGridView111
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            guna2DataGridView111.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            guna2DataGridView111.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            guna2DataGridView111.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.MediumSeaGreen;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            guna2DataGridView111.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            guna2DataGridView111.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            guna2DataGridView111.Columns.AddRange(new DataGridViewColumn[] { NoProyecto, NombreDelProyecto, EstatusActual, Empresa });
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            guna2DataGridView111.DefaultCellStyle = dataGridViewCellStyle3;
            guna2DataGridView111.GridColor = Color.DarkGreen;
            guna2DataGridView111.Location = new Point(42, 136);
            guna2DataGridView111.Name = "guna2DataGridView111";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            guna2DataGridView111.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            guna2DataGridView111.RowHeadersVisible = false;
            guna2DataGridView111.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            guna2DataGridView111.Size = new Size(1253, 730);
            guna2DataGridView111.TabIndex = 19;
            guna2DataGridView111.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            guna2DataGridView111.ThemeStyle.AlternatingRowsStyle.Font = null;
            guna2DataGridView111.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            guna2DataGridView111.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            guna2DataGridView111.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            guna2DataGridView111.ThemeStyle.BackColor = Color.White;
            guna2DataGridView111.ThemeStyle.GridColor = Color.DarkGreen;
            guna2DataGridView111.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            guna2DataGridView111.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Single;
            guna2DataGridView111.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2DataGridView111.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            guna2DataGridView111.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            guna2DataGridView111.ThemeStyle.HeaderStyle.Height = 29;
            guna2DataGridView111.ThemeStyle.ReadOnly = false;
            guna2DataGridView111.ThemeStyle.RowsStyle.BackColor = Color.White;
            guna2DataGridView111.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            guna2DataGridView111.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            guna2DataGridView111.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            guna2DataGridView111.ThemeStyle.RowsStyle.Height = 29;
            guna2DataGridView111.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            guna2DataGridView111.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            guna2DataGridView111.CellContentClick += guna2DataGridView1_CellContentClick;
            // 
            // NoProyecto
            // 
            NoProyecto.DividerWidth = 2;
            NoProyecto.HeaderText = "No. Proyecto";
            NoProyecto.MinimumWidth = 6;
            NoProyecto.Name = "NoProyecto";
            // 
            // NombreDelProyecto
            // 
            NombreDelProyecto.DividerWidth = 2;
            NombreDelProyecto.HeaderText = "Nombre Del Proyecto";
            NombreDelProyecto.MinimumWidth = 6;
            NombreDelProyecto.Name = "NombreDelProyecto";
            // 
            // EstatusActual
            // 
            EstatusActual.DividerWidth = 2;
            EstatusActual.HeaderText = "Estatus";
            EstatusActual.MinimumWidth = 6;
            EstatusActual.Name = "EstatusActual";
            // 
            // Empresa
            // 
            Empresa.DividerWidth = 2;
            Empresa.HeaderText = "Empresa";
            Empresa.MinimumWidth = 6;
            Empresa.Name = "Empresa";
            // 
            // ProjectSearch
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.averporque;
            BackgroundImageLayout = ImageLayout.Stretch;
            Controls.Add(guna2DataGridView111);
            Controls.Add(guna2TextBox1);
            Controls.Add(guna2GradientButton4);
            Name = "ProjectSearch";
            Size = new Size(1331, 917);
            ((System.ComponentModel.ISupportInitialize)guna2DataGridView111).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2GradientButton guna2GradientButton4;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
        private Guna.UI2.WinForms.Guna2DataGridView guna2DataGridView111;
        private DataGridViewTextBoxColumn NoProyecto;
        private DataGridViewTextBoxColumn NombreDelProyecto;
        private DataGridViewComboBoxColumn EstatusActual;
        private DataGridViewComboBoxColumn Empresa;
    }
}
