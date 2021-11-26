using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sequence", menuName = "Custom/Pieces Sequence")]
public class PiecesSequence : ScriptableObject
{
    [System.Serializable]
    public class Sequence
    {
        public string id;
        public List<GameObject> listOfPieces;
    }
    public List<Sequence> sequences; 
}
