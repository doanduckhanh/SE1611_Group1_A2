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
    /// Interaction logic for ShoppingWindow.xaml
    /// </summary>
    public partial class ShoppingWindow : Window
    {
        MusicStoreContext musicStoreContext = new MusicStoreContext();
        int pageIndex;
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
            if(genreid == 0)
            {
                List<Album> albums = new List<Album>();
                albums = musicStoreContext.Albums.Skip(4*pageIndex).Take(4).ToList();
                if(albums[0] != null) {
                    lbTitle1.Content = albums[0].Title;
                    lbPrice1.Content = albums[0].Price.ToString() + " USD";
                } else
                {
                    pnAlbum1.Visibility = Visibility.Hidden;
                }
                if (albums[1] != null)
                {
                    lbTitle2.Content = albums[1].Title;
                    lbPrice2.Content = albums[1].Price.ToString() + " USD";
                }
                else
                {
                    pnAlbum2.Visibility = Visibility.Hidden;
                }
                if (albums[2] != null)
                {
                    lbTitle3.Content = albums[2].Title;
                    lbPrice3.Content = albums[2].Price.ToString() + " USD";
                }
                else
                {
                    pnAlbum3.Visibility = Visibility.Hidden;
                }
                if (albums[0] != null)
                {
                    lbTitle4.Content = albums[3].Title;
                    lbPrice4.Content = albums[3].Price.ToString() + " USD";
                }
                else
                {
                    pnAlbum4.Visibility = Visibility.Hidden;
                }

            } else
            {
                List<Album> albums = new List<Album>();
                albums = musicStoreContext.Albums.Where(x => x.GenreId == genreid).Skip(4 * pageIndex).Take(4).ToList();
                if (albums[0] != null)
                {
                    lbTitle1.Content = albums[0].Title;
                    lbPrice1.Content = albums[0].Price.ToString() + " USD";
                }
                else
                {
                    pnAlbum1.Visibility = Visibility.Hidden;
                }
                if (albums[1] != null)
                {
                    lbTitle2.Content = albums[1].Title;
                    lbPrice2.Content = albums[1].Price.ToString() + " USD";
                }
                else
                {
                    pnAlbum2.Visibility = Visibility.Hidden;
                }
                if (albums[2] != null)
                {
                    lbTitle3.Content = albums[2].Title;
                    lbPrice3.Content = albums[2].Price.ToString() + " USD";
                }
                else
                {
                    pnAlbum3.Visibility = Visibility.Hidden;
                }
                if (albums[0] != null)
                {
                    lbTitle4.Content = albums[3].Title;
                    lbPrice4.Content = albums[3].Price.ToString() + " USD";
                }
                else
                {
                    pnAlbum4.Visibility = Visibility.Hidden;
                }
            }
        }

        private void btAddToCart_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
