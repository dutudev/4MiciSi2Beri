using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAssembler : MonoBehaviour
{
    [SerializeField] 
    private GameObject pair;
    [SerializeField] 
    private int index;
    
    private List<GameObject> _ingredients = new List<GameObject>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
