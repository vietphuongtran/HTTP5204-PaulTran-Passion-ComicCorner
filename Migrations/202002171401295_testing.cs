namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testing : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "ComicId", "dbo.Comics");
            DropIndex("dbo.Comments", new[] { "ComicId" });
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
            
            DropTable("dbo.Comments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        CommentContent = c.String(),
                        ComicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId);
            
            DropForeignKey("dbo.Reviews", "ComicId", "dbo.Comics");
            DropIndex("dbo.Reviews", new[] { "ComicId" });
            DropTable("dbo.Reviews");
            CreateIndex("dbo.Comments", "ComicId");
            AddForeignKey("dbo.Comments", "ComicId", "dbo.Comics", "ComicId", cascadeDelete: true);
        }
    }
}
