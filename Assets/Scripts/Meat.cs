using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Meat : Draggable
{
    [SerializeField] 
    private float moveSpeed;
    [SerializeField] 
    private float stopSpeed;
    [SerializeField] 
    private Ingredient ingredient;
    
    private bool _dragging = false;
    private float _cookedProgress = 0f;
    private Camera _camera;
    

    public void AffectCookedProgress(float set)
    {
        _cookedProgress += set;
    }

    public Ingredient GetIngredient()
    {
        return ingredient;
    }

    public bool IsDragging()
    {
        return _dragging;
    }
}
