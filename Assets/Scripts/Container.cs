using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Container : Draggable
{
    public List<Ingredient> ingredients = new List<Ingredient>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseEnter()
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
                GameManager.gameManager.ServeDish(this);
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Meat"))
        {
            var meat = other.GetComponent<Meat>();
            if (!meat.IsDragging())
            {
                ingredients.Add(meat.GetIngredient());
                Destroy(other.gameObject);
            }
        }
    }
}
