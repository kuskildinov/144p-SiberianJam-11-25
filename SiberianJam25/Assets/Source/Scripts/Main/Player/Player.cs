using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMovment _movment;
    [SerializeField] private PlayerAnimations _animations;
    [SerializeField] private PlayerInteractions _interactions;
    [Header("Camera Settings")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float defaultFOV = 60f;
    [SerializeField] private float maxFOV = 90f;
    [SerializeField] private float minFOV = 40f;
    [SerializeField] private float fovChangeSpeed = 2f;
    [SerializeField] private AnimationCurve fovCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private PlayerRoot _root;
    private bool _isActive;
    private float targetFOV;
    private float currentFOV;
    private Coroutine fovCoroutine;

    public bool IsActive => _isActive;

    public void initialize(PlayerRoot root)
    {
        _root = root;
        _movment?.initialize(this);
        _animations?.initialize(this);
        _interactions?.initialize(this);

        currentFOV = _camera.fieldOfView;
        targetFOV = currentFOV;

        _isActive = true;
    }

    private void Update()
    {
        if (_isActive == false)
            return;

        HandleCameraView();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            TryGlassOn();
        }
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    public void DetectedBySecure(Transform secureCam)
    {
        Debug.Log("Игрок замечен");
        _movment.OnLostControl(secureCam);
        SetFOV(minFOV);
    }

    public void LostDetectionBySecure()
    {
        Debug.Log("Игрок потерян");
        _movment.OnReturnControl();
        SetFOV(defaultFOV);
    }

    #region >>> GLASSES
    private void TryGlassOn()
    {
        _animations.PlayGlassOnAnimation();
        _root.ShowGlassOnFade();       
    }

    private void TryGlassOff()
    {
        _animations.PlayGlassOffAnimation();
        _root.ShowGlassOffFade();
    }

    public void OnGlassOnFull()
    {
        _root.OnGlassesOn();
    }

    public void OnGlassOffFull()
    {
        _root.OnGlassesOff();
    }

    #endregion

    #region Camera Settings

    private void HandleCameraView()
    {
        if (Mathf.Abs(_camera.fieldOfView - targetFOV) > 0.1f)
        {
            _camera.fieldOfView = Mathf.Lerp(
                _camera.fieldOfView,
                targetFOV,
                fovChangeSpeed * Time.deltaTime
            );
        }
    }

    public void SetFOV(float newFOV, float duration = -1f)
    {
        targetFOV = Mathf.Clamp(newFOV, minFOV, maxFOV);

        if (duration > 0 && gameObject.activeInHierarchy)
        {
            if (fovCoroutine != null)
                StopCoroutine(fovCoroutine);
            fovCoroutine = StartCoroutine(ChangeFOVCoroutine(targetFOV, duration));
        }
    }

    private IEnumerator ChangeFOVCoroutine(float targetFOVValue, float duration)
    {
        float startFOV = _camera.fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float curveValue = fovCurve.Evaluate(t);

            _camera.fieldOfView = Mathf.Lerp(startFOV, targetFOVValue, curveValue);
            currentFOV = _camera.fieldOfView;

            yield return null;
        }

        _camera.fieldOfView = targetFOVValue;
        currentFOV = targetFOVValue;
        targetFOV = targetFOVValue;
        fovCoroutine = null;
    }
    #endregion
}
