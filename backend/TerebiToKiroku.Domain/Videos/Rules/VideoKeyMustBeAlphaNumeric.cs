using System.Text.RegularExpressions;
using TerebiToKiroku.Domain.SeedWork;

namespace TerebiToKiroku.Domain.Videos.Rules
{
    public class VideoKeyMustBeAlphaNumeric : IBusinessRule
    {
        private readonly string _key;
        private static readonly Regex rule = new("(.*?)(^|\\/|v=)([A-Za-z0-9_-]{11})(.*)?");

        public VideoKeyMustBeAlphaNumeric(string key)
        {
            _key = key;
        }

        public bool IsBroken() => !rule.IsMatch(_key);

        public string Message => "Key must be valid y_video id";
    }
}
