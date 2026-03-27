using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : Draggable
{

    [SerializeField]
    private Ingredient ingredient;

    public Ingredient GetIngredient()
    {
        return ingredient;
    }

}
