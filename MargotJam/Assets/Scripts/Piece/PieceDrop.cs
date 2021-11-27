using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PieceDrop : MonoBehaviour
{
    public enum UbicacionSprite { Suelo, Pared, Detalles, Techo }
    public UbicacionSprite ubicacion;

    [SerializeField] private KeyCode dropKey;
    [SerializeField] private GameObject projection;

    [SerializeField] private float tweenDuration = 0.25f;

    public static float SNAP_THRESHOLD = 0.15f;

    public GameObject LeftNeighbour;
    public GameObject DownNeighbour;
    public GameObject RightNeighbour;


    private PieceMove move;

    private SpriteRenderer sprite;

    private float rightDistance, leftDistance, centerDistance;

    private Vector2 targetPos;

    private Rigidbody2D rb;

    private bool put;

    private Transform neighbour;

    private PiecesManager manager;

    private void Awake()
    {
        move = GetComponent<PieceMove>();
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        manager = GameObject.FindObjectOfType<PiecesManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(dropKey) && !put) { DropPiece(); }
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
            neighbour = centerHit.transform;
        }
        else 
        {
            if(rightDistance > leftDistance)
            {
                Debug.DrawRay(transform.position + new Vector3(-sprite.bounds.size.x / 2, 0), Vector2.down * 15f, Color.red);
                //Colisiona izquierda
                targetPos = leftHit.point + new Vector2(sprite.bounds.size.x / 2, sprite.bounds.size.y / 2);
                projection.transform.position = new Vector3(transform.position.x, leftHit.point.y + sprite.bounds.size.y / 2);
                neighbour = leftHit.transform;
            }
            else if(leftDistance > rightDistance)
            {
                Debug.DrawRay(transform.position + new Vector3(sprite.bounds.size.x / 2, 0), Vector2.down * 15f, Color.red);
                //Colisiona derecha
                targetPos = rightHit.point + new Vector2(-sprite.bounds.size.x / 2, sprite.bounds.size.y / 2);
                projection.transform.position = new Vector3(transform.position.x, rightHit.point.y + sprite.bounds.size.y / 2);
                neighbour = rightHit.transform;
            }
        }
    }

    void DropPiece()
    {
        put = true;

        projection.SetActive(false);
        move.enabled = false;

        transform.DOMove(targetPos, tweenDuration).SetEase(Ease.InQuad).OnComplete(OnPieceDropped).Play();
        VFXManager.Instance.FallingVFX(transform);
    }

    void OnPieceDropped()
    {
        VFXManager.Instance.DustVFX(transform.position - new Vector3(0, sprite.bounds.size.y / 2));

        CheckSnap();
        rb.constraints = RigidbodyConstraints2D.None;

        manager.CreateNextPiece();

        manager.CheckMaxHeight(transform.position.y + sprite.bounds.size.y / 2);
    }

    void CheckSnap()
    {
        var dist = transform.position.x - neighbour.transform.position.x;
        if(Mathf.Abs(dist) <= SNAP_THRESHOLD)
        {
            transform.position = new Vector3(neighbour.transform.position.x, transform.position.y);
            Debug.Log("Snap");
            //VFXManager.Instance.PerfectVFX(transform.position - new Vector3(0, sprite.bounds.size.y / 2));
            VFXManager.Instance.PerfectVFX(new Vector3(transform.position.x, -2));
            DifficultManager.Instance.PerfectPlacement(transform.position);
            CheckNeighbours();
        }
        else
        {
            DifficultManager.Instance.Fail();
        }
    }

    void CheckNeighbours()
    {
        RaycastHit2D centerHit = Physics2D.Raycast(transform.position + new Vector3(0, -sprite.bounds.size.y / 2 - 0.5f), Vector2.down, Mathf.Infinity);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + new Vector3(sprite.bounds.size.x / 2, 0), Vector2.right, Mathf.Infinity);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + new Vector3(-sprite.bounds.size.x / 2, 0), Vector2.left, Mathf.Infinity);

        if(LeftNeighbour)
        {
            if (leftHit.collider.gameObject == LeftNeighbour)
            {
                ScoreManager.Instance.AddPoints(500, transform.position);
            }
        }

        if(DownNeighbour)
        {
            if(centerHit.collider.gameObject == DownNeighbour)
            {
                ScoreManager.Instance.AddPoints(500, transform.position);
            }
        }

        if(RightNeighbour)
        {
            if(rightHit.collider.gameObject == RightNeighbour)
            {
                ScoreManager.Instance.AddPoints(500, transform.position);
            }
        }
    }
}
