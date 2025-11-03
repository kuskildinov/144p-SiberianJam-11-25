using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : InteractableObject
{
    private const string OpenTrigger = "Open";

    [SerializeField] private Animator _animator;

    public override void TryInteract(Player player = null)
    {
        base.TryInteract(player);

        Open();
    }

    private void Open()
    {
        _animator.SetTrigger(OpenTrigger);
    }
}
