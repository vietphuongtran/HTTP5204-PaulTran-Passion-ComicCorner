namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteaddprice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comics", "ComicPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comics", "ComicPrice");
        }
    }
}
