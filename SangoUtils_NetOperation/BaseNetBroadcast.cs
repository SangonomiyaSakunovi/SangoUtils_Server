using SangoNetProtol;

namespace SangoUtils_NetOperation
{
    public abstract class BaseNetBroadcast : BaseNetOperation
    {
        public abstract void DefaultOperationBroadcast();

        public abstract void OnBroadcast(string message);

        public virtual void OnInit(NetOperationCode netOperationCode, NetClientOperationHandler handler)
        {
            NetOperationCode = netOperationCode;
            handler.AddNetBroadcast(this);
        }

        public virtual void OnDispose(NetClientOperationHandler handler)
        {
            handler.RemoveNetBroadcast(this);
        }
    }
}