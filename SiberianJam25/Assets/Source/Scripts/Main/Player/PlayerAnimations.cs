using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    private const string WalkAnimationParam = "Walk";    
    private const string GlassSwitchAnimiationParam = "SwitchGlasses";
    
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

     public void PlayWalkAnimation() => _animator.SetBool(WalkAnimationParam, true);

    public void PlayIdleAnimation() => _animator.SetBool(WalkAnimationParam, false);

    public void PlayGlassSwitchAnimation() => _animator.SetTrigger(GlassSwitchAnimiationParam);

    public  void OnEndSwitchGlass() => _player.OnGlassSwitchEnded();
}
