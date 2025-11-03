using UnityEngine;

public class KeyLock : InteractableObject
{
    [SerializeField] private int _index;

    private FindObjectPuzzle _puzzle;
    private bool _isEmpty = true;

    public bool IsEmpty => _isEmpty;

    public void Initialize(FindObjectPuzzle puzzle)
    {
        _puzzle = puzzle;
    }

    public override void TryInteract(Player player = null)
    {
        base.TryInteract(player);

        Item item = player.CurrentItemOnHand;

        if (item == null || item.Index != _index || _isEmpty == false)
            return;

        _isEmpty = false;
        CanInteract = false;

        player.DropItem();
        item.SetParent(transform);
        item.CanInteract = false;

        _puzzle.OnKeyInserted();
    }
}
