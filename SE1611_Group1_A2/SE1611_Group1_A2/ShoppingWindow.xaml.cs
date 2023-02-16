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
        private static MusicStoreContext context = new MusicStoreContext();
        public ShoppingCart shoppingCart { get; set; } = ShoppingCart.GetCart();
        private PaginatedList<Album> albumPaging = new PaginatedList<Album>(context.Albums.ToList(), context.Albums.Count(), 1, 4);
        List<Album> albums = context.Albums.ToList();
        //LoginWindow login = new LoginWindow();

        public ShoppingWindow()
        {
            InitializeComponent();
            Shopping_Load();
        }

        private void Shopping_Load()
        {
            var listGenre = context.Genres.Distinct().ToList();
            listGenre.Insert(0, new Genre { GenreId = 0, Name = "All" });
            cbGenre.ItemsSource = listGenre;
            cbGenre.DisplayMemberPath = "Name";
            cbGenre.SelectedValuePath = "GenreId";
            cbGenre.SelectedIndex = 0;
            if (albumPaging.PageIndex == 1)
            {
                btnPrevious.IsEnabled = false;
            }
            Album_Load(GetAlbums(1, 4));
        }

        private void Album_Load(PaginatedList<Album> list)
        {
            int i = 0;
            foreach (var item in list)
            {              
                StackPanel stackPanel = new StackPanel();
                stackPanel.HorizontalAlignment = HorizontalAlignment.Center;
                stackPanel.Orientation = Orientation.Vertical;
                Label label = new Label();
                Image image = new Image();
                Button button = new Button();
                label.Content = item.Title +": "+item.Price+ " USD";
                label.Foreground = Brushes.Red;
                label.FontSize = 15;
                label.HorizontalAlignment = HorizontalAlignment.Center;   
                image.Source = new BitmapImage(new Uri(item.AlbumUrl,UriKind.Relative));
                image.Width = 180;
                button.Content = "Add to cart";
                button.Tag = item;
                button.Click += btnAddToCart_Click;
                button.HorizontalAlignment = HorizontalAlignment.Center;
                button.VerticalAlignment = VerticalAlignment.Center;
                button.Width = double.NaN;
                button.Margin = new Thickness(0, 10, 0, 0);
                stackPanel.Children.Add(label);
                stackPanel.Children.Add(image);
                stackPanel.Children.Add(button);
                Fgrid.Children.Add(stackPanel);
                if (i == 0)
                {
                    Grid.SetRow(stackPanel, 0);
                    Grid.SetColumn(stackPanel, 0);
                }
                else if (i == 1)
                {
                    Grid.SetRow(stackPanel, 0);
                    Grid.SetColumn(stackPanel, 1);
                }
                else if (i == 2)
                {
                    Grid.SetRow(stackPanel, 1);
                    Grid.SetColumn(stackPanel, 0);
                }
                else if (i == 3)
                {
                    Grid.SetRow(stackPanel, 1);
                    Grid.SetColumn(stackPanel, 1);
                }
                i++;
            }
        }

        private void btnAddToCart_Click(object sender, RoutedEventArgs e)
        {
            //if (login.UserId != -1)
            //{
            shoppingCart.AddToCart((sender as Button).Tag as Album);
            //CartWindow cartWindow = new CartWindow();
            //cartWindow.Show();
            //}
            //else
            //{
            //login.Show();
            //if (login.UserId != -1)
            //{
            //login.Close();
            //}
            //}
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            Fgrid.Children.Clear();
            albumPaging = GetAlbums(albumPaging.PageIndex + 1, 4);
            if (!albumPaging.HasNextPage)
            {
                btnNext.IsEnabled = false;
            }
            else
            {
                btnNext.IsEnabled = true;
            }
            if (albumPaging.HasPreviousPage)
            {
                btnPrevious.IsEnabled = true;
            }
            else
            {
                btnPrevious.IsEnabled = false;
            }
            Album_Load(albumPaging);
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            Fgrid.Children.Clear();
            albumPaging = GetAlbums(albumPaging.PageIndex - 1, 4);
            if (!albumPaging.HasNextPage)
            {
                btnNext.IsEnabled = false;
            }
            else
            {
                btnNext.IsEnabled = true;
            }
            if (albumPaging.HasPreviousPage)
            {
                btnPrevious.IsEnabled = true;
            }
            else
            {
                btnPrevious.IsEnabled = false;
            }
            Album_Load(albumPaging);
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if ((int)cbGenre.SelectedValue != 0)
                albums = context.Albums.Where(x => x.GenreId == (int)cbGenre.SelectedValue).ToList();
            else
                albums = context.Albums.ToList();
            albumPaging = new PaginatedList<Album>(albums, albums.Count, 1, 4);
            Fgrid.Children.Clear();
            if (albumPaging.HasNextPage)
            {
                btnNext.IsEnabled = true;
            }
            btnPrevious.IsEnabled = false;
            Album_Load(GetAlbums(1, 4));
        }

        public PaginatedList<Album> GetAlbums(int pageIndex, int pageSize)
        {
            return PaginatedList<Album>.Create(albums.AsQueryable<Album>(), pageIndex, pageSize);
        }


    }
}
