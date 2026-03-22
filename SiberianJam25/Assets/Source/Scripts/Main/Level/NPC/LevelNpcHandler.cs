using System;
using UnityEngine;

public class LevelNpcHandler : MonoBehaviour
{
    private LevelRoot _root;
    private NPC[] _levelNpcs;

    public event Action<WorldState> WorldStateChanged;

    public void Initialize(LevelRoot root)
    {
        _root = root;
        _levelNpcs = FindObjectsByType<NPC>(FindObjectsSortMode.None);
        InitializeNpcs();
        SubscribeToEvents();
    }

    private void InitializeNpcs()
    {
        if(_levelNpcs == null || _levelNpcs.Length <= 0)
        {
            Debug.LogError("Cant Find NPC on Level");
            return;
        }

        foreach (NPC npc in _levelNpcs)
        {
            npc.Initialize(this);
        }
    }

    #region >>> EVENTS
    private void SubscribeToEvents()
    {
        _root.WorldStateChanged += OnWorldStateChanged;
    }

    private void UnsubscribeToEvents()
    {
        _root.WorldStateChanged -= OnWorldStateChanged;
    }

    private void OnWorldStateChanged(WorldState newState)
    {
        WorldStateChanged?.Invoke(newState);
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
