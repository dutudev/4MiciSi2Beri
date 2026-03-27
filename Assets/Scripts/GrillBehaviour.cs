using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillBehaviour : MonoBehaviour
{
    public List<Meat> meatOnGrill = new List<Meat>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        foreach (Meat meat in meatOnGrill)
        {
            meat.AffectCookedProgress(Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Meat") && !meatOnGrill.Contains(collision.gameObject.GetComponent<Meat>()))
        {
            meatOnGrill.Add(collision.gameObject.GetComponent<Meat>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Meat") && meatOnGrill.Contains(collision.gameObject.GetComponent<Meat>()))
        {
            meatOnGrill.Remove(collision.gameObject.GetComponent<Meat>());

        }
    }
}
