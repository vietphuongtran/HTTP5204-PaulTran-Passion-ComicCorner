namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changingrelationshipsuccessfully : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Reviews", new[] { "CustomerId" });
            AlterColumn("dbo.Reviews", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Reviews", "CustomerId");
            AddForeignKey("dbo.Reviews", "CustomerId", "dbo.Customers", "CustomerId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Reviews", new[] { "CustomerId" });
            AlterColumn("dbo.Reviews", "CustomerId", c => c.Int());
            CreateIndex("dbo.Reviews", "CustomerId");
            AddForeignKey("dbo.Reviews", "CustomerId", "dbo.Customers", "CustomerId");
        }
    }
}
