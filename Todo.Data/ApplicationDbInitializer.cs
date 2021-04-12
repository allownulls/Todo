using Microsoft.AspNetCore.Identity;

namespace Todo.Data
{
    public static class ApplicationDbInitializer
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            if (userManager.FindByEmailAsync("abc@xyz.com").Result == null)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = "abc@xyz.com",
                    Email = "abc@xyz.com",
                    EmailConfirmed = true
                };

                IdentityResult result = userManager.CreateAsync(user, "Qwe123%").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }
            }
        }
    }
}
