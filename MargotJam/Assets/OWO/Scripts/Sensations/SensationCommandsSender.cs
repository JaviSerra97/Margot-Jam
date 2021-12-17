namespace OWO
{
    public class SensationCommandsSender
    {
        private readonly SensationsFactory sensationsFactory;
        private readonly OWOClient owoClient;

        public SensationCommandsSender(OWOClient _owoClient)
        {
            sensationsFactory = GetSensationsFactory();
            owoClient = _owoClient;
        }

        private SensationsFactory GetSensationsFactory()
        {
            var sensationFactory = new SensationsFactory().WithDynamicSensations()
                                                          .WithStaticSensations();

            return sensationFactory;
        }

        public void SendSingleSensation(SensationId _sensation, OWOMuscle _muscle)
        {
            var sensation = sensationsFactory.GetSensationById(_sensation);

            if (!sensation.CanSendSensationToMuscle(_muscle)) return;

            SendMessageToOWOApp($"owo/{(int)_sensation}/{(int)_muscle}/eof");
        }

        private void SendMessageToOWOApp(string _message)
        {
            owoClient.SendMessageToOWOApp(_message);
        }

        public void SendPercentageSensation(SensationId _sensation, OWOMuscle _muscle,
                                            int _percentage = 0)
        {
            var sensation = sensationsFactory.GetSensationById(_sensation);

            if (!sensation.CanSendSensationToMuscle(_muscle)) return;

            SendMessageToOWOApp($"owo/percentage/{(int)_sensation}/{(int)_muscle}/" +
                                $"{_percentage}/eof");
        }

        public void SendContinuosSensation(DynamicSensation _sensation, OWOMuscle _muscle)
        {
            var sensation = sensationsFactory.GetSensationById((SensationId)_sensation);

            if (!sensation.CanSendSensationToMuscle(_muscle)) return;

            SendMessageToOWOApp($"owo/continuos/{(int)_sensation}/{(int)_muscle}/eof");
        }

        public void SendContinuosPercentageSensation(DynamicSensation _sensation, OWOMuscle _muscle,
                                                     float _percentage = 1)
        {
            var sensation = sensationsFactory.GetSensationById((SensationId)_sensation);

            if (!sensation.CanSendSensationToMuscle(_muscle)) return;

            SendMessageToOWOApp($"owo/continuos/percentage/{(int)_sensation}/{(int)_muscle}/{_percentage}/eof");
        }
    }
}