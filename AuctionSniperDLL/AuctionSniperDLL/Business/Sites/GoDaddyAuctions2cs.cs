using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using ASEntityFramework;
using AuctionSniperService.Business;
using DeathByCaptcha;
using HtmlAgilityPack;
using LunchboxSource.Business.Http;
using Exception = System.Exception;

namespace AuctionSniperDLL.Business.Sites
{
    public class GoDaddyAuctions2Cs : HttpBase
    {
        private const int Timediff = 0;
        private string Viewstates { get; set; }

        public bool CaptchaOverload { get; set; }

        public bool Login(string username, string password, int attempNo = 0)
        {
            const string url = "https://auctions.godaddy.com";
            var responseData = Get(url);
            var key = GetSubString(responseData, "spkey=", "&");

            var loginUrl = string.Format("https://idp.godaddy.com/login.aspx?SPKey={0}", key);
            var hdoc = HtmlDocument(Get(loginUrl));
            if (QuerySelector(hdoc.DocumentNode, "img[class='LBD_CaptchaImage']") != null)
            {
                //Solve Captcha

                var captchaId =
                     QuerySelector(hdoc.DocumentNode, "input[id='LBD_VCID_idpCatpcha']").Attributes["value"]
                         .Value;
                var imagedata =
                    GetImage(QuerySelector(hdoc.DocumentNode, "img[class='LBD_CaptchaImage']").Attributes["src"].Value);

                
                try
                {
                    imagedata.Save(Path.Combine(Path.GetTempPath(), username + ".jpg"), ImageFormat.Jpeg);
                    Client client;
                    using (var ds = new ASEntities())
                    {
                        var user = ds.SystemConfig.First(x => x.PropertyID == "DBCUser").Value;
                        var pass = ds.SystemConfig.First(x => x.PropertyID == "DBCPass").Value;
                        client = new SocketClient(user, pass);
                    }
                    
                    //var balance = client.GetBalance();
                    var captcha = client.Decode(Path.Combine(Path.GetTempPath(), username+".jpg"), 20);
                    if (null != captcha)
                    {
                        /* The CAPTCHA was solved; captcha.Id property holds its numeric ID,
                           and captcha.Text holds its text. */
                        Console.WriteLine(@"CAPTCHA {0} solved: {1}", captcha.Id, captcha.Text);
                        var capturetext = captcha.Text;

                        var view = ExtractViewStateSearch(hdoc.DocumentNode.InnerHtml);
                        //var view = QuerySelector(hdoc.DocumentNode, "input[id='__VIEWSTATE'") == null ? "" :
                        //QuerySelector(hdoc.DocumentNode, "input[id='__VIEWSTATE'").Attributes["value"].Value;
                        var postData = string.Format("__VIEWSTATE={0}&Login%24userEntryPanel2%24UsernameTextBox={1}&Login%24userEntryPanel2%24PasswordTextBox={2}&captcha_value={3}&LBD_VCID_idpCatpcha={4}&Login%24userEntryPanel2%24LoginImageButton.x=0&Login%24userEntryPanel2%24LoginImageButton.y=0",
                            view, username, password, capturetext, captchaId);

                        responseData = Post(loginUrl, postData);

                        if (!responseData.Contains("sessionTimeout_onLogout"))
                        {
                            client.Report(captcha);
                        }

                        return responseData.Contains("sessionTimeout_onLogout");
                    }
                }
                catch(Exception e)
                {
                    new Error().Add(e.Message);
                    //Email.SendEmail(AppConfig.GetSystemConfig("AlertEmail"),"Captcha Failure",e.Message);
                    CaptchaOverload = true;
                }
            }
            else
            {
                var postData = string.Format("loginName={0}&password={1}", username, password);

                responseData = Post(loginUrl, postData);

                return responseData.Contains("sessionTimeout_onLogout");
            }

            if (attempNo < 3)
            {
                Login(username, password, attempNo++);
            }

            return false;
        }

        public bool LoggedIn()
        {
            var response = Get("https://auctions.godaddy.com");
            return response.Contains("sessionTimeout_onLogout");
        }

        private AuctionSearch GenerateAuctionSearch()
        {
            var p = new AuctionSearch();
            return p;
        }

        public bool WinCheck(string domainName)
        {
            const string url = "https://au.auctions.godaddy.com/trpMessageHandler.aspx?isc=cjcdplink";
            //var postData = string.Format("sec=Wo&sort=6&dir=D&page=1&rpp=50&at=0&rnd={0}", RandomDouble(0, 1).ToString("0.00000000000000000"));
            var postData = string.Format("sec=Wo&sort=6&dir=D&page=1&rpp=50&at=0&maadv=0|{0}|||&rnd={1}", domainName, RandomDouble(0, 1).ToString("0.00000000000000000"));
            var data = Post(url, postData);

            return data.Contains(domainName);
        }

        public int CheckAuction(string auctionRef)
        {
            const string url = "https://au.auctions.godaddy.com/trpItemListing.aspx?miid={0}";
            var data = Get(string.Format(url, auctionRef));

            var minbid =
                TryParse_INT(
                    GetSubString(HTMLDecode(data),"Bid $",
                        " or more").Trim().Replace(",", "").Replace("$", ""));

            var end = GetEndDate(auctionRef);
            using (var ds = new ASEntities())
            {
                var auction = ds.Auctions.FirstOrDefault(x => x.AuctionRef == auctionRef);
                if (auction != null)
                {
                    if (auction.EndDate != end)
                    {
                        new Error().Add("Checked at: " + DateTime.Now + "\r\nEnd date found to be different: " + end + "\r\n" + "Old date was: " + auction.EndDate, "Date Change Value");
                        //auction.EndDate = end;
                        //ds.Auctions.AddOrUpdate(auction);
                        //ds.SaveChanges();
                    }
                    
                }
            }


            return minbid;
        }

        private HtmlDocument GetAuctionDetails(string auctionNo)
        {
            var data = Post("https://auctions.godaddy.com/trpMessageHandler.aspx", string.Format("ad={0}&type=Search", auctionNo));
            var hdoc = HtmlDocument(data);

            return hdoc;
        }

        public DateTime GetEndDate(string auctionNo)
        {
            var endDate = DateTime.Now.AddHours(decimal.ToInt32(Timediff));
            var details = GetAuctionDetails(auctionNo);
            if (QuerySelector(details.DocumentNode, "span.OneLinkNoTx") != null)
            {
                endDate = (QuerySelector(details.DocumentNode, "span.OneLinkNoTx").InnerText.Contains("PM") &&
                    DateTime.Parse(QuerySelector(details.DocumentNode, "span.OneLinkNoTx").InnerText.Replace("AM", "").Replace("PM", "").Replace("(PST)", "").Replace("(PDT)", "").Trim(), new CultureInfo("en-US", false)).Hour < 12) ?
                    DateTime.Parse(QuerySelector(details.DocumentNode, "span.OneLinkNoTx").InnerText.Replace("AM", "").Replace("PM", "").Replace("(PST)", "").Replace("(PDT)", "").Trim(), new CultureInfo("en-US", false)).AddHours(12) :
                    DateTime.Parse(QuerySelector(details.DocumentNode, "span.OneLinkNoTx").InnerText.Replace("AM", "").Replace("PM", "").Replace("(PST)", "").Replace("(PDT)", "").Trim(), new CultureInfo("en-US", false));
            }
            else if (QuerySelector(details.DocumentNode, "td[style*=background-color]") != null)
            {
                endDate = (QuerySelector(details.DocumentNode, "td[style*=background-color]").InnerText.Contains("PM") &&
                    DateTime.Parse(QuerySelector(details.DocumentNode, "td[style*=background-color]").InnerText.Replace("AM", "").Replace("PM", "").Replace("(PST)", "").Replace("(PDT)", "").Trim(), new CultureInfo("en-US", false)).Hour < 12) ?
                    DateTime.Parse(QuerySelector(details.DocumentNode, "td[style*=background-color]").InnerText.Replace("AM", "").Replace("PM", "").Replace("(PST)", "").Replace("(PDT)", "").Trim(), new CultureInfo("en-US", false)).AddHours(12) :
                    DateTime.Parse(QuerySelector(details.DocumentNode, "td[style*=background-color]").InnerText.Replace("AM", "").Replace("PM", "").Replace("(PST)", "").Replace("(PDT)", "").Trim(), new CultureInfo("en-US", false));
            }

            return endDate;
        }

        public IQueryable<AuctionSearch> Search(string searchText)
        {
            const string searchString = "https://auctions.godaddy.com/trpSearchResults.aspx";
            var auctions = new SortableBindingList<AuctionSearch>();

            var doc = HtmlDocument(Post(searchString, 
                "t=22&action=search&hidAdvSearch=ddlAdvKeyword:1|txtKeyword:" + 
                searchText.Replace(" ", ",") + 
                "&rtr=7&baid=-1&searchMode=1sg=1&showAdult=true&searchDir=1&event=&rnd=0.698455864796415&EqnJYig=561ef36"));


            if (QuerySelectorAll(doc.DocumentNode, "tr.srRow2, tr.srRow1") != null)
            {
                foreach (var node in QuerySelectorAll(doc.DocumentNode, "tr.srRow2, tr.srRow1"))
                {
                    if (QuerySelector(node, "span.OneLinkNoTx") != null && QuerySelector(node, "td:nth-child(5)") != null)
                    {
                        AuctionSearch auction = GenerateAuctionSearch();
                        auction.DomainName = HTMLDecode(QuerySelector(node, "span.OneLinkNoTx").InnerText);
                        Console.WriteLine(auction.DomainName);

                        auction.BidCount = TryParse_INT(HTMLDecode(QuerySelector(node, "td:nth-child(5)").FirstChild.InnerHtml.Replace("&nbsp;", "")));
                        auction.Traffic = TryParse_INT(HTMLDecode(QuerySelector(node, "td:nth-child(5) > td").InnerText.Replace("&nbsp;", "")));
                        auction.Valuation = TryParse_INT(HTMLDecode(QuerySelector(node, "td:nth-child(5) > td:nth-child(2)").InnerText.Replace("&nbsp;", "")));
                        auction.Price = TryParse_INT(HTMLDecode(QuerySelector(node, "td:nth-child(5) > td:nth-child(3)").InnerText).Replace("$", "").Replace(",", "").Replace("C", ""));
                        try
                        {
                            if (QuerySelector(node, "td:nth-child(5) > td:nth-child(4) > div") != null)
                            {
                                if (HTMLDecode(QuerySelector(node, "td:nth-child(5) > td:nth-child(4) > div").InnerText).Contains("Buy Now for"))
                                {
                                    auction.BuyItNow = TryParse_INT(Regex.Split(HTMLDecode(QuerySelector(node, "td:nth-child(5) > td:nth-child(4) > div").InnerText), "Buy Now for")[1].Trim().Replace(",", "").Replace("$", ""));
                                }

                            }
                            else
                            {
                                auction.BuyItNow = 0;
                            }

                        }
                        catch (Exception) { auction.BuyItNow = 0; }

                        if (QuerySelector(node, "td:nth-child(5) > td:nth-child(5)") != null &&
                            HTMLDecode(QuerySelector(node, "td:nth-child(5) > td:nth-child(5)").InnerHtml).Contains("Bid $"))
                        {
                            auction.MinBid = TryParse_INT(GetSubString(HTMLDecode(QuerySelector(node, "td:nth-child(5) > td:nth-child(5)").InnerHtml), "Bid $", " or more").Trim().Replace(",", "").Replace("$", ""));
                        }
                        if (QuerySelector(node, "td:nth-child(5) > td:nth-child(5)") != null &&
                            !HTMLDecode(QuerySelector(node, "td:nth-child(5) > td:nth-child(5)").InnerHtml).Contains("Bid $"))
                        {
                            auction.EstimateEndDate = GenerateEstimateEnd(QuerySelector(node, "td:nth-child(5) > td:nth-child(5)"));
                        }
                        if (QuerySelector(node, "td:nth-child(5) > td:nth-child(4)") != null &&
                            HTMLDecode(QuerySelector(node, "td:nth-child(5) > td:nth-child(4)").InnerHtml).Contains("Bid $"))
                        {
                            auction.MinBid = TryParse_INT(GetSubString(HTMLDecode(QuerySelector(node, "td:nth-child(5) > td:nth-child(4)").InnerHtml), "Bid $", " or more").Trim().Replace(",", "").Replace("$", ""));
                        }
                        if (QuerySelector(node, "td > div > span") != null)
                        {
                            foreach (var item in GetSubStrings(QuerySelector(node, "td > div > span").InnerHtml, "'Offer $", " or more"))
                            {
                                auction.MinOffer = TryParse_INT(item.Replace(",", ""));
                            }
                        }
                        auction.EndDate = DateTime.Now.AddHours(Timediff);
                        foreach (var item in GetSubStrings(node.InnerHtml, "ShowAuctionDetails('", "',"))
                        {
                            auction.AuctionRef = item;
                            break;
                        }
                        //AppSettings.Instance.AllAuctions.Add(auction);
                        if (auction.MinBid > 0)
                        {
                            auctions.Add(auction);
                        }

                    }

                }
            }

            return auctions.AsQueryable();

        }

        public static double RandomDouble(double start, double end)
        {
            var rand = new Random();
            return (rand.NextDouble() * Math.Abs(end - start)) + start;
        }

        public void PlaceBid(GoDaddyAccount account, Auctions auction)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            var url = "https://auctions.godaddy.com/trpSearchResults.aspx";
            using (var ds = new ASEntities())
            {
                var item = new AuctionHistory
                {
                    HistoryID = Guid.NewGuid(),
                    Text = "Logging In",
                    CreatedDate = DateTime.Now,
                    AuctionLink = auction.AuctionID
                };

                ds.AuctionHistory.Add(item);
                ds.SaveChanges();
            }
            if (Login(account.GoDaddyUsername, account.GoDaddyPassword))
            {
                var postData = string.Format("action=review_selected_add&items={0}_B_{1}_1|&rnd={2}&JlPYXTX=347bde7",
                    auction.AuctionRef, auction.MyBid,
                    RandomDouble(0, 1).ToString("0.00000000000000000"));
                var responseData = Post(url, postData);

                using (var ds = new ASEntities())
                {
                    var item = new AuctionHistory
                    {
                        HistoryID = Guid.NewGuid(),
                        Text = "Setting Max Bid: " + auction.MyBid,
                        CreatedDate = DateTime.Now,
                        AuctionLink = auction.AuctionID
                    };

                    ds.AuctionHistory.Add(item);
                    ds.SaveChanges();
                }

                //Review
                url = "https://auctions.godaddy.com/trpMessageHandler.aspx";
                postData = "q=ReviewDomains";
                responseData = Post(url, postData);

                if (responseData.Contains("ERROR: Bid must be a minimum of"))
                {
                    using (var ds = new ASEntities())
                    {
                        var item = new AuctionHistory
                        {
                            HistoryID = Guid.NewGuid(),
                            Text = "Bid Process Ended - Your max bid is already too small to place",
                            CreatedDate = DateTime.Now,
                            AuctionLink = auction.AuctionID
                        };

                        ds.AuctionHistory.Add(item);
                        ds.SaveChanges();
                    }
                    return;
                }

                //KeepAlive
                Get("https://auctions.godaddy.com/trpMessageHandler.aspx?keepAlive=1");
                Get("https://idp.godaddy.com/KeepAlive.aspx?SPKey=GDDNAEB002");

                url = string.Format("https://img.godaddy.com/pageevents.aspx?page_name=/trphome.aspx&ci=37022" +
                                    "&eventtype=click&ciimpressions=&usrin=&relativeX=659&relativeY=325&absoluteX=659&" +
                                    "absoluteY=1102&r={0}&comview=0", RandomDouble(0, 1).ToString("0.00000000000000000"));
                responseData = Get(url);

                // Buy
                var view = ExtractViewStateSearch(responseData);
                url = @"https://auctions.godaddy.com/trpItemListingReview.aspx";
                postData = "__VIEWSTATE=" + Viewstates +
                           string.Format("&hidAdvSearch=ddlAdvKeyword%3A1%7CtxtKeyword%3Aportal" +
                                         "%7CddlCharacters%3A0%7CtxtCharacters%3A%7CtxtMinTraffic%3A%7CtxtMaxTraffic%3A%" +
                                         "7CtxtMinDomainAge%3A%7CtxtMaxDomainAge%3A%7CtxtMinPrice%3A%7CtxtMaxPrice%3A%7Cdd" +
                                         "lCategories%3A0%7CchkAddBuyNow%3Afalse%7CchkAddFeatured%3Afalse%7CchkAddDash%3Atrue" +
                                         "%7CchkAddDigit%3Atrue%7CchkAddWeb%3Afalse%7CchkAddAppr%3Afalse%7CchkAddInv%3Afalse%7" +
                                         "CchkAddReseller%3Afalse%7CddlPattern1%3A%7CddlPattern2%3A%7CddlPattern3%3A%7CddlPattern4" +
                                         "%3A%7CchkSaleOffer%3Afalse%7CchkSalePublic%3Atrue%7CchkSaleExpired%3Afalse%7CchkSaleCloseouts" +
                                         "%3Afalse%7CchkSaleUsed%3Afalse%7CchkSaleBuyNow%3Afalse%7CchkSaleDC%3Afalse%7CchkAddOnSale" +
                                         "%3Afalse%7CddlAdvBids%3A0%7CtxtBids%3A%7CtxtAuctionID%3A%7CddlDateOffset%3A%7CddlRecordsPerPageAdv" +
                                         "%3A3&hidADVAction=p_&txtKeywordContext=&ddlRowsToReturn=3&hidAction=&hidItemsAddedToCart=" +
                                         "&hidGetMemberInfo=&hidShopperId=46311038&hidValidatedMemberInfo=&hidCheckedDomains=&hidMS90483566" +
                                         "=O&hidMS86848023=O&hidMS70107049=O&hidMS91154790=O&hidMS39351987=O&hidMS94284110=O&hidMS53775077=" +
                                         "O&hidMS75408187=O&hidMS94899096=B&hidMS94899097=B&hidMS94899098=B&hidMS94899099=B&hidMS94937468=" +
                                         "B&hidMS95047168=B&hidMS{0}=B&hid_Agree1=on", auction.AuctionRef);


                using (var ds = new ASEntities())
                {
                    var item = new AuctionHistory
                    {
                        HistoryID = Guid.NewGuid(),
                        Text = "Bid Process Completed",
                        CreatedDate = DateTime.Now,
                        AuctionLink = auction.AuctionID
                    };

                    ds.AuctionHistory.Add(item);
                    ds.SaveChanges();
                }
                var hdoc = HtmlDocument(Post(url, postData));
                bool confirmed = false;
                foreach (var items in QuerySelectorAll(hdoc.DocumentNode, "tr"))
                {
                    if (items.InnerHtml.Contains(auction.AuctionRef) && items.InnerHtml.Contains("the high bidder"))
                    {
                        confirmed = true;
                        using (var ds = new ASEntities())
                        {
                            var item = new AuctionHistory
                            {
                                HistoryID = Guid.NewGuid(),
                                Text = "Bid Confirmed - You are the high bidder!",
                                CreatedDate = DateTime.Now,
                                AuctionLink = auction.AuctionID
                            };

                            ds.AuctionHistory.Add(item);
                            ds.SaveChanges();
                        }
                        break;
                    }
                    if (items.InnerHtml.Contains(auction.AuctionRef) && items.InnerHtml.Contains("ERROR: Not an auction"))
                    {
                        confirmed = true;
                        using (var ds = new ASEntities())
                        {
                            var item = new AuctionHistory
                            {
                                HistoryID = Guid.NewGuid(),
                                Text = "Bid Failed - The site is no longer an auction",
                                CreatedDate = DateTime.Now,
                                AuctionLink = auction.AuctionID
                            };

                            ds.AuctionHistory.Add(item);
                            ds.SaveChanges();
                        }
                        break;
                    }
                    
                }
                if (!confirmed)
                {
                    using (var ds = new ASEntities())
                    {
                        try
                        {
                            var content = hdoc.DocumentNode.InnerHtml;
                            new Error().Add(content);
                        }
                        catch (Exception)
                        {
                                
                        }
                        var item = new AuctionHistory
                        {
                            HistoryID = Guid.NewGuid(),
                            Text = "Bid Not Confirmed - Data logged",
                            CreatedDate = DateTime.Now,
                            AuctionLink = auction.AuctionID
                        };

                        ds.AuctionHistory.Add(item);
                        ds.SaveChanges();
                    }
                }
            }
            else
            {
                using (var ds = new ASEntities())
                {
                    if (CaptchaOverload)
                    {
                        var item = new AuctionHistory
                        {
                            HistoryID = Guid.NewGuid(),
                            Text = "Appologies - 3rd party capture solve failure. This has been reported.",
                            CreatedDate = DateTime.Now,
                            AuctionLink = auction.AuctionID
                        };

                        ds.AuctionHistory.Add(item);
                    }
                    else
                    {
                        var item = new AuctionHistory
                        {
                            HistoryID = Guid.NewGuid(),
                            Text = "Appologies - Login to account has failed. 3 Seperate attempts made",
                            CreatedDate = DateTime.Now,
                            AuctionLink = auction.AuctionID
                        };

                        ds.AuctionHistory.Add(item);
                    }

                    //ds.GoDaddyAccount.First(x => x.AccountID == account.AccountID).Verified = false;

                    ds.SaveChanges();
                }
            }

        }

        public static string ExtractViewStateSearch(string viewstatestring)
        {
            try
            {
                const string viewStateNameDelimiter = "__VIEWSTATE";
                const string valueDelimiter = "value=\"";

                int viewStateNamePosition = viewstatestring.IndexOf(viewStateNameDelimiter);
                int viewStateValuePosition = viewstatestring.IndexOf(valueDelimiter, viewStateNamePosition);

                int viewStateStartPosition = viewStateValuePosition + valueDelimiter.Length;
                int viewStateEndPosition = viewstatestring.IndexOf("\"", viewStateStartPosition);

                viewstatestring = viewstatestring.Substring(
                            viewStateStartPosition,
                            viewStateEndPosition - viewStateStartPosition);
            }
            catch (Exception)
            {

            }


            return viewstatestring;
        }

        private DateTime GenerateEstimateEnd(HtmlNode node)
        {
            var estimateEnd = DateTime.Now.AddHours(Timediff);
            if (node.InnerText != null)
            {
                var vals = HTMLDecode(node.InnerText).Trim().Split(new [] { ' ' });

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

    }
}
