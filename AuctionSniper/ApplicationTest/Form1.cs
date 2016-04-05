using System;
using System.Linq;
using System.Windows.Forms;
using DAS.Domain;
using DAS.Domain.Users;
using GoDaddy;
using Ninject;

namespace ApplicationTest
{
    public partial class Form1 : Form
    {
        private readonly IKernel _kernal;
        public Form1(IKernel kernal)
        {
            InitializeComponent();
            _kernal = kernal;
            _kernal.Inject(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GoDaddyAuctionSniper gd = new GoDaddyAuctionSniper("seo@ezsolution.com", _kernal.Get<IUserRepository>());
            //var loggedin = gd.Login();
            var repo = _kernal.Get<ISystemRepository>();
            gd.WinCheck("yolo.com");
            //var auc = repo.GetEndingAuctions().FirstOrDefault(x => x.DomainName == "newfantasy.net");
            //gd.PlaceBid(auc);
            //var resw = gd.GetEndDate("185921902");
            
            //var res = repo.GetAlerts();
            //var moo = "";
        }
    }
}
