//
//  CachingDiscordRestChannelAPI.cs
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
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Caching.Services;
using Remora.Discord.Core;
using Remora.Discord.Rest;
using Remora.Discord.Rest.API;
using Remora.Results;

namespace Remora.Discord.Caching.API
{
    /// <summary>
    /// Implements a caching version of the channel API.
    /// </summary>
    [PublicAPI]
    public class CachingDiscordRestChannelAPI : DiscordRestChannelAPI
    {
        private readonly CacheService _cacheService;

        /// <inheritdoc cref="DiscordRestChannelAPI" />
        public CachingDiscordRestChannelAPI
        (
            DiscordHttpClient discordHttpClient,
            IOptions<JsonSerializerOptions> jsonOptions,
            CacheService cacheService
        )
            : base(discordHttpClient, jsonOptions)
        {
            _cacheService = cacheService;
        }

        /// <inheritdoc />
        public override async Task<Result<IChannel>> GetChannelAsync
        (
            Snowflake channelID,
            CancellationToken ct = default
        )
        {
            var key = KeyHelpers.CreateChannelCacheKey(channelID);
            if (_cacheService.TryGetValue<IChannel>(key, out var cachedInstance))
            {
                return Result<IChannel>.FromSuccess(cachedInstance);
            }

            var getChannel = await base.GetChannelAsync(channelID, ct);
            if (!getChannel.IsSuccess)
            {
                return getChannel;
            }

            var channel = getChannel.Entity;
            _cacheService.Cache(key, channel);

            return getChannel;
        }

        /// <inheritdoc />
        public override async Task<Result<IChannel>> ModifyChannelAsync
        (
            Snowflake channelID,
            Optional<string> name = default,
            Optional<Stream> icon = default,
            Optional<ChannelType> type = default,
            Optional<int?> position = default,
            Optional<string?> topic = default,
            Optional<bool?> isNsfw = default,
            Optional<int?> rateLimitPerUser = default,
            Optional<int?> bitrate = default,
            Optional<int?> userLimit = default,
            Optional<IReadOnlyList<IPermissionOverwrite>?> permissionOverwrites = default,
            Optional<Snowflake?> parentId = default,
            Optional<VideoQualityMode?> videoQualityMode = default,
            Optional<bool> isArchived = default,
            Optional<AutoArchiveDuration> autoArchiveDuration = default,
            Optional<bool> isLocked = default,
            Optional<AutoArchiveDuration> defaultAutoArchiveDuration = default,
            Optional<string?> rtcRegion = default,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            var modificationResult = await base.ModifyChannelAsync
            (
                channelID,
                name,
                icon,
                type,
                position,
                topic,
                isNsfw,
                rateLimitPerUser,
                bitrate,
                userLimit,
                permissionOverwrites,
                parentId,
                videoQualityMode,
                isArchived,
                autoArchiveDuration,
                isLocked,
                defaultAutoArchiveDuration,
                rtcRegion,
                reason,
                ct
            );

            if (!modificationResult.IsSuccess)
            {
                return modificationResult;
            }

            var channel = modificationResult.Entity;
            var key = KeyHelpers.CreateChannelCacheKey(channelID);
            _cacheService.Cache(key, channel);

            return modificationResult;
        }

        /// <inheritdoc />
        public override async Task<Result> DeleteChannelAsync
        (
            Snowflake channelID,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            var deleteResult = await base.DeleteChannelAsync(channelID, reason, ct);
            if (!deleteResult.IsSuccess)
            {
                return deleteResult;
            }

            var key = KeyHelpers.CreateChannelCacheKey(channelID);
            _cacheService.Evict(key);

            return deleteResult;
        }

        /// <inheritdoc />
        public override async Task<Result<IMessage>> GetChannelMessageAsync
        (
            Snowflake channelID,
            Snowflake messageID,
            CancellationToken ct = default
        )
        {
            var key = KeyHelpers.CreateMessageCacheKey(channelID, messageID);
            if (_cacheService.TryGetValue<IMessage>(key, out var cachedInstance))
            {
                return Result<IMessage>.FromSuccess(cachedInstance);
            }

            var getMessage = await base.GetChannelMessageAsync(channelID, messageID, ct);
            if (!getMessage.IsSuccess)
            {
                return getMessage;
            }

            var message = getMessage.Entity;
            _cacheService.Cache(key, message);

            return getMessage;
        }

        /// <inheritdoc />
        public override async Task<Result<IMessage>> CreateMessageAsync
        (
            Snowflake channelID,
            Optional<string> content = default,
            Optional<string> nonce = default,
            Optional<bool> isTTS = default,
            Optional<FileData> file = default,
            Optional<IReadOnlyList<IEmbed>> embeds = default,
            Optional<IAllowedMentions> allowedMentions = default,
            Optional<IMessageReference> messageReference = default,
            Optional<IReadOnlyList<IMessageComponent>> components = default,
            Optional<IReadOnlyList<Snowflake>> stickerIds = default,
            CancellationToken ct = default
        )
        {
            var createResult = await base.CreateMessageAsync
            (
                channelID,
                content,
                nonce,
                isTTS,
                file,
                embeds,
                allowedMentions,
                messageReference,
                components,
                stickerIds,
                ct
            );

            if (!createResult.IsSuccess)
            {
                return createResult;
            }

            var message = createResult.Entity;
            var key = KeyHelpers.CreateMessageCacheKey(channelID, message.ID);
            _cacheService.Cache(key, message);

            return createResult;
        }

        /// <inheritdoc />
        public override async Task<Result<IMessage>> EditMessageAsync
        (
            Snowflake channelID,
            Snowflake messageID,
            Optional<string?> content = default,
            Optional<IReadOnlyList<IEmbed>> embeds = default,
            Optional<MessageFlags?> flags = default,
            Optional<IAllowedMentions?> allowedMentions = default,
            Optional<IReadOnlyList<IAttachment>> attachments = default,
            Optional<IReadOnlyList<IMessageComponent>> components = default,
            CancellationToken ct = default
        )
        {
            var editResult = await base.EditMessageAsync
            (
                channelID,
                messageID,
                content,
                embeds,
                flags,
                allowedMentions,
                attachments,
                components,
                ct
            );

            if (!editResult.IsSuccess)
            {
                return editResult;
            }

            var message = editResult.Entity;
            var key = KeyHelpers.CreateMessageCacheKey(channelID, messageID);
            _cacheService.Cache(key, message);

            return editResult;
        }

        /// <inheritdoc />
        public override async Task<Result> DeleteMessageAsync
        (
            Snowflake channelID,
            Snowflake messageID,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            var deleteResult = await base.DeleteMessageAsync(channelID, messageID, reason, ct);
            if (!deleteResult.IsSuccess)
            {
                return deleteResult;
            }

            var key = KeyHelpers.CreateMessageCacheKey(channelID, messageID);
            _cacheService.Evict(key);

            return deleteResult;
        }

        /// <inheritdoc />
        public override async Task<Result> BulkDeleteMessagesAsync
        (
            Snowflake channelID,
            IReadOnlyList<Snowflake> messageIDs,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            var deleteResult = await base.BulkDeleteMessagesAsync(channelID, messageIDs, reason, ct);
            if (!deleteResult.IsSuccess)
            {
                return deleteResult;
            }

            foreach (var messageID in messageIDs)
            {
                var key = KeyHelpers.CreateMessageCacheKey(channelID, messageID);
                _cacheService.Evict(key);
            }

            return deleteResult;
        }

        /// <inheritdoc />
        public override async Task<Result<IInvite>> CreateChannelInviteAsync
        (
            Snowflake channelID,
            Optional<TimeSpan> maxAge = default,
            Optional<int> maxUses = default,
            Optional<bool> isTemporary = default,
            Optional<bool> isUnique = default,
            Optional<InviteTarget> targetType = default,
            Optional<Snowflake> targetUserID = default,
            Optional<Snowflake> targetApplicationID = default,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            var createResult = await base.CreateChannelInviteAsync
            (
                channelID,
                maxAge,
                maxUses,
                isTemporary,
                isUnique,
                targetType,
                targetUserID,
                targetApplicationID,
                reason,
                ct
            );

            if (!createResult.IsSuccess)
            {
                return createResult;
            }

            var invite = createResult.Entity;
            var key = KeyHelpers.CreateInviteCacheKey(invite.Code);
            _cacheService.Cache(key, invite);

            return createResult;
        }

        /// <inheritdoc />
        public override async Task<Result> DeleteChannelPermissionAsync
        (
            Snowflake channelID,
            Snowflake overwriteID,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            var deleteResult = await base.DeleteChannelPermissionAsync(channelID, overwriteID, reason, ct);
            if (!deleteResult.IsSuccess)
            {
                return deleteResult;
            }

            var key = KeyHelpers.CreateChannelPermissionCacheKey(channelID, overwriteID);
            _cacheService.Evict(key);

            return deleteResult;
        }

        /// <inheritdoc />
        public override async Task<Result<IReadOnlyList<IMessage>>> GetPinnedMessagesAsync
        (
            Snowflake channelID,
            CancellationToken ct = default
        )
        {
            var key = KeyHelpers.CreatePinnedMessagesCacheKey(channelID);
            if (_cacheService.TryGetValue<IReadOnlyList<IMessage>>(key, out var cachedInstance))
            {
                return Result<IReadOnlyList<IMessage>>.FromSuccess(cachedInstance);
            }

            var getResult = await base.GetPinnedMessagesAsync(channelID, ct);
            if (!getResult.IsSuccess)
            {
                return getResult;
            }

            var messages = getResult.Entity;
            _cacheService.Cache(key, messages);

            foreach (var message in messages)
            {
                var messageKey = KeyHelpers.CreateMessageCacheKey(channelID, message.ID);
                _cacheService.Cache(messageKey, message);
            }

            return getResult;
        }

        /// <inheritdoc />
        public override async Task<Result> UnpinMessageAsync
        (
            Snowflake channelID,
            Snowflake messageID,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            var deleteResult = await base.UnpinMessageAsync(channelID, messageID, reason, ct);
            if (!deleteResult.IsSuccess)
            {
                return deleteResult;
            }

            var key = KeyHelpers.CreateMessageCacheKey(channelID, messageID);
            _cacheService.Evict(key);

            return deleteResult;
        }

        /// <inheritdoc />
        public override async Task<Result<IChannel>> StartThreadWithMessageAsync
        (
            Snowflake channelID,
            Snowflake messageID,
            string name,
            AutoArchiveDuration autoArchiveDuration,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            var createResult = await base.StartThreadWithMessageAsync
            (
                channelID,
                messageID,
                name,
                autoArchiveDuration,
                reason,
                ct
            );

            if (!createResult.IsSuccess)
            {
                return createResult;
            }

            var key = KeyHelpers.CreateChannelCacheKey(channelID);
            _cacheService.Cache(key, createResult.Entity);

            return createResult;
        }

        /// <inheritdoc />
        public override async Task<Result<IChannel>> StartThreadWithoutMessageAsync
        (
            Snowflake channelID,
            string name,
            AutoArchiveDuration autoArchiveDuration,
            Optional<ChannelType> type,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            var createResult = await base.StartThreadWithoutMessageAsync
            (
                channelID,
                name,
                autoArchiveDuration,
                type,
                reason,
                ct
            );

            if (!createResult.IsSuccess)
            {
                return createResult;
            }

            var key = KeyHelpers.CreateChannelCacheKey(channelID);
            _cacheService.Cache(key, createResult.Entity);

            return createResult;
        }

        /// <inheritdoc />
        public override async Task<Result<IThreadQueryResponse>> ListActiveThreadsAsync
        (
            Snowflake channelID,
            CancellationToken ct = default
        )
        {
            var getResult = await base.ListActiveThreadsAsync(channelID, ct);
            if (!getResult.IsSuccess)
            {
                return getResult;
            }

            foreach (var channel in getResult.Entity.Threads)
            {
                var key = KeyHelpers.CreateChannelCacheKey(channel.ID);
                _cacheService.Cache(key, channel);
            }

            return getResult;
        }

        /// <inheritdoc />
        public override async Task<Result<IThreadQueryResponse>> ListPublicArchivedThreadsAsync
        (
            Snowflake channelID,
            Optional<DateTimeOffset> before = default,
            Optional<int> limit = default,
            CancellationToken ct = default
        )
        {
            var getResult = await base.ListPublicArchivedThreadsAsync(channelID, before, limit, ct);
            if (!getResult.IsSuccess)
            {
                return getResult;
            }

            foreach (var channel in getResult.Entity.Threads)
            {
                var key = KeyHelpers.CreateChannelCacheKey(channel.ID);
                _cacheService.Cache(key, channel);
            }

            return getResult;
        }

        /// <inheritdoc />
        public override async Task<Result<IThreadQueryResponse>> ListPrivateArchivedThreadsAsync
        (
            Snowflake channelID,
            Optional<DateTimeOffset> before = default,
            Optional<int> limit = default,
            CancellationToken ct = default
        )
        {
            var getResult = await base.ListPrivateArchivedThreadsAsync(channelID, before, limit, ct);
            if (!getResult.IsSuccess)
            {
                return getResult;
            }

            foreach (var channel in getResult.Entity.Threads)
            {
                var key = KeyHelpers.CreateChannelCacheKey(channel.ID);
                _cacheService.Cache(key, channel);
            }

            return getResult;
        }

        /// <inheritdoc />
        public override async Task<Result<IThreadQueryResponse>> ListJoinedPrivateArchivedThreadsAsync
        (
            Snowflake channelID,
            Optional<DateTimeOffset> before = default,
            Optional<int> limit = default,
            CancellationToken ct = default
        )
        {
            var getResult = await base.ListJoinedPrivateArchivedThreadsAsync(channelID, before, limit, ct);
            if (!getResult.IsSuccess)
            {
                return getResult;
            }

            foreach (var channel in getResult.Entity.Threads)
            {
                var key = KeyHelpers.CreateChannelCacheKey(channel.ID);
                _cacheService.Cache(key, channel);
            }

            return getResult;
        }
    }
}
