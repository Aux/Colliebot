using Discord;
using Discord.Interactions;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace Colliebot.Discord.Interactions
{
    public class MinecraftModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly IConfiguration _config;

        public MinecraftModule(IConfiguration config)
        {
            _config = config;
        }

        [SlashCommand("ssh", "Just testing out ssh commands")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task SshAsync(string input)
        {
            await RespondAsync($"Executing command `{input}`", ephemeral: true);
            string? result = null;
            var auth = new PasswordConnectionInfo(
                _config["minecraft:serveraddress"], 
                _config["minecraft:serverusername"], 
                _config["minecraft:serverpassword"]);

            using (var ssh = new SshClient(auth))
            {
                // Add litedb storage for host key confirmation
                //ssh.HostKeyReceived += (object? sender, HostKeyEventArgs e) =>
                //{
                //    e.CanTrust = true;
                //};

                ssh.Connect();

                var command = ssh.RunCommand(input);
                result = command.Result.Length > 0 ?
                    command.Result[0..^1] : null;

                ssh.Disconnect();
            }

            if (result != null)
                await FollowupAsync(result, ephemeral: true);
        }
    }
}
