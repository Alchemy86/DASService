namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultsFix2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AuctionHistory", "HistoryID", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.Users", "UserID", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.BackOrders", "OrderID", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.AuctionSearch", "AuctionID", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.GoDaddyAccount", "AccountID", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
            AlterColumn("dbo.Auctions", "AuctionID", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
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
