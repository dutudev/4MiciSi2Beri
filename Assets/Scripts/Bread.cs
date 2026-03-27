using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : Draggable
{

    [SerializeField]
    private Ingredient ingredient;

    private float _cookedProgress = 0f;

    public Ingredient GetIngredient()
    {
        return ingredient;
    }

}
