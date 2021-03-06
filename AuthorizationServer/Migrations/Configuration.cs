using System.Data.Entity.Migrations;
using AuthorizationServer.Models;

namespace AuthorizationServer.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AuthenticationModelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AuthorizationServer.Models.AuthenticationModelContext";
        }

        protected override void Seed(AuthenticationModelContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}