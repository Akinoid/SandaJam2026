using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [Header("Configuración del Beam")]
    public float maxDistance = 20f;
    public LayerMask hitLayers = ~0; 
    public Vector3 originOffset = new Vector3(0f, 1f, 0f); 

    [Header("Debug / Gizmos")]
    public Color rayColor = Color.red;
    public Color hitColor = Color.green;
    public float hitSphereRadius = 0.2f;
    
    [HideInInspector] public bool hasHit;
    [HideInInspector] public RaycastHit hitInfo;

    void Update()
    {
        CastBeam();
    }

    void CastBeam()
    {
        Vector3 origin = transform.position + originOffset;
        Vector3 direction = transform.forward;

        hasHit = Physics.Raycast(origin, direction, out hitInfo, maxDistance, hitLayers);

        if (hasHit)
        {
            Debug.DrawLine(origin, hitInfo.point, hitColor);
        }
        else
        {
            Debug.DrawRay(origin, direction * maxDistance, rayColor);
        }
    }
    
    void OnDrawGizmos()
    {
        Vector3 origin = transform.position + originOffset;
        Vector3 direction = transform.forward;

        if (Application.isPlaying && hasHit)
        {
            Gizmos.color = hitColor;
            Gizmos.DrawLine(origin, hitInfo.point);
            Gizmos.DrawWireSphere(hitInfo.point, hitSphereRadius);
        }
        else
        {
            Gizmos.color = rayColor;
            Gizmos.DrawRay(origin, direction * maxDistance);
        }
    }
}
