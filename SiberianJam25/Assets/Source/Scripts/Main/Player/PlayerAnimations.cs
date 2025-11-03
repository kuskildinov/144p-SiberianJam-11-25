using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    private const string WalkAnimation = "Walk";
    private const string GlassOnAnimiation = "GlassOn";
    private const string GlassOffAnimiation = "GlassOff";

    [SerializeField] private Animator _animator;

    private Player _player;

    public void initialize(Player player)
    {
        _player = player;
    }

    private void Update()
    {
        HandleMovmentAnimation();
    }

    private void HandleMovmentAnimation()
    {
        float horizontal = Input.GetAxis(HorizontalAxis);
        float vertical = Input.GetAxis(VerticalAxis);

        if (horizontal != 0 || vertical != 0)
            PlayWalkAnimation();
        else
            PlayIdleAnimation();
    }

     public void PlayWalkAnimation()
    {
        _animator.SetBool(WalkAnimation, true);
    }

    public void PlayIdleAnimation()
    {
        _animator.SetBool(WalkAnimation, false);
    }

    public void PlayGlassOnAnimation()
    {
        _animator.SetTrigger(GlassOnAnimiation);
    }

    public void PlayGlassOffAnimation()
    {
        _animator.SetTrigger(GlassOffAnimiation);
    }

    public void OnGlassFullOn()
    {
        _player.OnGlassOnFull();
    }

    public void OnGlassFullOff()
    {
        _player.OnGlassOffFull();
    }


}
