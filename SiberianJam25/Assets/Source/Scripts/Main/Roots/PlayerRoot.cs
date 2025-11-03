using System.Collections;
using UnityEngine;

public class PlayerRoot : CompositeRoot
{
    private const string ShowFadeTrigger = "Show";

    [SerializeField] private Player _player;
    [SerializeField] private LevelRoot _levelRoot;
    [Header("Restart Settings")]
    [SerializeField] private Transform _restartPoint;
    [Header("UI")]
    [SerializeField] private Animator _glassOnFadeAnimation;
    [SerializeField] private Animator _glassOffFadeAnimation;
    [SerializeField] private GameObject _cantTakeItemInfo;
    [SerializeField] private GameObject _interactKeyinfo;

    public Player Player => _player;

    public override void Compose()
    {
        _player.initialize(this);

        ActivatePlayer();
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
    }

    public void GameOver()
    {
        DeactivatePlayer();
        _levelRoot.OnGameOver();
    }

    public void RestartLevel()
    {
        _player.transform.position = _restartPoint.position;
        ActivatePlayer();
    }

    #region GLASSES
    public void OnGlassesOn()
    {
        _levelRoot.TryShowPinkWorld();
    }

    public void OnGlassesOff()
    {
        _levelRoot.TryShowBadWorld();
    }

    public void ShowGlassOnFade()
    {
        _glassOnFadeAnimation.SetTrigger(ShowFadeTrigger);
    }

    public void ShowGlassOffFade()
    {
        _glassOffFadeAnimation.SetTrigger(ShowFadeTrigger);
    }

    #endregion

    #region >>> UI

    public void ShowCantTakeItemMessage()
    {
        StartCoroutine(ShowMessageRoutine());
    }

    public void ShowInteractionInfo()
    {
        _interactKeyinfo.gameObject.SetActive(true);
    }

    public void HideInteractionInfo()
    {
        _interactKeyinfo.gameObject.SetActive(false);
    }

    private IEnumerator ShowMessageRoutine()
    {
        _cantTakeItemInfo.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        _cantTakeItemInfo.gameObject.SetActive(false);
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
   
}
