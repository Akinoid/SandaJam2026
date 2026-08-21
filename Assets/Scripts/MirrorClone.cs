using UnityEngine;

public class MirrorClone : MonoBehaviour
{
   [Header("Estado")]
    public bool isLocked = false;

    private Transform player;
    private Vector3 spawnPosition;   
    private float spawnYaw;          

    [Header("Beam")]
    public Vector3 beamOriginOffset = new Vector3(0f, 1f, 0f);
    public float beamForwardOffset = 0.15f;

    public Vector3 BeamOrigin => transform.position + beamOriginOffset + transform.forward * beamForwardOffset;

    
    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;

        
        spawnPosition = player.position;
        spawnYaw = player.eulerAngles.y;

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
        
        Vector3 mirroredPos = 2f * spawnPosition - player.position;
        mirroredPos.y = spawnPosition.y; 
        transform.position = mirroredPos;

        
        float playerYaw = player.eulerAngles.y;
        float mirroredYaw = 2f * spawnYaw - playerYaw;
        transform.rotation = Quaternion.Euler(0f, mirroredYaw, 0f);
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
