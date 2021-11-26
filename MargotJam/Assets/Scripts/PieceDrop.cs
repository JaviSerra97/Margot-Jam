using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDrop : MonoBehaviour
{
    [SerializeField] private KeyCode dropKey;

    private PieceMove move;

    private SpriteRenderer sprite;

    public float rightDistance, leftDistance, centerDistance;

    private Vector2 targetPos;

    private Rigidbody2D rb;

    private bool put;

    [SerializeField] private Transform projection;

    private void Awake()
    {
        move = GetComponent<PieceMove>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(dropKey)) { DropPiece(); }
    }

    private void FixedUpdate()
    {
        if (put) { return; }

        RaycastHit2D centerHit = Physics2D.Raycast(transform.position + new Vector3(0, -sprite.bounds.size.y / 2 - 0.5f), Vector2.down, Mathf.Infinity);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + new Vector3(sprite.bounds.size.x / 2, -sprite.bounds.size.y / 2 - 0.5f), Vector2.down, Mathf.Infinity);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + new Vector3(-sprite.bounds.size.x / 2, -sprite.bounds.size.y / 2 - 0.5f), Vector2.down, Mathf.Infinity);

        if (centerHit.collider.CompareTag("Piece"))
        {
            centerDistance = Vector2.Distance(centerHit.point, transform.position);
        }
        else { centerDistance = 0; }

        if (rightHit.collider.CompareTag("Piece"))
        {
            rightDistance = Vector2.Distance(rightHit.point, transform.position + new Vector3(sprite.bounds.size.x / 2, 0));
        }
        else { rightDistance = 0; }

        if (leftHit.collider.CompareTag("Piece"))
        {
            leftDistance = Vector2.Distance(leftHit.point, transform.position + new Vector3(-sprite.bounds.size.x / 2, 0));
        }
        else { leftDistance = 0; }

        if(centerDistance <= rightDistance && centerDistance <= leftDistance)
        {
            Debug.DrawRay(transform.position, Vector2.down * 15f, Color.red);
            targetPos = centerHit.point + new Vector2(0, sprite.bounds.size.y / 2);
            projection.transform.position = new Vector3(transform.position.x, centerHit.point.y + sprite.bounds.size.y / 2);
        }
        else 
        {
            if(rightDistance > leftDistance)
            {
                Debug.DrawRay(transform.position + new Vector3(-sprite.bounds.size.x / 2, 0), Vector2.down * 15f, Color.red);
                //Colisiona izquierda
                targetPos = leftHit.point + new Vector2(sprite.bounds.size.x / 2, sprite.bounds.size.y / 2);
                projection.transform.position = new Vector3(transform.position.x, leftHit.point.y + sprite.bounds.size.y / 2);
            }
            else if(leftDistance > rightDistance)
            {
                Debug.DrawRay(transform.position + new Vector3(sprite.bounds.size.x / 2, 0), Vector2.down * 15f, Color.red);
                //Colisiona derecha
                targetPos = rightHit.point + new Vector2(-sprite.bounds.size.x / 2, sprite.bounds.size.y / 2);
                projection.transform.position = new Vector3(transform.position.x, rightHit.point.y + sprite.bounds.size.y / 2);
            }
        }
    }

    void DropPiece()
    {
        put = true;
        move.enabled = false;
        transform.position = targetPos;
        rb.constraints = RigidbodyConstraints2D.None;
    }

    #region GIZMOS

    private void OnDrawGizmosSelected()
    {
        /*Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Vector2.down * 15f);
        Gizmos.DrawRay(transform.position - new Vector3(sprite.bounds.size.x / 2, 0), Vector2.down * 15f);
        Gizmos.DrawRay(transform.position + new Vector3(sprite.bounds.size.x / 2, 0), Vector2.down * 15f);
        */
    }

    #endregion
}
