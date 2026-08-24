using UnityEngine;
using System.Collections.Generic;

public class ToriGate : MonoBehaviour
{
    [Header("Estado")]
    public bool IsOpen { get; private set; }

    [Header("Visual / Física")]    
    public Collider blockingCollider; 

    private readonly HashSet<StoneLantern> activeSources = new HashSet<StoneLantern>();

    void Start()
    {
        UpdateState();
    }

    public void AddLightSource(StoneLantern lantern)
    {
        activeSources.Add(lantern);
        UpdateState();
    }

    public void RemoveLightSource(StoneLantern lantern)
    {
        activeSources.Remove(lantern);
        UpdateState();
    }

    private void UpdateState()
    {
        IsOpen = activeSources.Count > 0;

        if (blockingCollider != null) blockingCollider.enabled = !IsOpen;
        Debug.Log(IsOpen);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = IsOpen ? Color.green : Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 2f, 0.3f));
    }
}
