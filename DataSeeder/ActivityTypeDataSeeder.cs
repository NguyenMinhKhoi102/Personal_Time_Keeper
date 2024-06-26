using System;
using MyProject.AppData;
using MyProject.Service;
using MyProject.Payload;

namespace MyProject.DataSeeder
{
    public class ActivityTypeDataSeeder
    {
        public ActivityTypeDataSeeder()
        {
        }

        public static async Task SeedDataBase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var dbContext = services.GetRequiredService<AppDBContext>();
            var activityTypeService = services.GetRequiredService<IActivityTypeService>();

            if (!dbContext.ActivityTypes.Any())
            {
                await activityTypeService.Add(new Payload.Request.ActivityTypeRequest { Name = "Fix Bug" });
                await activityTypeService.Add(new Payload.Request.ActivityTypeRequest { Name = "QA" });
                await activityTypeService.Add(new Payload.Request.ActivityTypeRequest { Name = "Research" });
                await activityTypeService.Add(new Payload.Request.ActivityTypeRequest { Name = "Design UX/UI" });
                await activityTypeService.Add(new Payload.Request.ActivityTypeRequest { Name = "Public Holiday" });
            }
        }
    }
}

