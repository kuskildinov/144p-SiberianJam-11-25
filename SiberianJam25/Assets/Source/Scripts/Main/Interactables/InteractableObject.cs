using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private bool _canInteract = true;

    public bool CanInteract { get => _canInteract; set => _canInteract = value; }

    public event Action<Player> Interacted;

    public virtual void TryInteract(Player player = null)
    {        
        Interacted?.Invoke(player);
    }
}
