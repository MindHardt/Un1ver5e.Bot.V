using Disqord;
using Disqord.Extensions.Interactivity.Menus;

namespace Un1ver5e.Bot.Utilities
{
    public static class Extensions
    {
        /// <summary>
        /// Shuffles the collection, making it random-ordered. This returns a lazy collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection">The original collection.</param>
        /// <returns></returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> collection, Random? randomOverride = null) =>
            collection.OrderBy((e) => (randomOverride ?? Random.Shared).Next());

        /// <summary>
        /// Gets random element of a <paramref name="collection"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T GetRandomElement<T>(this IEnumerable<T> collection, Random? randomOverride = null) =>
            collection.Shuffle(randomOverride).First();

        /// <summary>
        /// Formats string as a Discord Codeblock
        /// </summary>
        /// <param name="orig">The original string.</param>
        /// <returns></returns>
        public static string AsCodeBlock(this string original, string lang = "") => $"```{lang}\n{original}```";

        /// <summary>
        /// Returns either :green_circle: or :red_circle: emoji depending on <paramref name="value"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static LocalEmoji AsEmoji(this bool value) => value ? LocalEmoji.FromString(":green_circle:") : LocalEmoji.FromString(":red_circle:");

        /// <summary>
        /// Gets <paramref name="member"/>'s guild displayed name.
        /// </summary>
        /// <param name="member"></param>
        /// <returns>This guilds nick if specified, otherwise default name.</returns>
        public static string GetDisplayName(this IMember member) => member.Nick ?? member.Name;

        /// <summary>
        /// Formats <paramref name="number"/> as a modifyer. 
        /// <para>-4 -> "-4"</para>
        /// <para>4 -> "+4"</para>
        /// <para>0 -> "+0"</para>
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string AsModifyer(this int number) => number >= 0 ? $"+{number}" : number.ToString();

        /// <summary>
        /// Replaces <paramref name="view"/>s components with ones specified in <paramref name="components"/>.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="components"></param>
        /// <returns></returns>
        public static ViewBase ReplaceComponents(this ViewBase view, IEnumerable<ViewComponent> components)
        {
            view.ClearComponents();

            foreach(ViewComponent component in components)
            {
                view.AddComponent(component);
            }

            return view;
        }

        /// <summary>
        /// Replaces <paramref name="view"/>s components with ones specified in <paramref name="components"/>.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="components"></param>
        /// <returns></returns>
        public static async ValueTask<ViewBase> ReplaceComponentsAsync(this ViewBase view, IAsyncEnumerable<ViewComponent> components)
        {
            view.ClearComponents();

            await foreach (ViewComponent component in components)
            {
                view.AddComponent(component);
            }

            return view;
        }

        /// <summary>
        /// Replaces <paramref name="view"/>s components with ones specified in <paramref name="components"/>.
        /// </summary>
        /// <param name="view"></param>
        /// <param name="components"></param>
        /// <returns></returns>
        public static ViewBase ReplaceComponents(this ViewBase view, params ViewComponent[] components) => ReplaceComponents(view, (IEnumerable<ViewComponent>)components);

        /// <summary>
        /// Disables all <see cref="ButtonViewComponent"/>s of this <see cref="ViewBase"/>.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public static async ValueTask<ViewBase> DisableComponentsAsync(this ViewBase view)
        {
            IEnumerable<ViewComponent> components =
                await Task.WhenAll(view.EnumerateComponents()
                .Select(async com =>
                {
                    await Task.Run(() =>
                    {
                        if (com is ButtonViewComponent button)
                        {
                            button.IsDisabled = true;
                            return button;
                        }
                        if (com is SelectionViewComponent select)
                        {
                            select.IsDisabled = true;
                            return select;
                        }
                        if (com is LinkButtonViewComponent link)
                        {
                            link.IsDisabled = true;
                            return link;
                        };
                        return com;
                    });
                });

            return view.ReplaceComponents(components);
        }
    }
}
