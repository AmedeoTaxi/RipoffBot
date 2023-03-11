using Discord;

namespace YoutubeDiscordBot.Services.Logging
{
    public interface ILogger
    {
        public Task DiscordLog(LogMessage msg);

        public void WriteLog(string msg);
    }
    public class Logger : ILogger
    {
        public Task DiscordLog(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public void WriteLog(string msg)
        {
            Console.WriteLine(DateTime.Now.TimeOfDay.ToString() + " " + msg);
            return;
        }
    }
}
