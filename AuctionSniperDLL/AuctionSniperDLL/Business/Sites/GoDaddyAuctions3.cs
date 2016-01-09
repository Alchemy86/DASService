using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ASEntityFramework;
using DeathByCaptcha;
using LunchboxSource.Business.Http;
using Exception = System.Exception;

namespace AuctionSniperDLL.Business.Sites
{
    public class GoDaddyAuctions3 : HttpBase
    {
        private const int Timediff = 0;
        private string Viewstates { get; set; }

        public bool CaptchaOverload { get; set; }

        public bool Login(string username, string password, int attempNo = 0)
        {
            const string url = "https://www.godaddy.com";
            var responseData = Get(url);

            var loginUrl = "https://sso.godaddy.com/?path=sso%2Freturn&app=www";


            var postData = string.Format("name={0}&password={1}", username, Uri.EscapeDataString(password));
            //Login form auto resubmit data
            responseData = Post(loginUrl, postData);

            return responseData.Contains("sessionTimeout_onLogout");
            

            //if (attempNo < 3)
            //{
            //    Login(username, password, attempNo++);
            //}

            //return false;
        }
    }
}
