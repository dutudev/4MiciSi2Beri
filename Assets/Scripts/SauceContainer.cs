using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SauceContainer : Draggable
{
    [SerializeField] 
    private GameObject sauceToPlace;
    
    private int _rotId;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(1) && IsDragging())
        {
            Instantiate(sauceToPlace, transform.position + new Vector3(0, -.65f, 0), quaternion.identity);
            AudioManager.instance.PlaySoundOnce(2);
        }
    }

    protected override void Pickup()
    {
        base.Pickup();
        LeanTween.cancel(gameObject, _rotId);
        _rotId = LeanTween.rotateZ(gameObject, 180, 0.3f).setEaseOutExpo().id;
        Debug.Log("Yo gurt");
    }

    protected override void DropObj()
    {
        
        base.DropObj();
        LeanTween.cancel(gameObject, _rotId);
        _rotId = LeanTween.rotateZ(gameObject, 0, 0.3f).setEaseOutExpo().id;
        
    }
    
}
