using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuRoot : CompositeRoot
{
    [SerializeField] private MainMenuPanel _mainMenuPanel;
    public override void Compose()
    {
        _mainMenuPanel.Initialize(this);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(GlobalVars.GameLevelSceneName);
    }
}
