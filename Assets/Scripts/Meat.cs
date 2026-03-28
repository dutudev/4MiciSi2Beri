using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Meat : Draggable
{
    
    [SerializeField] 
    private Ingredient ingredient;
    [SerializeField] 
    private float cookDuration;
    [SerializeField] 
    private float perfectDuration;
    [SerializeField] 
    private float overcookDuration;

    int currentMeatState=0;

    private Material _meatMat;
    private float _cookedProgress = 0f;

    void Start()
    {
        _meatMat = GetComponent<SpriteRenderer>().material;
    }

    float SnapToStep(float value)
    {
        return Mathf.Round(value / 0.2f) * 0.2f;
    }

    void UpdateMeatStateTween()
    {
        if (currentMeatState == 1) GetComponentInChildren<ParticleSystem>().Play();
        else LeanTween.scale(gameObject, Vector3.one * .6f * (IsDragging() ? 1.2f : 1f), .2f).setEase(LeanTweenType.easeOutCubic)
                .setLoopPingPong(2);
    }

    public void AffectCookedProgress(float set)
    {
        if (_cookedProgress < 1)
        {
            _cookedProgress = Mathf.Clamp01(_cookedProgress + set / cookDuration);
            _meatMat.SetFloat("_progress", SnapToStep(_cookedProgress));
        }
        else if (_cookedProgress < 2)
        {
            if (currentMeatState != 1) UpdateMeatStateTween();
            currentMeatState = 1;
            _cookedProgress = Mathf.Clamp(_cookedProgress + set / perfectDuration, 1, 2);
        }else if (_cookedProgress < 3)
        {
            if (currentMeatState != 2) UpdateMeatStateTween();
            currentMeatState = 2;
            _cookedProgress = Mathf.Clamp(_cookedProgress + set / overcookDuration, 2, 3);
            _meatMat.SetFloat("_overcooked", SnapToStep(_cookedProgress) - 2);
        }
    }

    public Ingredient GetIngredient()
    {
        return ingredient;
    }

    
}
