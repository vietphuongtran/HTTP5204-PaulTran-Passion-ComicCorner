namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComicCategories",
                c => new
                    {
                        Comic_ComicId = c.Int(nullable: false),
                        Category_CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Comic_ComicId, t.Category_CategoryId })
                .ForeignKey("dbo.Comics", t => t.Comic_ComicId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryId, cascadeDelete: true)
                .Index(t => t.Comic_ComicId)
                .Index(t => t.Category_CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComicCategories", "Category_CategoryId", "dbo.Categories");
            DropForeignKey("dbo.ComicCategories", "Comic_ComicId", "dbo.Comics");
            DropIndex("dbo.ComicCategories", new[] { "Category_CategoryId" });
            DropIndex("dbo.ComicCategories", new[] { "Comic_ComicId" });
            DropTable("dbo.ComicCategories");
        }
    }
}
