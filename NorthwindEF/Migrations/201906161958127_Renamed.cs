namespace NorthwindEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renamed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "AnotherOneFoundationDate", c => c.DateTime());        
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "AnotherOneFoundationDate");
            RenameTable("Regionz", "Regions");
        }
    }
}
