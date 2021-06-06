using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class FirstPersonController : MonoBehaviour
{
    public float mouseSensitivityX = 250f;
    public float mouseSensitivityY = 250f;
    public float maxVerticalLookRotation = 60f;
    public float minVerticalLookRotation = -60f;
    public float walkSpeed = 8f;
    public float jumpForce = 220f;
    public LayerMask groundedMask;

    new Rigidbody rigidbody;
    Transform cameraT;
    float verticalLookRotation;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;
    new CapsuleCollider collider;
    float groundedRayDistance;

    bool isGrounded;

    void Start()
    {
        cameraT = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        groundedRayDistance = collider.height / 2 + .1f;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivityX);
        verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, minVerticalLookRotation, maxVerticalLookRotation);
        cameraT.localEulerAngles = Vector3.left * verticalLookRotation;

        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        Vector3 targetMoveAmount = moveDir * walkSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        if (Input.GetButtonDown("Jump"))
        {
            print("Jump!");
            if (isGrounded)
            {
                print("ok");
                rigidbody.AddForce(transform.up * jumpForce);
            }
        }

        isGrounded = false;

        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, groundedRayDistance, groundedMask))
        {
            isGrounded = true;
        }
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
}
