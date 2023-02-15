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
using SE1611_Group1_A2.Models;
namespace SE1611_Group1_A2
{
    /// <summary>
    /// Interaction logic for CheckoutWindow.xaml
    /// </summary>
    public partial class CheckoutWindow : Window
    {
        MusicStoreContext musicStoreContext = new MusicStoreContext();
        public CheckoutWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Order order = new Order();
            order.FirstName = txtFirstName.Text;
            order.LastName = txtLastName.Text;
            order.Address = txtAddress.Text;
            order.City = txtCity.Text;
            order.State = txtState.Text;
            order.Country = txtCountry.Text;
            order.Phone = txtPhone.Text;
            order.Email = txtEmail.Text;
            order.Total = decimal.Parse(txtTotal.Text);
            order.PromoCode = null;
            musicStoreContext.Orders.Add(order);
            musicStoreContext.SaveChanges();
            string s = musicStoreContext.Orders.OrderByDescending(x => x.OrderId).Select(x => x.OrderId).First().ToString();
            MessageBox.Show("Order id = "+s+" is saved!");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
