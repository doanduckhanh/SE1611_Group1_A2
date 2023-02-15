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
            genreCb.ItemsSource = listGenre;
            genreCb.DisplayMemberPath = "Name";
            genreCb.SelectedValuePath = "GenreId";
            genreCb.SelectedIndex = 0;
            if (albumPaging.PageIndex == 1)
            {
                previousBt.IsEnabled = false;
            }
            Album_Load(GetAlbums(1, 4));
        }

        private void Album_Load(PaginatedList<Album> list)
        {
            int i = 0;
            foreach (var item in list)
            {
                Grid grid = new Grid();
                TextBlock textBlock = new TextBlock();
                Image image = new Image();
                Button button = new Button();
                RowDefinition rowDef1 = new RowDefinition() { Height = new GridLength(16.0, GridUnitType.Star) };
                RowDefinition rowDef2 = new RowDefinition() { Height = new GridLength(66.0, GridUnitType.Star) };
                RowDefinition rowDef3 = new RowDefinition() { Height = new GridLength(16.0, GridUnitType.Star) };
                ColumnDefinition colDef1 = new ColumnDefinition() { Width = new GridLength(18.0, GridUnitType.Star) };
                ColumnDefinition colDef2 = new ColumnDefinition() { Width = new GridLength(64.0, GridUnitType.Star) };
                ColumnDefinition colDef3 = new ColumnDefinition() { Width = new GridLength(18.0, GridUnitType.Star) };
                grid.RowDefinitions.Add(rowDef1);
                grid.RowDefinitions.Add(rowDef2);
                grid.RowDefinitions.Add(rowDef3);
                grid.ColumnDefinitions.Add(colDef1);
                grid.ColumnDefinitions.Add(colDef2);
                grid.ColumnDefinitions.Add(colDef3);
                grid.Height = double.NaN;
                grid.Width = double.NaN;
                grid.VerticalAlignment = VerticalAlignment.Center;
                grid.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.Text = $"{item.Title}: {item.Price} USD";
                textBlock.TextAlignment = TextAlignment.Center;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.VerticalAlignment = VerticalAlignment.Top;
                textBlock.TextWrapping = TextWrapping.NoWrap;
                textBlock.FontSize = 13;
                textBlock.Width = double.NaN;
                textBlock.Foreground = Brushes.Red;
                textBlock.FontWeight = FontWeights.SemiBold;
                textBlock.Margin = new Thickness(0,0,0,4);
                image.Margin = new Thickness(0, 10, 0, 0);
                image.Source = new BitmapImage(new Uri("yasuo.png", UriKind.Relative));
                image.Height = double.NaN;
                image.Width = double.NaN;
                button.Content = "Add to cart";
                button.Tag = item;
                button.Click += AddToCart_Click;
                button.HorizontalAlignment = HorizontalAlignment.Center;
                button.VerticalAlignment = VerticalAlignment.Center;
                button.Width = double.NaN;
                button.Margin = new Thickness(0, 10, 0, 0);
                grid.Children.Add(textBlock);
                Grid.SetRow(textBlock, 0);
                Grid.SetColumnSpan(textBlock, 3);
                grid.Children.Add(image);
                Grid.SetRow(image, 1);
                Grid.SetColumn(image, 1);
                grid.Children.Add(button);
                Grid.SetRow(button, 2);
                Grid.SetColumnSpan(button, 3);
                FGrid.Children.Add(grid);
                Grid.SetRow(grid, i == 0 ? 1 : i == 1 ? 1 : 3);
                Grid.SetColumn(grid, i == 0 ? 1 : i == 2 ? 1 : 3);
                i++;
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
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

        private void nextBt_Click(object sender, RoutedEventArgs e)
        {
            FGrid.Children.RemoveRange(5, 4);
            albumPaging = GetAlbums(albumPaging.PageIndex + 1, 4);
            if (!albumPaging.HasNextPage)
            {
                nextBt.IsEnabled = false;
            }
            else
            {
                nextBt.IsEnabled = true;
            }
            if (albumPaging.HasPreviousPage)
            {
                previousBt.IsEnabled = true;
            }
            else
            {
                previousBt.IsEnabled = false;
            }
            Album_Load(albumPaging);
        }

        private void previousBt_Click(object sender, RoutedEventArgs e)
        {
            FGrid.Children.RemoveRange(5, 4);
            albumPaging = GetAlbums(albumPaging.PageIndex - 1, 4);
            if (!albumPaging.HasNextPage)
            {
                nextBt.IsEnabled = false;
            }
            else
            {
                nextBt.IsEnabled = true;
            }
            if (albumPaging.HasPreviousPage)
            {
                previousBt.IsEnabled = true;
            }
            else
            {
                previousBt.IsEnabled = false;
            }
            Album_Load(albumPaging);
        }
        private void searchBt_Click(object sender, RoutedEventArgs e)
        {
            if ((int)genreCb.SelectedValue != 0)
                albums = context.Albums.Where(x => x.GenreId == (int)genreCb.SelectedValue).ToList();
            else
                albums = context.Albums.ToList();
            albumPaging = new PaginatedList<Album>(albums, albums.Count, 1, 4);
            FGrid.Children.RemoveRange(5, 4);
            if (albumPaging.HasNextPage)
            {
                nextBt.IsEnabled = true;
            }
            previousBt.IsEnabled = false;
            Album_Load(GetAlbums(1, 4));
        }

        public PaginatedList<Album> GetAlbums(int pageIndex, int pageSize)
        {
            return PaginatedList<Album>.Create(albums.AsQueryable<Album>(), pageIndex, pageSize);
        }
    }
}
