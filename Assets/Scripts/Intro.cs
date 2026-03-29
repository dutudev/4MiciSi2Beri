using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text textus;
    [SerializeField] 
    private Image image;
    [SerializeField]
    private List<introScene> scenes = new List<introScene>();
    private int _index = -1;

    private void Start()
    {
        Next();
    }

    public void Next()
    {
        _index++;
        if (_index >= scenes.Count)
        {
            SceneManagerTransition.instance.MoveToScene("Vlad");
            return;
        }

        if (_index == 1)
        {
            AudioManager.instance.PlaySoundOnce(5);
        }
        if (_index == 2)
        {
            AudioManager.instance.PlaySoundOnce(6);
        }
        
        image.sprite = scenes[_index].imag;
        textus.text = scenes[_index].text;
    }
}


[System.Serializable]
public class introScene
{
    public Sprite imag;
    public String text;
}