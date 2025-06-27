namespace _011Global.JobsService.JobInterfaces;

public interface IJob
{
    public string Name { get; }
    public Task Start();
    public Task Stop();

}
