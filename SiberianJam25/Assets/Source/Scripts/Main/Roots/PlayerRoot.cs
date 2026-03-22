using UnityEngine;

public class PlayerRoot : CompositeRoot
{
    [SerializeField] private Player _player;
    [SerializeField] public LevelRoot _levelRoot;
    [Header("Restart Settings")]
    [SerializeField] private Transform _restartPoint;
    [Header("UI")]
    [SerializeField] private PlayerUI _playerUI;
    [SerializeField] private GameObject _cantTakeItemInfo;
    [SerializeField] private GameObject _interactKeyinfo;

    private bool _isPause;

    public Player Player => _player;
    public bool IsPause => _isPause;

    public override void Compose()
    {
        _player.Initialize(this);

        //ActivatePlayer();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ActivatePlayer()
    {
        _player.Activate();

        ToggleMouse(false);
    }

    public void DeactivatePlayer()
    {
        _player.Deactivate();

        ToggleMouse(true);

        _interactKeyinfo.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        DeactivatePlayer();
        _levelRoot.OnGameOvered();
    }

    public void BackPlayerToStart()
    {
        ActivatePlayer();
        
        _player.CharacterController.enabled = false;
        _player.gameObject.transform.localPosition = _restartPoint.position;
        _player.CharacterController.enabled = true;      
    }

    #region GLASSES
    public void OnGlassesOn() => _levelRoot.TryShowPinkWorld();

    public void OnGlassesOff() => _levelRoot.TryShowBadWorld();

    #endregion
    #region >>> INTERACTION


    #endregion
    #region >>> UI

    public void TryShowInteractionInfo(string infoText)
    {
        _playerUI.TryShowInteractionInfo(infoText);
    }

    public void ShowCantInteractText()
    {
        _playerUI.ShowCantInteractText();
    }
   
    #endregion
    #region >>> DIALOGS

    public void SetDialog(DialogComponent dialogComponent)
    {
        _playerUI.SetDialogPhrase(dialogComponent);
    }

    #endregion
    #region >>> PLAYER TIPS

    public void ShowPlayerTipsByType(PlayerTipsType type)
    {
        _playerUI.ShowTipByType(type);
    }

    #endregion

    private void ToggleMouse(bool value)
    {
        if(value)
            Cursor.lockState = CursorLockMode.None;      
        else
            Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = value;
    }

    private void PauseGame()
    {
        _isPause = true;
        Time.timeScale = 0f;

        DeactivatePlayer();
        ToggleMouse(true);
        _levelRoot.ShowPausePanel();
    }

    private void ResumeGame()
    {
        _isPause = false;
        Time.timeScale = 1f;

        ActivatePlayer();
        ToggleMouse(false);
        _levelRoot.HidePausePanel();
    }

}
