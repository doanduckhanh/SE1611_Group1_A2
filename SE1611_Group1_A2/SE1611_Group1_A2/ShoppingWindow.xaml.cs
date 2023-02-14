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
        int album1;

        public string ShoppingCartId { get; set; }
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
            //check login và đổ items vào acc
            //if(UserSession.UserName != null)
            //{
            //    MigrateCart();
            //}

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
                    album1 = albums[0].AlbumId;
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
            var cart = GetCart();
            var album = musicStoreContext.Albums.SingleOrDefault(x => x.AlbumId == album1);
            AddToCart(album, cart.ShoppingCartId);
            CartWindow cartWindow = new CartWindow();
            cartWindow.Show();
        }

        public static ShoppingWindow GetCart()
        {
            var cart = new ShoppingWindow();
            cart.ShoppingCartId = cart.GetCartId();
            return cart;
        }

        public string GetCartId()
        {
            if (Settings.Default["CartId"].ToString().Equals(string.Empty))
            {
                if (UserSession.UserName != null)
                    Settings.Default["CartId"] = UserSession.UserName;
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    Settings.Default["CartId"] = tempCartId.ToString();
                }
                Settings.Default.Save();
            }
            return Settings.Default["CartId"].ToString();
        }
        public void AddToCart(Album album,String ShoppingCartId)
        {
            // Get the matching cart and album instances
            var cartItem = musicStoreContext.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.AlbumId == album.AlbumId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    AlbumId = album.AlbumId,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                musicStoreContext.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            musicStoreContext.SaveChanges();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        //public void MigrateCart()
        //{
        //    var shoppingCart = musicStoreContext.Carts.Where(c => c.CartId == Settings.Default["CartId"].ToString());
        //    foreach (Cart item in shoppingCart)
        //    {
        //        item.CartId = UserSession.UserName;
        //    }
        //    musicStoreContext.SaveChanges();
        //    Settings.Default["CartId"] = string.Empty;
        //    Settings.Default.Save();
        //}

    }
}
