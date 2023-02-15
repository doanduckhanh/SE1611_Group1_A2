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
using

namespace SE1611_Group1_A2
{
    /// <summary>
    /// Interaction logic for ShoppingWindow.xaml
    /// </summary>
    public partial class ShoppingWindow : Window
    {
        MusicStoreContext musicStoreContext = new MusicStoreContext();
        int pageIndex;
        int idAlbum1;
        public ShoppingWindow()
        {
            InitializeComponent();
            cbGenre.ItemsSource = musicStoreContext.Genres.ToList();
            tbPage.Visibility = Visibility.Hidden;
            pageIndex = int.Parse(tbPage.Text);
            LoadShop(pageIndex, 0);
            if (pageIndex == 1)
            {
                btnPrevious.IsEnabled = false;
            }
            else
            {
                btnPrevious.IsEnabled = true;
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadShop(1, int.Parse(cbGenre.SelectedValue.ToString()));
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            pageIndex = pageIndex - 1;
            if(cbGenre.SelectedValue != null) {
                LoadShop(pageIndex, int.Parse(cbGenre.SelectedValue.ToString()));
            }
            else
            {
                LoadShop(pageIndex, 0);
            }
            
            if (pageIndex == 1)
            {
                btnPrevious.IsEnabled = false;
            }
            else
            {
                btnPrevious.IsEnabled = true;
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            pageIndex = pageIndex + 1;
            if (cbGenre.SelectedValue != null)
            {
                LoadShop(pageIndex, int.Parse(cbGenre.SelectedValue.ToString()));
            }
            else
            {
                LoadShop(pageIndex, 0);
            }
            if (pageIndex == 1)
            {
                btnPrevious.IsEnabled = false;
            }
            else
            {
                btnPrevious.IsEnabled = true;
            }
        }

        void LoadShop(int pageIndex, int genreid)
        {
            
        }

        private void btAddToCart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
