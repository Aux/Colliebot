using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tomlyn.Extensions.Configuration;

namespace Colliebot.Discord
{
    internal class Program
    {
        private readonly IConfiguration _config;
        private readonly IServiceProvider _services;
        public Program()
        {
            var discordSocketConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers,
                AlwaysDownloadUsers = true,
                LogLevel = LogSeverity.Verbose
            };

            _config = new ConfigurationBuilder()
                .AddTomlFile("./common/config.toml")
                .Build();

            _services = new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton(discordSocketConfig)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>(), new InteractionServiceConfig
                {
                    LogLevel = LogSeverity.Verbose
                }))
                .AddSingleton<InteractionHandler>()
                .AddLogging()
                .BuildServiceProvider();
        }

        static void Main(string[] args)
            => new Program().MainAsync(args).GetAwaiter().GetResult();

        public async Task MainAsync(string[] args)
        {
            var discord = _services.GetRequiredService<DiscordSocketClient>();

            await _services.GetRequiredService<InteractionHandler>()
                .StartAsync();

            await discord.LoginAsync(TokenType.Bot, _config["discord:token"]);
            await discord.StartAsync();

            await Task.Delay(-1);
        }
    }
}