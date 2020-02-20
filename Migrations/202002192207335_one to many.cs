namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class onetomany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "CustomerId", c => c.Int());
            CreateIndex("dbo.Reviews", "CustomerId");
            AddForeignKey("dbo.Reviews", "CustomerId", "dbo.Customers", "CustomerId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Reviews", new[] { "CustomerId" });
            DropColumn("dbo.Reviews", "CustomerId");
        }
    }
}
