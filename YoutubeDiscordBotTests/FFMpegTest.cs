using YoutubeDiscordBot.Commands.Modules;
using YoutubeDiscordBot.Services;
using YoutubeDiscordBot.Services.Logging;
using Xunit;

namespace YoutubeDiscordBotTests
{
    public class FFMpegTest
    {
        [Fact]
        public async Task can_convert_stream()
        {
            IYoutubeService _youtubeService = new YoutubeService();
            ILogger _logger = new Logger();
            var module = new Module(_youtubeService, _logger);

            var processResult = module.ConvertStream(await _youtubeService.GetAudioStreamInfo("xtrullor gold"));
        }
    }
}
