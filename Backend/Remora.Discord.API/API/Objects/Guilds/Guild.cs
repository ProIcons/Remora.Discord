//
//  Guild.cs
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
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.Core;

#pragma warning disable CS1591

namespace Remora.Discord.API.Objects
{
    /// <inheritdoc cref="IGuild" />
    [PublicAPI]
    public record Guild
    (
        Snowflake ID,
        string Name,
        IImageHash? Icon,
        IImageHash? Splash,
        IImageHash? DiscoverySplash,
        Optional<bool> IsOwner,
        Snowflake OwnerID,
        Optional<IDiscordPermissionSet> Permissions,
        Snowflake? AFKChannelID,
        TimeSpan AFKTimeout,
        VerificationLevel VerificationLevel,
        MessageNotificationLevel DefaultMessageNotifications,
        ExplicitContentFilterLevel ExplicitContentFilter,
        IReadOnlyList<IRole> Roles,
        IReadOnlyList<IEmoji> Emojis,
        IReadOnlyList<GuildFeature> GuildFeatures,
        MultiFactorAuthenticationLevel MFALevel,
        Snowflake? ApplicationID,
        Optional<bool> IsWidgetEnabled,
        Optional<Snowflake?> WidgetChannelID,
        Snowflake? SystemChannelID,
        SystemChannelFlags SystemChannelFlags,
        Snowflake? RulesChannelID,
        Optional<DateTimeOffset> JoinedAt,
        Optional<bool> IsLarge,
        Optional<bool> IsUnavailable,
        Optional<int> MemberCount,
        Optional<IReadOnlyList<IPartialVoiceState>> VoiceStates,
        Optional<IReadOnlyList<IGuildMember>> Members,
        Optional<IReadOnlyList<IChannel>> Channels,
        Optional<IReadOnlyList<IChannel>> Threads,
        Optional<IReadOnlyList<IPartialPresence>> Presences,
        Optional<int?> MaxPresences,
        Optional<int> MaxMembers,
        string? VanityUrlCode,
        string? Description,
        IImageHash? Banner,
        PremiumTier PremiumTier,
        Optional<int> PremiumSubscriptionCount,
        string PreferredLocale,
        Snowflake? PublicUpdatesChannelID,
        Optional<int> MaxVideoChannelUsers,
        Optional<int> ApproximateMemberCount,
        Optional<int> ApproximatePresenceCount,
        Optional<IWelcomeScreen> WelcomeScreen,
        GuildNSFWLevel NSFWLevel,
        Optional<IReadOnlyList<IStageInstance>> StageInstances,
        Optional<IReadOnlyList<ISticker>> Stickers
    ) : IGuild;
}
