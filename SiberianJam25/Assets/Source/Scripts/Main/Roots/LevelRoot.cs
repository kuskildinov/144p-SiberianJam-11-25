using System;
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
    [SerializeField] private MainTower _mainTower;
    [SerializeField] private List<MainLever> _levers;
    [SerializeField] private CodeGatePuzzle _codeGamePuzzle;
    [SerializeField] private FindObjectPuzzle _findObjectPuzzle;
    [SerializeField] private Blender _blender;
    [SerializeField] private List<GameObject> _nums;
    [SerializeField] private List<GameObject> _symbols;
    [Header("Monsters")]
    [SerializeField] private ShopMonster _shopMonster;
    [Header("UI")]
    [SerializeField] private BlackFadePanel _blackFadePanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _startFadePanel;
    [SerializeField] private GameObject _switchOffGlassesInfoPanel;
    [SerializeField] private GameObject _finalPanel;
    [Header("PostProcess")]
    [SerializeField] private GameObject _pinkVolume;
    [SerializeField] private GameObject _badVolume;
    [Header("SkyBox Settings")]
    [SerializeField] private Material _pinkkybox;
    [SerializeField] private Material _badSkybox;
    [Header("Final")]
    [SerializeField] private GameObject _finalCutScene; 

    private WorldState _currentWorldState;
    private bool _gameOver = false;

    public WorldState CurrensState => _currentWorldState;

    public event Action<WorldState> WorldStateChanged;

    public override void Compose()
    {
        _blackFadePanel.PlayFadeOffAnimation();

        _currentWorldState = WorldState.PINK;

        _mainTower?.Initialize(this);
        _codeGamePuzzle?.Initialize();
        _findObjectPuzzle?.Initialize();
        _playerRoom?.Initialize(this);
        _shopMonster?.Initialize(this);
        _blender?.Initialize(this);

        InitializePuzzles();
    }

    private void Update()
    {
        if(_gameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }
    }

    #region >>> UI

    public void ShowSwitchOffInfoPanel()
    {
        _playerRoot.Player.CanSwitchGlasses = true;
        StartCoroutine(ShowSwitchOffGlassesInfoRoutine());
    }

    private IEnumerator ShowSwitchOffGlassesInfoRoutine()
    {
        _switchOffGlassesInfoPanel?.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(5f);
        _switchOffGlassesInfoPanel?.gameObject.SetActive(false);
    }
       

    #endregion
    #region >>> WORLD SWITCHER
    public void TryShowPinkWorld()
    {
        _currentWorldState = WorldState.PINK;
        _enviernemtSwitcher.ShowPinkWorld();
        TrySwitchPostProcessVolume();
       
        WorldStateChanged?.Invoke(_currentWorldState);

        ShowNums();
    }

    public void TryShowBadWorld()
    {
        _currentWorldState = WorldState.BAD;
        _enviernemtSwitcher.ShowBadWorld();

        TrySwitchPostProcessVolume();
        TryChangeSkyBox();

        WorldStateChanged?.Invoke(_currentWorldState);

        ShowSymbols();
    }

    private void TrySwitchPostProcessVolume()
    {
        if (_pinkVolume == null || _badVolume == null)
            return;

        if (_currentWorldState == WorldState.PINK)
        {
            _badVolume.gameObject.SetActive(false);
            _pinkVolume.gameObject.SetActive(true);
        }
        else
        {
            _pinkVolume.gameObject.SetActive(false);
            _badVolume.gameObject.SetActive(true);
        }
    }

    private void TryChangeSkyBox()
    {
        if (_pinkkybox == null || _badSkybox == null)
            return;

        if (_currentWorldState == WorldState.PINK)
        {
            RenderSettings.skybox = _pinkkybox;
        }
        else
        {
            RenderSettings.skybox = _badSkybox;
        }
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

        _mainTower.OnPuzzleComplited(index);

        if(CheckAllPuzzlesReady())
        {
            _mainTower.OpenGate();
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
        _gameOver = true;
        _playerRoot.DeactivatePlayer();
        _playerRoot.Player.gameObject.SetActive(false);

        _finalPanel?.gameObject.SetActive(true);
        _finalCutScene?.gameObject.SetActive(true);
    }

    public void OnGameOver()
    {
        _gameOverPanel?.gameObject.SetActive(true);       
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
        _blackFadePanel.PlayOnAndOffAnimation();
        _gameOverPanel?.gameObject.SetActive(false);
        _playerRoot.BackPlayerToStart();
        
        yield return null;
    }

    #endregion

   
}

public enum WorldState
{
    PINK,
    BAD,
}

