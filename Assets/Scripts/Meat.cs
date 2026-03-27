using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Meat : MonoBehaviour
{
    [SerializeField] 
    private Rigidbody2D rb;
    [SerializeField] 
    private float moveSpeed;
    [SerializeField] 
    private float stopSpeed;
    
    private bool _dragging = false;
    private float cookedProgress = 0f;
    private Camera _camera;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _dragging = false;
        }
    }

    void FixedUpdate()
    {
        if (_dragging)
        {
            Vector2 force;
            force = _camera.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
            float stopForce = 1f;
            if (force.magnitude <= stopSpeed)
            {
                stopForce = 0;
            }
            rb.AddForce(force.normalized * moveSpeed * stopForce, ForceMode2D.Impulse);
        }
    }

    private void OnMouseDown()
    {
        _dragging = true;
    }

    public void AffectCookedProgress(float set)
    {
        cookedProgress += set;
    }
}
