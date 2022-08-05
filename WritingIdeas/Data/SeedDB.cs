using WritingIdeas.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace WritingIdeas.Data
{
    public class SeedDB
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPass)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                string ID = await EnsureUser(serviceProvider, testUserPass, "test@test.com");
                await EnsureRole(serviceProvider, ID, Constants.AdministratorRole);
            }
        }
        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUser, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                
            }
            var result = await userManager.CreateAsync(user, testUser);
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.FirstOrDefault().ToString());
            }
            if (user == null)
            {
                throw new Exception("User is missing");
            }
            string uid = user.Id;
            return uid;           
        }
        private static async Task<IdentityResult>EnsureRole(IServiceProvider serviceProvider,
                                                              string uid, string role)
        {
            IdentityResult identityResult;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if (roleManager == null)
                throw new Exception("Role manager is null");
            if (!await roleManager.RoleExistsAsync(role))
                identityResult = await roleManager.CreateAsync(new IdentityRole(role));
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            var user = await userManager.FindByIdAsync(uid);
            if (user == null)
                throw new Exception("User is missing");
            identityResult=await userManager.AddToRoleAsync(user, role);
            return identityResult;
        }
    }
}
