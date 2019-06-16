namespace NorthwindEF.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NorthwindEF.ado.Northwind>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NorthwindEF.ado.Northwind context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Categories.AddOrUpdate(new ado.Category() { CategoryName = "Backpack", Description = "I am backpack" });
            context.Regions.AddOrUpdate(new ado.Region() { RegionDescription = "San-Andreas" });
            context.Territories.AddOrUpdate(new ado.Territory() { TerritoryDescription = "Terraria", RegionID = 1, TerritoryID = "test" });
        }
    }
}
