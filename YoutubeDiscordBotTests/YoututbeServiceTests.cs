using Discord.Audio;
using YoutubeDiscordBot.Services;
using Xunit;

namespace YoutubeDiscordBotTests
{
    public class YoututbeServiceTests
    {
        [Fact]
        public async Task youtube_search_gives_correct_streamAsync()
        {
            var youtubeService = new YoutubeService();

            var stream = await youtubeService.GetAudioStream("xtrullor gold");

            using (var file = new FileStream("C:\\Users\\franf\\Desktop\\audiostream.bin", FileMode.CreateNew))
            {
                stream.CopyTo(file);
            }

            Assert.NotNull(stream);
        }
    }
}
