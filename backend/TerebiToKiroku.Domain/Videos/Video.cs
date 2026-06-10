using TerebiToKiroku.Domain.SeedWork;

namespace TerebiToKiroku.Domain.Videos
{
    public class Video : Entity, IAggregateRoot
    {
        public VideoId Id { get; private set; }

        public string Name { get; private set; }

        public string Key { get; private set; }

        private Video() {}

        public Video(string key, string name)
        {
            Id = new VideoId(Guid.NewGuid());
            Key = key;
            Name = name;
        }
    }
}