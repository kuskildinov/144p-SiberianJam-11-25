using UnityEngine;

public class EnviernmentSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _pinkWorldEnvienment;
    [SerializeField] private GameObject _badWorldEnviernment;

    public void ShowPinkWorld()
    {
        _pinkWorldEnvienment.gameObject.SetActive(true);
        _badWorldEnviernment.gameObject.SetActive(false);
    }

    public void ShowBadWorld()
    {
        _pinkWorldEnvienment.gameObject.SetActive(false);
        _badWorldEnviernment.gameObject.SetActive(true);
    }
}
