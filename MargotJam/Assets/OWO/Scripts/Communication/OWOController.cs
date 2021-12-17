using UnityEngine;

namespace OWO
{
    public class OWOController
    {
        private readonly OWOClient owoClient;
        private readonly DevicesFinder devicesFinder;
        private readonly SensationCommandsSender commandsBuilder;

        public bool IsConnected;

        public OWOController()
        {
            owoClient = new OWOUDPClient();
            devicesFinder = new DevicesFinder();
            commandsBuilder = new SensationCommandsSender(owoClient);
        }

        ~OWOController() => Disconnect();

        public bool Connect(in string _ip)
        {
            IsConnected = owoClient.Connect(_ip);

            return IsConnected;
        }

        public async void FindServersInLANAndConnect()
        {
            if (IsConnected) return;

            var serverIp = await devicesFinder.FindServersInLAN();
            Connect(serverIp);
        }

        public void SendSensation(in SensationId _sensationId, in OWOMuscle _muscle)
        {
            commandsBuilder.SendSingleSensation(_sensationId, _muscle);
        }

        public void SendSensationWithPercentage(in SensationId _sensationId, in OWOMuscle _muscle,
                                                in int _percentage = 0)
        {
            commandsBuilder.SendPercentageSensation(_sensationId, _muscle, _percentage);
        }

        public void SendContinuosSensation(in DynamicSensation _sensationId, in OWOMuscle _muscle)
        {
            commandsBuilder.SendContinuosSensation(_sensationId, _muscle);
        }

        public void SendContinuosSensationWithPercentage(in DynamicSensation _sensationId, in OWOMuscle _muscle,
                                                         in int _percentage)
        {
            commandsBuilder.SendContinuosPercentageSensation(_sensationId, _muscle, _percentage);
        }

        public void StopSensation() => owoClient.StopCurrentSensation();

        public void Disconnect()
        {
            if (!IsConnected) return;

            owoClient.Disconnect();
        }
    }
}