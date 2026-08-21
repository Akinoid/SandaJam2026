using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class IsometricMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float acceleration = 25f;
    public float deceleration = 30f;

    
    public float directionChangeAngleThreshold = 20f;
    public float rotationSpeed = 720f;

    public float movingThresholdFraction = 0.6f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector3 facingDir = Vector3.forward;
    private Vector3 currentVelocity = Vector3.zero;

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

        bool isAlreadyMoving = currentVelocity.magnitude > (speed * movingThresholdFraction);

        if (hasInput)
        {
            if (isAlreadyMoving)
            {
                facingDir = inputDir;
                Vector3 targetVel = inputDir * speed;
                currentVelocity = Vector3.MoveTowards(currentVelocity, targetVel, acceleration * Time.fixedDeltaTime);
            }
            else
            {
                float angleDiff = Vector3.Angle(inputDir, facingDir);
                bool directionChanged = angleDiff > directionChangeAngleThreshold;

                facingDir = inputDir;

                if (directionChanged)
                {
                    currentVelocity = Vector3.zero; 
                }
                else
                {
                    Vector3 targetVel = inputDir * speed;
                    currentVelocity = Vector3.MoveTowards(currentVelocity, targetVel, acceleration * Time.fixedDeltaTime);
                }
            }
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }

        Quaternion targetRot = Quaternion.LookRotation(facingDir);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));

        Vector3 finalVelocity = currentVelocity;
        finalVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = finalVelocity;
    }
    
}