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
    [SerializeField] private GameObject moneyText; // this should be tmp text 
    [SerializeField] private GameObject HoldCanvasObj;
    [SerializeField] private GameObject OrderCompletedSatisfaction;
    [SerializeField] private GameObject OrderCompletedTitle;
    [SerializeField] private GameObject OrderCompletedMoney;
    [SerializeField] private GameObject dayCounter;
    [SerializeField] private GameObject spriteCustomers; // ?
    public Dictionary<string, int> nameToCustomer = new Dictionary<string, int>(); // name to customer gameobject index
    [SerializeField] private List<GameObject> customersObjects = new List<GameObject>();
    [SerializeField] private TMP_Text timeTextTMP;
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private TMP_Text endText;
    [SerializeField] private GameObject priceText;
    [SerializeField] private GameObject priceTextParent;
    public GameObject CounterCanvas;
    public Canvas HoldCanvas;
    
    
    
    
    
    private bool _showTheMeat = false;
    private string _currentAsm;
    private float _fill = 0;
    private List<Ingredient> _ingredients = new List<Ingredient>();
    private List<Meat> _meats = new List<Meat>();
    private List<GameObject> _objects = new List<GameObject>();
    
    //tween ids
    private int _counterTweenId;
    private int _holdCanvasId;
    private int _orderDoneId;
    private int _orderDoneId2;
    
    //tween objects
    [SerializeField]
    private CanvasGroup counterGroup;
    [SerializeField] private CanvasGroup HoldCanvasGroup;
    [SerializeField] private CanvasGroup OrderCompleted;
    [SerializeField] private CanvasGroup endDayMenu;
    void Start()
    {
        gameManager = this;
        AddCoins(200);
        HoldCanvas = HoldCanvasObj.GetComponent<Canvas>();
        StartDay();
    }
    public void AddCoins(float delta)
    {
        delta = Mathf.Round(delta * 10f) / 10f;
        economy.coins += delta;
        _moneyEarnedToday += delta;
        economy.coins = Mathf.Round(economy.coins * 10f) / 10f;
        moneyText.GetComponent<TextMeshProUGUI>().text = $"{economy.coins}";
    }

    public void PayCoins(float coins)
    {
        economy.coins -= coins;
        _moneySpentToday += coins;
        moneyText.GetComponent<TextMeshProUGUI>().text = $"{economy.coins}";
    }
    
    private void EndOrder(Order order, List<Meat> meats)
    {
        if (_orders.Count<=0) return;
        float satisfaction = 100f;
        float t = (Time.time - _timeAdded[_orders[0]]) / _orders[0].preparationTime;
        int wrongMain = _orders[0].desiredOrder.Where(x => !meats.Any(x2=>x2.GetIngredient()==x && (x2.cookedValue() > 0))).Count();
        int wrongSides = _orders[0].desiredSides.Where(x => !order.desiredSides.Contains(x)).Count();
        int wrongSauce = _orders[0].Sauce == order.Sauce ? 0 : 1;
        satisfaction -= 100*(wrongMain + wrongSauce + wrongSides)/(float)(_orders[0].desiredOrder.Count+ _orders[0].desiredSides.Count+(order.Sauce==null?0:1));
        satisfaction = Mathf.Round(satisfaction);
        satisfaction = Mathf.Clamp(satisfaction, 0f, 100f);
        float timeManagement = (1f - t)*100f;
        float totalScore = satisfaction*timeManagement/100;
        float money = _orders[0].desiredOrder.Where(x => order.desiredOrder.Contains(x)).Sum(x => x.price);
        money += _orders[0].desiredSides.Where(x => order.desiredSides.Contains(x)).Sum(x => x.price);
        money += (_orders[0].Sauce == order.Sauce && order.Sauce != null) ? order.Sauce.price : 0;
        AddCoins(money*1.25f);
        Debug.Log(wrongMain);
        Debug.Log(wrongSauce);
        Debug.Log(wrongSides);
        _ordersCompletedToday++;
        _happiness += satisfaction;
        _timeAdded.Remove(_orders[0]);
        _orderDoneId = LeanTween.alphaCanvas(OrderCompleted, 1, 0.5f).setEaseOutExpo().id;
        /*
        OrderCompletedTitle.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        OrderCompletedSatisfaction.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        OrderCompletedMoney.GetComponent<TextMeshProUGUI>().color = new Color(1, 1, 1, 1);
        */ // change this to leantween with canvas group
        
        OrderCompletedTitle.GetComponent<TextMeshProUGUI>().text = "Order successfully completed!";
        OrderCompletedSatisfaction.GetComponent<TextMeshProUGUI>().text = $"Satisfaction Rate: {satisfaction}%";
        OrderCompletedMoney.GetComponent<TextMeshProUGUI>().text = $"+{money} Lei";
        _orderDoneId2 = LeanTween.alphaCanvas(OrderCompleted, 0, 1f).setDelay(2.5f).id;
        //LeanTween.alphaText(OrderCompletedTitle.GetComponent<RectTransform>(), 0, 1).setDelay(2);
        //LeanTween.alphaText(OrderCompletedSatisfaction.GetComponent<RectTransform>(), 0, 1).setDelay(2);
        //LeanTween.alphaText(OrderCompletedMoney.GetComponent<RectTransform>(), 0, 1).setDelay(2);
        _orders.Remove(_orders[0]);
        _timeUntilOrder = Random.Range(_minOrderAppearTime, _maxOrderAppearTime); // added this here so it actaulyl owkr
        ResetAllCustomerObjects();
        AudioManager.instance.PlaySoundOnce(0);
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
        var showOrders = Mathf.RoundToInt(_ordersCompletedToday);
        var showMoney = _moneyEarnedToday;
        var showMoney2 = _moneySpentToday;
        dayOngoing = false;
        _orders.Clear();
        todaysOrders.Clear();
        _ordersCompletedToday = 0;
        _moneySpentToday = 0;
        _happiness = 0;
        //show ui and set timescale to 0
        //make the button trigger the obj reset and transition
        Time.timeScale = 0;
        endText.text = "Orders completed : " + showOrders + "\nMoney earned : " + showMoney + "\nMoney spent : " + showMoney2;
        endDayMenu.gameObject.SetActive(true);
        endDayMenu.alpha = 0;
        LeanTween.alphaCanvas(endDayMenu, 1, 1).setEaseOutExpo().setIgnoreTimeScale(true);
        LeanTween.cancel(_orderDoneId);
        LeanTween.cancel(_orderDoneId2);
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
        timeTextTMP.text = "NaN";
        if (_orders.Count == 0)
        {
            ResetOrderText();    
        }
        
        if (_ordersCompletedToday > _ordersPerDay) EndDay();
        if (!dayOngoing) return;
        _timeUntilOrder -= Time.deltaTime; 
        List<Order> toRemove = new List<Order>();
        foreach (var kvp in _timeAdded)
        {
            timeText.GetComponent<TextMeshProUGUI>().text = "Time Left: "+Mathf.Round(kvp.Key.preparationTime - (Time.time - kvp.Value)).ToString()+"s";
            timeTextTMP.text = Mathf.Round(kvp.Key.preparationTime - (Time.time - kvp.Value)).ToString() + "s";
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
        if (_timeUntilOrder < 0 && cameraMovement.GetCounterState()) // addded the thingy for also to be on the counter screen yes
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
                if (!nameToCustomer.ContainsKey(newOrder.name))
                {
                    nameToCustomer[newOrder.name] = Random.Range(0, customersObjects.Count());
                }
                customersObjects[nameToCustomer[newOrder.name]].SetActive(true);
                AudioManager.instance.PlaySoundOnce(4);
                //maybe add some tween
            }
        }
        
        
        
       
    }

    private void ResetOrderText()
    {
        timeText.GetComponent<TextMeshProUGUI>().text = "";
        orderDescription.GetComponent<TextMeshProUGUI>().text = "Waiting for Customer";
        orderName.GetComponent<TextMeshProUGUI>().text = "No order";
    }

    private void UpdateDeliveryText()
    {
        if (!_showTheMeat)
        {
            return;
        }
        HoldCanvas.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKey(KeyCode.E) && (_ingredients.Count != 0 || _meats.Count != 0) && _orders.Count!=0)
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
        
        _objects = ingredients;
        GetIngredients(ingredients);
        if (!set)
        {
            _fill = 0;
        }
        
        LeanTween.cancel(_holdCanvasId);
        if (set)
        {
            _holdCanvasId = LeanTween.alphaCanvas(HoldCanvasGroup, 1, 0.3f).setEaseOutExpo().id;
        }
        else
        {
            _holdCanvasId = LeanTween.alphaCanvas(HoldCanvasGroup, 0, 0.3f).setEaseOutExpo().id;
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

    public void ShowCounterCanvas(bool show)
    {
        LeanTween.cancel(_counterTweenId);
        if (show)
        {
            counterGroup.gameObject.SetActive(true);
            _counterTweenId = LeanTween.alphaCanvas(counterGroup, 1, 0.5f).setEaseOutExpo().id;
        }
        else
        {
            _counterTweenId = LeanTween.alphaCanvas(counterGroup, 0, 0.5f).setEaseOutExpo().setOnComplete(() =>
            {
                counterGroup.gameObject.SetActive(false);
            }).id;
        }
    }

    public void ResetAllCustomerObjects()
    {
        foreach (var obj in customersObjects)
        {
            obj.SetActive(false);
        }
    }

    public void ClearAllDraggables()
    {
        List<Draggable> objects = GameObject.FindObjectsByType<Draggable>(FindObjectsSortMode.None).ToList();
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            
            var obj = objects[i];
            if (obj.gameObject.layer != LayerMask.NameToLayer("SauceContainer"))
            {
                objects.RemoveAt(i);
                Destroy(obj.gameObject);
            }
            
        }
    }

    public void NextDay()
    {
        SceneManagerTransition.instance.ShowTransition();
        LeanTween.delayedCall(0.6f, () =>
        {
            cameraMovement.ResetCamera();
            ClearAllDraggables();
            endDayMenu.gameObject.SetActive(false);
            StartDay();
            SceneManagerTransition.instance.UnshowTransition();
            Time.timeScale = 1f;
        }).setIgnoreTimeScale(true);
    }

    public void GoToMenu()
    {
        SceneManagerTransition.instance.MoveToScene("MainMenu");
    }

    public void SubtractMoneyWithText(float price)
    {
        PayCoins(price);
        var texty = Instantiate(priceText, Vector3.zero, Quaternion.identity, priceTextParent.transform);
        texty.GetComponent<TMP_Text>().text = "-" + price;
        texty.transform.localPosition = new Vector3(-260.8f, -199.5f, 0f);
        LeanTween.moveLocalY(texty, -136.7f, 1).setEaseOutCubic();
        var group = texty.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(group, 0, 0.4f).setDelay(0.6f);
    }
}
