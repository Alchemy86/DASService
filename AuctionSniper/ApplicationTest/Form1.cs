using System;
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
            GoDaddyAuctionSniper gd =  new GoDaddyAuctionSniper("urbanally@aol.com", _kernal.Get<IUserRepository>());
            //var loggedin = gd.Login();
            var repo = _kernal.Get<ISystemRepository>();
            var res = repo.GetAlerts();
            var moo = "";
        }
    }
}
