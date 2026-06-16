using TerebiToKiroku.Domain.SeedWork;

namespace TerebiToKiroku.Domain.Videos.Rules
{
    public class VideoKeyMustBeUniqueRule : IBusinessRule
    {
        private readonly IVideoUniquenessChecker _videoUniquenessChecker;

        private readonly string _key;

        public VideoKeyMustBeUniqueRule(
            IVideoUniquenessChecker videoUniquenessChecker,
            string key)
        {
            _videoUniquenessChecker = videoUniquenessChecker;
            _key = key;
        }

        public bool IsBroken() => !_videoUniquenessChecker.IsUnique(_key);

        public string Message => "Video with this key already exists.";
    }
}
