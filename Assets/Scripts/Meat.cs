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
    [SerializeField] 
    private ParticleSystem particles;

    int currentMeatState=0;

    private Material _meatMat;
    private float _cookedProgress = 0f;

    protected override void Start()
    {
        base.Start();
        _meatMat = GetComponent<SpriteRenderer>().material;
    }

    float SnapToStep(float value)
    {
        return Mathf.Round(value / 0.05f) * 0.05f;
    }

    void UpdateMeatStateTween()
    {
        
        LeanTween.scale(gameObject, Vector3.one * .6f * (IsDragging() ? 1.2f : 1f), .2f).setEase(LeanTweenType.easeOutCubic)
                .setLoopPingPong(1);
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
        }else if (currentMeatState == 2)
        {
            GetComponentInChildren<ParticleSystem>().Play();
            currentMeatState = 3;  
        }
    }

    public void SetIngredient(Ingredient ingredient2)
    {
        ingredient = ingredient2;
    }
    public Ingredient GetIngredient()
    {
        return ingredient;
    }

    public float cookedValue()
    {
        float x = _cookedProgress;
        // Between 2 and 3 ? return 2
        if (x > 2f && x < 3f)
            return 2f;

        // Lerp from 0 ? 1 as x goes 1.5 ? 2
        if (x >= 1.5f && x <= 2f)
        {
            float t = (x - 1.5f) / (2f - 1.5f); // normalize to 0–1
            return Mathf.Lerp(0f, 1f, t);
        }

        // Lerp from 1 ? 0 as x goes 3 ? 3.5
        if (x >= 3f && x <= 3.5f)
        {
            float t = (x - 3f) / (3.5f - 3f); // normalize to 0–1
            return Mathf.Lerp(1f, 0f, t);
        }

        // Outside ranges ? 0 (or whatever default you want)
        return 0f;
    }
}
