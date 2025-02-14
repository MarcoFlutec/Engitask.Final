namespace Engitask
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
            components = new System.ComponentModel.Container();
            label1 = new Label();
            cmdAddNewUser = new Button();
            txtName = new TextBox();
            txtEmail = new TextBox();
            txtLastName = new TextBox();
            txtUserById = new TextBox();
            cmdGetUser = new Button();
            lb1 = new Label();
            label3 = new Label();
            label4 = new Label();
            label2 = new Label();
            label5 = new Label();
            dataGridView1 = new DataGridView();
            idDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            nameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            lastNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            emailDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            createDateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            modifiedDateDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            userBindingSource = new BindingSource(components);
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            txtEditLastName = new TextBox();
            txtEditEmail = new TextBox();
            txtEditName = new TextBox();
            label10 = new Label();
            cmdSave = new Button();
            cmdDelete = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)userBindingSource).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(93, 27);
            label1.Name = "label1";
            label1.Size = new Size(137, 32);
            label1.TabIndex = 0;
            label1.Text = "Create User";
            // 
            // cmdAddNewUser
            // 
            cmdAddNewUser.Location = new Point(114, 149);
            cmdAddNewUser.Name = "cmdAddNewUser";
            cmdAddNewUser.Size = new Size(75, 23);
            cmdAddNewUser.TabIndex = 2;
            cmdAddNewUser.Text = "Add User";
            cmdAddNewUser.UseVisualStyleBackColor = true;
            cmdAddNewUser.Click += cmdAddNewUser_Click;
            // 
            // txtName
            // 
            txtName.Location = new Point(103, 62);
            txtName.Name = "txtName";
            txtName.Size = new Size(100, 23);
            txtName.TabIndex = 3;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(103, 120);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(100, 23);
            txtEmail.TabIndex = 4;
            // 
            // txtLastName
            // 
            txtLastName.Location = new Point(103, 91);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(100, 23);
            txtLastName.TabIndex = 5;
            // 
            // txtUserById
            // 
            txtUserById.Location = new Point(112, 254);
            txtUserById.Name = "txtUserById";
            txtUserById.Size = new Size(69, 23);
            txtUserById.TabIndex = 6;
            // 
            // cmdGetUser
            // 
            cmdGetUser.Location = new Point(187, 253);
            cmdGetUser.Name = "cmdGetUser";
            cmdGetUser.RightToLeft = RightToLeft.Yes;
            cmdGetUser.Size = new Size(75, 23);
            cmdGetUser.TabIndex = 7;
            cmdGetUser.Text = "Get User";
            cmdGetUser.UseVisualStyleBackColor = true;
            cmdGetUser.Click += cmdGetUser_Click;
            // 
            // lb1
            // 
            lb1.AutoSize = true;
            lb1.Location = new Point(64, 285);
            lb1.Name = "lb1";
            lb1.Size = new Size(42, 15);
            lb1.TabIndex = 8;
            lb1.Text = "Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(40, 311);
            label3.Name = "label3";
            label3.Size = new Size(66, 15);
            label3.TabIndex = 10;
            label3.Text = "Last Name:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(67, 340);
            label4.Name = "label4";
            label4.Size = new Size(39, 15);
            label4.TabIndex = 12;
            label4.Text = "Email:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(460, 27);
            label2.Name = "label2";
            label2.Size = new Size(71, 32);
            label2.TabIndex = 15;
            label2.Text = "Users";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(31, 219);
            label5.Name = "label5";
            label5.Size = new Size(105, 32);
            label5.TabIndex = 16;
            label5.Text = "Get User";
            label5.TextAlign = ContentAlignment.TopCenter;
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { idDataGridViewTextBoxColumn, nameDataGridViewTextBoxColumn, lastNameDataGridViewTextBoxColumn, emailDataGridViewTextBoxColumn, createDateDataGridViewTextBoxColumn, modifiedDateDataGridViewTextBoxColumn });
            dataGridView1.DataSource = userBindingSource;
            dataGridView1.Location = new Point(460, 78);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(698, 378);
            dataGridView1.TabIndex = 17;
            // 
            // idDataGridViewTextBoxColumn
            // 
            idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn.HeaderText = "Id";
            idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            // 
            // nameDataGridViewTextBoxColumn
            // 
            nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            nameDataGridViewTextBoxColumn.HeaderText = "Name";
            nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // lastNameDataGridViewTextBoxColumn
            // 
            lastNameDataGridViewTextBoxColumn.DataPropertyName = "LastName";
            lastNameDataGridViewTextBoxColumn.HeaderText = "LastName";
            lastNameDataGridViewTextBoxColumn.Name = "lastNameDataGridViewTextBoxColumn";
            // 
            // emailDataGridViewTextBoxColumn
            // 
            emailDataGridViewTextBoxColumn.DataPropertyName = "Email";
            emailDataGridViewTextBoxColumn.HeaderText = "Email";
            emailDataGridViewTextBoxColumn.Name = "emailDataGridViewTextBoxColumn";
            // 
            // createDateDataGridViewTextBoxColumn
            // 
            createDateDataGridViewTextBoxColumn.DataPropertyName = "CreateDate";
            createDateDataGridViewTextBoxColumn.HeaderText = "CreateDate";
            createDateDataGridViewTextBoxColumn.Name = "createDateDataGridViewTextBoxColumn";
            // 
            // modifiedDateDataGridViewTextBoxColumn
            // 
            modifiedDateDataGridViewTextBoxColumn.DataPropertyName = "ModifiedDate";
            modifiedDateDataGridViewTextBoxColumn.HeaderText = "ModifiedDate";
            modifiedDateDataGridViewTextBoxColumn.Name = "modifiedDateDataGridViewTextBoxColumn";
            // 
            // userBindingSource
            // 
            userBindingSource.DataSource = typeof(User);
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(31, 119);
            label6.Name = "label6";
            label6.Size = new Size(39, 15);
            label6.TabIndex = 20;
            label6.Text = "Email:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(31, 91);
            label7.Name = "label7";
            label7.Size = new Size(66, 15);
            label7.TabIndex = 19;
            label7.Text = "Last Name:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(31, 65);
            label8.Name = "label8";
            label8.Size = new Size(42, 15);
            label8.TabIndex = 18;
            label8.Text = "Name:";
            // 
            // txtEditLastName
            // 
            txtEditLastName.Location = new Point(112, 311);
            txtEditLastName.Name = "txtEditLastName";
            txtEditLastName.Size = new Size(150, 23);
            txtEditLastName.TabIndex = 23;
            // 
            // txtEditEmail
            // 
            txtEditEmail.Location = new Point(112, 340);
            txtEditEmail.Name = "txtEditEmail";
            txtEditEmail.Size = new Size(150, 23);
            txtEditEmail.TabIndex = 22;
            // 
            // txtEditName
            // 
            txtEditName.Location = new Point(112, 282);
            txtEditName.Name = "txtEditName";
            txtEditName.Size = new Size(150, 23);
            txtEditName.TabIndex = 21;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(86, 257);
            label10.Name = "label10";
            label10.Size = new Size(20, 15);
            label10.TabIndex = 25;
            label10.Text = "Id:";
            // 
            // cmdSave
            // 
            cmdSave.Location = new Point(112, 369);
            cmdSave.Name = "cmdSave";
            cmdSave.RightToLeft = RightToLeft.Yes;
            cmdSave.Size = new Size(69, 23);
            cmdSave.TabIndex = 26;
            cmdSave.Text = "Save";
            cmdSave.UseVisualStyleBackColor = true;
            cmdSave.Click += cmdSave_Click;
            // 
            // cmdDelete
            // 
            cmdDelete.Location = new Point(187, 369);
            cmdDelete.Name = "cmdDelete";
            cmdDelete.RightToLeft = RightToLeft.Yes;
            cmdDelete.Size = new Size(75, 23);
            cmdDelete.TabIndex = 27;
            cmdDelete.Text = "Delete";
            cmdDelete.UseVisualStyleBackColor = true;
            cmdDelete.Click += cmdDelete_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1170, 468);
            Controls.Add(cmdDelete);
            Controls.Add(cmdSave);
            Controls.Add(label10);
            Controls.Add(txtEditLastName);
            Controls.Add(txtEditEmail);
            Controls.Add(txtEditName);
            Controls.Add(label6);
            Controls.Add(label7);
            Controls.Add(label8);
            Controls.Add(dataGridView1);
            Controls.Add(label5);
            Controls.Add(label2);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(lb1);
            Controls.Add(cmdGetUser);
            Controls.Add(txtUserById);
            Controls.Add(txtLastName);
            Controls.Add(txtEmail);
            Controls.Add(txtName);
            Controls.Add(cmdAddNewUser);
            Controls.Add(label1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)userBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button cmdAddNewUser;
        private TextBox txtName;
        private TextBox txtEmail;
        private TextBox txtLastName;
        private TextBox txtUserById;
        private Button cmdGetUser;
        private Label lb1;
        private Label label3;
        private Label label4;

        private Label label2;
        private Label label5;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn lastNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn emailDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn createDateDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn modifiedDateDataGridViewTextBoxColumn;
        private BindingSource userBindingSource;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox txtEditLastName;
        private TextBox txtEditEmail;
        private TextBox txtEditName;
        private Label label10;
        private Button cmdSave;
        private Button cmdDelete;
    }
}