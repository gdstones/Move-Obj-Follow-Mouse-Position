using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    private bool isDragging = false;
    private Camera mainCamera;
    private Rigidbody rb;

    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
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
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            MoveObjectWithVelocity();
            // MoveObjectWithForce();  
            // MoveObjectWithMovePosition(); 
        }
    }



    private void MoveObjectToMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            if (!Physics.CheckBox(targetPosition, Vector3.one * 0.5f))
            {
                rb.MovePosition(targetPosition);
            }
        }
    }

    private void MoveObjectWithVelocity()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            Vector3 direction = (targetPosition - transform.position).normalized;

            if (!Physics.CheckBox(targetPosition, Vector3.one * 0.5f))
            {
                rb.velocity = direction * 100f;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void MoveObjectWithForce()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            Vector3 direction = (targetPosition - transform.position).normalized;

            if (!Physics.CheckBox(targetPosition, Vector3.one * 0.5f))
            {
                rb.AddForce(direction * 50f);
            }
        }
    }

    private void MoveObjectWithMovePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);

            if (!Physics.CheckBox(targetPosition, Vector3.one * 0.5f))
            {
                rb.MovePosition(targetPosition);
            }
        }
    }



}
