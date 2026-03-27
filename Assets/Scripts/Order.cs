using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Order : MonoBehaviour
{
    public List<Ingredient> desiredOrder = new();
    public Ingredient Sauce;
    public int preparationTime;
    public int price;

    void EndOrder(Container c)
    {
        Economy.coins += price;
        if (c.ingredients.OrderBy(x => x).SequenceEqual(desiredOrder.OrderBy(x => x)))
        {

        }
        else
        {

        }
    }


    public Order Randomize()
    {
        for (int i = 0; i< Random.Range(1, 3); i++)
        {
            if (Sauce == null && Random.Range(1, 3) == 1) Sauce = GameManager.gameManager.possibleSauces[Random.Range(0, GameManager.gameManager.possibleSauces.Count)];
            else
            {
                Ingredient dish = GameManager.gameManager.possibleDishes[Random.Range(0, GameManager.gameManager.possibleDishes.Count)];
                desiredOrder.Add(dish);
                Debug.Log(dish.name);
            }
        }
        return this;
    }
}
