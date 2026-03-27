using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Container : MonoBehaviour
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
    public static bool operator ==(Container c1, Container c2){
        return c1.ingredients.SequenceEqual(c2.ingredients);
    }
    public static bool operator !=(Container c1, Container c2)
    {
        return !c1.ingredients.SequenceEqual(c2.ingredients);
    }
}
