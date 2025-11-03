using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerRoom : MonoBehaviour
{
    [SerializeField] private PlayableDirector _openDooePlayable;
    [SerializeField] private PhrasePanel _friendPhrasePanel;
    [SerializeField] private PhrasePanel _policemanPhrasePanel;
    [SerializeField] private Animator _doorAnimator;
    [Header("Window Settings")]
    [SerializeField] private Window _window;   
    [Header("Door settings")]
    [SerializeField] private Door _door;
    [SerializeField] private float _timeBeforeKniking = 20f;
    [SerializeField] private AudioSource _doorSource;
    [SerializeField] private AudioClip _friendKnokSound;
    [SerializeField] private AudioClip _policeKnokingSound;
    [Header("Friend Phrases")]
    [SerializeField] private string _phrase_1;
    [SerializeField] private string _phrase_2;
    [SerializeField] private string _phrase_3;
    [SerializeField] private string _phrase_4;
    [Header("Outside Scene")]
    [SerializeField] private GameObject _outsideScene;

    private LevelRoot _root;
    private bool _needCheckGlassWearing;

    public void Initialize(LevelRoot root)
    {
        _root = root;

        _door.CanInteract = false;
        _window.CanInteract = false;
        StartCoroutine(StartCutSceneRoutine());
    }

    private void Update()
    {
        if(_needCheckGlassWearing && Input.GetKeyDown(KeyCode.Q))
        {
            _outsideScene.gameObject.SetActive(false);
            _needCheckGlassWearing = false;
            _window.CanInteract = true;

            PlayPolicemansPhrases();
        }
    }

    public void OnPlayerLeft()
    {
        StopPolicemanKnockingSound();
        _policemanPhrasePanel.Hide();
    }

    #region >>> DOOR

    public void TryOpenDoor()
    {       
        _openDooePlayable.Play();
        _doorAnimator.SetTrigger("Activate");
        _door.CanInteract = false;
        StopFriendKnockingSound();

    }

    private void PlayFriendKnockingSound()
    {
        _doorSource.clip = _friendKnokSound;
        _doorSource.loop = true;
        _doorSource.Play();
    }

    private void StopFriendKnockingSound()
    {
        _doorSource.Stop();
    }

    private void PlayPolicmanKnockingSound()
    {
        _doorSource.clip = _policeKnokingSound;
        _doorSource.playOnAwake = true;
        _doorSource.loop = true;
        _doorSource.Play();
    }

    private void StopPolicemanKnockingSound()
    {
        _doorSource.Stop();
    }

    private IEnumerator StartCutSceneRoutine()
    {
        yield return new WaitForSecondsRealtime(_timeBeforeKniking);
        _door.CanInteract = true;
        PlayFriendKnockingSound();
    }

    #endregion

    #region >>> PHRASES

    public void PlayFirstPhrase()
    {      
        _friendPhrasePanel.ShowPhrase(_phrase_1);
    }

    public void PlaySecondPhrase()
    {
        _friendPhrasePanel.ShowPhrase(_phrase_2);
    }

    public void PlayThirdPhrase()
    {
        _friendPhrasePanel.ShowPhrase(_phrase_3);
    }

    public void PlayFouthPhrase()
    {
        _friendPhrasePanel.ShowPhrase(_phrase_4);
    }

    public void PlayPolicemansPhrases()
    {       
        _policemanPhrasePanel.ShowPhrase("Полиция! Откройте дверь!");
        PlayPolicmanKnockingSound();
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
}
