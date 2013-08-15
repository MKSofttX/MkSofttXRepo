using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyRecyclingApp.Resources;
using System.Xml.Linq;
using MyRecyclingApp.Common;
using System.Xml;
using System.Windows.Media;
using Microsoft.Phone.Tasks;
using SearchEngine.Common;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;

namespace MyRecyclingApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        const string FileNameNewsByTag = "MyNewsByTag.xml";
        const string FileNameFavorites = "MyFavorites.xml";
        ObservableCollection<NewsByTag> _listNewsByTag;
        ObservableCollection<News> _listNews;
        ObservableCollection<NewsByTag> _listFavorites;
        private bool _bAddToFavorites = false;
        private string _sTag = string.Empty;

        private string _defaultIdiom = string.Empty;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SystemTray.SetForegroundColor(this, Colors.Green);
            _listFavorites = new ObservableCollection<NewsByTag>();

            //getIdiomToFilter();
            LoadNewsByTag();
            LoadFavorites();
            LoadNews();
        }

        #region Methods
        private async void LoadNews()
        {
            try
            {
                DeliciousUtility objDelUtility = new DeliciousUtility();
                _listNews = await objDelUtility.getScoopItNews();

                this.listNews2.ItemsSource = _listNews;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }

        private void LoadNewsByTag()
        {
            try
            {
                _listNewsByTag = SerializerDeserializerHelper.Deserialize(FileNameNewsByTag);

                if (_listNewsByTag != null && _listNewsByTag.Count > 0)
                    this.listNews.ItemsSource = _listNewsByTag;
                else
                    MessageBox.Show("You do not have any Recyword. Start searching for one ...", "INFO", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when loading News by tag. Error: " + ex.Message, "ERROR", MessageBoxButton.OK);
            }
        }

        private void LoadFavorites()
        {
            try
            {
                _listFavorites = SerializerDeserializerHelper.Deserialize(FileNameFavorites);
                this.listFavorites.ItemsSource = _listFavorites;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error when loading favorites. Error: " + ex.Message, "ERROR", MessageBoxButton.OK);
            }
        }

        private void GetIdiomToFilter()
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("defaultIdiom"))
            {
                _defaultIdiom = Convert.ToString(IsolatedStorageSettings.ApplicationSettings["defaultIdiom"]);
            }
            else
            {
                IsolatedStorageSettings.ApplicationSettings["defaultIdiom"] = "en-US";
                _defaultIdiom = "en-US";
            }
        }
        #endregion

        #region Events
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        /// <summary>
        /// Evento para realizar la busqueda por el Recyword
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtTag.Text.Trim()))
            {
                try
                {
                    _sTag = this.txtTag.Text.Trim();
                    DeliciousUtility objDelUtility = new DeliciousUtility();
                    this.progressIndicator.IsVisible = true;
                    _listNewsByTag = await objDelUtility.getDeliciousFeedsByTag(_sTag);

                    if (_listNewsByTag != null || _listNewsByTag.Count > 0)
                    {
                        this.listNews.ItemsSource = _listNewsByTag;
                        this.progressIndicator.IsVisible = false;

                        SerializerDeserializerHelper.SerializerNewsByTag(_listNewsByTag, FileNameNewsByTag);
                    }
                    else
                        MessageBox.Show("No se encontro el Recyword", "INFO", MessageBoxButton.OK);

                    #region OLD CODE
                    //    WebClient client = new WebClient();

                    //    client.DownloadStringCompleted += (s, a) =>
                    //    {
                    //        if (a.Error == null && !a.Cancelled)
                    //        {
                    //            var result = a.Result;
                    //            //LINQ to XML
                    //            var doc = XDocument.Parse(result);
                    //            //var query = from q in doc.Element("channel").Elements("item")
                    //            //            select new NewsByTag()
                    //            //            {
                    //            //                Title = (string)q.Element("title")
                    //            //            };

                    //            //var query2 = doc.Element("channel").Element("item");
                    //            using (XmlReader reader = doc.CreateReader())
                    //            {
                    //                SyndicationFeed feed = SyndicationFeed.Load(reader);
                    //                listNewsByTag = new List<NewsByTag>();

                    //                foreach (var RecyItem in feed.Items)
                    //                {
                    //                    NewsByTag objNewsByTag = new NewsByTag();
                    //                    objNewsByTag.Title = RecyItem.Title.Text;
                    //                    objNewsByTag.PubDate = RecyItem.PublishDate.LocalDateTime.ToString();
                    //                    objNewsByTag.Link = RecyItem.Links[0].Uri.OriginalString;
                    //                    objNewsByTag.Description = RecyItem.Summary.Text;

                    //                    listNewsByTag.Add(objNewsByTag);
                    //                }

                    //                this.listNews.ItemsSource = listNewsByTag;
                    //                //this.listNews2.ItemsSource = listNewsByTag;

                    //                this.progressIndicator.IsVisible = false;
                    //            }
                    //        }
                    //    };

                    //    client.DownloadStringAsync(new Uri(sUri, UriKind.Absolute));
                    //    this.progressIndicator.IsVisible = true;
                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when trying to make the call in the webClient. Error: " + ex.Message, "Error...", MessageBoxButton.OK);
                }
            }
            else
                MessageBox.Show("Debes escribir el Recyword a buscar.", "INFO", MessageBoxButton.OK);
        }

        private void listNews_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_bAddToFavorites)
            {
                NewsByTag objNewByTag = this.listNews.SelectedItem as NewsByTag;

                if (objNewByTag != null)
                {
                    WebBrowserTask webBrowserTask = new WebBrowserTask();
                    webBrowserTask.Uri = new Uri(objNewByTag.Link);
                    webBrowserTask.Show();
                }
            }
            else
            {
                try
                {
                    NewsByTag objNewByTag = this.listNews.SelectedItem as NewsByTag;
                    _listFavorites.Add(objNewByTag);

                    SerializerDeserializerHelper.SerializerFavorites(_listFavorites, FileNameFavorites);
                    this.listFavorites.ItemsSource = _listFavorites;

                    MessageBox.Show("The News has added to the favorite list.", "INFO", MessageBoxButton.OK);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error at saving to favorites. Error: " + ex.Message, "ERROR", MessageBoxButton.OK);
                }

                _bAddToFavorites = false;
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/Configuration.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddFavorites_Click(object sender, RoutedEventArgs e)
        {
            _bAddToFavorites = true;

            object clicked = (e.OriginalSource as FrameworkElement).DataContext;
            var lbi = this.listNews.ItemContainerGenerator.ContainerFromItem(clicked) as ListBoxItem;
            lbi.IsSelected = true;

        }

        private void panoramaControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int option = ((Panorama)sender).SelectedIndex;

            switch (option)
            {
                case 0:
                    //
                    break;
                case 1:
                    //
                    break;
                case 2:
                    //
                    break;
                default:
                    break;
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_sTag))
            {
                try
                {
                    _sTag = this.txtTag.Text.Trim();
                    DeliciousUtility objDelUtility = new DeliciousUtility();
                    this.progressIndicator.IsVisible = true;
                    _listNewsByTag = await objDelUtility.getDeliciousFeedsByTag(_sTag);

                    this.listNews.ItemsSource = _listNewsByTag;
                    this.progressIndicator.IsVisible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when trying to make the call in the webClient. Error: " + ex.Message, "Error...", MessageBoxButton.OK);
                }
            }
            else
                MessageBox.Show("You must first specified a Recyword.", "INFO", MessageBoxButton.OK);
        }

        private void listNews2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            News objNew = this.listNews2.SelectedItem as News;

            if (objNew != null)
            {
                WebBrowserTask webBrowserTask = new WebBrowserTask();
                webBrowserTask.Uri = new Uri(objNew.Link);
                webBrowserTask.Show();
            }
        }
        #endregion

    }
}