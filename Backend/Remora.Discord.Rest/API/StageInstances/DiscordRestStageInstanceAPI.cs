//
//  DiscordRestStageInstanceAPI.cs
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

using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Remora.Discord.API.Abstractions.Objects;
using Remora.Discord.API.Abstractions.Rest;
using Remora.Discord.Core;
using Remora.Discord.Rest.Extensions;
using Remora.Results;

namespace Remora.Discord.Rest.API
{
    /// <inheritdoc cref="Remora.Discord.API.Abstractions.Rest.IDiscordRestStageInstanceAPI" />
    [PublicAPI]
    public class DiscordRestStageInstanceAPI : AbstractDiscordRestAPI, IDiscordRestStageInstanceAPI
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscordRestStageInstanceAPI"/> class.
        /// </summary>
        /// <param name="discordHttpClient">The Discord HTTP client.</param>
        /// <param name="jsonOptions">The JSON options.</param>
        public DiscordRestStageInstanceAPI
        (
            DiscordHttpClient discordHttpClient,
            IOptions<JsonSerializerOptions> jsonOptions
        )
            : base(discordHttpClient, jsonOptions)
        {
        }

        /// <inheritdoc />
        public Task<Result<IStageInstance>> CreateStageInstanceAsync
        (
            Snowflake channelID,
            string topic,
            Optional<StagePrivacyLevel> privacyLevel = default,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            return this.DiscordHttpClient.PostAsync<IStageInstance>
            (
                "stage-instances",
                b => b
                .AddAuditLogReason(reason)
                .WithJson
                (
                    json =>
                    {
                        json.WriteString("channel_id", channelID.ToString());
                        json.WriteString("topic", topic);
                        json.Write("privacy_level", privacyLevel);
                    }
                ),
                ct: ct
            );
        }

        /// <inheritdoc />
        public Task<Result<IStageInstance>> GetStageInstanceAsync(Snowflake channelID, CancellationToken ct = default)
        {
            return this.DiscordHttpClient.GetAsync<IStageInstance>
            (
                $"stage-instances/{channelID}",
                ct: ct
            );
        }

        /// <inheritdoc />
        public Task<Result<IStageInstance>> UpdateStageInstanceAsync
        (
            Snowflake channelID,
            Optional<string> topic = default,
            Optional<StagePrivacyLevel> privacyLevel = default,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            return this.DiscordHttpClient.PatchAsync<IStageInstance>
            (
                $"stage-instances/{channelID}",
                b => b
                .AddAuditLogReason(reason)
                .WithJson
                (
                    json =>
                    {
                        json.Write("topic", topic);
                        json.Write("privacy_level", privacyLevel);
                    }
                ),
                ct: ct
            );
        }

        /// <inheritdoc />
        public Task<Result> DeleteStageInstance
        (
            Snowflake channelID,
            Optional<string> reason = default,
            CancellationToken ct = default
        )
        {
            return this.DiscordHttpClient.DeleteAsync
            (
                $"stage-instances/{channelID}",
                b => b.AddAuditLogReason(reason),
                ct
            );
        }
    }
}
