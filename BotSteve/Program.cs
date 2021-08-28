using System;
using static System.Console;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

namespace BotSteve
{
    class Program
    {
        private DiscordSocketClient client;
        private CommandService commands;


        static void Main(string[] args) => new Program().RunBotAsync().GetAwaiter().GetResult();
        
        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });
            client.Log += Log;

            commands = new CommandService();
            client.Ready += () =>
            {
                WriteLine("########## STARTED ##########");
                return Task.CompletedTask;
            };
            await InstallCommandsAsync(); //Attach command
            await client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordToken", EnvironmentVariableTarget.User));
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private Task Log(LogMessage arg)
        {
           WriteLine(arg.ToString());
            return Task.CompletedTask;
        }

        public async Task InstallCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task HandleCommandAsync(SocketMessage pMessage)
        {
            int argPos = 0;
            var message = (SocketUserMessage)pMessage;
            if (message == null) //Command cannot be null
                return;
            if (!message.HasCharPrefix('!', ref argPos)) //Command has to have '!' before the text
                return;
            var context = new SocketCommandContext(client, message);
            var result = await commands.ExecuteAsync(context, argPos, null);
            if (!result.IsSuccess) //Command is not valid
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
