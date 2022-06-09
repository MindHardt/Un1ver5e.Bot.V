using Disqord.Bot;
using Qmmands;

namespace Un1ver5e.Commands.Attributes
{
    /// <summary>
    ///     Specifies that the module or command can only be executed when in DEBUG configuration.
    /// </summary>
    public class RequireDebugAttribute : DiscordGuildCheckAttribute
    {
        public RequireDebugAttribute()
        { }

        public override ValueTask<CheckResult> CheckAsync(DiscordGuildCommandContext context)
        {
#if DEBUG
            return Success();
#endif
            return Failure($"This can only be executed in DEBUG configuration.");
        }
    }
}
