namespace WebApplication1.Migrations
{
    using Models;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication1.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApplication1.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Genre.AddOrUpdate(g => g.Name,
                new Genre { Name = "Jazz" },
                new Genre { Name = "Blues" },
                new Genre { Name = "Rock" },
                new Genre { Name = "Country" }
            );
        }
    }
}
