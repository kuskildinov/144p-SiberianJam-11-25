using UnityEngine;

public class WorldStateSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _pinkWorld;
    [SerializeField] private GameObject _badWorld;

    public void ShowPinkWorld()
    {
        _pinkWorld.gameObject.SetActive(true);
        _badWorld.gameObject.SetActive(false);
    }

    public void ShowBadWorld()
    {
        _pinkWorld?.gameObject.SetActive(false);
        _badWorld?.gameObject.SetActive(true);
    }
}
