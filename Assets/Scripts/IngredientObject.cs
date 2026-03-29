using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : Draggable
{
    [SerializeField] 
    private Ingredient ingredient;

    public Ingredient GetIngredient()
    {
        return ingredient;
    }
    
    protected override void ClickedFirstTime()
    {
        GameManager.gameManager.SubtractMoneyWithText(ingredient.price);
    }
}
