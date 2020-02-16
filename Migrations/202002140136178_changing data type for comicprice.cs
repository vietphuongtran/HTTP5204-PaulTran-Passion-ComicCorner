namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changingdatatypeforcomicprice : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comics", "ComicPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comics", "ComicPrice", c => c.Int(nullable: false));
        }
    }
}
