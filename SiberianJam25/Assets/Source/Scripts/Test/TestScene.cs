using UnityEngine;
using UnityEngine.UI;

public class TestScene : MonoBehaviour
{    
    [Header("Links")]
    [SerializeField] private PlayerRoot _playerRoot;
    [SerializeField] private LevelRoot _levelRoot;

    private void OnEnable()
    {
        _levelRoot.WorldStateChanged += OnWorldStateChanged;
    }

    private void OnDisable()
    {
        _levelRoot.WorldStateChanged -= OnWorldStateChanged;
    }

    private void OnWorldStateChanged(WorldState newState)
    {        
       
    }
}
