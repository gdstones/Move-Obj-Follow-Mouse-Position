using UnityEngine;
using DG.Tweening;

public class ObjectMover_v2 : MonoBehaviour
{
    private bool isDragging = false;
    private Camera mainCamera;
    private Rigidbody rb;
    private Tween moveTween;
    private bool hasCollided = false;
    private Vector3 lastDirection;
    private Vector3 collisionDirection;
    private bool isReversing = false;
    private GameObject collidedObject;  

    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = false;    
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
            moveTween?.Kill();
        }
    }

    private void FixedUpdate()
    {
        if (isDragging && !hasCollided && !isReversing)
        {
            MoveObjectWithVelocityAndDOTween();
        }
        else if (hasCollided && !isReversing)
        {
            Vector3 currentDirection = (mainCamera.ScreenPointToRay(Input.mousePosition).GetPoint(10) - transform.position).normalized;
            float angle = Vector3.Angle(collisionDirection, currentDirection);

            if (angle > 30f)
            {
                hasCollided = false;  
            }
            else if (collidedObject != null)
            {
                Collider collidedCollider = collidedObject.GetComponent<Collider>();
                if (collidedCollider != null && collidedCollider.bounds.Contains(mainCamera.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.y))))
                {
                    return;  
                }
                else
                {
                    hasCollided = false;  
                }
            }
        }
    }

    private void MoveObjectWithVelocityAndDOTween()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            Vector3 direction = (targetPosition - transform.position).normalized;
            lastDirection = direction;  

            moveTween?.Kill();

            moveTween = DOTween.To(() => rb.position, rb.MovePosition, targetPosition, 0.2f)
                               .SetEase(Ease.OutQuad);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (moveTween != null)
        {
            moveTween.Kill();
        }

        hasCollided = true;
        collidedObject = collision.gameObject; 

        Vector3 reversePosition = transform.position - lastDirection * 0.1f;
        collisionDirection = lastDirection;    

        isReversing = true;

        rb.DOMove(reversePosition, 0.1f).OnComplete(() =>
        {
            isReversing = false;
        });
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == collidedObject)
        {
            collidedObject = null; 
        }
        isReversing = false;
    }
}
