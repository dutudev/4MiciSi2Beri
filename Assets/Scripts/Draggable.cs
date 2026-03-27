using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] 
    private float moveSpeed;
    [SerializeField] 
    private float stopSpeed;
    
    private Rigidbody2D rb;
    private bool _dragging = false;
    private Camera _camera;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_camera == null || rb == null)
            {
                Recache();
            }
            rb.drag = 6;
            _dragging = false;
        }
    }
    public float stopPower = 2f;

    void FixedUpdate()
    {
        if (_dragging)
        {
            if (_camera == null || rb == null)
                Recache();

            Vector2 target = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 toTarget = target - (Vector2)transform.position;

            float stiffness = 200f;
            float damping = 14f;    // keep slightly under critical for soft overshoot

            Vector2 force = toTarget * stiffness - rb.velocity * damping;

            rb.AddForce(force, ForceMode2D.Force);
        }
    }

    private void Recache()
    {
        _camera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        _dragging = true;
    }
    
    public bool IsDragging()
    {
        return _dragging;
    }
}
