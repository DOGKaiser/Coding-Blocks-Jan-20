using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LightEffectUI : MonoBehaviour
{
    private Image spRenderer;
    public float delay = 0;
    public bool Reverse = false;
    public float speed = 5;
    public float movespeed = 5;
    private Vector3 origin;
    private bool left = false;
    // Start is called before the first frame update
    void Start()
    {
        left = !Reverse;
        Debug.Log("STAAART!");
        spRenderer = GetComponent<Image>();
        StartCoroutine("DelayFade");

        origin = transform.position;
        StartCoroutine("Shimmy");

    }

    IEnumerator Shimmy()
    {
        left = !left;
        if (left)
            transform.DOMove((origin+(Vector3.left * .2f)), movespeed).SetEase(Ease.InOutCubic).OnComplete(() => StartCoroutine("Shimmy")).Play();
        else
            transform.DOMove((origin + (Vector3.right * .2f)), movespeed).SetEase(Ease.InOutCubic).OnComplete(() => StartCoroutine("Shimmy")).Play();

        yield return null;
    }

    IEnumerator DelayFade()
    {
        yield return new WaitForSeconds(delay);
        if(Reverse)
            spRenderer.DOFade(0, speed).OnComplete(() => FadeIn()).Play();
        else
            spRenderer.DOFade(1, speed).OnComplete(() => FadeOut()).Play();
    }
    private void FadeIn()
    {
        spRenderer.DOFade(1, speed).SetEase(Ease.InOutCubic).OnComplete(() => FadeOut()).Play();
    }
    private void FadeOut()
    {
        spRenderer.DOFade(0, speed).SetEase(Ease.InOutCubic).OnComplete(() => FadeIn()).Play();
    }

}
