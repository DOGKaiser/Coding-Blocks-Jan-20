using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LogoBob : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 startScale;
    private Vector3 originR;
    public float bobSpeed = 5;
    public float bounceSpeed = 5;
    public float rotateSpeed = 5;
    private bool up = true;
    private bool big = true;
    private bool left = true;
    void Start()
    {
        Debug.Log("START");
        origin = transform.localPosition;
        startScale = transform.localScale;
        originR = transform.rotation.eulerAngles;
        StartCoroutine("Bob");
        StartCoroutine("Bounce");
        StartCoroutine("Wiggle");
    }

    private IEnumerator Bob()
    {
        up = !up;
        Debug.Log("BOBBING");
        if(up)
            transform.DOLocalMove((new Vector3(transform.localPosition.x,origin.y, transform.localPosition.z) + (Vector3.up * 10f)), bobSpeed).SetEase(Ease.InOutCubic).OnComplete(() => StartCoroutine("Bob")).Play();
        else
            transform.DOLocalMove((new Vector3(transform.localPosition.x, origin.y, transform.localPosition.z) + (Vector3.down * 10f)), bobSpeed).SetEase(Ease.InOutCubic).OnComplete(() => StartCoroutine("Bob")).Play();

        yield return null;
    }

    private IEnumerator Bounce()
    {
        big = !big;
        if (big)
            transform.DOScale((startScale + (Vector3.one/20f)), bounceSpeed).SetEase(Ease.InOutCubic).OnComplete(() => StartCoroutine("Bounce")).Play();
        else
            transform.DOScale((startScale - (Vector3.one/20f)), bounceSpeed).SetEase(Ease.InOutCubic).OnComplete(() => StartCoroutine("Bounce")).Play();

        yield return null;
    }

    private IEnumerator Wiggle()
    {
        left = !left;
        if (left)
            transform.DORotate((originR + (Vector3.forward *2f)), rotateSpeed).SetEase(Ease.InOutCubic).OnComplete(() => StartCoroutine("Wiggle")).Play();
        else
            transform.DORotate((originR - (Vector3.forward * 2f)), rotateSpeed).SetEase(Ease.InOutCubic).OnComplete(() => StartCoroutine("Wiggle")).Play();

        yield return null;
    }
}
