using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class PlayerLight : MonoBehaviour
{
    [Header("Configuracion del Beam")]
    public float maxDistance = 20f;
    public LayerMask hitLayers = ~0;
    public Vector3 originOffset = new Vector3(0f, 0f, 0f);
    public int maxBounces = 10;

    [Header("Input")]
    public InputActionReference beamAction;

    [Header("Condiciones para disparar")]
    [Tooltip("Velocidad maxima (unidades/seg) permitida para poder disparar")]
    public float maxMoveSpeed = 0.05f;
    [Tooltip("Grados por segundo maximos de cambio de mira permitidos para poder disparar")]
    public float maxLookSpeed = 5f;

    [Header("Debug / Gizmos")]
    public Color rayColor = Color.red;
    public Color hitColor = Color.green;
    public float hitSphereRadius = 0.2f;

    [HideInInspector] public bool hasHit;
    [HideInInspector] public RaycastHit hitInfo; 

    private bool isFiring = false;
    private Vector3 lastPosition;
    private Vector3 lastForward;
    private bool initialized = false;

    private readonly List<Vector3> beamPoints = new List<Vector3>();

    void OnEnable() => beamAction.action.Enable();
    void OnDisable() => beamAction.action.Disable();

    void Update()
    {
        if (!initialized)
        {
            lastPosition = transform.position;
            lastForward = transform.forward;
            initialized = true;
            return;
        }

        bool buttonHeld = beamAction.action.IsPressed();

        float moveSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        float lookSpeed = Vector3.Angle(transform.forward, lastForward) / Time.deltaTime;

        bool isStationary = moveSpeed <= maxMoveSpeed;
        bool isAimStable = lookSpeed <= maxLookSpeed;

        isFiring = buttonHeld && isStationary && isAimStable;

        if (isFiring)
            CastBeam();
        else
        {
            hasHit = false;
            beamPoints.Clear();
        }

        lastPosition = transform.position;
        lastForward = transform.forward;
    }

    void CastBeam()
    {
        beamPoints.Clear();

        Vector3 origin = transform.position + originOffset;
        Vector3 direction = transform.forward;
        beamPoints.Add(origin);

        hasHit = false;
        int bounces = 0;

        while (bounces < maxBounces)
        {
            bool didHit = Physics.Raycast(origin, direction, out hitInfo, maxDistance, hitLayers);

            if (!didHit)
            {
                beamPoints.Add(origin + direction * maxDistance);
                break;
            }

            beamPoints.Add(hitInfo.point);

            LightReflector reflector = hitInfo.collider.GetComponent<LightReflector>();
            if (reflector != null)
            {
                origin = reflector.BeamOrigin;
                direction = reflector.ReflectDirection;
                bounces++;
                continue;
            }

            hasHit = true; 

            StoneLantern lantern = hitInfo.collider.GetComponent<StoneLantern>();
            if (lantern != null)
                lantern.ReceiveLight();

            break;
        }

        for (int i = 0; i < beamPoints.Count - 1; i++)
        {
            bool isLastSegment = i == beamPoints.Count - 2;
            Debug.DrawLine(beamPoints[i], beamPoints[i + 1], (isLastSegment && hasHit) ? hitColor : rayColor);
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying || !isFiring || beamPoints.Count < 2) return;

        for (int i = 0; i < beamPoints.Count - 1; i++)
        {
            bool isLastSegment = i == beamPoints.Count - 2;
            Gizmos.color = (isLastSegment && hasHit) ? hitColor : rayColor;
            Gizmos.DrawLine(beamPoints[i], beamPoints[i + 1]);
        }

        if (hasHit)
            Gizmos.DrawWireSphere(beamPoints[beamPoints.Count - 1], hitSphereRadius);
    }
}