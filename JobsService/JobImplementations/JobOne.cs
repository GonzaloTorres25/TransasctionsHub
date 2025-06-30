using _011Global.JobsService.JobInterfaces;
using _011Global.Shared;

namespace _011Global.JobsService.JobImplementations;

public class JobOne : Job, IJob
{
    protected override int IterationWaitTime { get { return 5000; } }
    public JobOne(CancellationTokenBase cancellationTokenBase, ILogger<JobOne> logger) : base(cancellationTokenBase,logger)
    {
       
    }

    

    protected override async Task WorkLoad(CancellationToken cancellationToken)
    { 
        
        logger.LogInformation($"{Name} my last run time was: {DateTime.Now}");

    }
} 