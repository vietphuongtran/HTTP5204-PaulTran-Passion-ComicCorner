namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changingpricetofloat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comics", "ComicPrice", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comics", "ComicPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
