namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                        CategoryDesc = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Comics",
                c => new
                    {
                        ComicId = c.Int(nullable: false, identity: true),
                        ComicName = c.String(),
                        ComicDesc = c.String(),
                        ComicYear = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ComicId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Comics");
            DropTable("dbo.Categories");
        }
    }
}
