using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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
    private float _moneyEarnedToday = 0;
    private float _happiness = 0;

    [SerializeField] private int _minOrderAppearTime=5;
    [SerializeField] private int _maxOrderAppearTime=15;
    [SerializeField] private int _maxorders = 1;
    [SerializeField] private int _ordersPerDay = 4;
    [SerializeField] private int _day = 0;

    public List<Ingredient> possibleSauces;
    public List<Ingredient> possibleDishes;
    public List<Ingredient> possibleSides;

    public static GameManager gameManager;

    [SerializeField] private GameObject orderName;
    [SerializeField] private GameObject orderDescription;
    [SerializeField] private GameObject timeText;
    [SerializeField] private GameObject moneyText;
    [SerializeField] private GameObject HoldCanvasObj;
    [SerializeField] private GameObject OrderCompletedSatisfaction;
    [SerializeField] private GameObject OrderCompletedTitle;
    [SerializeField] private GameObject OrderCompletedMoney;
    [SerializeField] private GameObject dayCounter;
    public GameObject CounterCanvas;
    public Canvas HoldCanvas;
    
    
    private bool _showTheMeat = false;
    private string _currentAsm;
    private float _fill = 0;
    private List<Ingredient> _ingredients = new List<Ingredient>();
    private List<Meat> _meats = new List<Meat>();
    private List<GameObject> _objects = new List<GameObject>();
    void Start()
    {
        gameManager = this;
        AddCoins(200);
        HoldCanvas = HoldCanvasObj.GetComponent<Canvas>();
        StartDay();
    }
    public void AddCoins(float delta)
    {
        economy.coins += delta;
        _moneyEarnedToday += delta;
        moneyText.GetComponent<TextMeshProUGUI>().text = $"{economy.coins} Lei";
    }
    private void EndOrder(Order order, List<Meat> meats)
    {
        if (_orders.Count<=0) return;
        float satisfaction = 100f;
        float t = (Time.time - _timeAdded[_orders[0]]) / _orders[0].preparationTime;
        int wrongMain = _orders[0].desiredOrder.Where(x => !meats.Any(x2=>x2.GetIngredient()==x&&x2.isCookedWell())).Count();
        int wrongSides = _orders[0].desiredSides.Where(x => !order.desiredSides.Contains(x)).Count();
        int wrongSauce = _orders[0].Sauce == order.Sauce ? 0 : 1;
        satisfaction -= 100*(wrongMain + wrongSauce + wrongSides)/(float)(_orders[0].desiredOrder.Count+ _orders[0].desiredSides.Count+(order.Sauce==null?0:1));
        satisfaction = Mathf.Round(satisfaction);
        float timeManagement = (1f - t)*100f;
        float totalScore = satisfaction*timeManagement/100;
        float money = _orders[0].desiredOrder.Where(x => order.desiredOrder.Contains(x)).Sum(x => x.price);
        money += _orders[0].desiredSides.Where(x => order.desiredSides.Contains(x)).Sum(x => x.price);
        money += (_orders[0].Sauce == order.Sauce && order.Sauce != null) ? order.Sauce.price : 0;
        AddCoins(money);
        _ordersCompletedToday++;
        _happiness += satisfaction;
        _timeAdded.Remove(_orders[0]);
        OrderCompletedTitle.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        OrderCompletedSatisfaction.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        OrderCompletedMoney.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        OrderCompletedTitle.GetComponent<TextMeshProUGUI>().text = "Order successfully completed!";
        OrderCompletedSatisfaction.GetComponent<TextMeshProUGUI>().text = $"Satisfaction Rate: {satisfaction}%";
        OrderCompletedMoney.GetComponent<TextMeshProUGUI>().text = $"+{money} Lei";
        LeanTween.alphaText(OrderCompletedTitle.GetComponent<RectTransform>(), 0, 1).setDelay(2);
        LeanTween.alphaText(OrderCompletedSatisfaction.GetComponent<RectTransform>(), 0, 1).setDelay(2);
        LeanTween.alphaText(OrderCompletedMoney.GetComponent<RectTransform>(), 0, 1).setDelay(2);
        _orders.Remove(_orders[0]);
    }
    public void ServeDish(List<Ingredient> ingredients, List<Meat> meats)
    {
        if (_orders.Count >0 && _orders[0]!=null)
        {
            Order newOrder = new Order();
            newOrder.desiredOrder=meats.Select(x=>x.GetIngredient()).ToList();
            newOrder.desiredSides = ingredients.Where(x => possibleSides.Contains(x)).ToList();
            newOrder.Sauce=ingredients.Any(x=>possibleSauces.Contains(x))? ingredients.First(x => possibleSauces.Contains(x)):null;
            EndOrder(newOrder,meats);
        }
    }
    bool dayOngoing = true;
    public void EndDay()
    {
        dayOngoing = false;
        _orders.Clear();
        todaysOrders.Clear();
        _ordersCompletedToday = 0;
        _moneySpentToday = 0;
        _happiness = 0;
    }

    public void StartDay()
    {
        _timeUntilOrder = 0;
        _moneyEarnedToday = 0;
        dayOngoing=true;
        _day++;
        dayCounter.GetComponent<TextMeshProUGUI>().text = _day.ToString();
        var tmp = dayCounter.GetComponent<TextMeshProUGUI>();

        tmp.alpha = 0;

        LeanTween.value(dayCounter, 0f, 1f, 1.5f)
            .setOnUpdate((float val) =>
            {
                var c = tmp.color;
                c.a = val;
                tmp.color = c;
            })
            .setOnComplete(() =>
            {
                LeanTween.value(dayCounter, 1f, 0f, 1.5f)
                    .setDelay(4f)
                    .setOnUpdate((float val) =>
                    {
                        var c = tmp.color;
                        c.a = val;
                        tmp.color = c;
                    });
            });
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDeliveryText();
        if (_ordersCompletedToday > _ordersPerDay) EndDay();
        if (!dayOngoing) return;
        _timeUntilOrder -= Time.deltaTime; 
        List<Order> toRemove = new List<Order>();
        foreach (var kvp in _timeAdded)
        {
            timeText.GetComponent<TextMeshProUGUI>().text = "Time Left: "+Mathf.Round(kvp.Key.preparationTime - (Time.time - kvp.Value)).ToString()+"s";
            if (Time.time - kvp.Value >= kvp.Key.preparationTime)
            {
                _orders.Remove(kvp.Key);
                _ordersCompletedToday++;
                OrderCompletedTitle.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
                OrderCompletedSatisfaction.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
                OrderCompletedMoney.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
                OrderCompletedTitle.GetComponent<TextMeshProUGUI>().text = "Time Ran out!";
                OrderCompletedSatisfaction.GetComponent<TextMeshProUGUI>().text = $"Satisfaction Rate: 0%";
                OrderCompletedMoney.GetComponent<TextMeshProUGUI>().text = $"+0 Lei%";
                LeanTween.alphaText(OrderCompletedTitle.GetComponent<RectTransform>(), 0, 1).setDelay(2);
                LeanTween.alphaText(OrderCompletedSatisfaction.GetComponent<RectTransform>(), 0, 1).setDelay(2);
                LeanTween.alphaText(OrderCompletedMoney.GetComponent<RectTransform>(), 0, 1).setDelay(2);
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

    private void UpdateDeliveryText()
    {
        if (!_showTheMeat)
        {
            return;
        }
        HoldCanvas.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKey(KeyCode.E) && (_ingredients.Count != 0 || _meats.Count != 0))
        {
            if (_fill < 1)
            {
                //GameManager.gameManager.ServeDish();
                _fill += Time.deltaTime / 2.5f;
            }
            else
            {
                ServeDish(_ingredients,_meats);
                DestroyObjects();
            }
        }
        else
        {
            _fill = 0;
        }
        HoldCanvas.GetComponentInChildren<Image>().fillAmount = Mathf.Clamp(_fill,0,1);
    }
    
    
    public void ShowDeliveryText(bool set, string asmCur, List<GameObject> ingredients)
    {
        _showTheMeat = set;
        _currentAsm = asmCur;
        HoldCanvas.enabled = set;
        _objects = ingredients;
        GetIngredients(ingredients);
        if (!set)
        {
            _fill = 0;
        }
    }

    public void GetIngredients(List<GameObject> list)
    {
        _ingredients.Clear();
        _meats.Clear();
        foreach (var obj in list)
        {
            if (obj.GetComponent<Meat>() != null)
            {
                _meats.Add(obj.GetComponent<Meat>());
            }else if (obj.GetComponent<IngredientObject>() != null)
            {
                _ingredients.Add(obj.GetComponent<IngredientObject>().GetIngredient());
            }
            
        }
    }

    public void DestroyObjects()
    {
        for (int i = _objects.Count - 1; i >= 0; i--)
        {
            GameObject obj = _objects[i];
            _objects.RemoveAt(i);
            obj.GetComponent<Draggable>().DestroyObject();
        }
    }

    public string GetCurrentAsm()
    {
        return _currentAsm;
    }
}
