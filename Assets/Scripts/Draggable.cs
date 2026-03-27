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

    void FixedUpdate()
    {
        if (_dragging)
        {
            Vector2 force;
            if (_camera == null || rb == null)
            {
                Recache();
            }

            rb.drag = 3;
            force = _camera.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
            float stopForce = 1f;
            rb.AddForce(force.normalized * moveSpeed * stopForce, ForceMode2D.Impulse);
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
