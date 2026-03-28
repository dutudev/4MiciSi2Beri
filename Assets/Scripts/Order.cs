using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Order
{
    public List<Ingredient> desiredOrder = new();
    public List<Ingredient> desiredSides = new();
    public Ingredient Sauce;
    public int preparationTime=20;
    public float price=0;
    public string name = NameGenerator.GetRandomName();
    public string orderDescription="";
    public Order Randomize()
    {
        int j = 1;
        name = NameGenerator.GetRandomName();
        if (Random.Range(1, 5) == 1&&GameManager.gameManager.todaysOrders.Count>0)
        {
            Order randomOrder = GameManager.gameManager.todaysOrders[Random.Range(0, GameManager.gameManager.todaysOrders.Count)];
            desiredOrder=randomOrder.desiredOrder;
            preparationTime += randomOrder.desiredOrder.Count * 10;
            price += randomOrder.desiredOrder.Sum(x => x.price);
            orderDescription += $"{j}. Whatever Main Dishes {randomOrder.name} had\n";
            j++;
        }
        else if (Random.Range(1, 5) == 1 && GameManager.gameManager.nameToOrder.Count > 0)
        {
            var keys = GameManager.gameManager.nameToOrder.Keys.ToList();
            string randomKey = keys[Random.Range(0, keys.Count)];
            Order usualOrder = GameManager.gameManager.nameToOrder[randomKey];
            desiredOrder = usualOrder.desiredOrder;
            preparationTime += usualOrder.desiredOrder.Count*10;
            price += usualOrder.desiredOrder.Sum(x => x.price);
            name = randomKey;
            orderDescription += $"{j}. The usual Main Dishes\n";
        }
        else
            for (int i = 0; i < Random.Range(1, 5); i++)
            {
                preparationTime += 10;
                int dishCount = GameManager.gameManager.possibleDishes.Count;

                int randomIndex = Random.Range(0, dishCount);
                int dishIndex = randomIndex;

                Ingredient dish = GameManager.gameManager.possibleDishes[dishIndex];
                desiredOrder.Add(dish);
                price += dish.price;
                orderDescription += $"{j}. {dish.name}\n";
                j++;
            }
        if (Random.Range(1, 5) == 1 && GameManager.gameManager.todaysOrders.Count > 0)
        {
            Order randomOrder = GameManager.gameManager.todaysOrders[Random.Range(0, GameManager.gameManager.todaysOrders.Count)];
            desiredSides = randomOrder.desiredSides;
            preparationTime += randomOrder.desiredSides.Count * 10;
            price += randomOrder.desiredSides.Sum(x => x.price);
            orderDescription += $"{j}. Whatever Side Dishes {randomOrder.name} had\n";
            j++;
        }
        else if (Random.Range(1, 5) == 1 && GameManager.gameManager.nameToOrder.Count > 0)
        {
            var keys = GameManager.gameManager.nameToOrder.Keys.ToList();
            string randomKey = keys[Random.Range(0, keys.Count)];
            Order usualOrder = GameManager.gameManager.nameToOrder[randomKey];
            desiredSides = usualOrder.desiredSides;
            preparationTime += usualOrder.desiredSides.Count * 10;
            price += usualOrder.desiredSides.Sum(x => x.price);
            name = randomKey;
            orderDescription += $"{j}. The usual Side Dishes\n";
        }
        else
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                preparationTime += 10;
                int dishCount = GameManager.gameManager.possibleSides.Count;

                int randomIndex = Random.Range(0, dishCount);
                int dishIndex = randomIndex;

                Ingredient dish = GameManager.gameManager.possibleSides[dishIndex];
                desiredSides.Add(dish);
                price += dish.price;
                orderDescription += $"{j}. {dish.name}\n";
                j++;
            }
        if (Random.Range(1, 5) == 1 && GameManager.gameManager.todaysOrders.Any(x=>x.Sauce!=null))
        {

            var ordersWithSauce = GameManager.gameManager.todaysOrders.Where(x => x.Sauce != null).ToList();
            Order randomOrder = ordersWithSauce[Random.Range(0, ordersWithSauce.Count)];
            Sauce = randomOrder.Sauce;
            price += randomOrder.Sauce.price;
            preparationTime += 10;
            orderDescription += $"{j}. Whatever Sauce {randomOrder.name} had\n";
            j++;
        }
        else if (Random.Range(1, 5) == 1 && GameManager.gameManager.nameToOrder.Any(x => x.Value.Sauce != null))
        {
            var keys = GameManager.gameManager.nameToOrder.Keys.Where(x => GameManager.gameManager.nameToOrder[x].Sauce != null).ToList();
            string randomKey = keys[Random.Range(0, keys.Count)];
            Order usualOrder = GameManager.gameManager.nameToOrder[randomKey];
            Sauce = usualOrder.Sauce;
            name = randomKey;
            price += usualOrder.Sauce.price;
            preparationTime += 10;
            orderDescription += $"{j}. The usual Sauce\n";
            j++;
        }
        else
        if (Random.Range(0, 2) == 0) {
            int sauceCount = GameManager.gameManager.possibleSauces.Count;
            int randomIndex = Random.Range(0, sauceCount);
            Sauce = GameManager.gameManager.possibleSauces[randomIndex];
            orderDescription += $"{j}. {Sauce.name}\n";
            price += Sauce.price;
            preparationTime += 10;
            j++;
        }
        return this;
    }
}
