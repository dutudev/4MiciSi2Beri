using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillBehaviour : MonoBehaviour
{
    public List<GameObject> meatOnGrill = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        foreach (GameObject meatObject in meatOnGrill)
        {
            meatObject.GetComponent<Meat>().AffectCookedProgress(Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Meat") && !meatOnGrill.Contains(collision.gameObject))
        {
            meatOnGrill.Add(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Meat") && meatOnGrill.Contains(collision.gameObject))
        {
            meatOnGrill.Remove(collision.gameObject);

        }
    }
}
