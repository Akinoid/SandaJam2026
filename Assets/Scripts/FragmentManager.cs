using System.Collections.Generic;
using UnityEngine;

public class FragmentManager : MonoBehaviour
{
    public static FragmentManager Instance { get; private set; }

    public int maxFragments = 3;
    private readonly List<MirrorFragment> fragments = new List<MirrorFragment>();
    private MirrorFragment activeFragment;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public int ActiveFragmentCount => fragments.Count;

    public void RegisterFragment(MirrorFragment fragment)
    {
        fragments.Add(fragment);
        SetActiveFragment(fragment);
    }

    public void UnregisterFragment(MirrorFragment fragment)
    {
        fragments.Remove(fragment);
        if (activeFragment == fragment)
            activeFragment = fragments.Count > 0 ? fragments[fragments.Count - 1] : null;
    }

    public void SetActiveFragment(MirrorFragment fragment)
    {
        if (activeFragment != null) activeFragment.SetActive(false);
        activeFragment = fragment;
        if (activeFragment != null) activeFragment.SetActive(true);
    }

    public MirrorFragment GetActiveFragment() => activeFragment;
}
