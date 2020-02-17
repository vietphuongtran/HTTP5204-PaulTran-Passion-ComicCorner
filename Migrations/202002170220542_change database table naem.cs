namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedatabasetablenaem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "CategoryHasPic", c => c.Int(nullable: false));
            AddColumn("dbo.Categories", "CategoryPicExtension", c => c.String());
            DropColumn("dbo.Categories", "HasPic");
            DropColumn("dbo.Categories", "PicExtension");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "PicExtension", c => c.String());
            AddColumn("dbo.Categories", "HasPic", c => c.Int(nullable: false));
            DropColumn("dbo.Categories", "CategoryPicExtension");
            DropColumn("dbo.Categories", "CategoryHasPic");
        }
    }
}
