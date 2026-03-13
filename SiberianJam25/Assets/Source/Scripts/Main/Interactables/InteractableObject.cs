using System;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] protected bool _canInteract = true;
    [SerializeField] private bool _interactOnce = true;
    [SerializeField] private string _interactionInfo;

    public string InteractionInfo => _interactionInfo;
    
    public event Action<Player> Interacted;

    public virtual void TryInteract(Player player = null)
    {
        if (_interactOnce)
            SetInteractable(false);

         Interacted?.Invoke(player);
    }

    public virtual bool CheckCanInteract(Player player)
    {
        return _canInteract;
    }

    public void SetInteractable(bool value)
    {
        _canInteract = value;
    }
}
