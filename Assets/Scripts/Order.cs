using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

public class Order
{
    public List<Ingredient> desiredOrder = new();
    public List<Ingredient> desiredSides = new();
    public Ingredient Sauce;
    public int preparationTime=25;
    public float price=0;
    public string name = NameGenerator.GetRandomName();
    private List<string> ingredientDescriptions = new();
    public string orderDescription="";
    public Order Randomize()
    {
        bool usualRequested = false;
        name = NameGenerator.GetRandomName();
        if (Random.Range(1, 20) == 1&&GameManager.gameManager.todaysOrders.Count>0)
        {
            Order randomOrder = GameManager.gameManager.todaysOrders[Random.Range(0, GameManager.gameManager.todaysOrders.Count)];
            desiredOrder=randomOrder.desiredOrder;
            preparationTime += randomOrder.desiredOrder.Count * 15;
            price += randomOrder.desiredOrder.Sum(x => x.price);
            ingredientDescriptions.Add($"Whatever Main Dishes {randomOrder.name} had\n");
        }
        else if (Random.Range(1, 20) == 1 && GameManager.gameManager.nameToOrder.Where(x=>!GameManager.gameManager.todaysOrders.Contains(x.Value)).Count() > 0)
        {
            var keys = GameManager.gameManager.nameToOrder.Where(x => !GameManager.gameManager.todaysOrders.Contains(x.Value)).Select(x=>x.Key).ToList();
            string randomKey = keys[Random.Range(0, keys.Count)];
            Order usualOrder = GameManager.gameManager.nameToOrder[randomKey];
            desiredOrder = usualOrder.desiredOrder;
            preparationTime += usualOrder.desiredOrder.Count*15;
            price += usualOrder.desiredOrder.Sum(x => x.price);
            name = randomKey;
            usualRequested = true;
            ingredientDescriptions.Add($"My usual Main Dishes\n");
        }
        else
            for (int i = 0; i < Random.Range(1, 5); i++)
            {
                preparationTime += 15;
                int dishCount = GameManager.gameManager.possibleDishes.Count;

                int randomIndex = Random.Range(0, dishCount);
                int dishIndex = randomIndex;

                Ingredient dish = GameManager.gameManager.possibleDishes[dishIndex];
                desiredOrder.Add(dish);
                price += dish.price;

                if (ingredientDescriptions.Any(x => x.Contains($"{dish.name}\n")))
                {
                    string desc = ingredientDescriptions.Find(x => x.Contains($"{dish.name}\n"));
                    int index = ingredientDescriptions.IndexOf(desc);

                    ingredientDescriptions[index] = Regex.Replace(desc, @"\b(\d+)x\s", match =>
                    {
                        int n = int.Parse(match.Groups[1].Value);
                        return (n + 1) + "x ";
                    });
                }
                else
                {
                    ingredientDescriptions.Add($"1x {dish.name}\n");
                }
            }
        if (Random.Range(1, 20) == 1 && GameManager.gameManager.todaysOrders.Count > 0)
        {
            Order randomOrder = GameManager.gameManager.todaysOrders[Random.Range(0, GameManager.gameManager.todaysOrders.Count)];
            desiredSides = randomOrder.desiredSides;
            preparationTime += randomOrder.desiredSides.Count * 15;
            price += randomOrder.desiredSides.Sum(x => x.price);
            ingredientDescriptions.Add($"Whatever Side Dishes {randomOrder.name} had\n");
        }
        else if (Random.Range(1, 20) == 1 && !usualRequested && GameManager.gameManager.nameToOrder.Where(x => !GameManager.gameManager.todaysOrders.Contains(x.Value)).Count() > 0)
        {
            var keys = GameManager.gameManager.nameToOrder.Where(x => !GameManager.gameManager.todaysOrders.Contains(x.Value)).Select(x => x.Key).ToList();
            string randomKey = keys[Random.Range(0, keys.Count)];
            Order usualOrder = GameManager.gameManager.nameToOrder[randomKey];
            desiredSides = usualOrder.desiredSides;
            preparationTime += usualOrder.desiredSides.Count * 15;
            price += usualOrder.desiredSides.Sum(x => x.price);
            name = randomKey;
            usualRequested = true;

            ingredientDescriptions.Add($"My usual Side Dishes\n");
        }
        else
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                preparationTime += 15;
                int dishCount = GameManager.gameManager.possibleSides.Count;

                int randomIndex = Random.Range(0, dishCount);
                int dishIndex = randomIndex;

                Ingredient dish = GameManager.gameManager.possibleSides[dishIndex];
                desiredSides.Add(dish);
                price += dish.price;
                if (ingredientDescriptions.Any(x => x.Contains($"{dish.name}\n")))
                {
                    string desc = ingredientDescriptions.Find(x => x.Contains($"{dish.name}\n"));
                    int index = ingredientDescriptions.IndexOf(desc);

                    ingredientDescriptions[index] = Regex.Replace(desc, @"\b(\d+)x\s", match =>
                    {
                        int n = int.Parse(match.Groups[1].Value);
                        return (n + 1) + "x ";
                    });
                }
                else
                {
                    ingredientDescriptions.Add($"1x {dish.name}\n");
                }
            }
        if (Random.Range(1, 10) == 1 && GameManager.gameManager.todaysOrders.Any(x=>x.Sauce!=null&& x.name != name))
        {

            var ordersWithSauce = GameManager.gameManager.todaysOrders.Where(x => x.Sauce != null&& x.name != name).ToList();
            Order randomOrder = ordersWithSauce[Random.Range(0, ordersWithSauce.Count)];
            Sauce = randomOrder.Sauce;
            price += randomOrder.Sauce.price;
            preparationTime += 15;
            ingredientDescriptions.Add($"Whatever Sauce {randomOrder.name} had\n");
        }
        else if (Random.Range(1, 2) == 1 && !usualRequested && GameManager.gameManager.nameToOrder.Where(x => !GameManager.gameManager.todaysOrders.Contains(x.Value)).Count() > 0)
        {
            var keys = GameManager.gameManager.nameToOrder.Where(x => !GameManager.gameManager.todaysOrders.Contains(x.Value)).Select(x => x.Key).ToList();
            string randomKey = keys[Random.Range(0, keys.Count)];
            Order usualOrder = GameManager.gameManager.nameToOrder[randomKey];
            Sauce = usualOrder.Sauce;
            name = randomKey;
            price += usualOrder.Sauce.price;
            preparationTime += 15;
            usualRequested = true;

            ingredientDescriptions.Add($"The usual Sauce\n");
        }
        else if (Random.Range(1, 10) == 1 && GameManager.gameManager.todaysOrders.Count>0)
        {
            Sauce = GameManager.gameManager.todaysOrders
    .Where(o => o.Sauce != null)
    .GroupBy(o => o.Sauce)
    .OrderByDescending(g => g.Count())
    .FirstOrDefault()?.Key;
            price += Sauce.price;
            preparationTime += 15;
            ingredientDescriptions.Add($"Today's popular Sauce\n");
        }
        else
        if (Random.Range(0, 2) == 0) {
            int sauceCount = GameManager.gameManager.possibleSauces.Count;
            int randomIndex = Random.Range(0, sauceCount);
            Sauce = GameManager.gameManager.possibleSauces[randomIndex];
            ingredientDescriptions.Add($"{Sauce.name}\n");
        }
        for (int i = 0; i < ingredientDescriptions.Count; i++)
            orderDescription += $"- {ingredientDescriptions[i]}";
        /* Will fix this later
        if (GameManager.gameManager.nameToCustomer[name])
        {// put customer

        }
        else
        {// generate random customer

        }*/
            return this;
    }
}
