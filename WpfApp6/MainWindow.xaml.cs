using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using WpfApp6.Models;

namespace WpfApp6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // la collection rafraichie mais c'est la InotifyPropertyChanged qui rafraichie les autres propriété comme les booleen 

        // chaque fois qu'on on ajoute ou supprime un element ca rafraichie
        public ObservableCollection<RssItem> RssItems { get; set; } = new ObservableCollection<RssItem>();

        private bool _isLoading;

        // l'ampoule permet de creer l'evenement
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                // a chaque fois que ca change on declanche propeety changed avec le bool en parametres 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLoading)));
            }
        }


        public MainWindow()
        {
            // le constructeur
            // lie le design au code
            InitializeComponent();
            DataContext = this;
        }

        // c'est la fonction qu'on à recuperer dans demoXML
        private async Task<List<RssItem>> GetRss(string url)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var xml = await response.Content.ReadAsStringAsync();
                return XDocument.Parse(xml)
                    .Root
                    .Element("channel")
                    .Elements("item")
                    .Select(x => new RssItem
                    {
                        Title = x.Element("title")?.Value,
                        Description = x.Element("description")?.Value,
                        ImageUrl = x.Element("enclosure")?.Attribute("url")?.Value
                    })
                    .ToList();
            }
            return null;
        }

        // permet de ne pas mettre tout le code dans le consturcteur 
        // on met cette parametre dans le xaml du designe partie title 
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();

        }

        private void Click(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private async void Refresh()
        {
            // ETAPES: recupere mon liste, je boucle sur le collection ou liste, je les ajoute à l'observable collection 
            // ensuite comme on a binder sur RSSItem on peut binder sur chaque element
            // Data Temlplate permet de reorganisage l'affichage de chaque item


            IsLoading = true; // on le met avant lancer le flux a true
            RssItems.Clear();
            var items = await GetRss("https://www.lemonde.fr/rss/une.xml");
            foreach (var item in items)
            {
                RssItems.Add(item);
            }
            IsLoading = false; // quand on a fini on la passe a false
        }
    }
}
