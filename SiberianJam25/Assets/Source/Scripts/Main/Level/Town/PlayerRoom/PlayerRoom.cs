using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerRoom : MonoBehaviour
{
    [SerializeField] private PlayableDirector _openDoorPlayable;
    [Header("Visual")]
    [Header("Interier")]
    [SerializeField] private Renderer _interierRenderer;
    [SerializeField] private Material _interierPinkMaterial;
    [SerializeField] private Material _interierBadMaterial;
    [Header("Exterier")]
    [SerializeField] private Renderer _exterierRenderer;
    [SerializeField] private Material _exterierPinkMaterial;
    [SerializeField] private Material _exterierBadMaterial;
    [Header("Enviernments")]
    [SerializeField] private List<GameObject> _pinkEnviernments;
    [SerializeField] private List<GameObject> _badEnviernments;
    [Space]
    [Header("Friend")]
    [SerializeField] private FriendOnRoom _friend;
    [Space]
    [Header("Window Settings")]
    [SerializeField] private Window _window;   
    [Header("Door settings")]
    [SerializeField] private Door _door;    
    [SerializeField] private float _timeBeforeKniking = 20f;
    [Space]
    [Header("Outside Scene")]
    [SerializeField] private GameObject _outsideScene;

    private LevelRoot _root;
    private bool _needCheckGlassWearing;

    public void Initialize(LevelRoot root)
    {
        _root = root;
        _door.Initialize(this);

        SubscribeToEvents();

        StartCoroutine(StartCutSceneRoutine());
    }

    private void Update()
    {
        if(_needCheckGlassWearing && Input.GetKeyDown(KeyCode.Q))
        {
            _outsideScene.gameObject.SetActive(false);
            _needCheckGlassWearing = false;
            _window.SetInteractable(true);

            PlayPolicemansPhrases();
        }
    }

    #region >>> VISUAL

    private void OnWorldStateChanged(WorldState newState)
    {
        if (newState == WorldState.PINK)
        {
            _interierRenderer.material = _interierPinkMaterial;
            _exterierRenderer.material = _exterierPinkMaterial;

            ShowPinkEnviernment();
        }
        else if (newState == WorldState.BAD)
        {
            _interierRenderer.material = _interierBadMaterial;
            _exterierRenderer.material = _exterierBadMaterial;

            ShowBadEnviernment();
        }
    }

    private void ShowPinkEnviernment()
    {
        foreach (GameObject obj in _pinkEnviernments)
        {
            obj.gameObject.SetActive(true);
        }

        foreach (GameObject obj in _badEnviernments)
        {
            obj.gameObject.SetActive(false);
        }

    }

    private void ShowBadEnviernment()
    {
        foreach (GameObject obj in _pinkEnviernments)
        {
            obj.gameObject.SetActive(false);
        }

        foreach (GameObject obj in _badEnviernments)
        {
            obj.gameObject.SetActive(true);
        }
    }


    #endregion
    #region >>> DOOR

    public void OnDoorOpend()
    {       
        _openDoorPlayable.Play();
    }    

    private IEnumerator StartCutSceneRoutine()
    {
        yield return new WaitForSecondsRealtime(_timeBeforeKniking);
        _door.SetInteractable(true);
        _door.PlayFriendKnockingSound();
    }

    #endregion
    #region >>> PLAYER

    public void OnPlayerLeft()
    {
        _door.OnPlayerLeftRoom();

        //_policemanPhrasePanel.Hide();

        GlobalVars.PlayerLeftRoom = true;
    }

    #endregion
    #region >>> FRIEND

    public void OnFriendLeftRoom()
    {
        _door.Close();
    }

    #endregion
    #region >>> PHRASES

    public void PlayFirstPhrase()
    {
        DialogComponent dialogOmponent = _friend.Phrases;
        _root.TryShowDialog(dialogOmponent, 0);
    }

    public void PlaySecondPhrase()
    {
        DialogComponent dialogOmponent = _friend.Phrases;
        _root.TryShowDialog(dialogOmponent, 1);
    }

    public void PlayThirdPhrase()
    {
        DialogComponent dialogOmponent = _friend.Phrases;
        _root.TryShowDialog(dialogOmponent, 2);
    }

    public void PlayFouthPhrase()
    {
        DialogComponent dialogOmponent = _friend.Phrases;
        _root.TryShowDialog(dialogOmponent, 3);
    }

    public void PlayPolicemansPhrases()
    {              
        _door.PlayPolicmanKnockingSound();
    }  

    #endregion
    #region >>> UI

    public void ShowSwitchOffGlassesInfo()
    {
        _outsideScene.gameObject.SetActive(true);
        StartCoroutine(ShowSwitchOffGlassesRoutine());
    }

    private IEnumerator ShowSwitchOffGlassesRoutine()
    {
        yield return new WaitForSecondsRealtime(5f);
        _root.ShowSwitchOffInfoPanel();
        _needCheckGlassWearing = true;
      
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _root.WorldStateChanged += OnWorldStateChanged;
    }

    private void UnsubscribeToEvents()
    {
        _root.WorldStateChanged -= OnWorldStateChanged;
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
