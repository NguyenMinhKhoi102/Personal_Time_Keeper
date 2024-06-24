using System;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using MyProject.AppData;
using MyProject.Models;
using MyProject.Payload.Request;
using MyProject.Payload.Response;

namespace MyProject.Service
{
    public class LogTimeService : ILogTimeService
    {
        private readonly AppDBContext _context;

        public LogTimeService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(int accountId, LogTimeRequest rq)
        {
            try
            {
                var account = await _context.Accounts.FindAsync(accountId);

                if (account == null)
                {
                    Console.WriteLine("Account not found");
                    return false;
                }

                var activityType = await _context.ActivityTypes.FindAsync(rq.ActivityTypeId);
                if (activityType == null)
                {
                    Console.WriteLine("Activity type not found");
                    return false;
                }

                var logTime = new LogTime()
                {
                    LogDate = rq.LogDate,
                    Duration = rq.Duration,
                    ActivityTypeId = rq.ActivityTypeId,
                    Description = rq.Description,
                    AccountId = accountId,
                    ActivityType = activityType,
                    Account = account,
                };

                _context.LogTimes.Add(logTime);
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
                var logTime = await _context.LogTimes.FirstOrDefaultAsync(a => a.Id == id);

                if (logTime == null)
                {
                    Console.WriteLine("Log time not found");
                    return false;
                }

                _context.LogTimes.Remove(logTime);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> Edit(int id, LogTimeRequest rq)
        {
            try
            {
                var logTime = await _context.LogTimes.FirstOrDefaultAsync(a => a.Id == id);

                if (logTime == null)
                {
                    Console.WriteLine("Log time not found");
                    return false;
                }

                var activityType = await _context.ActivityTypes.FindAsync(rq.ActivityTypeId);
                if (activityType == null)
                {
                    Console.WriteLine("Activity type not found");
                    return false;
                }

                logTime.LogDate = rq.LogDate;
                logTime.Duration = rq.Duration;
                logTime.ActivityTypeId = rq.ActivityTypeId;
                logTime.ActivityType = activityType;
                logTime.Description = rq.Description;

                _context.LogTimes.Update(logTime);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<List<LogTimeResponse>> GetByAccountId(int accountId)
        {
            var account = await _context.Accounts.Include(a => a.LogTimes).ThenInclude(lt => lt.ActivityType).FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null || account.LogTimes == null)
                return new List<LogTimeResponse>();

            var response = account.LogTimes.Select(at => new LogTimeResponse
            {
                Id = at.Id,
                LogDate = at.LogDate,
                Duration = at.Duration,
                ActivityTypeId = at.ActivityType.Id,
                ActivityTypeName = at.ActivityType.Name,
                Description = at.Description
            }).ToList();

            return response;
        }

        public async Task<LogTimeResponse?> GetById(int id)
        {
            var logTime = await _context.LogTimes.FirstOrDefaultAsync(a => a.Id == id);

            if (logTime == null)
                return null;

            var response = new LogTimeResponse()
            {
                Id = logTime.Id,
                LogDate = logTime.LogDate,
                Duration = logTime.Duration,
                ActivityTypeId = logTime.ActivityType.Id,
                ActivityTypeName = logTime.ActivityType.Name,
                Description = logTime.Description
            };

            return response;
        }
    }
}

