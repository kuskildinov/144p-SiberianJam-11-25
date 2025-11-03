using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : InteractableObject
{
    [SerializeField] private int _index;

    public int Index => _index;

    public override void TryInteract(Player player = null)
    {
        base.TryInteract();

        player.TakeItem(this);
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
    }
}
