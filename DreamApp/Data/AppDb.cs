using ApiAUth.Models;
using DreamApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WbToken.AppDbContext
{
    public class AppDb :IdentityDbContext
    {
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Category> Category { get; set; }


        public AppDb(DbContextOptions<AppDb> options) : base(options)
        {

        }
        
    }
}
