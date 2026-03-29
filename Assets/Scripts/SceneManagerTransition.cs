using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerTransition : MonoBehaviour
{
    [SerializeField] 
    private Image transitionRenderer;
    [SerializeField] 
    private GameObject transitionObj;

    private bool _isTransitioning = false;

    public static SceneManagerTransition instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        LeanTween.value(0, 0.85f, 0.8f).setEaseOutExpo().setOnUpdate((float val) =>
        {
            transitionRenderer.material.SetFloat("_Progress", val);
        }).setOnComplete(() =>
        {
            transitionObj.SetActive(false);
        }).setIgnoreTimeScale(true);

    }

    public void MoveToScene(string sceneName)
    {
        transitionObj.SetActive(true);
        LeanTween.value(0.85f, 0, 0.5f).setEaseOutExpo().setOnUpdate((float val) =>
        {
            transitionRenderer.material.SetFloat("_Progress", val);
        }).setOnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);
        }).setIgnoreTimeScale(true);
    }

    public void ShowTransition()
    {
        _isTransitioning = true;
        transitionObj.SetActive(true);
        LeanTween.value(0.85f, 0, 0.5f).setEaseOutExpo().setOnUpdate((float val) =>
        {
            transitionRenderer.material.SetFloat("_Progress", val);
        }).setIgnoreTimeScale(true);
    }

    public void UnshowTransition()
    {
        LeanTween.value(0, 0.85f, 0.8f).setEaseOutExpo().setOnUpdate((float val) =>
        {
            transitionRenderer.material.SetFloat("_Progress", val);
        }).setOnComplete(() =>
        {
            transitionObj.SetActive(false);
            _isTransitioning = false;
        }).setIgnoreTimeScale(true);
    }

    public bool GetTransitionStatus()
    {
        return _isTransitioning;
    }
    
}
