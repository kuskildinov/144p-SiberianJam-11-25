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
    [SerializeField] private PlayerCamera _playerCamera;   
    [Header("Secure Detection Settings")]
    [SerializeField] private float _timeBeforeGameOver = 2.5f;
    [SerializeField] private AudioSource _camZoneSound;
    [Header("TakeItemSettings")]
    [SerializeField] private Transform _takeItemContainer;

    private PlayerRoot _root;
    private Camera _camera;
    private bool _isActive;
  
    [SerializeField] private bool _canSwitchGlass;    
    private bool _isDetectedBySecure = false;   
    private float _onDetectionTimer;
    private Item _currentItemOnHand;

    #region Properties
    public bool IsActive => _isActive;
    public bool IsDetected => _isDetectedBySecure;
    public bool CanSwitchGlasses { get => _canSwitchGlass; set => _canSwitchGlass = value; }
    public Item CurrentItemOnHand => _currentItemOnHand;
   
    public Camera Camera => _camera;
    #endregion
  
    public void Initialize(PlayerRoot root)
    {
        _root = root;
        _camera = Camera.main;

        _movment?.initialize(this);
        _animations?.initialize(this);
        _interactions?.initialize(this);
        _glassSwitcher?.Initialize(this);
        _playerCamera?.Initialize(this);
       
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
        _playerCamera.SetMinFOV();
        _isDetectedBySecure = true;

        _camZoneSound.Play();
    }

    public void LostDetectionBySecure()
    {
        Debug.Log("Игрок потерян");
        _movment.OnReturnControl();
        _isDetectedBySecure = false;
        _playerCamera.SetDefaultFOV();
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
        _glassSwitcher.TrySwitchGlasses();
    }

    #endregion
    #region >>> CAMERA SETTINGS

    private void HandleCameraView() => _playerCamera.HandleCameraView();
   
    #endregion

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<PlayerRoom> (out PlayerRoom playerRoom))
        {
            playerRoom.OnPlayerLeft();
        }
    }
}
