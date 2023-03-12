using Discord.Audio;
using YoutubeDiscordBot.Services;
using Xunit;
using System.Configuration;

namespace YoutubeDiscordBotTests
{
    public class YoututbeServiceTests
    {
        [Fact]
        public async Task youtube_search_gives_correct_streamAsync()
        {
            var youtubeService = new YoutubeService();

            var stream = await youtubeService.GetAudioStream("xtrullor gold");

            var path = ConfigurationManager.AppSettings["filePath"];

            using (var file = new FileStream($"{path}audiostream.bin", FileMode.CreateNew))
            {
                stream.CopyTo(file);
            }

            Assert.NotNull(stream);
        }
    }
}
