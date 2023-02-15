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
    /// Interaction logic for AlbumWindow.xaml
    /// </summary>
    public partial class AlbumWindow : Window
    {
        List<Album> albums;
        List<Genre> genres;
        List<Artist> artists;
        public AlbumWindow()
        {
            InitializeComponent();
            LoadList();
        }
        private void LoadList()
        {
            using (var context = new MusicStoreContext())
            {
                albums = context.Albums.ToList();
                genres = context.Genres.ToList();
                artists = context.Artists.ToList();
            }
            lvList.ItemsSource = albums;
            cbGenre.ItemsSource = genres;
            cbArtist.ItemsSource = artists;
        }
        private Album getAlbum()
        {
            try
            {
                Album album = new Album();
                if (cbGenre.SelectedValue != null && cbArtist.SelectedValue != null)
                {
                    album.GenreId = int.Parse(cbGenre.SelectedValue.ToString());
                    album.ArtistId = int.Parse(cbArtist.SelectedValue.ToString());
                    if (txtTitle.Text.Length > 0 && txtPrice.Text.Length > 0)
                    {
                        album.Title = txtTitle.Text;
                        album.Price = decimal.Parse(txtPrice.Text);
                        album.AlbumUrl = txtAlbumUrl.Text;
                        return album;
                    }
                    else
                    {
                        throw new Exception("Album can't has empty informations!");
                    }
                }
                else
                {
                    throw new Exception("Genre or Artist is not selected!");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Album album = new Album();
                album = getAlbum();
                if (album != null)
                {
                    using (var context = new MusicStoreContext())
                    {
                        context.Albums.Add(album);
                        context.SaveChanges();
                        LoadList();
                        MessageBox.Show("A new album is added");
                    }
                }
                else
                {
                    throw new Exception("Album is not added");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Album album = new Album();
                if (getAlbum() != null)
                {
                    album = getAlbum();
                    album.AlbumId = int.Parse(txtAlbumId.Text);
                    using (var context = new MusicStoreContext())
                    {
                        context.Albums.Update(album);
                        context.SaveChanges();
                        LoadList();
                        MessageBox.Show("That album is updated");
                    }
                }
                else
                {
                    throw new Exception("Album is not updated!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Album album = new Album();
                album.AlbumId = int.Parse(txtAlbumId.Text);
                if (MessageBox.Show("you sure want to Delete?", "Confirm!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                {
                    MessageBox.Show("Canceled");
                }
                else
                {
                    using (var context = new MusicStoreContext())
                    {
                        context.Remove(album);
                        context.SaveChanges();
                        LoadList();
                        MessageBox.Show("That album is deleted!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAlbumUrl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".png";
                Nullable<bool> result = dlg.ShowDialog();
                if (result == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                    if (filename.Contains(@"\Images"))
                    {
                        string s = filename.Substring(filename.IndexOf(@"\Images"));
                        string sw = s.Replace(@"\", "/");
                        txtAlbumUrl.Text = sw;
                    }
                    else
                    {
                        throw new Exception("Invalid select");
                    }
                    
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
