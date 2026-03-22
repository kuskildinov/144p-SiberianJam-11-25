using UnityEngine;

public class WorldStateSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _pinkWorld;
    [SerializeField] private GameObject _badWorld;

    private LevelRoot _root;

    public void Initialize(LevelRoot root)
    {
        _root = root;

        SubscribeToEvents();
    }

    private void ShowPinkWorld()
    {
        _pinkWorld.gameObject.SetActive(true);
        _badWorld.gameObject.SetActive(false);
    }

    private void ShowBadWorld()
    {
        _pinkWorld?.gameObject.SetActive(false);
        _badWorld?.gameObject.SetActive(true);
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
        if (newState == WorldState.PINK)
        {
            ShowPinkWorld();
        }
        else if (newState == WorldState.BAD)
        {
            ShowBadWorld();
        }
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
