namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingreviewstableanditsforeignkey : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        ReviewContent = c.String(),
                        ComicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.Comics", t => t.ComicId, cascadeDelete: true)
                .Index(t => t.ComicId);
            
            AddColumn("dbo.Categories", "HasPic", c => c.Int(nullable: false));
            AddColumn("dbo.Categories", "PicExtension", c => c.String());
            DropColumn("dbo.Categories", "CategoryHasPic");
            DropColumn("dbo.Categories", "CategoryPicExtension");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "CategoryPicExtension", c => c.String());
            AddColumn("dbo.Categories", "CategoryHasPic", c => c.Int(nullable: false));
            DropForeignKey("dbo.Reviews", "ComicId", "dbo.Comics");
            DropIndex("dbo.Reviews", new[] { "ComicId" });
            DropColumn("dbo.Categories", "PicExtension");
            DropColumn("dbo.Categories", "HasPic");
            DropTable("dbo.Reviews");
        }
    }
}
