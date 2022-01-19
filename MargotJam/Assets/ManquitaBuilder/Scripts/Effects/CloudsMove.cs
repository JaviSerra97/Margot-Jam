using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class CloudsMove : MonoBehaviour
{
    public float speed;
    private bool respawn = false;
    
    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    void RespawnClouds()
    {
        if(!respawn)
        {
            if (speed > 0) // aparecer izquierda
            {
                transform.position = new Vector3(-25, transform.position.y);
            }
            else if (speed < 0) //aparecer derecha
            {
                transform.position = new Vector3(25, transform.position.y);
            }
        }
    }
    
    private void OnBecameInvisible()
    {
        respawn = false;
        RespawnClouds();
    }

    private void OnBecameVisible()
    {
        respawn = true;
    }
}
