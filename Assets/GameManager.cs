using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Economy economy = new Economy();
    public int maxOrders = 3;
    public List<Order> orders = new List<Order>();
    // Start is called before the first frame update
    void Start()
    {

    }

    public void EndDay()
    {

    }

    public void StartDay()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (orders.Count < 3)
        {
            orders.Add(new Order().Randomize());
        }
    }
}
