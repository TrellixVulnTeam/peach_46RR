﻿namespace Renci.SshNet.Messages.Connection
{
    /// <summary>
    /// Represents SSH_MSG_REQUEST_SUCCESS message.
    /// </summary>
    [Message("SSH_MSG_REQUEST_SUCCESS", 81)]
    public class RequestSuccessMessage : Message
    {
        /// <summary>
        /// Gets the bound port.
        /// </summary>
        public uint? BoundPort { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestSuccessMessage"/> class.
        /// </summary>
        public RequestSuccessMessage()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestSuccessMessage"/> class.
        /// </summary>
        /// <param name="boundPort">The bound port.</param>
        public RequestSuccessMessage(uint boundPort)
        {
            BoundPort = boundPort;
        }

        /// <summary>
        /// Called when type specific data need to be loaded.
        /// </summary>
        protected override void LoadData()
        {
            if (!IsEndOfData)
                BoundPort = ReadUInt32();
        }

        /// <summary>
        /// Called when type specific data need to be saved.
        /// </summary>
        protected override void SaveData()
        {
            if (BoundPort != null)
                Write(BoundPort.Value);
        }
    }
}
