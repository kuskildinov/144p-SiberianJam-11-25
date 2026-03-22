using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPuzzlesHandler : MonoBehaviour
{
    [SerializeField] private MainTower _mainTower;
    [SerializeField] private List<MainLever> _levers;
    [SerializeField] private List<LevelPuzzle> _puzzles;

    private LevelRoot _root;

    public event Action<WorldState> WorlsStateChanged;

    public void Initialize(LevelRoot root)
    {
        _root = root;

        InitializeMainTower();
        InitializeLevers();
        InitializePuzzles();

        SubscribeToEvents();
    }

    #region >>> MAIN TOWER

    private void InitializeMainTower()
    {
        _mainTower?.Initialize(this);
    }

    public void OnLeverActivated(int index)
    {
        switch (index)
        {
            case 0:
                {
                    GlobalVars.PuzzleOneReady = true;
                    break;
                }
            case 1:
                {
                    GlobalVars.PuzzleTwoReady = true;
                    break;
                }
            case 2:
                {
                    GlobalVars.PuzzleTreeReady = true;
                    break;
                }
            case 3:
                {
                    GlobalVars.PuzzleFourReady = true;
                    break;
                }
        }

        _mainTower.OnPuzzleComplited(index);
        if (CheckAllPuzzlesReady())
        {
            _mainTower.OpenGate();
        }

    }

    private bool CheckAllPuzzlesReady()
    {
        return (GlobalVars.PuzzleOneReady && GlobalVars.PuzzleTwoReady && GlobalVars.PuzzleTreeReady && GlobalVars.PuzzleFourReady);
    }

    #endregion
    #region >>> LEVERS
    private void InitializeLevers()
    {
        foreach (MainLever lever in _levers)
        {
            lever.Initialize(this);
        }
    }
    #endregion
    #region >>> PUZZLES

    private void InitializePuzzles()
    {
        foreach (LevelPuzzle puzzle in _puzzles)
        {
            puzzle.Initialize(this);
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
        WorlsStateChanged?.Invoke(newState);
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
