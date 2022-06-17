using Disqord;
using System.Text.Json;

namespace Un1ver5e.Bot.Services.Database.Entities
{
    public class MessageTag
    {
        /// <summary>
        /// The internal database ID of the tag.
        /// </summary>
        public ulong Id { get; set; }
        /// <summary>
        /// The name of the tag.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The discord ID of the Author.
        /// </summary>
        public ulong AuthorId { get; set; }
        /// <summary>
        /// The message content of the tag.
        /// </summary>
        public string? Text { get; set; }
        /// <summary>
        /// The file attachments.
        /// </summary>
        public string Attachments { get; set; }

        /// <summary>
        /// Transforms this <see cref="MessageTag"/> into a <see cref="LocalMessage"/>.
        /// </summary>
        /// <returns></returns>
        public LocalMessage GetMessage()
        {
            using HttpClient client = new();
            string[]? attachmentUrls = JsonSerializer.Deserialize<string[]>(Attachments)!;

            Stream[] attachmentStreams = attachmentUrls
                .Select(async url => await client.GetStreamAsync(url))
                .Select(task => task.Result)
                .ToArray();

            LocalAttachment[] attachments = attachmentStreams
                .Select((str, index) => new LocalAttachment()
                {
                    Stream = str,
                    FileName = attachmentUrls[index]
                })
                .ToArray();

            return new LocalMessage()
                .WithContent(Text)
                .WithAttachments(attachments);
        }

        /// <summary>
        /// Transforms this <see cref="IUserMessage"/> into a <see cref="MessageTag"/>.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static MessageTag FromMessage(IUserMessage msg, string name)
        {
            string attachments = JsonSerializer.Serialize(msg.Attachments.Select(at => at.Url).ToArray());

            return new MessageTag()
            {
                Attachments = attachments,
                AuthorId = msg.Author.Id.RawValue,
                Name = name,
                Text = msg.Content
            };
        }

        public MessageTag(ulong id, string name, ulong authorId, string? text, string[] attachments)
        {
            Id = id;
            Name = name;
            AuthorId = authorId;
            Text = text;
            Attachments = JsonSerializer.Serialize(attachments);
        }

#pragma warning disable CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
        public MessageTag() { }
#pragma warning restore CS8618 // Поле, не допускающее значения NULL, должно содержать значение, отличное от NULL, при выходе из конструктора. Возможно, стоит объявить поле как допускающее значения NULL.
    }
}
