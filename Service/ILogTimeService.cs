using MyProject.Payload.Request;
using MyProject.Payload.Response;

namespace MyProject.Service
{
    public interface ILogTimeService
    {
        Task<List<LogTimeResponse>> GetByAccountId(int accountId);
        Task<LogTimeResponse?> GetById(int id);

        Task<bool> Add(int accountId, LogTimeRequest rq);
        Task<bool> Edit(int id, LogTimeRequest rq);
        Task<bool> Delete(int id);
    }
}

