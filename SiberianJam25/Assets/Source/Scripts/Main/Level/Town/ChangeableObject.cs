using UnityEngine;

public class ChangeableObject : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;
    [Header("Materials")]
    [SerializeField] private Material _pinkMaterial;
    [SerializeField] private Material _badMaterial;

    private LevelRoot _levelRoot;

    private void Start()
    {
        _levelRoot = FindObjectOfType<LevelRoot>();

        if (_levelRoot == null)
        {
            Debug.LogError($"{this.gameObject.name} - Cant Find Level Root");
            return;
        }

        SubscribeToEvents();
    }

    #region >>> EVENTS
    private void SubscribeToEvents()
    {
        _levelRoot.WorldStateChanged += OnWorldStateChanged;
    }

    private void UnsubscribeToEvents()
    {
        _levelRoot.WorldStateChanged -= OnWorldStateChanged;
    }

    private void OnWorldStateChanged(WorldState newState)
    {
        if (newState == WorldState.PINK)
        {
            _renderer.material = _pinkMaterial;
        }
        else if(newState == WorldState.BAD)
        {
            _renderer.material = _badMaterial;
        }
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
