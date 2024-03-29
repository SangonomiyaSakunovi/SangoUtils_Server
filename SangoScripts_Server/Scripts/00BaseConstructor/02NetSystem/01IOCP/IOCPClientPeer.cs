﻿using SangoNetProtol;
using SangoUtils_Server_Scripts.Converter;
using SangoUtils_Common.Utils;
using SangoUtils_IOCP;

namespace SangoUtils_Server_Scripts.Net
{
    public class IOCPClientPeer : IClientPeer_IOCP
    {
        private long _lastMessageTimestamp = long.MinValue;

        public string EntityID { get; private set; } = "";
        public Func<IOCPClientPeer,bool>? OnClientPeerDisconnected { get; set; }

        public void SetEntityID(string entityID)
        {
            EntityID = entityID;
        }

        protected override void OnOpen()
        {
            IOCPLogger.Info("A new client is Connected.");
        }

        protected override void OnClosed()
        {
            if (!string.IsNullOrEmpty(EntityID))
            {
                OnClientPeerDisconnected?.Invoke(this);
                EntityID = "";
            }
            IOCPLogger.Info("A client is DisConnected.");
        }

        protected override void OnBinary(byte[] byteMessages)
        {
            SangoNetMessage sangoNetMessage = ProtoUtils.DeProtoBytes<SangoNetMessage>(byteMessages);
            long messageTimestamp = Convert.ToInt64(sangoNetMessage.NetMessageTimestamp);
            if (messageTimestamp > _lastMessageTimestamp)
            {
                _lastMessageTimestamp = messageTimestamp;
                IOCPService.Instance.OnMessageReceived(sangoNetMessage, this);
            }
        }

        public void SendOperationResponse(NetOperationCode operationCode, string messageStr)
        {
            NetMessageHead messageHead = new()
            {
                NetOperationCode = operationCode,
                NetMessageCommandCode = NetMessageCommandCode.NetOperationResponse
            };
            NetMessageBody messageBody = new()
            {
                NetMessageStr = messageStr
            };
            SangoNetMessage message = new()
            {
                NetMessageHead = messageHead,
                NetMessageBody = messageBody,
                NetMessageTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now).ToString()
            };
            SendData(message);
        }

        public void SendOperationResponse(NetOperationCode operationCode, NetReturnCode returnCode)
        {
            NetMessageHead messageHead = new()
            {
                NetOperationCode = operationCode,
                NetMessageCommandCode = NetMessageCommandCode.NetOperationResponse
            };
            NetMessageBody messageBody = new()
            {
                NetReturnCode = returnCode
            };
            SangoNetMessage message = new()
            {
                NetMessageHead = messageHead,
                NetMessageBody = messageBody,
                NetMessageTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now).ToString()
            };
            SendData(message);
        }

        public void SendEvent(NetOperationCode operationCode, string messageStr)
        {
            NetMessageHead messageHead = new()
            {
                NetOperationCode = operationCode,
                NetMessageCommandCode = NetMessageCommandCode.NetEventData
            };
            NetMessageBody messageBody = new()
            {
                NetMessageStr = messageStr
            };
            SangoNetMessage message = new()
            {
                NetMessageHead = messageHead,
                NetMessageBody = messageBody,
                NetMessageTimestamp = TimeUtils.GetUnixDateTimeSeconds(DateTime.Now).ToString()
            };
            SendData(message);
        }

        private void SendData(SangoNetMessage message)
        {
            byte[] bytes = ProtoUtils.SetProtoBytes(message);
            Send(bytes);
        }
    }
}
