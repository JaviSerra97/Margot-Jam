using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Intro : MonoBehaviour
{
    static bool AlreadyShown = false;

    public List<GameObject> ListOfLogos;

    public float FadeTime = 0.5f;
    public float LogoTime = 1f;

    public Image fadeImage;
    private int index;
    
    void Start()
    {
        if (!AlreadyShown)
        {
            StartCoroutine(IntroRoutine());
            AlreadyShown = true;
        }
        else
        {
            fadeImage.DOFade(0,0.2f);
            gameObject.SetActive(false);
        }
    }

    void SetNextImage()
    {
        if (index == 0)
        {
            ListOfLogos[index].SetActive(true);
            index++;
        }
        else
        {
            ListOfLogos[index - 1].SetActive(false);
            index++;

            if (index > ListOfLogos.Count)
            {
                StopCoroutine(IntroRoutine());
                gameObject.SetActive(false);
                return;
            }

            ListOfLogos[index - 1].SetActive(true);
        }
    }
    
    IEnumerator IntroRoutine()
    {
        yield return null;
        yield return null;

        while (true)
        {
            SetNextImage();

            fadeImage.DOFade(0, FadeTime);

            yield return new WaitForSeconds(FadeTime + LogoTime);

            fadeImage.DOFade(1, FadeTime);

            yield return new WaitForSeconds(FadeTime);
        }
    }

    void OnDisable()
    {
        //Allow inputs
    }
}
