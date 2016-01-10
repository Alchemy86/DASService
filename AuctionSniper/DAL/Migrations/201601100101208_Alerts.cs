namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alerts : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Alerts", "AuctionID");
            AddForeignKey("dbo.Alerts", "AuctionID", "dbo.Auctions", "AuctionID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Alerts", "AuctionID", "dbo.Auctions");
            DropIndex("dbo.Alerts", new[] { "AuctionID" });
        }
    }
}
