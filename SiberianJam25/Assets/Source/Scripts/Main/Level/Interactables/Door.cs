using UnityEngine;

public class Door : InteractableObject
{
    [SerializeField] private PlayerRoom _playerRoom;

    public override void TryInteract(Player player = null)
    {
        base.TryInteract();

        _playerRoom.TryOpenDoor();
    }
}
