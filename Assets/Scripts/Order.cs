using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Order : MonoBehaviour
{
    public List<Ingredient> desiredOrder = new List<Ingredient>();
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

    void Randomize()
    {
        
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
