using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Auth.Data.Identity
{
    public static class AppIdentityDbContextSeed
    {
      
        public static async Task SeedUserAsync(UserManager<IdentityUser> userManager , ApplicationDbContext dbContext)
        {

            if (!userManager.Users.Any()) {
                var User = new IdentityUser
                {
                    UserName = "Kamal",
                    Email = "Kamal@gmail.com",
                    PhoneNumber = "01156053262"
                };

                await userManager.CreateAsync(User, "Pa$$w0rd");
            }
            if (!dbContext.Questions.Any())
            {
                var path = Path.GetFullPath("../Auth/Data/Seed/Questions.json");
                Console.WriteLine(path); 

                var questionsJson = File.ReadAllText(path);
                Console.WriteLine(questionsJson); 

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    ReadCommentHandling = JsonCommentHandling.Skip, 
                    AllowTrailingCommas = true, 
                    IgnoreNullValues = true, 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                };

                try
                {
                    var questionsList = JsonSerializer.Deserialize<List<Question>>(questionsJson, options);

                    if (questionsList?.Count > 0)
                    {
                        foreach (var question in questionsList)
                        {
                            question.Id = 0;

                            if (question.Options?.Count > 0)
                            {
                                foreach (var option in question.Options)
                                {
                                    option.Id = 0; 
                                    option.QuestionId = question.Id; 
                                }
                            }
                        }

                       
                        dbContext.Questions.AddRange(questionsList);
                        dbContext.SaveChanges();
                    }
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"خطأ في تحويل JSON: {ex.Message}");
                }
            }
        }

       
    }
}
