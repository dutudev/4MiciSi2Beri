using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSide : MonoBehaviour
{
    [SerializeField] 
    private GameObject objectToPickup; 
    [SerializeField] 
    private BoxCollider2D pickupZone;
    [SerializeField] 
    private Camera cameraScene;
    void Start()
    {
        
    }

    
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && pickupZone.OverlapPoint(cameraScene.ScreenToWorldPoint(Input.mousePosition)))
        {
           var obj =  Instantiate(objectToPickup, new Vector3(cameraScene.ScreenToWorldPoint(Input.mousePosition).x, cameraScene.ScreenToWorldPoint(Input.mousePosition).y, 0), Quaternion.identity);
           obj.GetComponent<Draggable>().SetDragStatus(true);
        }        
    }
}
