
namespace _011Global.Shared.JobsServiceDBContext.Interfaces;

public interface IJobsServiceRepository
{
    public Task<Dictionary<string, GlobalJob>> GetJobs(string hostName);

}

