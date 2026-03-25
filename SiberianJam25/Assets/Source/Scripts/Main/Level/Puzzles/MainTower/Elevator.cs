using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    private const string OpenDoorsAnimatorParam = "Open";

    [SerializeField] private InteractableObject _movePanel;
    [SerializeField] private Animator _animator;
    [Header("Open Doors")]
    [SerializeField] private InteractableObject _openDoorsPanel;
    [SerializeField] private float _openDoorsDeley = 3f;
    [SerializeField] private AudioSource _doorsAudioSource;
    [SerializeField] private AudioClip _openCloseClip;    

    private MainTower _tower;

    public void Initialize(MainTower tower)
    {
        _tower = tower;
        if(_movePanel != null)
            _movePanel.SetInteractable(false);
        SubscribeToEvents();
    }

    #region >>> DOORS

    public void OpenDoors()
    {
        _animator.SetBool(OpenDoorsAnimatorParam,true);
    }

    private void CloseDoors()
    {
        _animator.SetBool(OpenDoorsAnimatorParam, false);
    }

    private void PlayOpenCloseDoorsSound()
    {
        _doorsAudioSource.PlayOneShot(_openCloseClip);
    }

    private IEnumerator OpenDoorsRoutine()
    {
        PlayOpenCloseDoorsSound();
        yield return new WaitForSecondsRealtime(_openDoorsDeley);
        OpenDoors();
        yield return new WaitForSecondsRealtime(_openDoorsDeley);
        _movePanel.SetInteractable(true);
    }

    private IEnumerator CloseDoorsRoutine()
    {
        PlayOpenCloseDoorsSound();
        yield return new WaitForSecondsRealtime(_openDoorsDeley);
        CloseDoors();

        _tower.StartElevatorMovment();
    }

    #endregion

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        if(_openDoorsPanel != null)
            _openDoorsPanel.Interacted += OnOpenDoorsButtonClicked;
        if (_movePanel != null) 
            _movePanel.Interacted += OnMoveButtonClicked;
    }

    private void UnsubscribeToEvents()
    {
        if (_openDoorsPanel != null) 
            _openDoorsPanel.Interacted -= OnOpenDoorsButtonClicked;
        if (_movePanel != null) 
            _movePanel.Interacted -= OnMoveButtonClicked;
    }

    private void OnOpenDoorsButtonClicked(Player player)
    {
        StartCoroutine(OpenDoorsRoutine());
    }

    private void OnMoveButtonClicked(Player player)
    {
        StartCoroutine(CloseDoorsRoutine());
    }


    #endregion   

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
