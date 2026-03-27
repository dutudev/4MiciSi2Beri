using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillBehaviour : MonoBehaviour
{
    public Meat[] meatOnGrill;
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
}
