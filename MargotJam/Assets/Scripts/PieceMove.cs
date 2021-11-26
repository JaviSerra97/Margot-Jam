using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMove : MonoBehaviour
{
    [SerializeField] private float speed;

    private void Start()
    {
        int rand = Random.Range(0, 2);
        if(rand == 0) { ChangeDirection(); }
    }

    private void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * speed);

    }

    void ChangeDirection() { speed *= -1; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ScreenLimit")) { ChangeDirection(); }
    }
}
