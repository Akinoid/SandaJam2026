using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class IsometricMovement : MonoBehaviour
{
   [Header("Movimiento")]
    public float speed = 5f;
    public float acceleration = 25f;
    public float deceleration = 30f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 currentVelocity = Vector3.zero;
    public Vector3 CurrentInputDir { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 inputDir = new Vector3(moveInput.x, 0f, moveInput.y);
        bool hasInput = inputDir.sqrMagnitude > 0.01f;
        inputDir = hasInput ? inputDir.normalized : Vector3.zero;

        CurrentInputDir = inputDir;

        Vector3 targetVel = inputDir * speed;
        float rate = hasInput ? acceleration : deceleration;
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVel, rate * Time.fixedDeltaTime);

        Vector3 finalVelocity = currentVelocity;
        finalVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = finalVelocity;
    }
}