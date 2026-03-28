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
