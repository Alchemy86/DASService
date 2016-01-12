using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Xml;
using DAS.Domain;
using DAS.Domain.GoDaddy.Alerts;
using DAS.Domain.Users;
using GoDaddy;
using Ninject;
using Timer = System.Timers.Timer;

namespace AuctionSniperWindowsService
{
    public partial class Service1 : ServiceBase
    {
        public Timer CheckTimer { get; set; }
        public Timer AlertTimer { get; set; }
        public string BidTime { get; set; }
        public bool ProcessingBids;

        private readonly API.LunchboxCodeAPI API = new API.LunchboxCodeAPI();

        private bool Debug { get; set; }
        public IEmail Email;
        public IKernel Kernel;
        public ISystemRepository SystemRepository;
        public IUserRepository UserRepository;

        public void OnDebug()
        {
            Debug = true;
            OnStart(null);
        }

        public Service1(IKernel kernel)
        {
            InitializeComponent();
            Kernel = kernel;
            kernel.Inject(this);

            Email = kernel.Get<IEmail>();
            SystemRepository = kernel.Get<ISystemRepository>();
            UserRepository = kernel.Get<IUserRepository>();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ProcessingBids = false;
                BidTime = SystemRepository.BidTime;
                Email.SendEmail(SystemRepository.AlertEmail, "ServiceStarted", "Started Azure service",
                    Global.GetPacificTime);
                CheckTimer = new Timer(10000); // Run every 10 sec
                CheckDB();
                Email.SendEmail(SystemRepository.AlertEmail, "ServiceTested", "Successfull test", Global.GetPacificTime);
            }
            catch (Exception ex)
            {
                Email.SendEmail(SystemRepository.AlertEmail, "ServiceTested", "failed test: " + ex.Message, Global.GetPacificTime);
            }

            var th1 = new Thread(() =>
            {
                CheckTimer = new Timer(10000); // Run every 10 sec
                CheckTimer.Elapsed += EmailCheckTimer_Elapsed;
                CheckTimer.Enabled = true;
                CheckTimer.Start();
            });
            th1.SetApartmentState(ApartmentState.STA);
            th1.IsBackground = true;
            th1.Start();

            var th2 = new Thread(() =>
            {
                AlertTimer = new Timer(10000); // Run every 10 sec
                AlertTimer.Elapsed += AlertTimer_Elapsed;
                AlertTimer.Enabled = true;
                AlertTimer.Start();
            });
            th2.SetApartmentState(ApartmentState.STA);
            th2.IsBackground = true;
            th2.Start();
        }

        void AlertTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                CheckAlerts();
            }
            catch (Exception ex)
            {
                SendErrorReport(ex);
                AlertTimer.Stop();
                AlertTimer.Enabled = false;
                AlertTimer.Enabled = true;
                AlertTimer.Start();
            }

        }

        void SendErrorReport(Exception ex)
        {
            try
            {
                var message = ex.Message + Environment.NewLine +
                          ex.StackTrace + Environment.NewLine;
                message += ex.InnerException.Message;
                Email.SendEmail(SystemRepository.AlertEmail, "Azure Service Error - Alerts",
                        ex.Message + Environment.NewLine +
                        ex.StackTrace + Environment.NewLine, Global.GetPacificTime);
            }
            catch (Exception)
            {

            }
        }

        void EmailCheckTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                CheckDB();
                
            }
            catch (Exception ex)
            {
                SendErrorReport(ex);
                CheckTimer.Stop();
                CheckTimer.Enabled = false;
                CheckTimer.Enabled = true;
                CheckTimer.Start();
            }

        }

        /// <summary>
        /// Check and process bids
        /// </summary>
        private void CheckDB()
        {
            ProcessingBids = true;
            CheckTimer.Stop();
            var auctionsToProcess = SystemRepository.GetEndingAuctions();
            if (auctionsToProcess != null)
            {
                foreach (var auction in auctionsToProcess.ToList())
                {
                    var ts =
                        auction.EndDate.Subtract(Global.GetPacificTime);
                    if (ts.TotalSeconds < Convert.ToInt32(BidTime) &&
                        (auction.MinBid < auction.MyBid || auction.MinBid == auction.MyBid) && ts.TotalSeconds > 0)
                    {
                        try
                        {
                            auction.Processed = true;
                            SystemRepository.MarkAuctionAsProcess(auction.AuctionId);
                            UserRepository.AddHistoryRecord("Bid Process Started", auction.AuctionId);

                            var account = auction.GoDaddyAccount;
                            var auction1 = auction;
                            var th = new Thread(() =>
                            {
                                try
                                {
                                    var gd = new GoDaddyAuctionSniper(account.AccountUsername, UserRepository);
                                    gd.PlaceBid(auction1);
                                    Email.SendEmail(SystemRepository.AlertEmail, "Azure: Placing a bid",
                                        "Account: " + account.Username + Environment.NewLine +
                                        "Site: " + auction1.DomainName, Global.GetPacificTime);
                                }
                                catch (Exception e)
                                {
                                    SendErrorReport(e);
                                    CheckTimer.Start();
                                    ProcessingBids = false;
                                }
                            });
                            th.SetApartmentState(ApartmentState.STA);
                            th.IsBackground = true;
                            th.Start();
                        }
                        catch (Exception ex)
                        {
                            SendErrorReport(ex);
                            CheckTimer.Start();
                            ProcessingBids = false;
                        }

                    }
                }
            }

            ProcessingBids = false;
            CheckTimer.Start();
        }

        private void CheckAlerts()
        {
            AlertTimer.Stop();
            var alertsToProcess = SystemRepository.GetAlerts();
            if (alertsToProcess != null)
            {
                foreach (var alert in alertsToProcess.ToList())
                {
                    var ts =
                        alert.TriggerTime.Subtract(Global.GetPacificTime);
                    if (ts.TotalSeconds < Convert.ToInt32(BidTime))
                    {
                        try
                        {

                            alert.Processed = true;
                            var id = alert.AuctionId;


                            var alert1 = alert;
                            var gd = new GoDaddyAuctionSniper(alert1.Auction.AccountUsername, UserRepository);
                            if (alert1.Type == AlertType.Win)
                            {
                                gd.Login();
                                if (gd.WinCheck(alert1.Auction.DomainName))
                                {
                                    if (Global.GetPacificTime > alert1.TriggerTime.AddHours(4))
                                    {
                                        UserRepository.AddHistoryRecord("Auction Check (Delayed)", alert1.AuctionId);
                                        UserRepository.AddHistoryRecord("Auction WON", alert1.AuctionId);
                                    }
                                    else
                                    {
                                        UserRepository.AddHistoryRecord("Auction WON", alert1.AuctionId);
                                    }
                                }
                                else
                                {
                                    if (Global.GetPacificTime > alert1.TriggerTime.AddHours(4))
                                    {
                                        UserRepository.AddHistoryRecord("Auction Check (Delayed)", alert1.AuctionId);
                                        UserRepository.AddHistoryRecord("Auction LOST", alert1.AuctionId);
                                    }
                                    else
                                    {
                                        UserRepository.AddHistoryRecord("Auction LOST", alert1.AuctionId);
                                    }
                                }
                            }
                            else
                            {
                                var minBid = gd.CheckAuction(alert1.Auction.AuctionRef);
                                if (alert1.Auction.MyBid >= minBid)
                                {
                                    if (alert1.Auction.GoDaddyAccount.ReceiveEmail)
                                    {
                                        UserRepository.AddHistoryRecord("Bid Reminder Email Sent", alert1.AuctionId);
                                        API.Email(alert1.Auction.GoDaddyAccount.Username, "12 Hour Auction Reminder",
                                            "A quick reminder!" + Environment.NewLine +
                                            "Your maximum bid of $" + alert1.Auction.MyBid +
                                            "will (thus far!) seal the win!" +
                                            Environment.NewLine +
                                            "Site: " + alert1.Auction.DomainName, "Domain Auction Sniper");
                                    }
                                }
                                else
                                {
                                    if (alert1.Auction.GoDaddyAccount.ReceiveEmail)
                                    {
                                        UserRepository.AddHistoryRecord("Alerted - Auction will be lost",
                                            alert1.AuctionId);
                                        API.Email(alert1.Auction.GoDaddyAccount.Username, "Auction will be lost",
                                            "A quick reminder!" + Environment.NewLine +
                                            "If you don't increase your maximum bid to $" + minBid +
                                            " or more you will loose!" + Environment.NewLine +
                                            "Site: " + alert1.Auction.DomainName, "Domain Auction Sniper");
                                    }
                                    else
                                    {

                                        UserRepository.AddHistoryRecord(
                                            "Auction will be lost - Email warning is disabled", alert1.AuctionId);
                                    }
                                }
                            }



                        }
                        catch (Exception ex)
                        {
                            SendErrorReport(ex);
                            AlertTimer.Start();
                        }

                    }
                }
            }
            AlertTimer.Start();
        }

        protected override void OnStop()
        {
            Email.SendEmail(SystemRepository.AlertEmail, "Azure Service Stopped", "Stopped the service", Global.GetPacificTime);
            Console.WriteLine(@"Service Stopped");
        }
    }
}
