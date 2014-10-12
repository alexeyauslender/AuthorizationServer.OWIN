namespace AuthorizationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthenticationTicketModels",
                c => new
                    {
                        ContextToken = c.Guid(nullable: false),
                        AuthenticationTicket = c.String(),
                    })
                .PrimaryKey(t => t.ContextToken);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuthenticationTicketModels");
        }
    }
}
