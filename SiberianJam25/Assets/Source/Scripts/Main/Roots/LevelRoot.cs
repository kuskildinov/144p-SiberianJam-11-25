using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRoot : CompositeRoot
{
    [SerializeField] private EnviernmentSwitcher _enviernemtSwitcher;
    [SerializeField] private PlayerRoot _playerRoot;
    [SerializeField] private PlayerRoom _playerRoom;
    [Header("Puzzles")]
    [SerializeField] private MainDoorIndicators _mainDoorIndicator;
    [SerializeField] private List<MainLever> _levers;
    [SerializeField] private CodeGatePuzzle _codeGamePuzzle;
    [SerializeField] private FindObjectPuzzle _findObjectPuzzle;
    [Header("UI")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _startFadePanel;
    [SerializeField] private GameObject _switchOffGlassesInfoPanel;

    private WorldState _currentWorldState;

    public override void Compose()
    {
        StartCoroutine(StartFadePanelRoutine());

        _currentWorldState = WorldState.PINK;

        _mainDoorIndicator?.Initialize();
        _codeGamePuzzle?.Initialize();
        _findObjectPuzzle?.Initialize();
        _playerRoom.Initialize(this);

        InitializePuzzles();
    }

    #region >>> UI

    public void ShowSwitchOffInfoPanel()
    {
        _playerRoot.Player.CanSwitchGlasses = true;
        StartCoroutine(ShowSwitchOffGlassesInfoRoutine());
    }

    private IEnumerator ShowSwitchOffGlassesInfoRoutine()
    {
        _switchOffGlassesInfoPanel.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(5f);
        _switchOffGlassesInfoPanel.gameObject.SetActive(false);
    }

    #endregion

    #region >>> WORLD SWITCHER
    public void TryShowPinkWorld()
    {
        _enviernemtSwitcher.ShowPinkWorld();
    }

    public void TryShowBadWorld()
    {
        _enviernemtSwitcher.ShowBadWorld();
    }

    #endregion

    #region >>> PUZZLES

    public void OnPuzzleComplited(int index)
    {
        switch(index)
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
        }

        _mainDoorIndicator.OnPuzzleComplited();

        if(CheckAllPuzzlesReady())
        {
            //Открываем дверь
        }
    }

    private void InitializePuzzles()
    {
        foreach (MainLever lever in _levers)
        {
            lever.Initialize(this);
        }
    }

    private bool CheckAllPuzzlesReady()
    {
        return (GlobalVars.PuzzleOneReady && GlobalVars.PuzzleTwoReady && GlobalVars.PuzzleTreeReady);
    }

    #endregion

    #region >>> WIN LOSE

    public void OnWinGame()
    {

    }

    public void OnGameOver()
    {
        _gameOverPanel.gameObject.SetActive(true);
    }

    public void RestartLevel()
    {
        StartCoroutine(RestartLevelRoutine());
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(GlobalVars.MainMenuSceneName);
    }

    private IEnumerator RestartLevelRoutine()
    {
        _playerRoot.RestartLevel();
        yield return new WaitForSecondsRealtime(1f);
        _gameOverPanel.gameObject.SetActive(false);
        yield return StartFadePanelRoutine();
    }

    private IEnumerator StartFadePanelRoutine()
    {
        _startFadePanel.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        _startFadePanel.gameObject.SetActive(false);
    }
    #endregion
}

public enum WorldState
{
    PINK,
    BAD,
}

