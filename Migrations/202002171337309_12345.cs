namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12345 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "ComicId", "dbo.Comics");
            DropIndex("dbo.Reviews", new[] { "ComicId" });
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentId = c.Int(nullable: false, identity: true),
                        CommentContent = c.String(),
                        ComicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentId)
                .ForeignKey("dbo.Comics", t => t.ComicId, cascadeDelete: true)
                .Index(t => t.ComicId);
            
            DropTable("dbo.Reviews");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        ReviewContent = c.String(),
                        ComicId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewId);
            
            DropForeignKey("dbo.Comments", "ComicId", "dbo.Comics");
            DropIndex("dbo.Comments", new[] { "ComicId" });
            DropTable("dbo.Comments");
            CreateIndex("dbo.Reviews", "ComicId");
            AddForeignKey("dbo.Reviews", "ComicId", "dbo.Comics", "ComicId", cascadeDelete: true);
        }
    }
}
