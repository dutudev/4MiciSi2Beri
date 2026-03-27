using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    public Container desiredOrder = new Container();
    public int preparationTime;
    public int price;

    void EndOrder(Container c)
    {
        Economy.coins += price;
        if (c == desiredOrder)
        {
        }
        else
        {
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
