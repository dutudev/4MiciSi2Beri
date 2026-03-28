using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Order
{
    public List<Ingredient> desiredOrder = new();
    public Ingredient Sauce;
    public int preparationTime=0;
    public int price;
    public string name = NameGenerator.GetRandomName();

    public Order Randomize()
    {
        for (int i = 0; i< Random.Range(1, 3); i++)
        {
            preparationTime += 30;
            int sauceCount = GameManager.gameManager.possibleSauces.Count;
            int dishCount = GameManager.gameManager.possibleDishes.Count;

            int randomIndex = Random.Range(0, sauceCount + dishCount);

            if (Sauce == null && randomIndex < sauceCount)
            {
                Sauce = GameManager.gameManager.possibleSauces[randomIndex];
            }
            else
            {
                int dishIndex = randomIndex - sauceCount;

                if (dishIndex >= 0 && dishIndex < dishCount)
                {
                    Ingredient dish = GameManager.gameManager.possibleDishes[dishIndex];
                    desiredOrder.Add(dish);
                }
            }
        }
        return this;
    }
}
