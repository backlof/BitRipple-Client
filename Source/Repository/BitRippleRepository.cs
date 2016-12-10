namespace Repository
{
    public class BitRippleRepository
    {
        public Context Context { get; set; }

        public DownloadRepository Downloader { get; private set; }
        public DataRepository Data { get; private set; }
        public TorrentFilterRepository Filter { get; set; }

        public BitRippleRepository(Context context)
        {
            Context = context;

            Downloader = new DownloadRepository(Context);
            Data = new DataRepository(Context);
            Filter = new TorrentFilterRepository(Context);
        }
    }
}