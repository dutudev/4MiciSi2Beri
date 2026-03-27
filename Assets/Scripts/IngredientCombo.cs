using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientCombo : Ingredient
{
    public List<Ingredient> baseIngredients = new List<Ingredient>();
    public IngredientCombo(List<Ingredient> baseIngredients)
    {
        this.baseIngredients = baseIngredients;
    }
}
