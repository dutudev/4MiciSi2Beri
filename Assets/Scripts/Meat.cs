using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Meat : Draggable
{
    
    [SerializeField] 
    private Ingredient ingredient;
    
    private float _cookedProgress = 0f;
    

    public void AffectCookedProgress(float set)
    {
        _cookedProgress += set;
    }

    public Ingredient GetIngredient()
    {
        return ingredient;
    }

    
}
