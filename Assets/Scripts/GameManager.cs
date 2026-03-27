using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Economy economy = new Economy();
    public int maxOrders = 3;
    public List<Order> orders = new List<Order>();
    public int minOrderAppearTime=5;
    public int maxOrderAppearTime=20;
    // Start is called before the first frame update
    public float timeUntilOrder = 0;
    public List<Ingredient> possibleSauces;
    public List<Ingredient> possibleDishes;
    public static GameManager gameManager;
    void Start()
    {
        gameManager = this;
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
        timeUntilOrder -= Time.deltaTime;
        if (timeUntilOrder < 0)
        {
            timeUntilOrder = Random.Range(minOrderAppearTime, maxOrderAppearTime);
            if (orders.Count < 3)
            {
                orders.Add(new Order().Randomize());
            }
        }
    }
}
