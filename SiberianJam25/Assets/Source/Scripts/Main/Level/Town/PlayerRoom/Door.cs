using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : InteractableObject
{
    [SerializeField] private AudioSource _doorSource;
    [SerializeField] private AudioClip _friendKnokSound;
    [SerializeField] private AudioClip _policeKnokingSound;
    [Header("Police")]
    [SerializeField] private GameObject _policeTextPanel;

    private const string OpenDoorAnimatorParam = "Open";

    private PlayerRoom _playerRoom;
    private Animator _doorAnimator;

    public void Initialize(PlayerRoom playerRoom)
    {
        _playerRoom = playerRoom;
        _doorAnimator = GetComponent<Animator>();

        SetInteractable(false);
    }

    public override void TryInteract(Player player = null)
    {
        base.TryInteract();

        Open();
        _playerRoom.OnDoorOpend();
    }

    public void Open()
    {
        _doorAnimator.SetBool(OpenDoorAnimatorParam,true);
        StopFriendKnockingSound();
    }

    public void Close()
    {
        _doorAnimator.SetBool(OpenDoorAnimatorParam, false);
    }
   
    public void OnPlayerLeftRoom()
    {
        _doorSource.playOnAwake = false;
        StopPolicemanKnockingSound();
    }

    public void PlayFriendKnockingSound()
    {
        _doorSource.clip = _friendKnokSound;
        _doorSource.loop = true;
        _doorSource.Play();
    }

    public void PlayPolicmanKnockingSound()
    {
        _policeTextPanel.gameObject.SetActive(true);
        _doorSource.clip = _policeKnokingSound;
        _doorSource.playOnAwake = true;
        _doorSource.loop = true;
        _doorSource.Play();
    }

    private void StopFriendKnockingSound()
    {
        _doorSource.Stop();
    }

    private void StopPolicemanKnockingSound()
    {
        _policeTextPanel.gameObject.SetActive(false);
        _doorSource.Stop();
    }
}
