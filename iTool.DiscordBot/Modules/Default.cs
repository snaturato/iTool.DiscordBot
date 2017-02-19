﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace iTool.DiscordBot.Modules
{
    public class Default : ModuleBase
    {
        [Command("help")]
        [Summary("Returns all the enabled commands")]
        public async Task Help()
        {
            // TODO: replace
            EmbedBuilder b = new EmbedBuilder()
            {
                Title = "Commands",
                Color = new Color(3, 144, 255),
            };

            foreach (CommandInfo cmd in Program.CommandHandler.CommandService.Commands)
            {
               b.AddField(delegate (EmbedFieldBuilder f)
                {
                    f.Name = cmd.Name;
                    f.Value = cmd.Summary;
                });
            }
            await ReplyAsync("", embed: b);
        }

        [Command("info")]
        [Summary("Returns info about the bot")]
        public async Task Info()
        {
            IApplication application = await Context.Client.GetApplicationInfoAsync();
            EmbedBuilder b = new EmbedBuilder();
            b.Color = new Color(3, 144, 255);
            b.AddField(delegate (EmbedFieldBuilder f)
            {
                f.Name = "Info";
                f.Value = $"- Author: {application.Owner.Username} (ID {application.Owner.Id})" + Environment.NewLine +
                            $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                            $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}" + Environment.NewLine +
                            $"- Uptime: {Utils.GetUptime().ToString(@"dd\.hh\:mm\:ss")}";
            });
            b.AddField(delegate (EmbedFieldBuilder f)
            {
                f.Name = "Stats";
                f.Value = $"- Heap Size: {Utils.GetHeapSize()} MB" + Environment.NewLine +
                            $"- Guilds: {(Context.Client as DiscordSocketClient).Guilds.Count}" + Environment.NewLine +
                            $"- Channels: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}" + Environment.NewLine +
                            $"- Users: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count)}";
            });
            await ReplyAsync("", embed: b);
        }

        [Command("invite")]
        [Summary("Returns the OAuth2 Invite URL of the bot")]
        public async Task Invite()
        {
            IApplication application = await Context.Client.GetApplicationInfoAsync();
            await ReplyAsync($"A user with `MANAGE_SERVER` can invite me to your server here: <https://discordapp.com/oauth2/authorize?client_id={application.Id}&scope=bot>");
        }

        [Command("leave")]
        [Summary("Instructs the bot to leave this Guild")]
        [RequireUserPermission(GuildPermission.ManageGuild)]
        public async Task Leave()
        {
            if (Context.Guild == null) { await ReplyAsync("This command can only be ran in a server."); return; }
            await ReplyAsync("Leaving...");
            await Context.Guild.LeaveAsync();
        }

        [Command("say")]
        [Alias("echo")]
        [Summary("Echos the provided input")]
        public async Task Say([Remainder] string input)
        {
            await ReplyAsync(input);
        }

        [Command("setgame")]
        [Summary("Sets the bot's game")]
        public async Task SetGame([Remainder] string input)
        {
            if ((await Context.Client.GetApplicationInfoAsync()).Owner.Id == Context.User.Id)
            {
                Program.Settings.Game = input;
                await (Context.Client as DiscordSocketClient).SetGameAsync(input);
            }
        }

        // TODO: Allow without parm
        [Command("userinfo")]
        [Summary("Returns info about the user")]
        public async Task UserInfo(IGuildUser user)
        {
            EmbedBuilder b = new EmbedBuilder()
            {
                Color = new Color(3, 144, 255),
                ThumbnailUrl = user.AvatarUrl
            };
            b.AddField(delegate (EmbedFieldBuilder f)
            {
                f.IsInline = true;
                f.Name = "Username";
                f.Value = user.Username;
            });
            b.AddField(delegate (EmbedFieldBuilder f)
            {
                f.IsInline = true;
                f.Name = "Discriminator";
                f.Value = user.Discriminator;
            });
            b.AddField(delegate (EmbedFieldBuilder f)
            {
                f.IsInline = true;
                f.Name = "Id";
                f.Value = user.Id.ToString();
            });
            b.AddField(delegate (EmbedFieldBuilder f)
            {
                f.IsInline = true;
                f.Name = "Bot";
                f.Value = user.IsBot.ToString();
            });
            b.AddField(delegate (EmbedFieldBuilder f)
            {
                f.IsInline = true;
                f.Name = "Created at";
                f.Value = user.CreatedAt.UtcDateTime.ToString("dd/MM/yyyy HH:mm:ss");
            });
            if (user.JoinedAt == null)
            {
                b.AddField(delegate (EmbedFieldBuilder f)
                {
                    f.IsInline = true;
                    f.Name = "Joined at";
                    f.Value = user.JoinedAt.Value.UtcDateTime.ToString("dd/MM/yyyy HH:mm:ss");
                });
            }
            await ReplyAsync("", embed: b);
        }

        [Command("quit")]
        [Alias("exit", "stop")]
        [Summary("Quits the bot")]
        public async Task Quit()
        {
            if ((await Context.Client.GetApplicationInfoAsync()).Owner.Id == Context.User.Id)
            {
                await Program.Quit();
            }
        }
    }
}
