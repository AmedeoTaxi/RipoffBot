using AngleSharp.Io;
using Discord;
using Discord.Audio;
using Discord.Audio.Streams;
using Discord.Commands;
using YoutubeDiscordBot.Services;
using YoutubeDiscordBot.Services.Logging;
using System.Diagnostics;
using YoutubeExplode.Videos.Streams;
using System.Configuration;

namespace YoutubeDiscordBot.Commands.Modules
{
    public class Module : ModuleBase<SocketCommandContext>
    {
        private readonly IYoutubeService _youtubeService;
        private readonly ILogger _logger;

        public Module(IYoutubeService youtubeService, ILogger logger) : base()
        {
            _youtubeService = youtubeService;
            _logger = logger;
        }

        [Command("!p", RunMode = RunMode.Async)]
        public async Task Play([Remainder]string query = "")
        {
            var channel = (Context.User as IGuildUser)?.VoiceChannel;
            if (channel == null)
            {
                await Context.Channel.SendMessageAsync("User must be in a voice channel");
                return;
            }

            if (string.IsNullOrEmpty(query))
            {
                await Context.Channel.SendMessageAsync("Search message is empty");
                return;
            }

            await Context.Channel.SendMessageAsync($"Cerco: \"{query}\" sul tubo");

            //var convertedStream = ConvertStream(audioStream);

            var audioStreamInfo = await _youtubeService.GetAudioStreamInfo(query);

            _logger.WriteLog("Got audio stream");

            //==========================================================================================================
            // For the next step with transmitting audio, you would want to pass this Audio Client in to a service.
            var audioClient = await channel.ConnectAsync();

            using (var ffmpeg = ConvertStream(audioStreamInfo))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var discordStream = audioClient.CreatePCMStream(AudioApplication.Music))
            {
                _logger.WriteLog("Decoded, trying to copy stream");

                try
                {
                    await output.CopyToAsync(discordStream);
                }
                catch (Exception e)
                {
                    _logger.WriteLog(e.ToString());
                }
                finally
                {
                    await discordStream.FlushAsync();
                }
            }

            Thread.Sleep(10000);
            await channel.DisconnectAsync();
        }

        #region Buono

        public Process ConvertStream(IStreamInfo streamInfo)
        {
            //if (!stream.CanRead)
            //{
            //    _logger.WriteLog("Cannot read stream");
            //    throw new Exception();
            //}

            var path = ConfigurationManager.AppSettings["filePath"];

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}lom.{streamInfo.Container}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            _logger.WriteLog("I'm decoding");

            var process = new Process
            {
                StartInfo = processStartInfo
            };


            try
            {
                _logger.WriteLog($"The process is {process.Start()} that is started");

                #region Using Streams

                //Stream inputStream = process.StandardInput.BaseStream;

                //byte[] buffer = new byte[1];

                //List<byte> bytes = new List<byte>();

                //for (int i = 0; i < stream.Length; i++)
                //{
                //    stream.Read(buffer, i, 1);
                //    bytes.Add(buffer[0]);
                //}


                //inputStream.Write(buffer, 0, buffer.Length);

                //Stream stream = new FileStream($"{path}lom.{streamInfo.Container}", FileMode.Open);

                //stream.CopyTo(inputStream);

                //process.StandardInput.Close();

                #endregion
            }
            catch (Exception e)
            {
                _logger.WriteLog(e.ToString());
            }

            _logger.WriteLog("I, tecnically, ended decoding");

            return process;
        }

        #endregion

        // Probably doesn't work
        //private async Task<Stream> CreateStream(string path)
        //{
        //    IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(path);

        //    IStream audioStream = mediaInfo.AudioStreams.FirstOrDefault().SetChannels(2);

        //    return (Stream)FFmpeg.Conversions.New().AddStream(audioStream)
        //        .SetAudioBitrate(48000)
        //        .SetOutputFormat(Xabe.FFmpeg.Format.s16le);
        //}
    }
}
