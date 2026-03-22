using UnityEngine;

public class SkyBoxHandler : MonoBehaviour
{
    [Header("SkyBox Settings")]
    [SerializeField] private Material _pinkkybox;
    [SerializeField] private Material _badSkybox;

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
        if(_pinkkybox == null || _badSkybox == null)
            return;

        if (newState == WorldState.PINK)
        {
            RenderSettings.skybox = _pinkkybox;
        }
        else if(newState == WorldState.BAD)
        {
            RenderSettings.skybox = _badSkybox;
        }
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
