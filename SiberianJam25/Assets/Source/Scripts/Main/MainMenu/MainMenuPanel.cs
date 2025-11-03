using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [Header("Buttons")]
    [SerializeField] private Button _startGameButton;
    private MainMenuRoot _root;

    public void Initialize(MainMenuRoot root)
    {
        _root = root;

        Open();
    }

    public void Open()
    {
        _panel.gameObject.SetActive(true);

        SubscribeToButtonsEvent();
    }

    public void Close()
    {
        _panel.gameObject.SetActive(false);

        UnSubscribeToButtonsEvents();
    }

    private void OnStartGameButtonClicked()
    {
        _root.LoadGameScene();
    }

    private void SubscribeToButtonsEvent()
    {
        _startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    private void UnSubscribeToButtonsEvents()
    {
        _startGameButton.onClick.RemoveAllListeners();
    }
}
