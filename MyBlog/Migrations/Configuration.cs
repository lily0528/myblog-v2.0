namespace MyBlog.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MyBlog.Models;
    using MyBlog.Models.Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyBlog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;


        }

        protected override void Seed(MyBlog.Models.ApplicationDbContext context)
        {
            var roleManager =
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

            //UserManager, used to manage users
            var userManager =
                new UserManager<ApplicationUser>(
                        new UserStore<ApplicationUser>(context));

            //Adding admin role if it doesn't exist.
            if (!context.Roles.Any(p => p.Name == "Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                roleManager.Create(adminRole);
            }

            //Creating the adminuser
            ApplicationUser adminUser;

            if (!context.Users.Any(
                p => p.UserName == "admin@blog.com"))
            {
                adminUser = new ApplicationUser();
                adminUser.UserName = "admin@blog.com";
                adminUser.Email = "admin@blog.com";

                userManager.Create(adminUser, "Password-1");
            }
            else
            {
                adminUser = context
                    .Users
                    .First(p => p.UserName == "admin@blog.com");
            }

            //Make sure the user is on the admin role
            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }

            //Adding Moderator role if it doesn't exist.
            if (!context.Roles.Any(p => p.Name == "Moderator"))
            {
                var ModeratorRole = new IdentityRole("Moderator");
                roleManager.Create(ModeratorRole);
            }

            //Creating the user
            ApplicationUser userModerator;

            if (!context.Users.Any(
                p => p.UserName == "moderator@blog.com"))
            {
                userModerator = new ApplicationUser();
                userModerator.UserName = "moderator@blog.com";
                userModerator.Email = "moderator@blog.com";

                userManager.Create(userModerator, "Password-1");
            }
            else
            {
                userModerator = context
                    .Users
                    .First(p => p.UserName == "moderator@blog.com");
            }

            //Make sure the user is on the Moderator role
            if (!userManager.IsInRole(userModerator.Id, "Moderator"))
            {
                userManager.AddToRole(userModerator.Id, "Moderator");
            }

            var blog = new Blog();
            blog.Title = "Why I love Spring: A short story";
            blog.Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent vel tortor facilisis, volutpat nulla placerat, tincidunt mi. Nullam vel orci dui. Suspendisse sit amet laoreet neque. Fusce sagittis suscipit sem a consequat. Proin nec interdum sem. Quisque in porttitor magna, a imperdiet est.";
            blog.Published = true;
            blog.DateCreated = DateTime.Now;
            blog.DateUpdated = DateTime.Now;
            blog.MediaUrl = "~/Upload/1.png";
            blog.Slug = "kk";
            blog.UserId = adminUser.Id;

            context.Blogs.AddOrUpdate(p => p.Title, blog);
            context.SaveChanges();

            var blogitem2 = new Blog();
            blogitem2.Title = "Why I love Summer: A short story1";
            blogitem2.Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent vel tortor facilisis, volutpat nulla placerat, tincidunt mi. Nullam vel orci dui. Suspendisse sit amet laoreet neque. Fusce sagittis suscipit sem a consequat. Proin nec interdum sem. Quisque in porttitor magna, a imperdiet est.";
            blogitem2.Published = true;
            blogitem2.DateCreated = DateTime.Now;
            blogitem2.DateUpdated = DateTime.Now;
            blogitem2.MediaUrl = "~/Upload/2.jpg";
            blogitem2.Slug = "kk01";
            blogitem2.UserId = adminUser.Id;

            context.Blogs.AddOrUpdate(p => p.Title, blogitem2);
            context.SaveChanges();

            var blogitem3 = new Blog();
            blogitem3.Title = "Why I love Fall: A short story";
            blogitem3.Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent vel tortor facilisis, volutpat nulla placerat, tincidunt mi. Nullam vel orci dui. Suspendisse sit amet laoreet neque. Fusce sagittis suscipit sem a consequat. Proin nec interdum sem. Quisque in porttitor magna, a imperdiet est.";
            blogitem3.Published = true;
            blogitem3.DateCreated = DateTime.Now;
            blogitem3.DateUpdated = DateTime.Now;
            blogitem3.MediaUrl = "~/Upload/3.jpg";
            blogitem3.Slug = "kk03";
            blogitem3.UserId = adminUser.Id;

            context.Blogs.AddOrUpdate(p => p.Title, blogitem3);
            context.SaveChanges();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
