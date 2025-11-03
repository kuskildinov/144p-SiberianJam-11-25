using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerRoom : MonoBehaviour
{
    [SerializeField] private PlayableDirector _openDooePlayable;
    [SerializeField] private PhrasePanel _friendPhrasePanel;
    [SerializeField] private Animator _doorAnimator;
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

    public void Initiaalize()
    {
        _door.CanInteract = false;

        StartCoroutine(StartCutSceneRoutine());
    }

    #region >>> DOOR

    public void TryOpenDoor()
    {       
        _openDooePlayable.Play();
        _doorAnimator.SetTrigger("Activate");
        _door.CanInteract = false;
    }

    private void PlayFriendKnockingSound()
    {

    }

    private void StopFriendKnockingSound()
    {

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

    #endregion
}
