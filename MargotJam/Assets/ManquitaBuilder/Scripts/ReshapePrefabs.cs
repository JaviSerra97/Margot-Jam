using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteAlways]
public class ReshapePrefabs : MonoBehaviour
{
    public Sprite SpriteToReshape;
    public Vector3 ScaleProjection;
    public List<GameObject> Prefabs;
    [ContextMenu("Reshape")]
    public void Reshape()
    {
        foreach(GameObject go in Prefabs)
        {
            DestroyImmediate(go.transform.GetChild(2).GetComponent<BoxCollider2D>(), true);
            Debug.Log(go.transform.GetChild(2).gameObject.name + " ha sido reshapeado");
            //go.transform.GetChild(2).gameObject.AddComponent<BoxCollider2D>();
        }
    }
}
