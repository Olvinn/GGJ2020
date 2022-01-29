using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Widget : MonoBehaviour
{
    CanvasGroup _cg;

    private void Awake()
    {
        _cg = GetComponent<CanvasGroup>();
    }

    public void Show(float speed = 1)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(1, speed));
    }

    public void Hide(float speed = 1)
    {
        StopAllCoroutines();
        StartCoroutine(Animate(0, speed));
    }

    IEnumerator Animate(float alpha, float speed)
    {
        while (_cg.alpha != alpha)
        {
            _cg.alpha = Mathf.MoveTowards(_cg.alpha, alpha, Time.deltaTime * 1 / speed);
            yield return new WaitForEndOfFrame();
        }
    }
}
