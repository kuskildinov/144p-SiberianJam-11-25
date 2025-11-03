using System.Collections;
using UnityEngine;

public class PlayerRoot : CompositeRoot
{
    [SerializeField] private Player _player;
    [SerializeField] private LevelRoot _levelRoot;
    [Header("UI")]
    [SerializeField] private Animator _glassOnFadeAnimation;
    //[SerializeField] private Animator _glassOffFadeAnimation;
   

    public override void Compose()
    {
        _player.initialize(this);

        ActivatePlayer();
    }

    public void ActivatePlayer()
    {
        _player.Activate();

        ToggleMouse(false);
    }

    public void DeactivatePlayer()
    {
        _player.Deactivate();

        ToggleMouse(true);
    }

    public void OnGlassesOn()
    {
        _levelRoot.TryShowBadWorld();
    }

    public void OnGlassesOff()
    {
        _levelRoot.TryShowPinkWorld();
    }

    public void ShowGlassOnFade()
    {
        _glassOnFadeAnimation.SetTrigger("Show");
    }

    public void ShowGlassOffFade()
    {

    }

    

    private void ToggleMouse(bool value)
    {
        if(value)
            Cursor.lockState = CursorLockMode.None;      
        else
            Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = value;
    }
   
}
