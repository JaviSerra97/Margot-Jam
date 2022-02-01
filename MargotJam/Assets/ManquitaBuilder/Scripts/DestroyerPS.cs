using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ManquitaBuilder
{
    public class DestroyerPS : MonoBehaviour
    {
        private ParticleSystem _ps;

        // Start is called before the first frame update
        void Start()
        {
            _ps = GetComponent<ParticleSystem>();
            Destroy(gameObject, _ps.main.duration);
        }
    }
}