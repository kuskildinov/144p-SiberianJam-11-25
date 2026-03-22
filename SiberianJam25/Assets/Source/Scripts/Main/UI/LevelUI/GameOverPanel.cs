using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [Header("Buttons")]
    [SerializeField] private Button _restartGameButton;
    [SerializeField] private Button _backToMainMenuButton;

    private LevelUI _levelUI;

    public void Initialize(LevelUI levelUI)
    {
        _levelUI = levelUI;
    }

    public void Show()
    {
        _panel.gameObject.SetActive(true);

        SubscribeToButtonsEvents();
    }

    public void Hide()
    {
        _panel.gameObject.SetActive(false);

        UnsubscribeToButtonsEvents();
    }

    #region >>> EVENTS

    private void SubscribeToButtonsEvents()
    {
        _restartGameButton.onClick.AddListener(OnRestartGameButtonClicked);
        _backToMainMenuButton.onClick.AddListener(OnBackToMainMenuButtonClicked);
    }

    private void UnsubscribeToButtonsEvents()
    {
        _restartGameButton.onClick.RemoveAllListeners();
        _backToMainMenuButton.onClick.RemoveAllListeners();
    }

    private void OnRestartGameButtonClicked()
    {
        _levelUI.OnRestartGameButtonClicked();
    }

    private void OnBackToMainMenuButtonClicked()
    {
        _levelUI.OnBackToMainMenuButtonClicked();
    }

    #endregion
}
