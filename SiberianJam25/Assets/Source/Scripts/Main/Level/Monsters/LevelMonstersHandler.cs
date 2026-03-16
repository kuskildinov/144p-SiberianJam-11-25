using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMonstersHandler : MonoBehaviour
{
    [SerializeField] private Monster[] _monsters;
    private LevelRoot _root;

    public void Initialize(LevelRoot root)
    {
        _root = root;

        InitializeMonsters();
    }

    private void InitializeMonsters()
    {
        if (_monsters == null || _monsters.Length <= 0)
            _monsters = FindObjectsByType<Monster>(FindObjectsSortMode.None);

        foreach (Monster monster in _monsters)
        {
            monster.Initialize(this);
        }

        Debug.Log($"{_monsters.Length} monsters Inited");
    }

    #region >>> CHANGE VISUAL

    private void SetAllMonstersGoodVisual()
    {
        foreach (Monster monster in _monsters)
        {
            monster.ShowGoodMesh();
        }
    }

    private void SetAllMonstersBadVisual()
    {
        foreach (Monster monster in _monsters)
        {
            monster.ShowBadMesh();
        }
    }

    #endregion
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
            SetAllMonstersGoodVisual();
        }
        else if (newState == WorldState.BAD)
        {
            SetAllMonstersBadVisual();
        }
    }
    #endregion
}
