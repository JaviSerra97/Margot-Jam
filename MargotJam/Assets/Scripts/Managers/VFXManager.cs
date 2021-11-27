using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;
    public GameObject PerfectVFX_Prefab;
    public GameObject DustVFX_Prefab;
    public GameObject FallVFX_Prefab;

    private void Awake()
    {
        Instance = this;
    }

    public void FallingVFX(Transform cubetransform)
    {
        Instantiate(FallVFX_Prefab, cubetransform);
    }

    public void DustVFX(Vector3 pos)
    {
        Instantiate(DustVFX_Prefab, pos, DustVFX_Prefab.transform.rotation);
    }

    public void PerfectVFX(Vector3 pos)
    {
        Instantiate(PerfectVFX_Prefab, pos, PerfectVFX_Prefab.transform.rotation);
    }
}
