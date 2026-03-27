using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Ingredient", order = 2)]
public class Ingredient : ScriptableObject
{
    public string name;
    public float price;
    public Sprite sprite;
}
