using System;
using System.Windows.Forms;
using DAS.Domain.Users;
using GoDaddy;

namespace ApplicationTest
{
    public partial class Form1 : Form
    {
        private readonly IUserRepository _repository;
        public Form1(IUserRepository repository)
        {
            InitializeComponent();
            _repository = repository;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GoDaddyAuctionSniper gd = new GoDaddyAuctionSniper(_repository.GetSessionDetails("urbanally@aol.com"));
            var loggedin = gd.AuctionsLogin();
            var won = gd.WinCheck("techinreview.com");
            var moo = "";
        }
    }
}
