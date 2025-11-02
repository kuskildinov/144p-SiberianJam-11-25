using UnityEngine;

public class PlayerRoot : CompositeRoot
{
    [SerializeField] private Player _player;

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

    private void ToggleMouse(bool value)
    {
        if(value)
            Cursor.lockState = CursorLockMode.None;      
        else
            Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = value;
    }
}
