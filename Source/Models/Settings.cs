using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class Settings
    {
        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public IndexStorage NextIndex { get; set; }

        [DataMember]
        public int Interval { get; set; }

        [DataMember]
        public int TorrentDownloadCount { get; set; }

        [DataMember]
        public int FeedDownloadCount { get; set; }
    }

    [DataContract]
    public class IndexStorage
    {
        [DataMember]
        public int Filter { get; set; }

        [DataMember]
        public int Download { get; set; }

        [DataMember]
        public int Feed { get; set; }
    }
}