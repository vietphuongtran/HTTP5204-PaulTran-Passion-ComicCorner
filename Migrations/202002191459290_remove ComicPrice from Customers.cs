namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeComicPricefromCustomers : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Customers", "ComicPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "ComicPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
