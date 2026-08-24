using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LightReflector))]
public class MirrorFragment : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float acceleration = 25f;
    public float deceleration = 30f;

    [Header("Mira (mouse, solo si está activo)")]
    public Camera cam;
    public float rotationSpeed = 1080f;

    public bool IsLocked { get; private set; } = false;
    public bool IsActive { get; private set; } = false;

    private Rigidbody rb;
    private IsometricMovement playerMovement;
    private FullBodyMirror sourceMirror;
    private Vector3 currentVelocity;

    public void Initialize(IsometricMovement player, FullBodyMirror source)
    {
        playerMovement = player;
        sourceMirror = source;
        if (cam == null) cam = Camera.main;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void FixedUpdate()
    {
        HandleMovement();
        if (IsActive && !IsLocked)
            HandleMouseAim();
    }

    private void HandleMovement()
    {
        if (IsLocked)
        {
            currentVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero; 
            return;
        }

        Vector3 inputDir = playerMovement != null ? playerMovement.CurrentInputDir : Vector3.zero;
        bool hasInput = inputDir.sqrMagnitude > 0.01f;

        Vector3 targetVel = inputDir * speed;
        float rate = hasInput ? acceleration : deceleration;
        currentVelocity = Vector3.MoveTowards(currentVelocity, targetVel, rate * Time.fixedDeltaTime);

        Vector3 finalVel = currentVelocity;
        finalVel.y = rb.linearVelocity.y;
        rb.linearVelocity = finalVel;
    }

    private void HandleMouseAim()
    {
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mouseScreenPos);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, transform.position.y, 0f));
        if (!groundPlane.Raycast(ray, out float enter)) return;

        Vector3 targetPoint = ray.GetPoint(enter);
        Vector3 dir = targetPoint - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir.normalized);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));
    }

    public void ToggleLock() => IsLocked = !IsLocked;
    public void SetActive(bool active) => IsActive = active;

    
    public void DestroyFragment()
    {
        FragmentManager.Instance.UnregisterFragment(this);
        if (sourceMirror != null) sourceMirror.Reform();
        Destroy(gameObject);
    }

    
}
