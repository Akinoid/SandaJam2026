using UnityEngine;

public class MirrorClone : MonoBehaviour
{
   [Header("Estado")]
    public bool isLocked = false;

    private Transform player;
    private Vector3 spawnPosition;   
            

    [Header("Beam")]
    public Vector3 beamOriginOffset = new Vector3(0f, 0f, 0f);
    public float beamForwardOffset = 0.15f;

    public Vector3 BeamOrigin => transform.position + beamOriginOffset + transform.forward * beamForwardOffset;

    
    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
        spawnPosition = player.position;

        transform.position = spawnPosition;
        transform.rotation = player.rotation;
    }

    void Update()
    {
        if (isLocked || player == null) return;

        UpdateMirrorTransform();
    }

    private void UpdateMirrorTransform()
    {
        
        Vector3 offset = player.position - spawnPosition;
        Vector3 mirroredOffset = new Vector3(
            offset.x,   
            0f,
            -offset.z   
        );

        Vector3 newPos = spawnPosition + mirroredOffset;
        newPos.y = spawnPosition.y; 
        transform.position = newPos;
        
        Vector3 playerForward = player.forward;
        Vector3 mirroredForward = new Vector3(playerForward.x, 0f, -playerForward.z);

        if (mirroredForward.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(mirroredForward.normalized);
        }
    }

    public void ToggleLock()
    {
        isLocked = !isLocked;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = isLocked ? Color.cyan : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawRay(transform.position + beamOriginOffset, transform.forward * 1.5f);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(spawnPosition, 0.15f); 
        }
    }
}
