using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OWO
{
    public class DevicesFinder
    {
        private readonly UdpClient socket;
        private readonly IPEndPoint endPoint;

        private const string broadcastMessage = "ping";
        private const string expectedResponse = "pong";
        private const int ipFindTimeout = 2000;

        private bool isSearchingDevices;

        public DevicesFinder()
        {
            endPoint = new IPEndPoint(IPAddress.Broadcast, 54010);
            socket = new UdpClient();
            socket.EnableBroadcast = true;
        }

        public async Task<string> FindServersInLAN()
        {
            if (isSearchingDevices)
            {
                return string.Empty;
            }

            return await WaitForResponse();
        }

        private async Task<string> WaitForResponse()
        {
            DisableSearchingAfterTimeout();
            SendBroadcastMessage();

            isSearchingDevices = true;

            while (isSearchingDevices)
            {
                var result = await socket.ReceiveAsync();
                var address = GetServerAddressIfMessageIsExpected(result);

                if (!string.IsNullOrEmpty(address))
                {
                    return address;
                }
            }

            return string.Empty;
        }

        private async void DisableSearchingAfterTimeout()
        {
            await Task.Delay(ipFindTimeout);

            isSearchingDevices = false;
        }

        private void SendBroadcastMessage()
        {
            byte[] message = Encoding.ASCII.GetBytes(broadcastMessage);
            socket.Send(message, message.Length, endPoint);
        }

        private string GetServerAddressIfMessageIsExpected(UdpReceiveResult _result)
        {
            var senderEndPoint = _result.RemoteEndPoint;
            var data = _result.Buffer;
            var message = Encoding.ASCII.GetString(data, 0, data.Length);

            if (message.Equals(expectedResponse))
            {
                return senderEndPoint.Address.ToString();
            }

            return string.Empty;
        }
    }
}