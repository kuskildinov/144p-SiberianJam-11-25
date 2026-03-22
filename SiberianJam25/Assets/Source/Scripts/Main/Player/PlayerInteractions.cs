using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float _interactDistance = 1.0f;    
    [Header("Links")]

    private Player _player;
    private Camera _camera => _player.Camera;
    private InteractableObject _currentInteractableObject;

    public void Initialize(Player player)
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
        if(_camera == null)
        {           
            return;
        }
        
        RaycastHit hit;
      
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward,
            out hit, _interactDistance))
        {           
            if(hit.collider.gameObject.TryGetComponent<InteractableObject>(out InteractableObject interactableObject))
            {               
                _currentInteractableObject = interactableObject;
                if (_currentInteractableObject.CheckCanInteract(_player))
                {
                    _player.TryShowInteractionInfo(_currentInteractableObject.InteractionInfo);
                }               
            }          
            else
            {
                _currentInteractableObject = null;             
            }
        }
       
        if(_currentInteractableObject != null && _currentInteractableObject.CheckCanInteract(_player))
        {           
            if (Input.GetKeyDown(GlobalVars.InteractionKeyPrimary) || Input.GetKeyDown(GlobalVars.InteractionKeySecondary))
            {               
                _currentInteractableObject.TryInteract(_player);
            }
        }
    }
}
