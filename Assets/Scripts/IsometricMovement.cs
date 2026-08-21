using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class IsometricMovement : MonoBehaviour
{
     [Header("Movimiento")]
    public float speed = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();     
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        Vector3 targetVelocity = moveDir * speed;
        targetVelocity.y = rb.linearVelocity.y; 

        rb.linearVelocity = targetVelocity;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            rb.MoveRotation(Quaternion.LookRotation(moveDir));
        }
    }
}