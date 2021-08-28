﻿using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotSteve.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync() => await ReplyAsync("pong");

        [Command("avatar")]
        public async Task AvatarAsync(ushort size = 512) => await ReplyAsync(CDN.GetUserAvatarUrl(Context.User.Id, Context.User.AvatarId, size, ImageFormat.Auto));

        [Command("react")]
        public async Task ReactAsync(string pMessage, string pEmoji)
        {
            var message = await Context.Channel.SendMessageAsync(pMessage);
            await message.AddReactionAsync(new Emoji(pEmoji));
        }
    }
}
