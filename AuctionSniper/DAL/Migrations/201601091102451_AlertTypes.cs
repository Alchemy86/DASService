namespace DAL.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AlertTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Alerts", "AlertType", c => c.String(maxLength: 50));
            Sql("UPDATE dbo.Alerts SET AlertType = CASE [Description] WHEN 'WIN ALERT' THEN 'Win' WHEN '1 Hour Alert' THEN 'Reminder1Hour' WHEN '12 Hour Alert' THEN 'Reminder12Hours' END WHERE AlertType IS NULL");
            AlterColumn("dbo.Alerts", "AlertType", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Alerts", "AlertType");
        }
    }
}
