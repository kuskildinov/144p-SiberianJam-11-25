using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerMovment _movment;
    [SerializeField] private PlayerAnimations _animations;
    [SerializeField] private PlayerInteractions _interactions;
    [SerializeField] private Camera _camera;

    private PlayerRoot _root;
    private bool _isActive;

    public bool IsActive => _isActive;

    public void initialize(PlayerRoot root)
    {
        _root = root;
        _movment?.initialize(this);
        _animations?.initialize(this);
        _interactions?.initialize(this);
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    public void DetectedBySecure(Transform secureCam)
    {
        Debug.Log("Игрок замечен");       
    }

    public void LostDetectionBySecure()
    {
        Debug.Log("Игрок потерян");
    }
}
