namespace ComicCorner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changingdatatypeofcustomeradd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "CustomerAddress", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "CustomerAddress", c => c.Int(nullable: false));
        }
    }
}
