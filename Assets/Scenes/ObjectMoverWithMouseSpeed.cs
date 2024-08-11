using UnityEngine;
using DG.Tweening;

public class ObjectMoverWithMouseSpeed : MonoBehaviour
{
    private bool isDragging = false;
    private Camera mainCamera;
    private Rigidbody rb;
    private Vector3 lastMouseWorldPosition;
    private float lastFrameTime;
    private bool hasCollided = false;
    private Collider collidedCollider;

    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
        }

        lastMouseWorldPosition = GetMouseWorldPosition();
        lastFrameTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.transform == transform)
            {
                isDragging = true;
                lastMouseWorldPosition = GetMouseWorldPosition();
                lastFrameTime = Time.time;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            rb.velocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (isDragging && !hasCollided)
        {
            MoveObjectWithMouseSpeed();
        }
        else if (hasCollided)
        {
            if (collidedCollider != null)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (!collidedCollider.Raycast(ray, out hit, Mathf.Infinity))
                {
                    hasCollided = false;
                }
            }
        }
    }

    private void MoveObjectWithMouseSpeed()
    {
        Vector3 currentMouseWorldPosition = GetMouseWorldPosition();
        Vector3 mouseDelta = currentMouseWorldPosition - lastMouseWorldPosition;
        float deltaTime = Time.time - lastFrameTime;

        Vector3 mouseVelocity = mouseDelta / deltaTime;   

        Vector3 direction = mouseDelta.normalized;

        rb.velocity = direction * mouseVelocity.magnitude * 1f;   

        lastMouseWorldPosition = currentMouseWorldPosition;
        lastFrameTime = Time.time;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);   
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);   
        }

        return lastMouseWorldPosition;  
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    rb.velocity = Vector3.zero;
    //    hasCollided = true;
    //    collidedCollider = collision.collider;
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.collider == collidedCollider)
    //    {
    //        collidedCollider = null;
    //    }
    //}
}
