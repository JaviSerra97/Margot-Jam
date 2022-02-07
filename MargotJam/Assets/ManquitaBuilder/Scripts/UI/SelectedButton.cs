using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public float blinkTime;
    public GameObject marker;

    public bool blink;

    public void SelectButton()
    {
        //StartCoroutine(nameof(BlinkEffect));
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        blink = true;
        StartCoroutine(nameof(BlinkEffect));
        marker.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        blink = false;
        StopCoroutine(nameof(BlinkEffect));
        marker.SetActive(false);
    }

    IEnumerator BlinkEffect()
    {
        while (blink)
        {
            marker.SetActive(!marker.activeSelf);
            yield return new WaitForSeconds(blinkTime);
        }
    }
}
