namespace NorthwindEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditCards : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        CardID = c.Int(nullable: false, identity: true),
                        CardHolder = c.String(maxLength: 60),
                        ExperationDate = c.DateTime(nullable: false),
                        EmployeeID = c.Int(),
                    })
                .PrimaryKey(t => t.CardID)
                .ForeignKey("dbo.Employees", t => t.EmployeeID)
                .Index(t => t.EmployeeID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditCards", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.CreditCards", new[] { "EmployeeID" });
            DropTable("dbo.CreditCards");
        }
    }
}
