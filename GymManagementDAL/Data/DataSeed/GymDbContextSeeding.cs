using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextSeeding
    {
        public static bool SeedData(GymDbContext dbContext)
        {
            try
            {
                var HasPlans = dbContext.Plans.Any();
                var HasCategories = dbContext.Categories.Any();

                if (HasCategories || HasPlans) return false;

                if (!HasPlans)
                {
                    var Plans = JsonToList<Plan>("plans.json");
                    if (Plans.Any())
                    {
                        dbContext.Plans.AddRange(Plans);
                    }
                }
                if (!HasCategories)
                {
                    var Categories = JsonToList<Category>("categories.json");
                    if (Categories.Any())
                    {
                        dbContext.Categories.AddRange(Categories);
                    }
                }

                return dbContext.SaveChanges() > 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed : {ex}");
                return false;   
            }


        }

        private static List<T> JsonToList<T>(string  filename)
        {
            // D:\Route_Aliaa_Tarek\Eng Aliaa Tarek\07 MVC\Projects\GymManagementSolution\GymManagementPL\wwwroot\Files\categories.json
            // D:\Route_Aliaa_Tarek\Eng Aliaa Tarek\07 MVC\Projects\GymManagementSolution\GymManagementPL => [Entry Point] Current Directory

            // Get The File
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", filename);
            // Check If The File Exists
            if (!File.Exists(FilePath)) throw new FileNotFoundException();

            // Read The Data
            string Data = File.ReadAllText(FilePath);

            // Serialize The Data
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();



        }


    }
}
