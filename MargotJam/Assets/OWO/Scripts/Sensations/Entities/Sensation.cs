namespace OWO
{
    public struct Sensation
    {
        public SensationType Type { get; private set; }
        public SensationId Id { get; private set; }
        public OWOMuscle[] AvailableMuscles { get; private set; }

        public Sensation(SensationType _type, SensationId _id, OWOMuscle[] _availableMuscles)
        {
            Type = _type;
            Id = _id;
            AvailableMuscles = _availableMuscles;
        }

        public bool CanSendSensationToMuscle(OWOMuscle _muscle)
        {
            foreach (var muscle in AvailableMuscles)
            {
                if (muscle == _muscle) return true;
            }

            return false;
        }
    }
}