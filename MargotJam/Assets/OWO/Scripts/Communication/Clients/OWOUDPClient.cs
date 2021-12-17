using System.Net;
using System.Net.Sockets;
using System.Text;

namespace OWO
{
    public class OWOUDPClient : OWOClient
    {
        private UdpClient client;
        private IPEndPoint endPoint;
        private string currentIp;

        public override bool Connect(in string _ip)
        {
            currentIp = _ip;
            IsConnected = IsIPValid() && StartClient();

            return IsConnected;
        }

        private bool IsIPValid() => !string.IsNullOrEmpty(currentIp);

        private bool StartClient()
        {
            try
            {
                InitializeUDPClient();
                StopCurrentSensation();

                OnConnected?.Invoke(currentIp);
                return true;
            }
            catch
            {
                OnConnectionFailed?.Invoke();
                return false;
            }
        }

        private void InitializeUDPClient()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(currentIp), port);
            client = new UdpClient(currentIp, port);
            client.Connect(endPoint);
        }

        public override void StopCurrentSensation() => SendMessageToOWOApp("owo/-1/eof");

        public override void Disconnect()
        {
            NotifyDisconnectionToOWOApp();
            CloseClient();

            OnDisconnected?.Invoke();
        }

        private void NotifyDisconnectionToOWOApp() => SendMessageToOWOApp("Close");

        private void CloseClient()
        {
            client.Close();
            client = null;
            IsConnected = false;
        }

        public override void SendMessageToOWOApp(in string _message)
        {
            var encodedMessage = Encoding.ASCII.GetBytes(_message);

            client?.Send(encodedMessage, encodedMessage.Length);
            OnSensationSent?.Invoke(_message);
        }
    }
}