using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float _interactDistance = 1.0f;    
    [Header("Links")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Button _interactionButton;

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

    private void OnEnable()
    {
        _interactionButton.onClick.AddListener(OnInteractionButtonClicked);
    }

    private void OnDisable()
    {
        _interactionButton.onClick.RemoveAllListeners();
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
                if (_currentInteractableObject.CanInteract)
                    _interactionButton.gameObject.SetActive(true);
                else
                    _interactionButton.gameObject.SetActive(false);
            }          
            else
            {
                _currentInteractableObject = null;
                _interactionButton.gameObject.SetActive(false);
            }
        }
        else
        {
            _player.HideInteractionInfo();
        }

        
    }

    private void OnInteractionButtonClicked()
    {
        if (_currentInteractableObject != null && _currentInteractableObject.CanInteract)
        {
            _currentInteractableObject.TryInteract(_player);
        }
    }
}
