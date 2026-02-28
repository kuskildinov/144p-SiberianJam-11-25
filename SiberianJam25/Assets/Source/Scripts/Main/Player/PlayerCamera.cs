using System.Collections;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _playerVCam;
    [SerializeField] private float _defaultFOV = 60f;
    [SerializeField] private float _maxFOV = 90f;
    [SerializeField] private float _minFOV = 40f;
    [SerializeField] private float _fovChangeSpeed = 2f;
    [SerializeField] private AnimationCurve fovCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
       
    private float _targetFOV;
    private Coroutine _fovCoroutine;

    public void Initialize()
    {       
        _playerVCam.m_Lens.FieldOfView = _defaultFOV;
    }
   
    public void SetMinFOV()
    {
        SetFOV(_minFOV);
    }

    public void SetDefaultFOV()
    {
        SetFOV(_defaultFOV);
    }

    private void SetFOV(float newFOV, float duration = 1f)
    {
        _targetFOV = Mathf.Clamp(newFOV, _minFOV, _maxFOV);

        if (duration > 0 && gameObject.activeInHierarchy)
        {
            if (_fovCoroutine != null)
                StopCoroutine(_fovCoroutine);
            _fovCoroutine = StartCoroutine(ChangeFOVCoroutine(_targetFOV, duration));
        }
    }

    private IEnumerator ChangeFOVCoroutine(float targetFOVValue, float duration)
    {
        float startFOV = _playerVCam.m_Lens.FieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float curveValue = fovCurve.Evaluate(t);
            _playerVCam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOVValue, curveValue);

            yield return null;
        }

        _playerVCam.m_Lens.FieldOfView = targetFOVValue;
        _targetFOV = targetFOVValue;
        _fovCoroutine = null;
    }
}
