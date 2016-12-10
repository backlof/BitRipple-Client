using System;
using Models;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Repository
{
    public class WebTorrentDownloader : ITorrentDownloader
    {
        public void Download(Download download, string location)
        {
            using (var request = GetRequest(download.FileUrl).GetResponse())
            {
                using (var filewriter = GetBinaryWriter(location, CleanInvalidFileNameCharacters(download.Name) + ".torrent"))
                {
                    filewriter.Write(GetBytes(request));
                }
            }
        }

        private byte[] GetBytes(WebResponse wr, byte[] buffer = null)
        {
            using (var responseStream = GetStream(wr))
            {
                buffer = new byte[4096];

                using (var memoryStream = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = responseStream.Read(buffer, 0, buffer.Length);
                        memoryStream.Write(buffer, 0, count);
                    }
                    while (count != 0);

                    return memoryStream.ToArray();
                }
            }
        }

        private string CleanInvalidFileNameCharacters(string fileName)
        {
            return new string(fileName.Where(x => !Path.GetInvalidFileNameChars().Contains(x)).ToArray());
        }

        private BinaryWriter GetBinaryWriter(string location, string filename)
        {
            return new BinaryWriter(new FileStream(Path.Combine(location, filename), FileMode.Create));
        }

        private FileStream GetFileStream(string location, string filename)
        {
            return new FileStream(Path.Combine(location, filename), FileMode.Create);
        }

        private WebRequest GetRequest(string url, WebRequest wr = null)
        {
            wr = WebRequest.Create(url);
            wr.Timeout = 4000;
            wr.ContentType = "application/x-bittorrent";
            return wr;
        }

        private Stream GetStream(WebResponse response)
        {
            return IsGzipped(response) ?
                new GZipStream(response.GetResponseStream(), CompressionMode.Decompress) :
                response.GetResponseStream();
        }

        private bool IsGzipped(WebResponse response)
        {
            return response.Headers["Content-Encoding"] == "gzip";
        }
    }
}