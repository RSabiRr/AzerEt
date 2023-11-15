using Microsoft.AspNetCore.Identity;

namespace AzerEt
{
    public class SeedData
    {



        public static class RoleInitializer
        {
            public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
            {
                // İlgili rol zaten varsa işlemi atlayın
                if (await roleManager.RoleExistsAsync("Admin")) return;

                // Yeni bir rol oluşturun
                var adminRole = new IdentityRole("Admin");
                await roleManager.CreateAsync(adminRole);
            }
        }
    }
}
