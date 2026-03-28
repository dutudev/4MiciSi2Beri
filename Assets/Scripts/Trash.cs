using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] 
    private BoxCollider2D trashSpace;
    [SerializeField]
    private Camera cameraScene;

    private bool _popUp = false;
    
    
    void Update()
    {
        if(trashSpace.OverlapPoint(cameraScene.ScreenToWorldPoint(Input.mousePosition)))
        {
            if (!_popUp)
            {
                _popUp = true;
                LeanTween.cancel(gameObject);
                LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1.1f), 0.3f).setEaseOutExpo();
            }
        }
        else
        {
            if (_popUp)
            {
                _popUp = false;
                LeanTween.cancel(gameObject);
                LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.3f).setEaseOutExpo();
            }
            return;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            
            List<Draggable> draggables = GameObject.FindObjectsByType<Draggable>(FindObjectsSortMode.None).ToList();
            foreach (var drag in draggables)
            {
                if (drag.IsDragging())
                {
                    drag.DestroyObject();
                }
            }
        }
    }
    
    
}
