namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultsFix : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AuctionHistory", "HistoryID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Users", "UserID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.BackOrders", "OrderID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.AuctionSearch", "AuctionID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.GoDaddyAccount", "AccountID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Auctions", "AuctionID", c => c.Guid(nullable: false, identity: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AuctionHistory", "HistoryID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Users", "UserID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.BackOrders", "OrderID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.AuctionSearch", "AuctionID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.GoDaddyAccount", "AccountID", c => c.Guid(nullable: false, identity: true));
            AlterColumn("dbo.Auctions", "AuctionID", c => c.Guid(nullable: false, identity: true));
        }
    }
}
