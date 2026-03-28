using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Topbar : MonoBehaviour
{
    private float initialY;
    private void Start()
    {
        initialY = gameObject.transform.localPosition.y;
    }
    private void OnTriggerEnter2D()
    {
        float targetY = initialY - transform.localScale.y;

        LeanTween.moveLocalY(gameObject, targetY,.5f);
    }
    private void OnTriggerExit2D()
    {
        LeanTween.moveLocalY(gameObject, initialY, .5f);
    }
}
