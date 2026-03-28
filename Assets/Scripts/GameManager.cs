using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Economy economy = new Economy();

    public Dictionary<string,Order> nameToOrder = new Dictionary<string, Order>();
    public List<Order> todaysOrders = new List<Order>();
    private List<Order> _orders = new List<Order>();
    private Dictionary<Order,float> _timeAdded = new Dictionary<Order, float>();
    private float _timeUntilOrder = 0;
    private float _ordersCompletedToday = 0;

    [SerializeField] private int _minOrderAppearTime=5;
    [SerializeField] private int _maxOrderAppearTime =20;
    [SerializeField] private int _maxorders = 3;
    [SerializeField] private int _ordersPerDay = 9;

    public List<Ingredient> possibleSauces;
    public List<Ingredient> possibleDishes;

    public static GameManager gameManager;
    void Start()
    {
        gameManager = this;
    }
    void EndOrder(Order order)
    {
        _ordersCompletedToday++;
        economy.coins += order.price;
        _orders.Remove(order);
        _timeAdded.Remove(order);
    }
    public void ServeDish(Container container)
    {
        if (_orders.Any(x => x.desiredOrder.SequenceEqual(container.ingredients)))
        {
            Order orderToComplete = _orders.First(x => x.desiredOrder.OrderBy(x => x).SequenceEqual(container.ingredients.OrderBy(x => x)));
            EndOrder(orderToComplete);
        }
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
        _timeUntilOrder -= Time.deltaTime; 
        List<Order> toRemove = new List<Order>();
        foreach (var kvp in _timeAdded)
        {
            if (Time.time - kvp.Value >= kvp.Key.preparationTime)
            {
                _orders.Remove(kvp.Key);
                _ordersCompletedToday++;
            }
        }
        foreach (Order o in toRemove)
        {
            _timeAdded.Remove(o);
        }
        if (_timeUntilOrder < 0)
        {
            _timeUntilOrder = Random.Range(_minOrderAppearTime, _maxOrderAppearTime);
            if (_orders.Count < 3)
            {
                Order newOrder = new Order().Randomize();
                _orders.Add(newOrder);
                todaysOrders.Add(newOrder);
                nameToOrder.Add(newOrder.name, newOrder);
                _timeAdded.Add(newOrder, Time.time);
            }
        }
    }
}
