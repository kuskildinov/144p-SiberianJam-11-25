using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRoot : CompositeRoot
{
    [SerializeField] private WorldStateSwitcher _worldStateSwitcher;
    [SerializeField] private PlayerRoot _playerRoot;
    [SerializeField] private PlayerRoom _playerRoom;
    [Header("Puzzles")]
    [SerializeField] private LevelPuzzlesHandler _puzzlesHandler;    
    [Header("Monsters")]
    [SerializeField] private LevelMonstersHandler _levelMonstersHandler;
    [Header("UI")]
    [SerializeField] private LevelUI _levelUI;
    [Header("PostProcess")]
    [SerializeField] private LevelPostProcessHandler _postProcessHandler;
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
        _levelUI.ShowBlackFadeOff();

        _currentWorldState = WorldState.PINK;
        _levelUI?.Initialize(this);      
              
        _playerRoom?.Initialize(this);

        _levelMonstersHandler.Initialize(this);
        _puzzlesHandler.Initialize(this);
        _postProcessHandler.Initialize(this);
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
        _playerRoot.ShowPlayerTipsByType(PlayerTipsType.GlassSwitch);
    }

    public void ShowPausePanel()
    {
        _levelUI.ShowPausePanel();
    }

    public void HidePausePanel()
    {
        _levelUI.HidePausePanel();
    }
    #endregion
    #region >>> WORLD SWITCHER
    public void TryShowPinkWorld()
    {
        _currentWorldState = WorldState.PINK;
        _worldStateSwitcher.ShowPinkWorld();
       
        WorldStateChanged?.Invoke(_currentWorldState);
    }

    public void TryShowBadWorld()
    {
        _currentWorldState = WorldState.BAD;
        _worldStateSwitcher.ShowBadWorld();
       
        TryChangeSkyBox();

        WorldStateChanged?.Invoke(_currentWorldState);
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
    #region >>> WIN LOSE

    public void OnWinGame()
    {
        _gameOver = true;
        _playerRoot.DeactivatePlayer();
        _playerRoot.Player.gameObject.SetActive(false);
       
        _finalCutScene?.gameObject.SetActive(true);
    }

    public void OnGameOvered()
    {
        _levelUI.ShowGameOverPanel();
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
        _levelUI.ShowBlackFadeOff();
        _levelUI.HideGameOverPanel();
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

