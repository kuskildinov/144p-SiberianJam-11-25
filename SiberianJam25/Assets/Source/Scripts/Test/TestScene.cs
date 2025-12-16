using UnityEngine;
using UnityEngine.UI;

public class TestScene : MonoBehaviour
{
    [SerializeField] private Text _currentWorldStateText;
    [Header("Links")]
    [SerializeField] private PlayerRoot _playerRoot;
    [SerializeField] private LevelRoot _levelRoot;

    private void OnEnable()
    {
        _levelRoot.OnWorldStateChanged += OnWorldStateChanged;
    }

    private void OnDisable()
    {
        _levelRoot.OnWorldStateChanged -= OnWorldStateChanged;
    }

    private void OnWorldStateChanged(WorldState newState)
    {        
        _currentWorldStateText.text = $"{newState}";
    }
}
