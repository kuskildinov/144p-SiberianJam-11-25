using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool _canInteract = true;

    public bool CanInteract { get => _canInteract; set => _canInteract = value; } 

    public virtual void TryInteract(Player player = null)
    {
      
    }
}
