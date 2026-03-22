using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainTower : MonoBehaviour
{
    [Header("Indicators")]
    [SerializeField] private MainDoorIndicators _doorIndicators;
    [SerializeField] private CinemachineVirtualCamera _vCam;
    [Header("Rays")]
    [SerializeField] private List<TowerRay> _rays;
    [Header("Gate")]
    [SerializeField] private Animator _gate;
    [Header("UI")]
    [SerializeField] private BlackFadePanel _fadePanel;
    [SerializeField] private GameObject _playerUI;

    private LevelPuzzlesHandler _puzzleHandler;

    public void Initialize(LevelPuzzlesHandler puzzleHandler)
    {
        _puzzleHandler = puzzleHandler;

        InitializeCam();

        _doorIndicators.Initialize(this);
    }

    #region >>> CAMERA

    private void InitializeCam()
    {
        _vCam.Priority = GlobalVars.MainTowerIndicatorsCamPriority;
        DeactivateCam();
    }

    private void ActivateCam()
    {
        _vCam.gameObject.SetActive(true);
    }

    private void DeactivateCam()
    {
        _vCam.gameObject.SetActive(false);
    }
    #endregion

    public void OnPuzzleComplited(int index)
    {
        StartCoroutine(OnPuzzleComplitedRoutine(index));       
    }

    public void OpenGate()
    {
        StartCoroutine(OpenGateRoutine());
    }

    private void DeactivateRayByIndex(int index)
    {
        foreach (TowerRay ray in _rays)
        {
            if (ray.Index == index)
                ray.Deactivate();
        }
    }

    private IEnumerator OnPuzzleComplitedRoutine(int index)
    {
        _fadePanel.PlayOnAndOffAnimation();
        yield return new WaitForSecondsRealtime(1f);
        _playerUI.gameObject.SetActive(false);
        ActivateCam();
        yield return new WaitForSecondsRealtime(2f);
        _doorIndicators.UpdateLights();
        yield return new WaitForSecondsRealtime(2f);
        _fadePanel.PlayOnAndOffAnimation();
        yield return new WaitForSecondsRealtime(1f);
        _playerUI.gameObject.SetActive(true);
        DeactivateCam();
        DeactivateRayByIndex(index);
        yield return null;
    }

    private IEnumerator OpenGateRoutine()
    {
        yield return new WaitForSecondsRealtime(4f);
        _gate.SetTrigger("Open");
    }
}
