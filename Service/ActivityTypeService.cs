using System;
using Microsoft.EntityFrameworkCore;
using MyProject.AppData;
using MyProject.Models;
using MyProject.Payload.Request;
using MyProject.Payload.Response;

namespace MyProject.Service
{
    public class ActivityTypeService : IActivityTypeService
    {
        private readonly AppDBContext _context;

        public ActivityTypeService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<ActivityTypeResponse>> GetAll()
        {
            var activityTypeList = await _context.ActivityTypes.ToListAsync();

            var response = activityTypeList.Select(at => new ActivityTypeResponse
            {
                Id = at.Id,
                Name = at.Name
            }).ToList();

            return response;
        }

        public async Task<ActivityTypeResponse?> GetById(int id)
        {
            var activityType = await _context.ActivityTypes.FindAsync(id);

            if (activityType == null)
            {
                Console.WriteLine("Activity Type Not Found");
                return null;
            }

            var response = new ActivityTypeResponse
            {
                Id = activityType.Id,
                Name = activityType.Name,
            };

            return response;
        }

        public async Task<bool> Add(ActivityTypeRequest rq)
        {
            try
            {
                var activityType = new ActivityType() { Name = rq.Name };

                _context.ActivityTypes.Add(activityType);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> Edit(int id, ActivityTypeRequest rq)
        {
            try
            {
                var activityType = await _context.ActivityTypes.FirstAsync(a => a.Id == id);

                if (activityType == null)
                {
                    Console.WriteLine("Activity Type Not Found");
                    return false;
                }

                activityType.Name = rq.Name;

                _context.ActivityTypes.Update(activityType);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var activityType = await _context.ActivityTypes.FirstAsync(a => a.Id == id);

                if (activityType == null)
                {
                    Console.WriteLine("Activity Type Not Found");
                    return false;
                }

                _context.ActivityTypes.Remove(activityType);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> NameExist(string name)
        {
            return await _context.ActivityTypes.AnyAsync(at => at.Name == name);
        }
    }
}

