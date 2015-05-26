using System;
using System.Linq;
using ASEntityFramework;
using HtmlAgilityPack;
using LunchboxSource.Business.Encryption;
using LunchboxSource.Business.Http;

namespace AuctionSniperDLL.Business.Sites
{
    public class DomCop : HttpBase
    {

        public bool Login(string username, string password)
        {
            const string url = "https://www.domcop.com/login";
            Get(url);

            var postData = string.Format("email={0}&password={1}", username, password);
            var data = Post(url, postData);
            return data.Contains("Logout");
        }

        public void Logout()
        {
            Get("https://www.domcop.com/signOut");
        }

        public void FixEfProviderServicesProblem()
        {
            //The Entity Framework provider type 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer'
            //for the 'System.Data.SqlClient' ADO.NET provider could not be loaded. 
            //Make sure the provider assembly is available to the running application. 
            //See http://go.microsoft.com/fwlink/?LinkId=260882 for more information.

            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }

        private int GetNumbers(string input)
        {
            if (input.ToLower().Contains("k"))
            {
                return int.Parse(Math.Floor(Convert.ToDouble(input.ToLower().Replace("k", "").Replace("$", "").Trim()) * 1000).ToString());
            }
            if (input.ToLower().Contains("m"))
            {
                return int.Parse(Math.Floor(Convert.ToDouble(input.ToLower().Replace("m", "").Replace("$", "").Trim()) * 1000).ToString());
            }
            return TryParse_INT(input.Replace("$", "").Trim());
            //return new string(input.Where(char.IsDigit).ToArray());
        }

        private decimal GetDecimal(string input)
        {
            if (input.ToLower().Contains("k"))
            {
                return Decimal.Parse((Convert.ToDouble(input.ToLower().Replace("k", "").Replace("$", "").Trim()) * 1000).ToString());
            }
            if (input.ToLower().Contains("m"))
            {
                return Decimal.Parse((Convert.ToDouble(input.ToLower().Replace("m", "").Replace("$", "").Trim()) * 1000).ToString());
            }
            return Decimal.Parse(input.Replace("$", "").Trim());
            //return new string(input.Where(char.IsDigit).ToArray());
        }

        private DateTime ExtractEndDate(string value)
        {
            var items = value.Split(new char[] {' '});
            var datetime = DateTime.Now;
            foreach (var item in items)
            {
                if (item.Contains("d"))
                {
                    datetime = datetime.AddDays(int.Parse(item.Replace("d", "").Trim()));
                }
                if (item.Contains("h"))
                {
                    datetime = datetime.AddHours(int.Parse(item.Replace("h", "").Trim()));
                }
                if (item.Contains("m"))
                {
                    datetime = datetime.AddMinutes(int.Parse(item.Replace("m", "").Trim()));
                }
            }

            return datetime;

        }

        private DateTime GenerateEstimateEnd(HtmlNode node)
        {
            var estimateEnd = DateTime.Now;
            if (node.InnerText != null)
            {
                var vals = HTMLDecode(node.InnerText).Trim().Split(new[] { ' ' });

                foreach (var item in vals)
                {
                    if (item.Contains("D"))
                    {
                        estimateEnd = estimateEnd.AddDays(double.Parse(item.Replace("D", "")));
                    }
                    else if (item.Contains("H"))
                    {
                        estimateEnd = estimateEnd.AddHours(double.Parse(item.Replace("H", "")));
                    }
                    else if (item.Contains("M"))
                    {
                        estimateEnd = estimateEnd.AddMinutes(double.Parse(item.Replace("M", "")));
                    }
                }

            }

            return estimateEnd;
        }

        public void Search(Guid userid)
        {
            const string url = "https://www.domcop.com/getDomains.php";
            using (var ds = new ASEntities())
            {
                var godaddyaccount = ds.GoDaddyAccount.First(x => x.UserID == userid);
                ds.USP_SearchQueryBuilder2(userid.ToString());
                var data = "&page_rank_upper={0}&page_rank_lower={1}&fake_pr=false&require_pr=false&fake_alexa=false&require_semrush_keywords=false&require_semrush_traffic=false&adv_age_lower=0&domain_authority_upper=999&domain_authority_lower=0&page_authority_upper=999&page_authority_lower=0&citation_flow_upper=999&citation_flow_lower=0&trust_flow_upper=999&trust_flow_lower=0&maj_back_links_upper=99999&maj_back_links_lower=0&maj_ref_ips_upper=99999&maj_ref_ips_lower=0&maj_million=false&quantcast_million=false&alexa_rank=false&require_semrush_rank=false&require_similarweb_rank=false&require_available=false&hide_adult=false&hide_spammy=false&hide_brand=false&soc_fb_like_upper=99999&soc_fb_like_lower=0&soc_fb_share_upper=99999&soc_fb_share_lower=0&soc_twitter_upper=99999&soc_twitter_lower=0&soc_reditt_upper=99999&soc_reditt_lower=0&soc_pinterest_upper=99999&soc_pinterest_lower=0&soc_google_upper=99999&soc_google_lower=0&ends_in_lower=0&ends_in_upper=14&parent_category=&child_category=&parent_topical_category=&child_topical_category=&google_index=false&dmoz=false&allow_dashes=true&allow_digits=true&only_digits=false&saletype_auction=true&saletype_buynow=true&saletype_closeout=true&saletype_pending=true&saletype_offer=true&saletype_bargain=true&search_type=keyword&keyword=&keyword_search_type=contains&pattern=&patternType=&ext_6=true&ext_17=true&ext_23=true&ext_18=true&ext_5=true&ext_10=true&ext_25=true&ext_16=true&ext_27=true&ext_1=true&ext_8=true&ext_21=true&ext_11=true&ext_32=true&ext_19=true&ext_4=true&ext_15=true&ext_28=true&ext_7=true&ext_22=true&ext_2=true&ext_24=true&ext_12=true&ext_3=true&ext_31=true&ext_20=true&ext_13=true&ext_29=true&ext_26=true&ext_14=true&ext_9=true&ext_misc=true&type_godaddy=true&type_namejet=true&type_snapnames=true&type_sedo=true&type_dynadot=true&type_droplists=true";
                var searchString = "sEcho=2&iColumns=17&sColumns=&iDisplayStart=0&iDisplayLength=125&mDataProp_0=0&mDataProp_1=1&mDataProp_2=2&mDataProp_3=3&mDataProp_4=4&mDataProp_5=5&mDataProp_6=6&mDataProp_7=7&mDataProp_8=8&mDataProp_9=9&mDataProp_10=10&mDataProp_11=11&mDataProp_12=12&mDataProp_13=13&mDataProp_14=14&mDataProp_15=15&mDataProp_16=16&iSortingCols=0&bSortable_0=false&bSortable_1=true&bSortable_2=true&bSortable_3=true&bSortable_4=true&bSortable_5=true&bSortable_6=true&bSortable_7=true&bSortable_8=true&bSortable_9=true&bSortable_10=true&bSortable_11=true&bSortable_12=true&bSortable_13=true&bSortable_14=true&bSortable_15=true&bSortable_16=true&screen_width=1349&data_source=live";
                var res = ds.SearchTable.Where(x => x.UserID == userid && x.ItemOrder < 67).OrderBy(x => x.ItemOrder);
                foreach (var item in res)
                {
                    searchString += item.SearchValue + (item.DataType == "bit" ? item.Value == "1" ? "true" : "false" : item.Value);
                }

                var res2 = ds.SearchTable.Where(x => x.UserID == userid && x.ItemOrder >= 67).OrderBy(x => x.ItemOrder);
                foreach (var item in res2)
                {
                    searchString += item.SearchValue + (item.DataType == "bit" ? item.Value == "1" ? "true" : "false" : item.Value);
                }

                var results = Post(url, searchString);

                var json = JSONSerializer.DeserializeDynamic(results.Trim());

                foreach (var site in json["aaData"])
                {
                    var adv = new AdvSearch{UserID = userid};
                    try
                    {
                        var moo = site["1"];
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    var content = (string)site["1"].ToString();
                    var hdoc = HtmlDocument((string)content);
                    adv.DomainName = QuerySelector(hdoc.DocumentNode, "span[class='big-domain'] > a").Attributes["alt"].Value;
                    
                    if (content.Contains("godaddy"))
                    {
                        adv.DomainLink = "https://au.auctions.godaddy.com/trpItemListing.aspx?domain=" + adv.DomainName;//QuerySelector(hdoc.DocumentNode, "span[class='big-domain'] > a").Attributes["href"].Value;
                        adv.IsGOdaddy = true;
                        adv.IsAuction = true; //for now.. now way to tell
                    }
                    else if (content.Contains("namejet"))
                    {
                        adv.DomainLink = "http://www.namejet.com/Pages/Auctions/BackorderDetails.aspx?domainname=" + adv.DomainName;
                        adv.IsAuction = true;
                    }
                    else if (content.Contains("dynadot.com"))
                    {
                        adv.DomainLink = "https://www.dynadot.com/market/backorder/" + adv.DomainName;
                        adv.IsAuction = false;
                    }
                    else if (content.Contains("snapnames"))
                    {
                        adv.DomainLink = string.Format("https://www.snapnames.com/domain/{0}.action", adv.DomainName);
                        adv.IsAuction = false;
                    }
                    else if (content.Contains("sedo"))
                    {
                        adv.DomainLink = "http://sedo.com/search/details.php4?domain=" + adv.DomainName;
                        adv.IsAuction = true;
                    }

                    content = site["0"];
                    hdoc = HtmlDocument((string)content);
                    adv.Ref = QuerySelector(hdoc.DocumentNode, "input").Attributes["id"].Value.Split(new []{'|'}).First();
 
                    content = site["2"].ToString();
                    int defval = 0;
                    hdoc = HtmlDocument((string)content);
                    adv.Age = int.Parse(QuerySelector(hdoc.DocumentNode, "b") == null ? "0" : QuerySelector(hdoc.DocumentNode, "b").InnerText.Contains("-") ? "0" : QuerySelector(hdoc.DocumentNode, "b").InnerText);

                    content = site["3"].ToString();
                    hdoc = HtmlDocument((string)content);
                    adv.PageRank = int.Parse(QuerySelector(hdoc.DocumentNode, "span[class='big-text-pr']") == null ? "0" : QuerySelector(hdoc.DocumentNode, "span[class='big-text-pr']").InnerText);

                    content = site["4"].ToString();
                    hdoc = HtmlDocument((string)content);
                    adv.MOZDA = int.Parse(QuerySelector(hdoc.DocumentNode, "a") == null ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText.Contains("-") ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText);

                    content = site["5"].ToString();
                    hdoc = HtmlDocument((string)content);
                    adv.MOZPA = int.Parse(QuerySelector(hdoc.DocumentNode, "a") == null ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText.Contains("-") ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText);

                    //Needs to be decimal
                    content = site["6"].ToString();
                    hdoc = HtmlDocument((string)content);
                    adv.MozRank = GetDecimal(QuerySelector(hdoc.DocumentNode, "a") == null ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText.Contains("-") ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText);

                    //Needs to be decimal
                    content = site["7"].ToString();
                    hdoc = HtmlDocument((string)content);
                    adv.MozTrust = GetDecimal(QuerySelector(hdoc.DocumentNode, "a") == null ? "0.00" : QuerySelector(hdoc.DocumentNode, "a").InnerText.Contains("-") ? "0.00" : QuerySelector(hdoc.DocumentNode, "a").InnerText);

                    content = site["8"].ToString();
                    hdoc = HtmlDocument((string)content);
                    adv.CF = int.Parse(QuerySelector(hdoc.DocumentNode, "a") == null ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText.Contains("-") ? "0" :  QuerySelector(hdoc.DocumentNode, "a").InnerText);

                    content = site["9"].ToString();
                    hdoc = HtmlDocument((string)content);
                    adv.TF = int.Parse(QuerySelector(hdoc.DocumentNode, "a") == null ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText.Contains("-") ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText);

                    content = site["9"].ToString();
                    hdoc = HtmlDocument((string)content);
                    adv.TF = int.Parse(QuerySelector(hdoc.DocumentNode, "a") == null ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText.Contains("-") ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText);

                    //Needs to be decimal - maybe
                    content = site["10"].ToString();
                    hdoc = HtmlDocument((string)content);
                    adv.AlexARank = GetDecimal((QuerySelector(hdoc.DocumentNode, "a") == null || QuerySelector(hdoc.DocumentNode, "a").InnerText == "-") ? "0.00" : QuerySelector(hdoc.DocumentNode, "a").InnerText);

                    content = site["11"].ToString();
                    hdoc = HtmlDocument((string)content);
                    adv.MozDom = GetNumbers((QuerySelector(hdoc.DocumentNode, "a") == null || QuerySelector(hdoc.DocumentNode, "a").InnerText == "-") ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText);

                    content = site["12"].ToString();
                    hdoc = HtmlDocument((string)content);
                    adv.MajDom = GetNumbers(QuerySelector(hdoc.DocumentNode, "a") == null ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText.Contains("-") ? "0" : QuerySelector(hdoc.DocumentNode, "a").InnerText);

                    content = site["15"].ToString();
                    adv.DomainPrice = GetNumbers(content);

                    content = (string)HTMLDecode(site["16"].ToString());
                    if (((string)content).Contains("<"))
                    {
                        content = ((string) content).Split(new [] {'<'}).First().Trim();
                    }
                    adv.EsitmateEnd = ExtractEndDate(content);

                    var auc = new AuctionSearch
                    {
                        AuctionRef = adv.Ref,
                        DomainName = adv.DomainName,
                        EstimateEndDate = (DateTime)adv.EsitmateEnd,
                        EndDate =  (DateTime)adv.EsitmateEnd,
                        MinBid = adv.DomainPrice ?? 0,
                        AccountID = godaddyaccount.AccountID
                    };
                    ds.AuctionSearch.Add(auc);
                    ds.SaveChanges();
                    ds.AdvSearch.Add(adv);
                    ds.SaveChanges();
                }
                
                //json["aaData"][0]["1"]
            }
        }


    }
}
