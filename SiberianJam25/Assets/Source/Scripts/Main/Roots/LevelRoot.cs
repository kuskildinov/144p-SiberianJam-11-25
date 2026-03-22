using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelRoot : CompositeRoot
{
    [SerializeField] private PlayerRoot _playerRoot;
    [Header("World State")]
    [SerializeField] private WorldStateSwitcher _worldStateSwitcher;
    [Header("NPC")]
    [SerializeField] private LevelNpcHandler _npcHandler;
    [Header("Puzzles")]
    [SerializeField] private LevelPuzzlesHandler _puzzlesHandler;    
    [Header("Monsters")]
    [SerializeField] private LevelMonstersHandler _levelMonstersHandler;
    [Header("UI")]
    [SerializeField] private LevelUI _levelUI;
    [Header("PostProcess")]
    [SerializeField] private LevelPostProcessHandler _postProcessHandler;
    [Header("SkyBox")]
    [SerializeField] private SkyBoxHandler _skyBoxHandler;
    [Header("other")]
    [SerializeField] private PlayerRoom _playerRoom;
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
        _npcHandler.Initialize(this);
        _levelMonstersHandler.Initialize(this);
        _puzzlesHandler.Initialize(this);
        _worldStateSwitcher.Initialize(this);
        _postProcessHandler.Initialize(this);
        _skyBoxHandler.Initialize(this);

        _playerRoom?.Initialize(this);
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
        WorldStateChanged?.Invoke(_currentWorldState);
    }

    public void TryShowBadWorld()
    {
        _currentWorldState = WorldState.BAD;      
        WorldStateChanged?.Invoke(_currentWorldState);
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

