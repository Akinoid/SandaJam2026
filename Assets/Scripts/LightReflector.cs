using UnityEngine;

public class LightReflector : MonoBehaviour
{
    public Vector3 beamOriginOffset = new Vector3(0f, 0f, 0f);
    public float beamForwardOffset = 0.15f;

    [Header("Gizmo")]
    public Color gizmoColor = Color.cyan;
    public float gizmoRayLength = 3f;
    public float gizmoSphereRadius = 0.15f;

    public Vector3 BeamOrigin => transform.position + beamOriginOffset + transform.forward * beamForwardOffset;
    public Vector3 ReflectDirection => transform.forward;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        
        Gizmos.DrawWireSphere(BeamOrigin, gizmoSphereRadius);

       
        Gizmos.DrawRay(BeamOrigin, ReflectDirection * gizmoRayLength);
    }
}
