using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMovment _movment;
    [SerializeField] private PlayerAnimations _animations;
    [SerializeField] private PlayerInteractions _interactions;
    [SerializeField] private PlayerGlassSwitcher _glassSwitcher;
    [Header("Camera Settings")]
    [SerializeField] private Camera _camera;
    [SerializeField] private float _defaultFOV = 60f;
    [SerializeField] private float _maxFOV = 90f;
    [SerializeField] private float _minFOV = 40f;
    [SerializeField] private float _fovChangeSpeed = 2f;
    [SerializeField] private AnimationCurve fovCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [Header("Secure Detection Settings")]
    [SerializeField] private float _timeBeforeGameOver = 2.5f;
    [SerializeField] private AudioSource _camZoneSound;
    [Header("TakeItemSettings")]
    [SerializeField] private Transform _takeItemContainer;

    private PlayerRoot _root;
    private bool _isActive;
    private PlayerState _currentPlayerState = PlayerState.DEFAULT;

    [SerializeField] private bool _canSwitchGlass;    
    private bool _isDetectedBySecure = false;
    private float _targetFOV;
    private float _currentFOV;
    private float _onDetectionTimer;
    private Coroutine _fovCoroutine;
    private Item _currentItemOnHand;

    #region Properties
    public bool IsActive => _isActive;
    public PlayerState CurrentPlayerState => _currentPlayerState;
    public bool CanSwitchGlasses { get => _canSwitchGlass; set => _canSwitchGlass = value; }
    public Item CurrentItemOnHand => _currentItemOnHand;
   
    public Camera Camera => _camera;
    #endregion

    public event Action PlayerStateChanged;

    public void Initialize(PlayerRoot root)
    {
        _root = root;
        _movment?.initialize(this);
        _animations?.initialize(this);
        _interactions?.initialize(this);
        _glassSwitcher?.Initialize(this);

        _currentFOV = _camera.fieldOfView;
        _targetFOV = _currentFOV;

        _isActive = true;
    }

    private void Update()
    {
        if (!_isActive)
            return;

        HandleCameraView();
        SwitchGlassesHandler();
        SecureDetectionHandler();
    }

    #region >>> ACTIVATE DEACTIVATE
    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }
    #endregion
    #region >>> STATE

    private void SetNewState(PlayerState newState)
    {
        _currentPlayerState = newState;
        PlayerStateChanged?.Invoke();
    }
    #endregion
    #region >>> INTERACTION INFO 

    public void ShowInteractionInfo() => _root.ShowInteractionInfo();

    public void HideInteractionInfo() => _root.HideInteractionInfo();
    #endregion
    #region >>> ITEMS INTERACTION
    public void TakeItem(Item item)
    {
        if(_currentItemOnHand != null)
        {
            _root.ShowCantTakeItemMessage();
            return;
        }

        _currentItemOnHand = item;

        item.SetParent(_takeItemContainer);
    }

    public void DropItem()
    {
        _currentItemOnHand = null;

    }
    #endregion
    #region >>> SECURE DETECTION

    public void DetectedBySecure(Transform secureCam)
    {
        Debug.Log("Игрок замечен");
        _movment.OnLostControl(secureCam);
        SetFOV(_minFOV);
        _isDetectedBySecure = true;

        _camZoneSound.Play();
    }

    public void LostDetectionBySecure()
    {
        Debug.Log("Игрок потерян");
        _movment.OnReturnControl();
        SetFOV(_defaultFOV);
        _isDetectedBySecure = false;
        _onDetectionTimer = 0f;

        _camZoneSound.Stop();
    }

    private void StartUnderSecureTimer()
    {
        _onDetectionTimer += Time.deltaTime;

        if(_onDetectionTimer >= _timeBeforeGameOver)
        {
            _root.GameOver();
            _isDetectedBySecure = false;
            _onDetectionTimer = 0f;
        }
    }

    public bool CheckCanBeDetected()
    {
        if (_root._levelRoot.CurrensState == WorldState.PINK)
            return false;
        else
            return true;
    }

    private void SecureDetectionHandler() 
    {
        if (_isDetectedBySecure)
        {
            StartUnderSecureTimer();
        }
    }

    #endregion
    #region >>> GLASSES

    public void OnGlassSwitchEnded()
    {
        _canSwitchGlass = true;
        SetNewState(PlayerState.DEFAULT);
        _glassSwitcher.OnEndSwitchGlasses();
    }

    public void PlaySwitchGlassesAnimation()
    {
        _animations.PlayGlassSwitchAnimation();
    }

    public void OnGlassesOn() => _root.OnGlassesOn();

    public void OnGlassesOff() => _root.OnGlassesOff();

    private void SwitchGlassesHandler()
    {        
        if (Input.GetKeyDown(KeyCode.Q) && _canSwitchGlass)
        {
            TrySwitchGlasses();
        }
    }

    private void TrySwitchGlasses()
    {
        _canSwitchGlass = false;      
        SetNewState(PlayerState.GLASS_PUTTING);
        _glassSwitcher.TrySwitchGlasses();
    }   
  
    #endregion
    #region >>> CAMERA SETTINGS

    private void HandleCameraView()
    {
        if (Mathf.Abs(_camera.fieldOfView - _targetFOV) > 0.1f)
        {
            _camera.fieldOfView = Mathf.Lerp(
                _camera.fieldOfView,
                _targetFOV,
                _fovChangeSpeed * Time.deltaTime
            );
        }
    }

    public void SetFOV(float newFOV, float duration = -1f)
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
        float startFOV = _camera.fieldOfView;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            float curveValue = fovCurve.Evaluate(t);

            _camera.fieldOfView = Mathf.Lerp(startFOV, targetFOVValue, curveValue);
            _currentFOV = _camera.fieldOfView;

            yield return null;
        }

        _camera.fieldOfView = targetFOVValue;
        _currentFOV = targetFOVValue;
        _targetFOV = targetFOVValue;
        _fovCoroutine = null;
    }
    #endregion

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<PlayerRoom> (out PlayerRoom playerRoom))
        {
            playerRoom.OnPlayerLeft();
        }
    }
}

public enum PlayerState
{
    DEFAULT,
    GLASS_PUTTING
}
