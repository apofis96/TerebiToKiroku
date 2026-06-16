namespace TerebiToKiroku.Domain.Videos
{
    public interface IVideoUniquenessChecker
    {
        bool IsUnique(string videoKey);
    }
}