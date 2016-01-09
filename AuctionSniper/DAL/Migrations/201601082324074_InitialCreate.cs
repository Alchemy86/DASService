namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdvSearch",
                c => new
                    {
                        AdvSearchID = c.Guid(nullable: false),
                        DomainName = c.String(nullable: false, maxLength: 200),
                        DomainLink = c.String(maxLength: 500),
                        Age = c.Int(),
                        PageRank = c.Int(),
                        MOZDA = c.Int(),
                        MOZPA = c.Int(),
                        MozRank = c.Decimal(precision: 18, scale: 2),
                        MozTrust = c.Decimal(precision: 18, scale: 2),
                        CF = c.Int(),
                        TF = c.Int(),
                        AlexARank = c.Decimal(precision: 18, scale: 2),
                        MozDom = c.Int(),
                        MajDom = c.Int(),
                        SimilarWeb = c.String(maxLength: 200),
                        SemTarf = c.String(maxLength: 50),
                        DomainPrice = c.Int(),
                        EndTime = c.DateTime(),
                        EsitmateEnd = c.DateTime(),
                        UserID = c.Guid(nullable: false),
                        Ref = c.String(nullable: false, maxLength: 50),
                        IsAuction = c.Boolean(nullable: false),
                        IsGOdaddy = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AdvSearchID);
            
            CreateTable(
                "dbo.Alerts",
                c => new
                    {
                        AlertID = c.Guid(nullable: false),
                        Description = c.String(nullable: false, maxLength: 800),
                        TriggerTime = c.DateTime(nullable: false),
                        Processed = c.Boolean(nullable: false),
                        AuctionID = c.Guid(nullable: false),
                        Custom = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AlertID);
            
            CreateTable(
                "dbo.AuctionHistory",
                c => new
                    {
                        HistoryID = c.Guid(nullable: false),
                        Text = c.String(nullable: false, maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        AuctionLink = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.HistoryID);
            
            CreateTable(
                "dbo.AuctionHistoryView",
                c => new
                    {
                        HistoryID = c.Guid(nullable: false),
                        Text = c.String(nullable: false, maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        AuctionLink = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.HistoryID, t.Text, t.CreatedDate, t.AuctionLink });
            
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        AuctionID = c.Guid(nullable: false),
                        DomainName = c.String(nullable: false, maxLength: 250),
                        AuctionRef = c.String(nullable: false, maxLength: 150),
                        BidCount = c.Int(nullable: false),
                        Traffic = c.Int(nullable: false),
                        Valuation = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        MinBid = c.Int(nullable: false),
                        MinOffer = c.Int(nullable: false),
                        BuyItNow = c.Int(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        EstimateEndDate = c.DateTime(),
                        AccountID = c.Guid(nullable: false),
                        Status = c.String(maxLength: 100),
                        MyBid = c.Int(),
                        Processed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AuctionID)
                .ForeignKey("dbo.GoDaddyAccount", t => t.AccountID, cascadeDelete: true)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.GoDaddyAccount",
                c => new
                    {
                        AccountID = c.Guid(nullable: false),
                        GoDaddyUsername = c.String(nullable: false, maxLength: 200),
                        GoDaddyPassword = c.String(nullable: false, maxLength: 250),
                        UserID = c.Guid(nullable: false),
                        Verified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AccountID)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.AuctionSearch",
                c => new
                    {
                        AuctionID = c.Guid(nullable: false),
                        DomainName = c.String(nullable: false, maxLength: 250),
                        AuctionRef = c.String(nullable: false, maxLength: 150),
                        BidCount = c.Int(nullable: false),
                        Traffic = c.Int(nullable: false),
                        Valuation = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        MinBid = c.Int(nullable: false),
                        MinOffer = c.Int(nullable: false),
                        BuyItNow = c.Int(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        EstimateEndDate = c.DateTime(),
                        AccountID = c.Guid(nullable: false),
                        Status = c.String(maxLength: 100),
                        MyBid = c.Int(),
                        Processed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AuctionID)
                .ForeignKey("dbo.GoDaddyAccount", t => t.AccountID, cascadeDelete: true)
                .Index(t => t.AccountID);
            
            CreateTable(
                "dbo.BackOrders",
                c => new
                    {
                        OrderID = c.Guid(nullable: false),
                        DomainName = c.String(nullable: false, maxLength: 100),
                        AlertEmail1 = c.String(nullable: false, maxLength: 100),
                        AlertEmail2 = c.String(maxLength: 100),
                        DateToOrder = c.DateTime(nullable: false),
                        CreditsToUse = c.Int(nullable: false),
                        Private = c.Boolean(nullable: false),
                        GoDaddyAccount = c.Guid(nullable: false),
                        Processed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.GoDaddyAccount", t => t.GoDaddyAccount, cascadeDelete: true)
                .Index(t => t.GoDaddyAccount);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Guid(nullable: false),
                        Username = c.String(nullable: false, maxLength: 150),
                        Password = c.String(nullable: false, maxLength: 150),
                        ReceiveEmails = c.Boolean(nullable: false),
                        AccessLevel = c.Int(nullable: false),
                        UseAccountForSearch = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Chart_AuctionHistory",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 150),
                        TotalAuctions = c.Int(),
                        AuctionsThisMonth = c.Int(),
                        AuctionsWonTotal = c.Int(),
                    })
                .PrimaryKey(t => t.Username);
            
            CreateTable(
                "dbo.Chart_AuctionsEnding",
                c => new
                    {
                        AuctionRef = c.String(nullable: false, maxLength: 150),
                        DomainName = c.String(nullable: false, maxLength: 250),
                        EndDate = c.DateTime(nullable: false),
                        Username = c.String(nullable: false, maxLength: 150),
                        MinBid = c.Int(nullable: false),
                        MyBid = c.Int(),
                    })
                .PrimaryKey(t => new { t.AuctionRef, t.DomainName, t.EndDate, t.Username, t.MinBid });
            
            CreateTable(
                "dbo.Chart_PopularDomainsThisMonth",
                c => new
                    {
                        AuctionRef = c.String(nullable: false, maxLength: 150),
                        DomainName = c.String(nullable: false, maxLength: 250),
                        Bids = c.Int(),
                    })
                .PrimaryKey(t => new { t.AuctionRef, t.DomainName });
            
            CreateTable(
                "dbo.EventLog",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        Event = c.String(nullable: false, maxLength: 50),
                        Message = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SearchConfig",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        UserID = c.Guid(nullable: false),
                        Active = c.Boolean(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        RequirePR = c.Boolean(nullable: false),
                        PRMin = c.Int(nullable: false),
                        PRMax = c.Int(nullable: false),
                        MajesticBacklinksMin = c.Int(nullable: false),
                        MajesticBacklinksMAX = c.Int(nullable: false),
                        MajesticTrustFlowMin = c.Int(nullable: false),
                        MajesticTrustFlowMax = c.Int(nullable: false),
                        MajesticCitationFlowMax = c.Int(nullable: false),
                        MajesticCitationFlowMin = c.Int(nullable: false),
                        MOZPA = c.Boolean(nullable: false),
                        MOZPAMin = c.Int(nullable: false),
                        MOZPAMax = c.Int(nullable: false),
                        MOZDA = c.Boolean(nullable: false),
                        MOZDAMin = c.Int(nullable: false),
                        MOZDAMax = c.Int(nullable: false),
                        MajesticIPS = c.Boolean(nullable: false),
                        MajesticIPSMin = c.Int(nullable: false),
                        MajesticIPSMax = c.Int(nullable: false),
                        DomainAgeMin = c.Int(nullable: false),
                        DomainAgeMax = c.Int(nullable: false),
                        DomainPrice = c.Boolean(nullable: false),
                        DomainPriceMin = c.Int(nullable: false),
                        DomainPriceMax = c.Int(nullable: false),
                        FacebookLikesMin = c.Int(nullable: false),
                        FacebookLikesMax = c.Int(nullable: false),
                        FacebookSharesMin = c.Int(nullable: false),
                        FacebookSharesMax = c.Int(nullable: false),
                        TwitterShares = c.Boolean(nullable: false),
                        TwitterSharesMin = c.Int(nullable: false),
                        TwitterSharesMax = c.Int(nullable: false),
                        RedditShares = c.Boolean(nullable: false),
                        RedditSharesMin = c.Int(nullable: false),
                        RedditSharesMax = c.Int(nullable: false),
                        PintrestShares = c.Boolean(nullable: false),
                        PintrestSharesMin = c.Int(nullable: false),
                        PintrestSharesMax = c.Int(nullable: false),
                        GPlusShares = c.Boolean(nullable: false),
                        GPlusSharesMin = c.Int(nullable: false),
                        GPlusSharesMax = c.Int(nullable: false),
                        Auction = c.Boolean(nullable: false),
                        BuyNow = c.Boolean(nullable: false),
                        BarginBin = c.Boolean(nullable: false),
                        CloseOut = c.Boolean(nullable: false),
                        PendingDelete = c.Boolean(nullable: false),
                        Expired = c.Boolean(nullable: false),
                        GoDaddy = c.Boolean(nullable: false),
                        NameJet = c.Boolean(nullable: false),
                        Dynadot = c.Boolean(nullable: false),
                        SnapName = c.Boolean(nullable: false),
                        Sedo = c.Boolean(nullable: false),
                        FakePR = c.Boolean(nullable: false),
                        FakeAlexa = c.Boolean(nullable: false),
                        RequireSemrushKey = c.Boolean(nullable: false),
                        MajMillion = c.Boolean(nullable: false),
                        QuantMillion = c.Boolean(nullable: false),
                        AlexARankRequired = c.Boolean(nullable: false),
                        SemrushRankReq = c.Boolean(nullable: false),
                        SimilarWebRankReq = c.Boolean(nullable: false),
                        RequireAv = c.Boolean(nullable: false),
                        HideAdult = c.Boolean(nullable: false),
                        HideSpammy = c.Boolean(nullable: false),
                        HideBrand = c.Boolean(nullable: false),
                        EndInLower = c.Int(nullable: false),
                        EndInUpper = c.Int(nullable: false),
                        ParentCat = c.String(nullable: false, maxLength: 50),
                        ChildCat = c.String(nullable: false, maxLength: 50),
                        ParentTopCat = c.String(nullable: false, maxLength: 50),
                        ChildTopCat = c.String(nullable: false, maxLength: 50),
                        GoogleIndex = c.Boolean(nullable: false),
                        dmoz = c.Boolean(nullable: false),
                        AllowDash = c.Boolean(nullable: false),
                        AllowDigits = c.Boolean(nullable: false),
                        OnlyDigits = c.Boolean(nullable: false),
                        SalesTypeOffer = c.Boolean(nullable: false),
                        SearchType = c.String(nullable: false, maxLength: 50),
                        Keyword = c.String(nullable: false, maxLength: 100),
                        KeywordSearchType = c.String(nullable: false, maxLength: 50),
                        Pattern = c.String(nullable: false, maxLength: 50),
                        PatternType = c.String(nullable: false, maxLength: 50),
                        Droplists = c.Boolean(nullable: false),
                        RequireSemrushTraffic = c.Boolean(nullable: false),
                        end_asia = c.Boolean(nullable: false),
                        end_at = c.Boolean(nullable: false),
                        end_au = c.Boolean(nullable: false),
                        end_be = c.Boolean(nullable: false),
                        end_biz = c.Boolean(nullable: false),
                        end_ca = c.Boolean(nullable: false),
                        end_cc = c.Boolean(nullable: false),
                        end_ch = c.Boolean(nullable: false),
                        end_co = c.Boolean(nullable: false),
                        end_com = c.Boolean(nullable: false),
                        end_de = c.Boolean(nullable: false),
                        end_eu = c.Boolean(nullable: false),
                        end_fr = c.Boolean(nullable: false),
                        end_ie = c.Boolean(nullable: false),
                        end_in = c.Boolean(nullable: false),
                        end_info = c.Boolean(nullable: false),
                        end_it = c.Boolean(nullable: false),
                        end_me = c.Boolean(nullable: false),
                        end_mobi = c.Boolean(nullable: false),
                        end_mx = c.Boolean(nullable: false),
                        end_net = c.Boolean(nullable: false),
                        end_nl = c.Boolean(nullable: false),
                        end_nu = c.Boolean(nullable: false),
                        end_org = c.Boolean(nullable: false),
                        end_pl = c.Boolean(nullable: false),
                        end_ru = c.Boolean(nullable: false),
                        end_se = c.Boolean(nullable: false),
                        end_su = c.Boolean(nullable: false),
                        end_tv = c.Boolean(nullable: false),
                        end_uk = c.Boolean(nullable: false),
                        end_us = c.Boolean(nullable: false),
                        end_misc = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SearchLayout",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SearchValue = c.String(nullable: false, maxLength: 50),
                        FieldName = c.String(nullable: false, maxLength: 50),
                        Order = c.Int(nullable: false),
                        ControlType = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SearchTable",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        SearchValue = c.String(nullable: false, maxLength: 50),
                        FieldName = c.String(nullable: false, maxLength: 50),
                        Value = c.String(maxLength: 50),
                        DataType = c.String(maxLength: 50),
                        ItemOrder = c.Int(nullable: false),
                        UserID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SystemConfig",
                c => new
                    {
                        PropertyID = c.String(nullable: false, maxLength: 50),
                        Value = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.PropertyID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoDaddyAccount", "UserID", "dbo.Users");
            DropForeignKey("dbo.BackOrders", "GoDaddyAccount", "dbo.GoDaddyAccount");
            DropForeignKey("dbo.AuctionSearch", "AccountID", "dbo.GoDaddyAccount");
            DropForeignKey("dbo.Auctions", "AccountID", "dbo.GoDaddyAccount");
            DropIndex("dbo.BackOrders", new[] { "GoDaddyAccount" });
            DropIndex("dbo.AuctionSearch", new[] { "AccountID" });
            DropIndex("dbo.GoDaddyAccount", new[] { "UserID" });
            DropIndex("dbo.Auctions", new[] { "AccountID" });
            DropTable("dbo.SystemConfig");
            DropTable("dbo.SearchTable");
            DropTable("dbo.SearchLayout");
            DropTable("dbo.SearchConfig");
            DropTable("dbo.EventLog");
            DropTable("dbo.Chart_PopularDomainsThisMonth");
            DropTable("dbo.Chart_AuctionsEnding");
            DropTable("dbo.Chart_AuctionHistory");
            DropTable("dbo.Users");
            DropTable("dbo.BackOrders");
            DropTable("dbo.AuctionSearch");
            DropTable("dbo.GoDaddyAccount");
            DropTable("dbo.Auctions");
            DropTable("dbo.AuctionHistoryView");
            DropTable("dbo.AuctionHistory");
            DropTable("dbo.Alerts");
            DropTable("dbo.AdvSearch");
        }
    }
}
