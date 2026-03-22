using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private BlackFadePanel _blackFade;
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private GameOverPanel _gameOverPanel;

    private LevelRoot _root;

    public void Initialize(LevelRoot root)
    {
        _root = root;

        _gameOverPanel.Initialize(this);
    }

    #region >>> BLACK FADE

    public void ShowBlackFadeOff()
    {
        _blackFade.PlayFadeOffAnimation();
    }

    #endregion
    #region >>> PAUSE PANEL

    public void ShowPausePanel()
    {
        _pausePanel.Show();
    }

    public void HidePausePanel()
    {
        _pausePanel.Hide();
    }

    #endregion
    #region >>> GAME OVER PANEL

    public void ShowGameOverPanel()
    {
        _gameOverPanel.Show();
    }

    public void HideGameOverPanel()
    {
        _gameOverPanel.Hide();
    }

    public void OnRestartGameButtonClicked()
    {
        _root.RestartLevel();
    }

    public void OnBackToMainMenuButtonClicked()
    {
        _root.BackToMainMenu();
    }

    #endregion
}
