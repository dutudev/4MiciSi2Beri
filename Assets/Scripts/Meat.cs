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

    

    private Material _meatMat;
    private float _cookedProgress = 0f;

    void Start()
    {
        _meatMat = GetComponent<SpriteRenderer>().material;
    }
    

    public void AffectCookedProgress(float set)
    {
        if (_cookedProgress < 1)
        {
            _cookedProgress = Mathf.Clamp01(_cookedProgress + set / cookDuration);
            _meatMat.SetFloat("_progress", _cookedProgress);
        }
        else if (_cookedProgress < 2)
        {
            _cookedProgress = Mathf.Clamp(_cookedProgress + set / perfectDuration, 1, 2);
        }else if (_cookedProgress < 3)
        {
            _cookedProgress = Mathf.Clamp(_cookedProgress + set / overcookDuration, 2, 3);
            _meatMat.SetFloat("_overcooked", _cookedProgress - 2);
        }
    }

    public Ingredient GetIngredient()
    {
        return ingredient;
    }

    
}
