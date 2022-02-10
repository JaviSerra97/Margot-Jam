using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyGO : MonoBehaviour
{
   void Awake()
   {
      DontDestroyOnLoad(gameObject);
   }
}
