namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datatypeforcomicpricetodecimal : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comics", "ComicPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comics", "ComicPrice", c => c.Double(nullable: false));
        }
    }
}
