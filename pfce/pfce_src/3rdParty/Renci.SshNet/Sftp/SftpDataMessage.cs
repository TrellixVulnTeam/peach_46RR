﻿using Renci.SshNet.Common;
using Renci.SshNet.Messages.Connection;

namespace Renci.SshNet.Sftp
{
    internal class SftpDataMessage : ChannelDataMessage
    {
        public SftpDataMessage(uint localChannelNumber, SftpMessage sftpMessage)
        {
            LocalChannelNumber = localChannelNumber;

            var messageData = sftpMessage.GetBytes();

            var data = new byte[4 + messageData.Length];

            ((uint) messageData.Length).GetBytes().CopyTo(data, 0);
            messageData.CopyTo(data, 4);
            Data = data;
        }
    }
}
