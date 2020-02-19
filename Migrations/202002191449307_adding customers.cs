namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingcustomers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        CustomerEmail = c.String(),
                        CustomerAddress = c.Int(nullable: false),
                        ComicPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HasPic = c.Int(nullable: false),
                        PicExtension = c.String(),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            AlterColumn("dbo.Comics", "ComicPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comics", "ComicPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropTable("dbo.Customers");
        }
    }
}
