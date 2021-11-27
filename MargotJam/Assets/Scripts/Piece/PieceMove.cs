using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMove : MonoBehaviour
{
    public static float speed;

    
    private float _sign;
    private CubeSounds _sounds;
    private void Start()
    {
        _sign = 1;
        _sounds = GetComponent<CubeSounds>();
        int rand = Random.Range(0, 2);
        if(rand == 0) { _sign = -1; }
    }

    private void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed * _sign);
    }

    void ChangeDirection() 
    { 
        _sign *= -1; 
        _sounds.LimitsPlaySound();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ScreenLimit")) { ChangeDirection(); }
    }
}
