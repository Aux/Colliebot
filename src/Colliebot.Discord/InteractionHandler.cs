using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Colliebot.Discord
{
    public class InteractionHandler
    {
        private readonly DiscordSocketClient _discord;
        private readonly InteractionService _interactions;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _config;

        public InteractionHandler(DiscordSocketClient discord, InteractionService interactions, IServiceProvider services, IConfiguration config)
        {
            _discord = discord;
            _interactions = interactions;
            _services = services;
            _config = config;
        }

        public async Task StartAsync()
        {
            _discord.Log += OnLogAsync;
            _interactions.Log += OnLogAsync;
            _discord.Ready += OnReadyAsync;

            _discord.InteractionCreated += OnInteractionAsync;

            await _interactions.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task OnReadyAsync()
        {
            await _interactions.RegisterCommandsGloballyAsync(true);
        }

        private Task OnLogAsync(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task OnInteractionAsync(SocketInteraction interaction)
        {
            try
            {
                var context = new SocketInteractionContext(_discord, interaction);
                var result = await _interactions.ExecuteCommandAsync(context, _services);

                if (!result.IsSuccess)
                    await interaction.RespondAsync(result.ErrorReason);
            }
            catch
            {
                if (interaction.Type is InteractionType.ApplicationCommand)
                    await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }
}
