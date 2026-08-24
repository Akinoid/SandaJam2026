using UnityEngine;
using UnityEngine.InputSystem;

public class MouseAimRotator : MonoBehaviour
{
    [Header("Referencias")]
    public Camera cam;
    public InputActionReference lookAction; 

    [Header("Rotación")]
    public float rotationSpeed = 1080f; 

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (cam == null) cam = Camera.main;
    }

    void OnEnable() => lookAction.action.Enable();
    void OnDisable() => lookAction.action.Disable();

    void FixedUpdate()
    {
        Vector2 mouseScreenPos = lookAction.action.ReadValue<Vector2>();
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
}
