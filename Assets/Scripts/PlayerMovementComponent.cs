using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementComponent : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        HandleMovement();
        LookAtMouseDirection();
    }
    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movementAxis = new Vector3 (horizontal,0,vertical);
        rb.AddForce(movementAxis * movementSpeed * Time.deltaTime);
        
    }
    private void LookAtMouseDirection()
    {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 rayHitPoint = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(rayHitPoint);
        }
    }
}
