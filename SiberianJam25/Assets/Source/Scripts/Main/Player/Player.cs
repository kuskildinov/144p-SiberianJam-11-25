using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const string DiaryShowAnimParam = "Show";

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
    [Header("Deary Book Settings")]
    [SerializeField] private DiaryBook _diaryBook;
    [SerializeField] private Animator _diaryAnimator;

    private PlayerRoot _root;
    private Camera _camera;
    private bool _isActive;
  
    [SerializeField] private bool _canSwitchGlass;    
    private bool _isDetectedBySecure = false;
    [SerializeField] private bool _canOpenDiary = true;
    private bool _diaryOpen = false;
    private float _onDetectionTimer;
    private Item _currentItemOnHand;

    #region Properties
    public bool IsActive => _isActive;
    public bool IsDetected => _isDetectedBySecure;
    public bool CanSwitchGlasses { get => _canSwitchGlass; set => _canSwitchGlass = value; }
    public Item CurrentItemOnHand => _currentItemOnHand;
   
    public Camera Camera => _camera;
    public CharacterController CharacterController => _movment.CharacterController;
    #endregion
  
    public void Initialize(PlayerRoot root)
    {
        _root = root;
        _camera = Camera.main;

        _movment?.Initialize(this);
        _animations?.initialize(this);
        _interactions?.Initialize(this);
        _glassSwitcher?.Initialize(this);
        _playerCamera?.Initialize();
        _diaryBook?.Initialize();
        _isActive = true;
    }

    private void Update()
    {
        ShowDiaryHandler();

        if (!_isActive || _root.IsPause)
            return;                
      
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

    public void TryShowInteractionInfo(string infoText)
    {
        _root.TryShowInteractionInfo(infoText);
    }

    #endregion
    #region >>> ITEMS INTERACTION
    public void TakeItem(Item item)
    {
        if(_currentItemOnHand != null)
        {
            _root.ShowCantInteractText();
            return;
        }

        _currentItemOnHand = item;
        _currentItemOnHand.Rigidbody.isKinematic = true;
        _currentItemOnHand.SetParent(_takeItemContainer);
    }

    public void DropItem()
    {
        _currentItemOnHand.Rigidbody.isKinematic = false;
        _currentItemOnHand = null;
    }

    
    #endregion
    #region >>> SECURE DETECTION

    public void DetectedBySecure(Transform secureCam)
    {      
        _movment.OnLostControl(secureCam);
        _playerCamera.SetMinFOV();
        _isDetectedBySecure = true;

        _camZoneSound.Play();
    }

    public void LostDetectionBySecure()
    {      
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
    #region >>> DIARY

    public void OnDiaryPageTaked(SheetData newData)
    {
        _diaryBook.AddSheet(newData);
    }

    private void ShowDiaryHandler()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && _canOpenDiary)
        {
            if (_diaryOpen)
                HideDiary();
            else
                ShowDiary();
        }
    }

    private void ShowDiary()
    {
        _diaryOpen = true;
        _diaryAnimator.SetBool(DiaryShowAnimParam, true);
        _diaryBook.PlayOpenCloseSound();
        _isActive = false;
    }

    private void HideDiary()
    {
       
        StartCoroutine(HideDiaryRoutine());
    }

    private IEnumerator HideDiaryRoutine()
    {
        _diaryOpen = false;
        _diaryAnimator.SetBool(DiaryShowAnimParam, false);
        _diaryBook.PlayOpenCloseSound();
        _isActive = true;

        yield return new WaitForSecondsRealtime(0.7f);

        _diaryBook.ResetToFirstPage();
    }
    #endregion
    #region >>> DIALOG SYSTEM
    public void TryActivateDialog(DialogComponent dialogComponent)
    {
        _root.SetDialog(dialogComponent);
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
