using System;

namespace OWO
{
    public abstract class OWOClient
    {
        protected const int port = 54010;

        public static Action<string> OnConnected = delegate { };
        public static Action OnDisconnected = delegate { };
        public static Action OnConnectionFailed = delegate { };
        public static Action<string> OnSensationSent = delegate { };

        public bool IsConnected { get; protected set; }

        public abstract bool Connect(in string _ip);
        public abstract void SendMessageToOWOApp(in string _message);
        public abstract void StopCurrentSensation();
        public abstract void Disconnect();
    }
}