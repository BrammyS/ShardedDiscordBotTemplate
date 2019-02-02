using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bot.Interfaces.Discord.Services;
using Bot.Persistence.UnitOfWorks;
using Discord;
using Discord.Commands;
using Discord.Net;

namespace Bot.Discord.Services
{
    public class SpamFilter : ISpamFilter
    {

        /// <summary>
        /// A <see cref="List{T}"/> containing all the channel ids that are whitelisted from the spam filter.
        /// </summary>
        private readonly List<ulong> _whiteListedChannels = new List<ulong>
        {
            491904518263537667,
            436515735091806212
        };


        /// <inheritdoc />
        public async Task<bool> FilterAsync(SocketCommandContext context)
        {
            if (_whiteListedChannels.Contains(context.Channel.Id)) return true;
            using (var unitOfWork = Unity.Resolve<IUnitOfWork>())
            {
                var user = await unitOfWork.Users.GetOrAddUserAsync(context.User.Id, context.User.Username).ConfigureAwait(false);
                if (!(DateTime.Now.Subtract(user.CommandUsed).TotalSeconds > Constants.SpamFilterSeconds))
                {
                    if (user.SpamWarning >= Constants.SpamFilterTimeouts)
                    {
                        user.CommandUsed = user.CommandUsed.AddMinutes(Constants.SpamFilterTimeoutMinutes);
                        user.TotalTimesTimedOut++;
                        user.SpamWarning = 0;
                        try
                        {
                            await context.Channel.SendMessageAsync($"{context.User.Mention} you got timed out for {Constants.SpamFilterTimeoutMinutes} min\nBecause you were trying to use my commands to often.").ConfigureAwait(false);
                        }
                        catch (HttpException)
                        {
                            await context.User.SendMessageAsync($"{context.User.Mention} you got timed out for {Constants.SpamFilterTimeoutMinutes} min\nBecause you were trying to use my commands to often.").ConfigureAwait(false);
                        }
                    }
                    else if (DateTime.Now.Subtract(user.CommandUsed).TotalSeconds > 0)
                    {
                        user.SpamWarning++;
                        try
                        {
                            await context.Channel.SendMessageAsync($"pls wait {Constants.SpamFilterSeconds - Math.Round(DateTime.Now.Subtract(user.CommandUsed).TotalSeconds, 1)}s before using another command.").ConfigureAwait(false);
                        }
                        catch (HttpException)
                        {
                            await context.User.SendMessageAsync("I can't send messages in that channel!\n" +
                                                                "Please change my permission so i can send messages in that channel!").ConfigureAwait(false);
                        }
                    }
                    await unitOfWork.SaveAsync().ConfigureAwait(false);
                    return false;
                }
                if (DateTime.Now.Subtract(user.CommandUsed).TotalSeconds > 60) user.SpamWarning = 0;
                user.CommandUsed = DateTime.Now;
                await unitOfWork.SaveAsync().ConfigureAwait(false);
                return true;
            }
        }
    }
}