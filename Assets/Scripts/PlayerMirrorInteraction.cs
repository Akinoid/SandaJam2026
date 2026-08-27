using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMirrorInteraction : MonoBehaviour
{
    public InputActionReference interactAction; 
    public IsometricMovement playerMovement;
    public float searchRadius = 3f;
    public LayerMask mirrorLayer;

    void OnEnable() => interactAction.action.performed += OnInteract;
    void OnDisable() => interactAction.action.performed -= OnInteract;

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        FullBodyMirror nearest = FindNearestUnbrokenMirrorInRange();

        if (nearest != null)
        {
            nearest.Break(playerMovement);
        }
        else
        {
            MirrorFragment active = FragmentManager.Instance.GetActiveFragment();
            active?.ToggleLock();
        }
    }

    private FullBodyMirror FindNearestUnbrokenMirrorInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, searchRadius, mirrorLayer);
        FullBodyMirror nearest = null;
        float minDist = float.MaxValue;

        foreach (var col in hits)
        {
            FullBodyMirror mirror = col.GetComponent<FullBodyMirror>();
            if (mirror == null || mirror.IsBroken) continue;
            if (!mirror.IsInRange(transform.position)) continue;

            float dist = Vector3.Distance(transform.position, mirror.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = mirror;
            }
        }
        return nearest;
    }
}
