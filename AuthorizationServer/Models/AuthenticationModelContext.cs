﻿using System.Data.Entity;
using Authorization.Models;

namespace AuthorizationServer.Models
{
    public class AuthenticationModelContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public AuthenticationModelContext() : base("name=AuthenticationModelContext")
        {
        }

        public DbSet<AuthenticationTicketModel> AuthenticationTicketModels { get; set; }

        public DbSet<ConsumerModel> ConsumerModels { get; set; }
    }
}