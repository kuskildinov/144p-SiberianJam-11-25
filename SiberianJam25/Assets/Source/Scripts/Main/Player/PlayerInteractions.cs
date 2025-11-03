using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float _interactDistance = 1.0f;    
    [Header("Links")]
    [SerializeField] private Camera _camera;

    private Player _player;
    private InteractableObject _currentInteractableObject;

    public void initialize(Player player)
    {
        _player = player;      
    }

    private void Update()
    {
        if (_player.IsActive == false)
            return;

        HandleInteraction();
    }

    private void HandleInteraction()
    {
        RaycastHit hit;
      
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward,
            out hit, _interactDistance))
        {           
            if(hit.collider.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObject))
            {
                _currentInteractableObject = interactableObject;              
            }          
            else
            {
                _currentInteractableObject = null;
            }
        }

        if(_currentInteractableObject != null && _currentInteractableObject.CanInteract)
        {           
            if (Input.GetKeyDown(GlobalVars.InteractionKeyPrimary) || Input.GetKeyDown(GlobalVars.InteractionKeySecondary))
            {               
                _currentInteractableObject.TryInteract(_player);
            }
        }
    }
}
