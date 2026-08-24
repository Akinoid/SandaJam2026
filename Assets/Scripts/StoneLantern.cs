using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class StoneLantern : MonoBehaviour
{
     [Header("Duración")]
    public bool infiniteDuration = true;
    [Tooltip("Solo se usa si infiniteDuration está desactivado")]
    public float duration = 5f;

    [Header("Área de efecto")]
    [Tooltip("Radio del área que activa Torii Gates cercanos")]
    public float areaRadius = 5f;

    [Header("Visual (opcional)")]
    public Light lanternLight;
    public Light lanternLight2;

    

    public bool IsLit { get; private set; }

    private float remainingTime;
    private SphereCollider areaTrigger;
    private readonly List<ToriGate> gatesInRange = new List<ToriGate>();

    void Awake()
    {
        areaTrigger = GetComponent<SphereCollider>();
        areaTrigger.isTrigger = true;
        SyncAreaRadius();
        SetVisual(false);
    }

    void OnValidate()
    {
        
        if (TryGetComponent(out SphereCollider col))
        {
            col.isTrigger = true;
            col.radius = areaRadius;
        }
    }

    void SyncAreaRadius()
    {
        areaTrigger.radius = areaRadius;
    }

    void Update()
    {
        if (IsLit && !infiniteDuration)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
                TurnOff();
        }
    }

    
    public void ReceiveLight()
    {
        if (IsLit) return; 
        TurnOn();
    }

    private void TurnOn()
    {
        IsLit = true;
        remainingTime = duration;
        SetVisual(true);

        foreach (var gate in gatesInRange)
            gate.AddLightSource(this);
    }

    private void TurnOff()
    {
        IsLit = false;
        SetVisual(false);

        foreach (var gate in gatesInRange)
            gate.RemoveLightSource(this);
    }

    private void SetVisual(bool on)
    {
        if (lanternLight != null) lanternLight.enabled = on;
        if (lanternLight2 != null) lanternLight2.enabled = on;        
    }

    void OnTriggerEnter(Collider other)
    {
        ToriGate gate = other.GetComponent<ToriGate>();
        if (gate == null) return;

        gatesInRange.Add(gate);
        if (IsLit) gate.AddLightSource(this);
    }

    void OnTriggerExit(Collider other)
    {
        ToriGate gate = other.GetComponent<ToriGate>();
        if (gate == null) return;

        gatesInRange.Remove(gate);
        gate.RemoveLightSource(this);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = IsLit ? new Color(1f, 0.8f, 0f, 0.3f) : new Color(0.3f, 0.3f, 0.3f, 0.2f);
        Gizmos.DrawSphere(transform.position, areaRadius);
        Gizmos.color = IsLit ? Color.yellow : Color.gray;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }
    
}
