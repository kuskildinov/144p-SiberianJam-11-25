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
    [SerializeField] private List<GameObject> _nums;
    [SerializeField] private List<GameObject> _symbols;
    [Header("UI")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _startFadePanel;
    [SerializeField] private GameObject _switchOffGlassesInfoPanel;
    [Header("PostProcess")]
    [SerializeField] private GameObject _pinkVolume;
    [SerializeField] private GameObject _badVolume;
    [Header("SkyBox Settings")]
    [SerializeField] private Material _pinkkybox;
    [SerializeField] private Material _badSkybox;

    private WorldState _currentWorldState;

    public WorldState CurrensState => _currentWorldState;

    public override void Compose()
    {
        StartCoroutine(StartFadePanelRoutine());

        _currentWorldState = WorldState.PINK;

        _mainDoorIndicator?.Initialize();
        _codeGamePuzzle?.Initialize();
        _findObjectPuzzle?.Initialize();
        _playerRoom?.Initialize(this);

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
        _currentWorldState = WorldState.PINK;
        _enviernemtSwitcher.ShowPinkWorld();

        _badVolume.gameObject.SetActive(false);
        _pinkVolume.gameObject.SetActive(true);
        RenderSettings.skybox = _pinkkybox;

        ShowNums();
    }

    public void TryShowBadWorld()
    {
        _currentWorldState = WorldState.BAD;
        _enviernemtSwitcher.ShowBadWorld();

        _pinkVolume.gameObject.SetActive(false);
        _badVolume.gameObject.SetActive(true);
        RenderSettings.skybox = _badSkybox;

        ShowSymbols();
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
            _mainDoorIndicator.OpenGate();
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

    public void ShowNums()
    {
        foreach (GameObject num in _nums)
        {
            num.gameObject.SetActive(true);
        }

        foreach (GameObject symbol in _symbols)
        {
            symbol.gameObject.SetActive(false);
        }
    }

    public void ShowSymbols()
    {
        foreach (GameObject num in _nums)
        {
            num.gameObject.SetActive(false);
        }

        foreach (GameObject symbol in _symbols)
        {
            symbol.gameObject.SetActive(true);
        }
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

