namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datadefaultsCorrection : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Alerts", "AuctionID", "dbo.Auctions");
            DropForeignKey("dbo.Auctions", "AccountID", "dbo.GoDaddyAccount");
            DropForeignKey("dbo.AuctionSearch", "AccountID", "dbo.GoDaddyAccount");
            DropForeignKey("dbo.BackOrders", "GoDaddyAccount", "dbo.GoDaddyAccount");
            DropForeignKey("dbo.GoDaddyAccount", "UserID", "dbo.Users");
            DropPrimaryKey("dbo.Auctions");
            DropPrimaryKey("dbo.GoDaddyAccount");
            DropPrimaryKey("dbo.AuctionSearch");
            DropPrimaryKey("dbo.BackOrders");
            DropPrimaryKey("dbo.Users");
            DropPrimaryKey("dbo.AuctionHistory");
            AlterColumn("dbo.Auctions", "AuctionID", c => c.Guid(nullable: false));
            AlterColumn("dbo.GoDaddyAccount", "AccountID", c => c.Guid(nullable: false));
            AlterColumn("dbo.AuctionSearch", "AuctionID", c => c.Guid(nullable: false));
            AlterColumn("dbo.BackOrders", "OrderID", c => c.Guid(nullable: false));
            AlterColumn("dbo.Users", "UserID", c => c.Guid(nullable: false));
            AlterColumn("dbo.AuctionHistory", "HistoryID", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Auctions", "AuctionID");
            AddPrimaryKey("dbo.GoDaddyAccount", "AccountID");
            AddPrimaryKey("dbo.AuctionSearch", "AuctionID");
            AddPrimaryKey("dbo.BackOrders", "OrderID");
            AddPrimaryKey("dbo.Users", "UserID");
            AddPrimaryKey("dbo.AuctionHistory", "HistoryID");
            AddForeignKey("dbo.Alerts", "AuctionID", "dbo.Auctions", "AuctionID", cascadeDelete: true);
            AddForeignKey("dbo.Auctions", "AccountID", "dbo.GoDaddyAccount", "AccountID", cascadeDelete: true);
            AddForeignKey("dbo.AuctionSearch", "AccountID", "dbo.GoDaddyAccount", "AccountID", cascadeDelete: true);
            AddForeignKey("dbo.BackOrders", "GoDaddyAccount", "dbo.GoDaddyAccount", "AccountID", cascadeDelete: true);
            AddForeignKey("dbo.GoDaddyAccount", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GoDaddyAccount", "UserID", "dbo.Users");
            DropForeignKey("dbo.BackOrders", "GoDaddyAccount", "dbo.GoDaddyAccount");
            DropForeignKey("dbo.AuctionSearch", "AccountID", "dbo.GoDaddyAccount");
            DropForeignKey("dbo.Auctions", "AccountID", "dbo.GoDaddyAccount");
            DropForeignKey("dbo.Alerts", "AuctionID", "dbo.Auctions");
            DropPrimaryKey("dbo.AuctionHistory");
            DropPrimaryKey("dbo.Users");
            DropPrimaryKey("dbo.BackOrders");
            DropPrimaryKey("dbo.AuctionSearch");
            DropPrimaryKey("dbo.GoDaddyAccount");
            DropPrimaryKey("dbo.Auctions");
            AlterColumn("dbo.AuctionHistory", "HistoryID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Users", "UserID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.BackOrders", "OrderID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.AuctionSearch", "AuctionID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.GoDaddyAccount", "AccountID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Auctions", "AuctionID", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.AuctionHistory", "HistoryID");
            AddPrimaryKey("dbo.Users", "UserID");
            AddPrimaryKey("dbo.BackOrders", "OrderID");
            AddPrimaryKey("dbo.AuctionSearch", "AuctionID");
            AddPrimaryKey("dbo.GoDaddyAccount", "AccountID");
            AddPrimaryKey("dbo.Auctions", "AuctionID");
            AddForeignKey("dbo.GoDaddyAccount", "UserID", "dbo.Users", "UserID", cascadeDelete: true);
            AddForeignKey("dbo.BackOrders", "GoDaddyAccount", "dbo.GoDaddyAccount", "AccountID", cascadeDelete: true);
            AddForeignKey("dbo.AuctionSearch", "AccountID", "dbo.GoDaddyAccount", "AccountID", cascadeDelete: true);
            AddForeignKey("dbo.Auctions", "AccountID", "dbo.GoDaddyAccount", "AccountID", cascadeDelete: true);
            AddForeignKey("dbo.Alerts", "AuctionID", "dbo.Auctions", "AuctionID", cascadeDelete: true);
        }
    }
}
