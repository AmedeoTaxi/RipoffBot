using System.Configuration;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos.Streams;

namespace YoutubeDiscordBot.Services
{
    public interface IYoutubeService
    {
        public Task<Stream> GetAudioStream(string query);
        public Task<IStreamInfo> GetAudioStreamInfo(string query);
        public Task CopyTo(Stream dest, IStreamInfo streamInfo);
    }
    public class YoutubeService : IYoutubeService
    {
        private readonly YoutubeClient _client;
        public YoutubeClient Client
        {
            get { return _client; }
        }

        public YoutubeService()
        {
            _client = new YoutubeClient();
        }

        //==========================================================
        //Don't use
        public async Task<Stream> GetAudioStream(string query)
        {
            var streamInfo = await GetAudioStreamInfo(query);

            var path = ConfigurationManager.AppSettings["filePath"];

            await _client.Videos.Streams.DownloadAsync(streamInfo, $"{path}lom.{streamInfo.Container}");

            var streamTask = _client.Videos.Streams.GetAsync(streamInfo);

            while(!streamTask.IsCompleted)
            {
                Thread.Sleep(500);
            }

            return streamTask.Result;
        }

        public async Task<IStreamInfo> GetAudioStreamInfo(string query)
        {
            var searchResult = await _client.Search.GetVideosAsync(query).FirstAsync();

            var manifest = await _client.Videos.Streams.GetManifestAsync(searchResult.Id);

            var streamInfo = manifest.GetAudioOnlyStreams().GetWithHighestBitrate();

            var path = ConfigurationManager.AppSettings["filePath"];

            await _client.Videos.Streams.DownloadAsync(streamInfo, $"{path}lom.{streamInfo.Container}");

            return streamInfo;
        }

        public async Task CopyTo(Stream dest, IStreamInfo streamInfo)
        {
            await _client.Videos.Streams.CopyToAsync(streamInfo, dest);
        }
    }
}
