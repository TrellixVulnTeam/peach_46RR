﻿using System;

namespace Renci.SshNet.Messages.Transport
{
    /// <summary>
    /// Represents SSH_MSG_KEX_DH_GEX_REQUEST message.
    /// </summary>
    [Message("SSH_MSG_KEX_DH_GEX_REQUEST", 34)]
    internal class KeyExchangeDhGroupExchangeRequest : Message, IKeyExchangedAllowed
    {
        /// <summary>
        /// Gets or sets the minimal size in bits of an acceptable group.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        public UInt32 Minimum { get; private set; }

        /// <summary>
        /// Gets or sets the preferred size in bits of the group the server will send.
        /// </summary>
        /// <value>
        /// The preferred.
        /// </value>
        public UInt32 Preferred { get; private set; }

        /// <summary>
        /// Gets or sets the maximal size in bits of an acceptable group.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        public UInt32 Maximum { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyExchangeDhGroupExchangeRequest"/> class.
        /// </summary>
        /// <param name="minimum">The minimum.</param>
        /// <param name="preferred">The preferred.</param>
        /// <param name="maximum">The maximum.</param>
        public KeyExchangeDhGroupExchangeRequest(uint minimum, uint preferred, uint maximum)
        {
            Minimum = minimum;
            Preferred = preferred;
            Maximum = maximum;
        }

        /// <summary>
        /// Called when type specific data need to be loaded.
        /// </summary>
        protected override void LoadData()
        {
            Minimum = ReadUInt32();
            Preferred = ReadUInt32();
            Maximum = ReadUInt32();
        }

        /// <summary>
        /// Called when type specific data need to be saved.
        /// </summary>
        protected override void SaveData()
        {
            Write(Minimum);
            Write(Preferred);
            Write(Maximum);
        }
    }
}
