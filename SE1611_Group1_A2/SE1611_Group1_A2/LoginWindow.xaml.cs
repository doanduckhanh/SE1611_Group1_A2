using Microsoft.EntityFrameworkCore;
using SE1611_Group1_A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SE1611_Group1_A2
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        MusicStoreContext dbContext;
   
        public LoginWindow()
        {
            InitializeComponent();
            dbContext= new MusicStoreContext();
        }

       
        private void btn_Login(object sender, RoutedEventArgs e)
        {
            string username = tbUsername.Text.Trim().ToString();
            string password = pbPassword.Password.Trim().ToString();



            if (AuthenticateUser(username, password))
            {
                var user = dbContext.Users
                    .Where(u => u.UserName == username && u.Password == password)
                    .FirstOrDefault();
                UserSession.UserName = user.UserName;
                UserSession.Password = user.Password;
                UserSession.Role = user.Role;
                if (username.Equals(user.UserName, StringComparison.Ordinal) && password.Equals(user.Password, StringComparison.Ordinal))
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Username or password incorrect");
                }
            }
            else
            {
                MessageBox.Show("Don't have user");
            }
        }
        public bool AuthenticateUser(string username, string password)
        {
            // use entity framework to retrieve the user information from the database
            var user = dbContext.Users
                .Where(u => u.UserName == username && u.Password == password)
                .FirstOrDefault();

            return user != null;
        }



        private void btn_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
