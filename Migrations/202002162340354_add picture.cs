namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comics", "HasPic", c => c.Int(nullable: false));
            AddColumn("dbo.Comics", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comics", "PicExtension");
            DropColumn("dbo.Comics", "HasPic");
        }
    }
}
