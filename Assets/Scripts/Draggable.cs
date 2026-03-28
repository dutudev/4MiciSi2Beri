using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float stopSpeed;

    [SerializeField]
    private bool rotationFixed = false; // <-- NEW

    private Rigidbody2D rb;
    private SpriteRenderer _spriteRenderer;
    private bool _dragging = false;
    private bool _active = true;
    private Camera _camera;
    private Vector2 grabLocalPoint;
    private int _baseOrder;
    

    protected virtual void Update()
    {
        if (Input.GetMouseButtonUp(0) && _active)
        {
            Debug.Log("Yo");
            DropObj();
        }
        
    }



    protected virtual void DropObj()
    {
        if (_camera == null || rb == null)
        {
            Recache();
        }
        rb.drag = 6;
        _dragging = false;
        LeanTween.scale(gameObject, Vector3.one * .5f, .3f)
            .setEase(LeanTweenType.easeInOutCubic);
        _spriteRenderer.sortingOrder = 1;
    }

    public float stopPower = 2f;

    void FixedUpdate()
    {
        if (_dragging && _active)
        {
            if (_camera == null || rb == null)
                Recache();

            Vector2 mouse = _camera.ScreenToWorldPoint(Input.mousePosition);

            Vector2 worldGrabPoint = (Vector2)transform.TransformPoint(grabLocalPoint);
            Vector2 toTarget = mouse - worldGrabPoint;

            float stiffness = 250f;
            float damping = 14f;

            Vector2 force = toTarget * stiffness - rb.velocity * damping;

            if (rotationFixed)
            {
                // Apply force at center  no torque
                rb.AddForce(force, ForceMode2D.Force);

                // Extra safety: cancel any existing rotation
                rb.angularVelocity = 0f;
            }
            else
            {
                // Normal behavior with torque
                rb.AddForceAtPosition(force, worldGrabPoint, ForceMode2D.Force);
            }
        }
    }

    private void Recache()
    {
        _camera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        
        // Apply rotation constraint when caching
        rb.freezeRotation = rotationFixed;
    }

    private void OnMouseDown()
    {
        Pickup();
    }

    protected virtual void Pickup()
    {
        if (!_active)
        {
            return;
        }

        if (_camera == null || rb == null)
            Recache();

        _spriteRenderer.sortingOrder = 10;
        
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

    public void DestroyObject()
    {
        _active = false;
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, Vector3.zero, 0.4f).setOnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}