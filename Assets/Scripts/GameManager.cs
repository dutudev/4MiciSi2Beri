using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public Economy economy = new Economy();

    public Dictionary<string,Order> nameToOrder = new Dictionary<string, Order>();
    public List<Order> todaysOrders = new List<Order>();
    private List<Order> _orders = new List<Order>();
    private Dictionary<Order,float> _timeAdded = new Dictionary<Order, float>();
    private float _timeUntilOrder = 0;
    private float _ordersCompletedToday = 0;
    private float _moneySpentToday = 0;
    private float _happiness = 0;

    [SerializeField] private int _minOrderAppearTime=5;
    [SerializeField] private int _maxOrderAppearTime=15;
    [SerializeField] private int _maxorders = 1;
    [SerializeField] private int _ordersPerDay = 9;

    public List<Ingredient> possibleSauces;
    public List<Ingredient> possibleDishes;
    public List<Ingredient> possibleSides;

    public static GameManager gameManager;

    [SerializeField] private GameObject orderName;
    [SerializeField] private GameObject orderDescription;
    [SerializeField] private GameObject timeText;
    [SerializeField] private GameObject moneyText;
    void Start()
    {
        gameManager = this;
        AddCoins(200);
    }
    public void AddCoins(float delta)
    {
        economy.coins += delta;
        moneyText.GetComponent<TextMeshProUGUI>().text = $"{economy.coins} Lei";
    }
    private void EndOrder(Order order)
    {
        float satisfaction = 100f;
        float t = (Time.time - _timeAdded[order]) / order.preparationTime;
        satisfaction -= Mathf.InverseLerp(0.4f, 1f, t) * 10f;
        int wrongMain = _orders[0].desiredOrder.Where(x => !order.desiredOrder.Contains(x)).Count();
        int wrongSides = _orders[0].desiredSides.Where(x => !order.desiredSides.Contains(x)).Count();
        int wrongSauce = _orders[0].Sauce == order.Sauce ? 0 : 1;
        satisfaction -= 90*(wrongMain + wrongSauce + wrongSides)/(_orders[0].desiredOrder.Count+ _orders[0].desiredSides.Count+(order.Sauce==null?0:1));
        _ordersCompletedToday++;
        _orders.Remove(order);
        _happiness += satisfaction;
        _timeAdded.Remove(order);
    }
    public void ServeDish(Container container)
    {
        if (_orders[0]!=null)
        {
            AddCoins(_orders[0].price); 

            EndOrder(_orders[0]);
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
            timeText.GetComponent<TextMeshProUGUI>().text = "Time Left: "+Mathf.Round(kvp.Key.preparationTime - (Time.time - kvp.Value)).ToString()+"s";
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
            if (_orders.Count < _maxorders)
            {
                Order newOrder = new Order().Randomize();
                _orders.Add(newOrder);
                todaysOrders.Add(newOrder);
                nameToOrder[newOrder.name]= newOrder;
                _timeAdded.Add(newOrder, Time.time);
                orderDescription.GetComponent<TextMeshProUGUI>().text = newOrder.orderDescription;
                orderName.GetComponent<TextMeshProUGUI>().text = newOrder.name;
            }
        }
    }
}
