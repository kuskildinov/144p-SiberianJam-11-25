using UnityEngine;

public class LevelNpcHandler : MonoBehaviour
{
    private LevelRoot _root;

    public void Initialize(LevelRoot root)
    {
        _root = root;

        SubscribeToEvents();
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
        
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
