using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using DAL;
using DAS.Domain.GoDaddy.Users;
using DeathByCaptcha;
using Lunchboxweb;
using Exception = System.Exception;

namespace GoDaddy
{
    public class GoDaddyAuctionSniper : HttpBase
    {
        public IGoDaddySession SessionDetails;
        public GoDaddyAuctionSniper(IGoDaddySession session)
        {
            SessionDetails = session;
        }

        private const int Timediff = 0;
        private string Viewstate { get; set; }

        public bool CaptchaOverload { get; set; }

        /// <summary>
        /// Checks to see if you are still logged in
        /// </summary>
        /// <returns></returns>
        public bool LoggedIn(string html)
        {
            return html.Contains("Customer Number:");// && !html.Contains("shopperId: '{{pc:shopperid}}'");
        }

        public bool AuctionsLogin()
        {
            const string url = "https://auctions.godaddy.com/";
            var responseData = Get(url);

            var key = GetSubString(responseData, "spkey=", "\"");
            var loginurl = string.Format("https://idp.godaddy.com/login.aspx?SPKey={0}", key);
            var secondaryLogin = string.Format("https://sso.godaddy.com/?app=idp&path=%2flogin.aspx%3fSPKey%3d{0}", key);

            var loginData = string.Format("loginName={0}&password={1}", Uri.EscapeDataString(SessionDetails.GoDaddyAccount.Username), Uri.EscapeDataString(SessionDetails.GoDaddyAccount.Password));
            var firstLoginResponse = Post(loginurl, loginData);
            var login2Data = string.Format("name={0}&password={1}", Uri.EscapeDataString(SessionDetails.GoDaddyAccount.Username), Uri.EscapeDataString(SessionDetails.GoDaddyAccount.Password));

            var secondaryLoginResponse = Post(secondaryLogin, login2Data);

            return false;
        }

        /// <summary>
        /// Login to GoDaddy
        /// </summary>
        /// <param name="attempNo"></param>
        /// <returns></returns>
        public bool Login(int attempNo = 0)
        {
            const string url = "https://mya.godaddy.com";
            var values = new Dictionary<string, string>
            {
                {"Accept-Language", "en-GB,en-US;q=0.8,en;q=0.6"},
                {"Upgrade-Insecure-Requests", "1"}
            };
            var responseData = Get(url, values);
            if (LoggedIn(responseData))
            {
                return true;
            }
            var key = GetSubString(responseData, "spkey=", "\"");
            var loginUrl = "https://sso.godaddy.com/?path=sso%2Freturn&app=www";
            var hdoc = HtmlDocument(responseData);
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
                    imagedata.Save(Path.Combine(Path.GetTempPath(), SessionDetails.GoDaddyAccount.Username + ".jpg"), ImageFormat.Jpeg);
                    var user = SessionDetails.DeathByCapture.Username;
                    var pass = SessionDetails.DeathByCapture.Password;
                    Client client = new SocketClient(user, pass);

                    //var balance = client.GetBalance();
                    var captcha = client.Decode(Path.Combine(Path.GetTempPath(), SessionDetails.GoDaddyAccount.Username + ".jpg"), 20);
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
                            view, Uri.EscapeDataString(SessionDetails.GoDaddyAccount.Username), Uri.EscapeDataString(SessionDetails.GoDaddyAccount.Password), capturetext, captchaId);

                        responseData = Post(loginUrl, postData);

                        if (!LoggedIn(responseData))
                        {
                            client.Report(captcha);
                        }

                        return LoggedIn(responseData);
                    }
                }
                catch (Exception e)
                {
                    //new Error().Add(e.Message);
                    //Email.SendEmail(AppConfig.GetSystemConfig("AlertEmail"),"Captcha Failure",e.Message);
                    CaptchaOverload = true;
                }
            }
            else
            {
                var postData = string.Format("name={0}&password={1}", Uri.EscapeDataString(SessionDetails.GoDaddyAccount.Username), Uri.EscapeDataString(SessionDetails.GoDaddyAccount.Password));
                responseData = Post(loginUrl, postData);

                return LoggedIn(responseData);
            }

            if (attempNo < 3)
            {
                Login(attempNo++);
            }

            return false;
        }

        private AuctionSearch GenerateAuctionSearch()
        {
            var p = new AuctionSearch();
            return p;
        }

        public bool WinCheck(string domainName)
        {
            const string url = "https://auctions.godaddy.com/trpMessageHandler.aspx ";
            //var postData = string.Format("sec=Wo&sort=6&dir=D&page=1&rpp=50&at=0&rnd={0}", RandomDouble(0, 1).ToString("0.00000000000000000"));
            var postData = string.Format("sec=Wo&sort=6&dir=D&page=1&rpp=50&at=0&maadv=0|{0}|||&rnd={1}", domainName, RandomDouble().ToString("0.00000000000000000"));
            var data = Post(url, postData);

            return data.Contains(domainName);
        }

        /// <summary>
        /// Random double generator for requests
        /// </summary>
        /// <returns></returns>
        private static double RandomDouble()
        {
            var rand = new Random();
            return (rand.NextDouble() * Math.Abs(1 - 0)) + 0;
        }

    }
}
