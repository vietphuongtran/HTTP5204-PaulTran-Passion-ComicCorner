namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpictocategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "HasPic", c => c.Int(nullable: false));
            AddColumn("dbo.Categories", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Categories", "PicExtension");
            DropColumn("dbo.Categories", "HasPic");
        }
    }
}
