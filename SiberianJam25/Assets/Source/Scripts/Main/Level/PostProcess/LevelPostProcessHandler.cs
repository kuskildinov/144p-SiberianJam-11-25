using UnityEngine;
using UnityEngine.Rendering;

public class LevelPostProcessHandler : MonoBehaviour
{
    [SerializeField] private Volume _volume;
    [SerializeField] private VolumeProfile _goodPrifile;
    [SerializeField] private VolumeProfile _badProfile;

    private LevelRoot _root;

    public void Initialize(LevelRoot root)
    {
        _root = root;

        SubscribeToEvents();
    }

    private void SetGoodProfile()
    {
        _volume.profile = _goodPrifile;
    }

    private void SetBadProfile()
    {
        _volume.profile = _badProfile;
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
            SetGoodProfile();
        }
        else if(newState == WorldState.BAD)
        {
            SetBadProfile();
        }
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
