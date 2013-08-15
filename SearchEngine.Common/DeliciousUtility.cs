using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace SearchEngine.Common
{
    public class DeliciousUtility
    {
        public async Task<ObservableCollection<NewsByTag>> getDeliciousFeedsByTag(string sTag)
        {
            try
            {
                //Private links
                //string sUri = "http://previous.delicious.com/v2/rss/mksofttx?private=COI4Ljpv1F27m5wWgui3X-ZW1ME_H4yxtjthVCPd2EE=";
                //Private links by tags
                //string sTag = this.txtTag.Text.Trim();
                string sUri = string.Format("http://previous.delicious.com/v2/rss/mksofttx/{0}?private=COI4Ljpv1F27m5wWgui3X-ZW1ME_H4yxtjthVCPd2EE=", sTag);

                ObservableCollection<NewsByTag> listNewsByTag;

                HttpClient client = new HttpClient();
                var result = await client.GetStringAsync(sUri);
                var doc = XDocument.Parse(result);

                var data = doc.Element("rss").Element("channel").Elements("item").ToList();

                #region Alternatives
                //var data2 = (from d in doc.Element("rss").Element("channel").Descendants("item")
                //            select new NewsByTag()
                //            {
                //                Title = d.Element("title").Value
                //                ,Description = d.Element("descripton").Value
                //                ,Link = d.Element("link").Value
                //                ,PubDate = string.Format("{0:dd/MM/yyyy HH:mm}", d.Element("pubDate").Value)
                //            }).ToList();

                //var data3 = from d in doc.Element("rss").Element("channel").Descendants("item")
                //            select new NewsByTag()
                //            {
                //                Title = d.Element("title").Value
                //                ,Description = d.Element("descripton").Value
                //                ,Link = d.Element("link").Value
                //                ,PubDate = string.Format("{0:dd/MM/yyyy HH:mm}", d.Element("pubDate").Value)
                //            };
                #endregion

                listNewsByTag = new ObservableCollection<NewsByTag>();
                foreach (var item in data)
                {
                    NewsByTag objNewsByTag = new NewsByTag();
                    objNewsByTag.Title = item.Element("title").Value;
                    objNewsByTag.PubDate = Convert.ToDateTime(item.Element("pubDate").Value).ToString();
                    objNewsByTag.Link = item.Element("link").Value;
                    objNewsByTag.Description = item.Element("description").Value;

                    listNewsByTag.Add(objNewsByTag);
                }

                return listNewsByTag;
            }
            catch (Exception ex)
            {                    
                throw new Exception(ex.Message);
            }
        }


        public async Task<ObservableCollection<News>> getScoopItNews()
        {
            try
            {
                //string sUri = "http://www.scoop.it/t/recycling-ideas/rss.xml";
                string[] UriSources = new string[] { "http://www.scoop.it/t/recycling-ideas/rss.xml", "http://www.scoop.it/t/recycling-by-miguel18/rss.xml" };
                ObservableCollection<News> listScoopItNews = new ObservableCollection<News>();

                foreach (string source in UriSources)
                {
                    HttpClient client = new HttpClient();
                    var result = await client.GetStringAsync(source);
                    var parseResult = XDocument.Parse(result);

                    var data = parseResult.Element("rss").Element("channel").Elements("item").ToList();

                    foreach (var item in data)
                    {
                        News objNews = new News();
                        objNews.Title = item.Element("title").Value;
                        objNews.PubDate = Convert.ToDateTime(item.Element("pubDate").Value).ToString();
                        objNews.Link = item.Element("source").Attribute("url").Value;
                        //string desc = item.Element("description").Value.Replace("&lt;", "<").Replace("&gt;", ">");
                        string desc = Uri.UnescapeDataString(item.Element("description").Value);
                        objNews.Image = desc.Substring(10, desc.IndexOf("/>") - 12);
                        string desc2 = desc.Substring(desc.IndexOf("<blockquote>") + 12, desc.IndexOf("</blockquote>") - desc.IndexOf("<blockquote>") - 12);
                        string desc3 = desc2.Replace("&nbsp;", "").Replace("<br>", "\n").Replace("&amp;", "&").Replace("&apos;", "'").Replace("&qout;", "\"\"");
                        objNews.Description = desc3.Replace("&lsquo;", "‘").Replace("&rsquo;", "’");

                        listScoopItNews.Add(objNews);
                    }
                }


                return listScoopItNews;
            }
            catch (Exception ex)
            {                
                throw new Exception("ScoopIt Error: " + ex.Message);
            }
        }
    }
}
