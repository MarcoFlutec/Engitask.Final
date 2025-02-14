using Engitask.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Engitask
{
    public partial class Form1 : Form
    {
        private UserRepositories _usrRepo;
        private List<User> _users = new();
        private BindingSource _source = new();
        private User _activeUser = null!;

        public Form1()
        {
            _usrRepo = new();
            InitializeComponent();

        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await RefreshUsers();
        }

        private async void cmdAddNewUser_Click(object sender, EventArgs e)
        {
            var id = await AddNewUser();
            MessageBox.Show($"New User Added {id}");
            await RefreshUsers();
            _source.ResetBindings(false);
        }

        private async void cmdGetUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserById.Text))
            {
                MessageBox.Show("Id Is Empty");
                return;
            }

            var id = int.Parse(txtUserById.Text);
            var user = await GetUserById(id);

            if (user == null)
            {
                MessageBox.Show("User not found");
                return;
            }
            _activeUser = user;
            txtEditName.Text = _activeUser.Name;
            txtEditLastName.Text = _activeUser.LastName;
            txtEditEmail.Text = _activeUser.Email;
        }

        private async void cmdSave_Click(object sender, EventArgs e)
        {
            _activeUser.Name = txtEditName.Text;
            _activeUser.LastName = txtEditLastName.Text;
            _activeUser.Email = txtEditEmail.Text;
            await _usrRepo.UpdateUser(_activeUser.Id, _activeUser);
            await RefreshUsers();
            MessageBox.Show("User Updated");
        }

        private async void cmdDelete_Click(object sender, EventArgs e)
        {
            await _usrRepo.DeleteUser(_activeUser.Id);
            await RefreshUsers();
            CleanTexts();
            MessageBox.Show("User Deleted");
        }

        private async Task RefreshUsers()
        {
            _users = await _usrRepo.GetUsers();
            _source.DataSource = _users;
            dataGridView1.DataSource = _source;
        }

        private async Task<int> AddNewUser()
        {
            var user = new User()
            {
                Name = txtName.Text,
                LastName = txtLastName.Text,
                Email = txtEmail.Text,
                CreateDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var idUser = await _usrRepo.CreateUser(user);
            return idUser;
        }

        private async Task<User?> GetUserById(int id)
        {
            var user = await _usrRepo.GetUser(id);
            return user;
        }

        private void CleanTexts()
        {
            txtEditEmail.Text = "";
            txtEditLastName.Text = "";
            txtEditName.Text = "";
            _activeUser = null!;
        }
    }
}
