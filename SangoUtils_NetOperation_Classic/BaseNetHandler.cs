﻿using SangoNetProtocol_Classic;

namespace SangoUtils_NetOperation_Classic
{
    public abstract class BaseNetHandler : BaseNetOperation
    {
        protected abstract void DefaultOperationResponse(BaseNetClientPeer peer);

        public abstract void OnOperationRequest(string message, BaseNetClientPeer peer);

        public virtual void OnInit(int netOperationCode, NetServerOperationHandler handler)
        {
            NetOperationCode = netOperationCode;
            handler.AddNetHandler(this);
        }

        public virtual void OnDispose(NetServerOperationHandler handler)
        {
            handler.RemoveNetHandler(this);
        }
    }
}
