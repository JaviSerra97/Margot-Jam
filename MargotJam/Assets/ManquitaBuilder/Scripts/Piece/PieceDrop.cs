using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PieceDrop : MonoBehaviour
{
    public LayerMask RAYCAST_LAYER = 7;

    //[SerializeField] private KeyCode dropKey;
    [SerializeField] private GameObject projection;

    [SerializeField] private float tweenDuration = 0.25f;

    public static float SNAP_THRESHOLD = 0.15f;
    public static float SNAP_SAFE = 0.8f;
    public static float RAYCAST_VARIATION = 0.50f;

    public GameObject DownNeighbour;
    
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

        int rand = Random.Range(0, 4);
        switch (rand)
        {
            case 0:
                break;
            case 1:
                TurnRight();
                break;
            case 2:
                TurnRight();
                TurnRight();
                break;
            case 3:
                TurnRight();
                TurnRight();
                TurnRight();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (put) { return; }

        RaycastHit2D centerHit = Physics2D.Raycast(transform.position + new Vector3(0, -sprite.bounds.size.y / 2 - 1f), Vector2.down, Mathf.Infinity, RAYCAST_LAYER);
        RaycastHit2D rightHit = Physics2D.Raycast(transform.position + new Vector3(sprite.bounds.size.x / 2 - RAYCAST_VARIATION, -sprite.bounds.size.y / 2 - 0.5f), Vector2.down, Mathf.Infinity, RAYCAST_LAYER);
        RaycastHit2D leftHit = Physics2D.Raycast(transform.position + new Vector3(-sprite.bounds.size.x / 2 + RAYCAST_VARIATION, -sprite.bounds.size.y / 2 - 0.5f), Vector2.down, Mathf.Infinity, RAYCAST_LAYER);
        
        if (centerHit.collider && centerHit.collider.CompareTag("Piece"))
        {
            centerDistance = Vector2.Distance(centerHit.point, transform.position);
        }
        else { centerDistance = 0; }

        if (rightHit.collider && rightHit.collider.CompareTag("Piece"))
        {
            rightDistance = Vector2.Distance(rightHit.point, transform.position + new Vector3(sprite.bounds.size.x / 2, 0));
        }
        else { rightDistance = 0; }

        if (leftHit.collider && leftHit.collider.CompareTag("Piece"))
        {
            leftDistance = Vector2.Distance(leftHit.point, transform.position + new Vector3(-sprite.bounds.size.x / 2, 0));
        }
        else { leftDistance = 0; }

        if(centerDistance <= rightDistance && centerDistance <= leftDistance)
        {
            Debug.DrawRay(transform.position, Vector2.down * 15f, Color.red);
            targetPos = centerHit.point + new Vector2(0, sprite.bounds.size.y / 2);
            projection.transform.position = new Vector3(transform.position.x, centerHit.point.y + projection.GetComponent<SpriteRenderer>().bounds.size.y / 2);
            neighbour = centerHit.transform;
        }
        else 
        {
            if(rightDistance > leftDistance)
            {
                Debug.DrawRay(transform.position + new Vector3(-sprite.bounds.size.x / 2 + RAYCAST_VARIATION, 0), Vector2.down * 15f, Color.red);
                //Colisiona izquierda
                targetPos = leftHit.point + new Vector2(sprite.bounds.size.x / 2, sprite.bounds.size.y / 2);
                projection.transform.position = new Vector3(transform.position.x, leftHit.point.y + projection.GetComponent<SpriteRenderer>().bounds.size.y / 2);
                neighbour = leftHit.transform;
            }
            else if(leftDistance > rightDistance)
            {
                Debug.DrawRay(transform.position + new Vector3(sprite.bounds.size.x / 2 - RAYCAST_VARIATION, 0), Vector2.down * 15f, Color.red);
                //Colisiona derecha
                targetPos = rightHit.point + new Vector2(-sprite.bounds.size.x / 2, sprite.bounds.size.y / 2);
                projection.transform.position = new Vector3(transform.position.x, rightHit.point.y + projection.GetComponent<SpriteRenderer>().bounds.size.y / 2);
                neighbour = rightHit.transform;
            }
        }
    }

    public void DropPiece()
    {
        put = true;

        gameObject.layer = 0;

        projection.SetActive(false);
        move.enabled = false;
        
        transform.position = new Vector3(targetPos.x, transform.position.y);
        transform.DOMove(targetPos, tweenDuration).SetEase(Ease.InQuad).OnComplete(OnPieceDropped).Play();
        VFXManager.Instance.FallingVFX(transform);
    }

    void OnPieceDropped()
    {
        VFXManager.Instance.DustVFX(transform.position - new Vector3(0, sprite.bounds.size.y / 2));

        CheckSnap();
        rb.constraints = RigidbodyConstraints2D.None;

        manager.CheckMaxHeight(transform.position.y + sprite.bounds.size.y / 2);

        manager.CreateNextPiece();
    }

    void CheckSnap()
    {
        SFX_Manager.Instance.PlayHitSound();
        var dist = transform.position.x - neighbour.transform.position.x;
        if(Mathf.Abs(dist) <= SNAP_THRESHOLD * sprite.bounds.size.x)
        {
            transform.position = new Vector3(neighbour.transform.position.x, transform.position.y);
            //VFXManager.Instance.PerfectVFX(transform.position - new Vector3(0, sprite.bounds.size.y / 2));
            VFXManager.Instance.PerfectVFX(new Vector3(transform.position.x, -2));
            DifficultManager.Instance.PerfectPlacement(transform.position);
            SFX_Manager.Instance.PlayPerfectSFX();
        }
        else if(Mathf.Abs(dist) <= SNAP_SAFE * sprite.bounds.size.x)
        {
            transform.position = new Vector3(neighbour.transform.position.x, transform.position.y);
        }
        else
        {
            DifficultManager.Instance.Fail();
            SFX_Manager.Instance.PlayFailSound();
        }

        GetComponent<BoxCollider2D>().enabled = false;
        transform.GetChild(0).gameObject.AddComponent<PolygonCollider2D>();
    }

    public bool CheckNeighbour()
    {
        if (!DownNeighbour) return false;

        bool _isNeighbouhDown = false;
        RaycastHit2D centerHit = Physics2D.Raycast(transform.position + new Vector3(0, -sprite.bounds.size.y / 2 - 0.5f), Vector2.down, 0.5f);

        if (centerHit.collider)
            _isNeighbouhDown = centerHit.collider.transform.parent.gameObject.name.Contains(DownNeighbour.name) || centerHit.collider.gameObject.name.Contains(DownNeighbour.name);
        else
            return _isNeighbouhDown;

        if (_isNeighbouhDown)
        {
            //transform.GetChild(0).GetComponent<SpriteRenderer>().DOColor(Color.blue, 0.4f).SetLoops(2, LoopType.Yoyo).Play(); // Cambiar por vfx a elecci�n
            transform.GetChild(0).DOScale(1.10f, 0.1f).SetLoops(2, LoopType.Yoyo).Play();
            transform.GetChild(0).GetComponent<SpriteRenderer>().material.DOFloat(1f, "FresnelRatio", 0.15f);
            transform.GetChild(0).GetComponent<SpriteRenderer>().material.DOFloat(0.05f, "Distortion", 0.2f);
            Invoke(nameof(ReturnNormal), 0.3f);
        }
        else
        {
            //transform.GetChild(0).GetComponent<SpriteRenderer>().DOColor(Color.black, 0.4f).SetLoops(2, LoopType.Yoyo).Play(); // Cambiar por vfx a elecci�n
            transform.GetChild(0).DOShakePosition(1.1f, 0.4f).Play();
            transform.GetChild(0).GetComponent<SpriteRenderer>().material.DOFloat(0f, "Saturation", 0.2f);
            //transform.GetChild(0).DOScale(0.9f, 0.1f).SetLoops(2, LoopType.Yoyo).Play();
            Invoke(nameof(ReturnNormal), 0.3f);
        }
        return _isNeighbouhDown;
    }

    private void ReturnNormal()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.DOFloat(0f, "FresnelRatio", 0.5f);
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.DOFloat(0f, "Distortion", 0.5f);
        transform.GetChild(0).GetComponent<SpriteRenderer>().material.DOFloat(1f, "Saturation", 0.5f);

    }
    public void TurnRight()
    {
        transform.GetChild(0).rotation *= Quaternion.Euler(new Vector3(0, 0, 90));
        transform.GetChild(1).rotation *= Quaternion.Euler(new Vector3(0, 0, 90));
    }

    public void TurnLeft()
    {
        transform.GetChild(0).rotation *= Quaternion.Euler(new Vector3(0, 0, -90));
        transform.GetChild(1).rotation *= Quaternion.Euler(new Vector3(0, 0, -90));
    }

    [ContextMenu("Check neighbour")]
    public void CheckNeighbourVariant()
    {
        Debug.Log("The object is equal to the Neighbour Prefab:" + (DownNeighbour == gameObject));
    }
}
