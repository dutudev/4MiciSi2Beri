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
    private Vector2 grabLocalPoint;
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
            LeanTween.scale(gameObject, Vector3.one * .5f, .3f).setEase(LeanTweenType.easeInOutCubic);
        }
    }
    public float stopPower = 2f;

    void FixedUpdate()
    {
        if (_dragging)
        {
            if (_camera == null || rb == null)
                Recache();

            Vector2 mouse = _camera.ScreenToWorldPoint(Input.mousePosition);

            // world position of grab point on object
            Vector2 worldGrabPoint = (Vector2)transform.TransformPoint(grabLocalPoint);

            Vector2 toTarget = mouse - worldGrabPoint;

            float stiffness = 250f;
            float damping = 14f;

            Vector2 force = toTarget * stiffness - rb.velocity * damping;

            //THIS restores torque
            rb.AddForceAtPosition(force, worldGrabPoint, ForceMode2D.Force);
        }
    }

    private void Recache()
    {
        _camera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        if (_camera == null || rb == null)
            Recache();

        Vector2 mouse = _camera.ScreenToWorldPoint(Input.mousePosition);

        grabLocalPoint = transform.InverseTransformPoint(mouse);

        LeanTween.scale(gameObject, Vector3.one * .6f, .3f)
            .setEase(LeanTweenType.easeOutCubic);

        _dragging = true;
    }

    public bool IsDragging()
    {
        return _dragging;
    }
}
