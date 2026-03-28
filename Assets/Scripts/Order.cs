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
    public string orderDescription="";
    public Order Randomize()
    {
        int j = 0;
        name = NameGenerator.GetRandomName();
        if (Random.Range(1, 5) == 1&&GameManager.gameManager.todaysOrders.Count>0)
        {
            Order randomOrder = GameManager.gameManager.todaysOrders[Random.Range(1, GameManager.gameManager.todaysOrders.Count)];
            desiredOrder=randomOrder.desiredOrder;
            orderDescription += $"{j}. Whatever Main Dishes {randomOrder.name} had\n";
            j++;
        }
        else if (Random.Range(1, 5) == 1 && GameManager.gameManager.nameToOrder.Count > 0)
        {
            var keys = GameManager.gameManager.nameToOrder.Keys.ToList();
            string randomKey = keys[Random.Range(0, keys.Count)];
            Order usualOrder = GameManager.gameManager.nameToOrder[randomKey];
            desiredOrder = usualOrder.desiredOrder;
            name = randomKey;
            orderDescription += $"{j}. The usual Main Dishes\n";
        }
        else
            for (int i = 0; i < Random.Range(1, 5); i++)
            {
                preparationTime += 30;
                int dishCount = GameManager.gameManager.possibleDishes.Count;

                int randomIndex = Random.Range(0, dishCount);
                int dishIndex = randomIndex;

                if (dishIndex >= 0 && dishIndex < dishCount)
                {
                    Ingredient dish = GameManager.gameManager.possibleDishes[dishIndex];
                    desiredOrder.Add(dish);
                    orderDescription += $"{j}. {dish.name}\n";
                }
                j++;
            }
        if (Random.Range(1, 5) == 1 && GameManager.gameManager.todaysOrders.Count > 0)
        {
            Order randomOrder = GameManager.gameManager.todaysOrders[Random.Range(1, GameManager.gameManager.todaysOrders.Count)];
            Sauce = randomOrder.Sauce;
            orderDescription = $"{j}. Whatever Sauce {randomOrder.name} had\n";
            j++;
        }
        else if (Random.Range(1, 5) == 1 && GameManager.gameManager.nameToOrder.Count > 0)
        {
            var keys = GameManager.gameManager.nameToOrder.Keys.ToList();
            string randomKey = keys[Random.Range(0, keys.Count)];
            Order usualOrder = GameManager.gameManager.nameToOrder[randomKey];
            Sauce = usualOrder.Sauce;
            name = randomKey;
            orderDescription = $"{j}. The usual Sauce\n";
            j++;
        }
        else
            for (int i = 0; i < Random.Range(0, 2); i++)
        {
            int sauceCount = GameManager.gameManager.possibleSauces.Count;

            int randomIndex = Random.Range(0, sauceCount);
            Sauce = GameManager.gameManager.possibleSauces[randomIndex];
            orderDescription += $"{j}. {Sauce.name}\n";
            j++;
        }
        return this;
    }
}
