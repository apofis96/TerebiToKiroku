namespace TerebiToKiroku.Application.Interfaces
{
    public interface IVideoExistenceChecker
    {
        Task<bool> IsExistAsync(string key, CancellationToken cancellationToken);
    }
}
