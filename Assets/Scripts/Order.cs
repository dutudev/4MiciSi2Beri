using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Order : MonoBehaviour
{
    public List<Ingredient> desiredOrder = new List<Ingredient>();
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

    List<Ingredient> possibleSauces;
    List<Ingredient> possibleDishes = new List<Ingredient>{new Meat(),new Bread()};

    void Randomize()
    {
        for (int i = 1; i< Random.Range(1, 3); i++)
        {
            if (!Sauce && Random.Range(1, 2) == 1) Sauce = possibleSauces[Random.Range(1,possibleSauces.Count)];
            else desiredOrder.Add(possibleDishes[Random.Range(1, possibleDishes.Count)]);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
