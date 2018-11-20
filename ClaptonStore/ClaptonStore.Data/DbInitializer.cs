namespace ClaptonStore.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Dto;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Identity;
    using Newtonsoft.Json;

    public static class DbInitializer
    {
        public static async void Seed(IServiceProvider serviceProvider, string jsonString)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope
                    .ServiceProvider
                    .GetService<ClaptonStoreContext>();

                context.Database.Migrate();

                if (context.Database.GetPendingMigrations().Any())
                {
                    return;
                }

                if (!context.Users.Any())
                {
                    var roleManager = serviceScope
                        .ServiceProvider
                        .GetService<RoleManager<IdentityRole>>();

                    var userManager = serviceScope
                        .ServiceProvider
                        .GetService<UserManager<ApplicationUser>>();

                    var deserializedUser = JsonConvert.DeserializeObject<UserDto[]>(jsonString);

                    foreach (var userDto in deserializedUser)
                    {
                        await CreateUser(userManager, userDto.Username, userDto.Email, userDto.Password);
                    }

                    await CreateRole(roleManager, "Administrators");
                    await AddUserToRole(userManager, "admin@gmail.com", "Administrators");

                    context.SaveChanges();
                }
            }
        }

        private static async Task CreateUser(
            UserManager<ApplicationUser> userManager,
            string username,
            string email, 
            string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = email
            };

            var userCreateResult = await userManager.CreateAsync(user, password);

            if (!userCreateResult.Succeeded)
            {
                throw new Exception(string.Join("; ", userCreateResult.Errors));
            }
        }

        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var roleCreateResult = await roleManager
                .CreateAsync(new IdentityRole(roleName));

            if (!roleCreateResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join("; ", roleCreateResult.Errors));
            }
        }

        private static async Task AddUserToRole(
            UserManager<ApplicationUser> userManager, 
            string username, 
            string roleName)
        {
            var user = await userManager
                .FindByEmailAsync(username);

            var addRoleResult = await userManager
                .AddToRoleAsync(user, roleName);

            if (!addRoleResult.Succeeded)
            {
                throw new InvalidOperationException(string.Join("; ", addRoleResult.Errors));
            }
        }
    }
}