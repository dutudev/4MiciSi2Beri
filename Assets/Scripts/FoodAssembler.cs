using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodAssembler : MonoBehaviour
{
    [SerializeField] 
    private GameObject pair;
    [SerializeField] 
    private int index;
    [SerializeField] 
    private Camera cameraScene;
    [SerializeField] 
    private BoxCollider2D collider;
    
    private List<GameObject> _ingredients = new List<GameObject>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (collider.OverlapPoint(cameraScene.ScreenToWorldPoint(Input.mousePosition)))
        {
            GameManager.gameManager.ShowDeliveryText(true, gameObject.name, _ingredients);
        }
        else if(GameManager.gameManager.GetCurrentAsm() == gameObject.name)
        {
            GameManager.gameManager.ShowDeliveryText(false, "", new List<GameObject>());
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("SauceContainer") && col.GetComponent<Draggable>() != null)
        {
            _ingredients.Add(col.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        _ingredients.Remove(col.gameObject);
    }

    public void MoveObjects()
    {
        foreach (var obj in _ingredients)
        {
            obj.transform.position = obj.transform.position - transform.position + pair.transform.position;
        }
    }
    
   /* private void OnMouseEnter()
    {
        GameManager.gameManager.HoldCanvas.enabled = true;
    }
    float fill = 0;

    private void OnMouseOver()
    {
        GameManager.gameManager.HoldCanvas.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKey(KeyCode.E))
        {
            if (fill < 1 && fill + Time.deltaTime / 2.5f >= 1)
                //GameManager.gameManager.ServeDish();
            fill += Time.deltaTime / 2.5f;
        }
        else
        {
            fill = 0;
        }
        GameManager.gameManager.HoldCanvas.GetComponentInChildren<Image>().fillAmount=Math.Clamp(fill,0,1);
    }
    private void OnMouseExit()
    {
        GameManager.gameManager.HoldCanvas.enabled = false;
    }

    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log("Mic");
    }*/
}
