using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;


    public void Show()
    {
        _panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _panel.gameObject.SetActive(false);
    }
}
