using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp6.Models
{
    public class RssItem
    {
        // la classe que j'ai recuperer dans le Demo XML
        // pour  ne pas le mettre au milieu on le met dans un repertoire 
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public override string ToString() => $"{Title}: {Description}";
    }
}
