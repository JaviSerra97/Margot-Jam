using System;
using System.Collections.Generic;

namespace OWO
{
    public class SensationsFactory
    {
        private Dictionary<SensationId, Sensation> idToSensation;

        public SensationsFactory()
        {
            idToSensation = new Dictionary<SensationId, Sensation>();
        }

        public void AddSensations(Sensation[] _sensations)
        {
            foreach (var sensation in _sensations)
            {
                idToSensation.Add(sensation.Id, sensation);
            }
        }

        public Sensation GetSensationById(SensationId _id)
        {
            if (idToSensation.TryGetValue(_id, out Sensation _sensation))
            {
                return _sensation;
            }

            throw new Exception("The Specified Sensation does not exist!");
        }
    }
}