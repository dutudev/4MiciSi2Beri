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
        _camera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }
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
}
