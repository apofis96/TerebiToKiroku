namespace TerebiToKiroku.Application.Interfaces
{
    public interface IVideoUniquenessChecker
    {
        Task<bool> IsUniqueAsync(
            string key,
            CancellationToken cancellationToken);
    }
}
