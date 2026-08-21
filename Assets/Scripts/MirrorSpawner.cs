using UnityEngine;
using UnityEngine.InputSystem;

public class MirrorSpawner : MonoBehaviour
{
   [Header("Prefab")]
    public GameObject mirrorPrefab;

    private MirrorClone currentMirror; 

    
    public void OnSpawnMirror(InputValue value)
    {
        if (!value.isPressed) return;
        SpawnAtPlayer();
    }

    public void SpawnAtPlayer()
    {
        GameObject clone = Instantiate(mirrorPrefab, transform.position, transform.rotation);
        currentMirror = clone.GetComponent<MirrorClone>();
        currentMirror.Initialize(transform);
    }

    
    public void OnLockMirror(InputValue value)
    {
        if (!value.isPressed) return;
        currentMirror?.ToggleLock();
    }
}
