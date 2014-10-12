namespace AuthorizationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addconsumermodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConsumerModels",
                c => new
                    {
                        ConsumerId = c.Int(nullable: false, identity: true),
                        ConsumerKey = c.String(),
                        ConsumerSecret = c.String(),
                        RedirectUrl = c.String(),
                    })
                .PrimaryKey(t => t.ConsumerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ConsumerModels");
        }
    }
}
