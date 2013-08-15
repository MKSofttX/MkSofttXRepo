using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine.Common
{
    public class NewsByTag
    {
        public string Title { get; set; }
        public string PubDate { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }

        //Default Constructor
        public NewsByTag() { }

        //Parameterize Constructor
        public NewsByTag(string title, string pubDate, string link, string description)
        {
            this.Title = title;
            this.PubDate = pubDate;
            this.Link = link;
            this.Description = description;
        }
    }
}
