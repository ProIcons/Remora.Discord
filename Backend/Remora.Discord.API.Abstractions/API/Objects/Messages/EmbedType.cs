//
//  EmbedType.cs
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

using JetBrains.Annotations;

namespace Remora.Discord.API.Abstractions.Objects
{
    /// <summary>
    /// Enumerates the various embed types.
    /// </summary>
    [PublicAPI]
    public enum EmbedType
    {
        /// <summary>
        /// A rich embed.
        /// </summary>
        Rich,

        /// <summary>
        /// An image embed.
        /// </summary>
        Image,

        /// <summary>
        /// A video embed.
        /// </summary>
        Video,

        /// <summary>
        /// An animated gif embed.
        /// </summary>
        GIFV,

        /// <summary>
        /// An article embed.
        /// </summary>
        Article,

        /// <summary>
        /// A link embed.
        /// </summary>
        Link
    }
}
