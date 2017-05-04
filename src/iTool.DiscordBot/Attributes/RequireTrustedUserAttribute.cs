using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace iTool.DiscordBot
{
    public class RequireTrustedUserAttribute : PreconditionAttribute
    {
        public async override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService<Settings>().TrustedUsers.Contains(context.User.Id)
                || context.User.Id == (await (context.Client as DiscordSocketClient).GetApplicationInfoAsync()).Owner.Id)
            {
                return PreconditionResult.FromSuccess();
            }
            return PreconditionResult.FromError("You must be a trusted user or the owner of the bot to run this command.");
        }
    }
}
