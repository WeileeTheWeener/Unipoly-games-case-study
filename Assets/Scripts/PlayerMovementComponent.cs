using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementComponent : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float fallingForce;
    [SerializeField] bool isGrounded;
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        HandleJumping();
    }
    private void FixedUpdate()
    {
        HandleMovement();     
    }
    private void HandleMovement()
    {   

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 moveDirection = (cameraForward.normalized * vertical + cameraRight.normalized * horizontal).normalized;

        rb.AddForce(moveDirection * movementSpeed * Time.fixedDeltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        targetRotation.eulerAngles = new Vector3(0, targetRotation.eulerAngles.y, 0);
        transform.rotation = targetRotation;

        //original movement code
        //Vector3 movementAxis = new Vector3(horizontal, 0, vertical);
        //rb.AddForce(movementAxis * movementSpeed * Time.deltaTime);

    }
    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        if(rb.velocity.y <= -0.1f)
        {
            rb.AddForce(Vector3.down * fallingForce, ForceMode.Acceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

}

