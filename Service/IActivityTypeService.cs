using System;
using MyProject.Payload.Request;
using MyProject.Payload.Response;

namespace MyProject.Service
{
    public interface IActivityTypeService
    {
        Task<List<ActivityTypeResponse>> GetAll();
        Task<ActivityTypeResponse?> GetById(int id);

        Task<bool> Add(ActivityTypeRequest rq);
        Task<bool> Edit(int id, ActivityTypeRequest rq);
        Task<bool> Delete(int id);

        Task<bool> NameExist(string name);
    }
}

