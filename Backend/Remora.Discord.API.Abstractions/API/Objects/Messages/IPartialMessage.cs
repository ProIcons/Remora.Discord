//
//  IPartialMessage.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Remora.Discord.Core;

namespace Remora.Discord.API.Abstractions.Objects
{
    /// <summary>
    /// Represents a message.
    /// </summary>
    [PublicAPI]
    public interface IPartialMessage
    {
        /// <summary>
        /// Gets the message ID.
        /// </summary>
        Optional<Snowflake> ID { get; }

        /// <summary>
        /// Gets the ID of the channel the message was sent in.
        /// </summary>
        Optional<Snowflake> ChannelID { get; }

        /// <summary>
        /// Gets the ID of the guild the message was sent in.
        /// </summary>
        Optional<Snowflake> GuildID { get; }

        /// <summary>
        /// Gets the author of the message. This author is not guaranteed to be a valid user; in the case of a webhook
        /// message, the object corresponds to the webhook's ID, username, and avatar - this is the case when
        /// <see cref="WebhookID"/> contains a valid value.
        /// </summary>
        Optional<IUser> Author { get; }

        /// <summary>
        /// Gets the member properties for the author. The member object exists in MESSAGE_CREATE and
        /// MESSAGE_UPDATE events from text-based guild channels. This allows bots to obtain real-time member data
        /// without requiring bots to keep member state in memory.
        /// </summary>
        Optional<IPartialGuildMember> Member { get; }

        /// <summary>
        /// Gets the contents of the message.
        /// </summary>
        Optional<string> Content { get; }

        /// <summary>
        /// Gets the time when the messages was sent.
        /// </summary>
        Optional<DateTimeOffset> Timestamp { get; }

        /// <summary>
        /// Gets the time when the message was last edited.
        /// </summary>
        Optional<DateTimeOffset?> EditedTimestamp { get; }

        /// <summary>
        /// Gets a value indicating whether this was a TTS message.
        /// </summary>
        Optional<bool> IsTTS { get; }

        /// <summary>
        /// Gets a value indicating whether this message mentions everyone.
        /// </summary>
        Optional<bool> MentionsEveryone { get; }

        /// <summary>
        /// Gets a list of users mentioned in the message.
        /// </summary>
        Optional<IReadOnlyList<IUserMention>> Mentions { get; }

        /// <summary>
        /// Gets a list of mentioned roles.
        /// </summary>
        Optional<IReadOnlyList<Snowflake>> MentionedRoles { get; }

        /// <summary>
        /// Gets a list of channel mentions.
        /// <remarks>
        /// Not all channel mentions in a message will appear in mention_channels. Only
        /// textual channels that are visible to everyone in a lurkable guild will ever be included. Only crossposted
        /// messages (via Channel Following) currently include mention_channels at all. If no mentions in the message
        /// meet these requirements, this field will not be sent.
        /// </remarks>
        /// </summary>
        Optional<IReadOnlyList<IChannelMention>> MentionedChannels { get; }

        /// <summary>
        /// Gets a list of attached files.
        /// </summary>
        Optional<IReadOnlyList<IAttachment>> Attachments { get; }

        /// <summary>
        /// Gets a list of embeds.
        /// </summary>
        Optional<IReadOnlyList<IEmbed>> Embeds { get; }

        /// <summary>
        /// Gets an array of reaction objects.
        /// </summary>
        Optional<IReadOnlyList<IReaction>> Reactions { get; }

        /// <summary>
        /// Gets a nonce, used for validating a message was sent. Technically, this can be either an integer or a
        /// string.
        /// </summary>
        Optional<string> Nonce { get; }

        /// <summary>
        /// Gets a value indicating whether the messages is pinned.
        /// </summary>
        Optional<bool> IsPinned { get; }

        /// <summary>
        /// Gets the ID of the webhook that sent this message.
        /// </summary>
        Optional<Snowflake> WebhookID { get; }

        /// <summary>
        /// Gets the message type.
        /// </summary>
        Optional<MessageType> Type { get; }

        /// <summary>
        /// Gets the activity the message belongs to. Sent with rich presence-related chat embeds.
        /// </summary>
        Optional<IMessageActivity> Activity { get; }

        /// <summary>
        /// Gets the application the message belongs to. Sent with rich presence-related chat embeds.
        /// </summary>
        Optional<IPartialApplication> Application { get; }

        /// <summary>
        /// Gets the ID of the application the message's interaction belongs to. Sent with interactions.
        /// </summary>
        Optional<Snowflake> ApplicationID { get; }

        /// <summary>
        /// Gets the message reference. Sent with cross-posted messages.
        /// </summary>
        Optional<IMessageReference> MessageReference { get;  }

        /// <summary>
        /// Gets a set of bitwise flags describing extra features of the message.
        /// </summary>
        Optional<MessageFlags> Flags { get; }

        /// <summary>
        /// Gets the referenced message, if any. A null value in this context refers to a deleted message.
        /// </summary>
        Optional<IMessage?> ReferencedMessage { get; }

        /// <summary>
        /// Gets the interaction associated with this message, if any.
        /// </summary>
        Optional<IMessageInteraction> Interaction { get; }

        /// <summary>
        /// Gets the thread that was started from this message, if any.
        /// </summary>
        Optional<IChannel> Thread { get; }

        /// <summary>
        /// Gets the components in the message.
        /// </summary>
        Optional<IReadOnlyList<IMessageComponent>> Components { get; }

        /// <summary>
        /// Gets the stickers sent with the message.
        /// </summary>
        Optional<IReadOnlyList<IStickerItem>> StickerItems { get; }
    }
}
