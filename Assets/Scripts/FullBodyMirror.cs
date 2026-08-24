using UnityEngine;

public class FullBodyMirror : MonoBehaviour
{
    [Header("Config")]
    public float interactionRange = 2.5f;
    public GameObject fragmentPrefab;

    [Header("Visual")]
    public GameObject mirrorVisual;
    public Collider mirrorCollider;

    private bool isBroken = false;

    public bool IsBroken => isBroken;

    public bool IsInRange(Vector3 fromPosition)
    {
        return Vector3.Distance(transform.position, fromPosition) <= interactionRange;
    }

    public void Break(IsometricMovement playerMovement)
    {
        if (isBroken) return;

        if (FragmentManager.Instance.ActiveFragmentCount >= FragmentManager.Instance.maxFragments)
        {
            Debug.Log("Máximo de fragmentos alcanzado.");
            return;
        }

        isBroken = true;
        if (mirrorVisual) mirrorVisual.SetActive(false);
        if (mirrorCollider) mirrorCollider.enabled = false;

        GameObject fragObj = Instantiate(fragmentPrefab, transform.position, transform.rotation);
        MirrorFragment frag = fragObj.GetComponent<MirrorFragment>();
        frag.Initialize(playerMovement, this);

        FragmentManager.Instance.RegisterFragment(frag);
    }

    public void Reform()
    {
        isBroken = false;
        if (mirrorVisual) mirrorVisual.SetActive(true);
        if (mirrorCollider) mirrorCollider.enabled = true;
    }
}
