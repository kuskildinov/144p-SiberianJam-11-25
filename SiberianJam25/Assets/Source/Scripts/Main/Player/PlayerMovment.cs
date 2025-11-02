using UnityEngine;

public class PlayerMovment : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    private const string MouseX = "Mouse X";
    private const string MouseY = "Mouse Y";

    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 5.0f;
    [SerializeField] private float _runSpeed = 8.0f;
    [SerializeField] private float _jumpSpeed = 7.0f;   

    [Header("Ground Detection")]
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask _groundMask = 1; // Слой Default
    [SerializeField] private float _slopeLimit = 45f;
    [SerializeField] private GameObject _groundCheckPoint;

    [Header("Mouse Look")]
    [SerializeField] private float _mouseSensitivity = 2.0f;
    [SerializeField] private float _verticalLookLimit = 80.0f;

    [Header("Links")]    
    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody _rigidbody;

    [Header("Head Shake")]
    [SerializeField] private bool _enableHeadShake = true;
    [SerializeField] private float _shakeFrequency = 1.5f;
    [SerializeField] private float _shakeAmplitude = 0.1f;

    private Player _player;
    private Vector3 _moveDirection = Vector3.zero;
    private float _rotationX = 0;
    private float _currentSpeed;
    private bool _isGrounded = true;
    private bool _isJumping = false;
    private bool _jumpRequested = false;
    private bool _isRunning = false;

    private float _defaultCameraY;
    private float _shakeTimer = 0;   

    public void initialize(Player player)
    {
        _player = player;

        _defaultCameraY = _camera.transform.localPosition.y;
        _currentSpeed = _walkSpeed;
    }

    private void Update()
    {
        if (_player.IsActive == false)
            return;

        HandleMouseLook();
        HandleJump();
        HandleMovement();
        HandleHeadShake();
        HandleRunning();
        CheckGroundHandle();
    }

    private void FixedUpdate()
    {
        if (_player.IsActive == false)
            return;

       
    }

    private void HandleMouseLook()
    {      
        float mouseX = Input.GetAxis(MouseX) * _mouseSensitivity;
        _player.transform.Rotate(0, mouseX, 0);
               
        _rotationX -= Input.GetAxis(MouseY) * _mouseSensitivity;
        _rotationX = Mathf.Clamp(_rotationX, -_verticalLookLimit, _verticalLookLimit);
        _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
    }

    private void HandleMovement()
    {      
        float horizontal = Input.GetAxis(HorizontalAxis);
        float vertical = Input.GetAxis(VerticalAxis);
      
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
              
        float curSpeedX = _currentSpeed * vertical;
        float curSpeedY = _currentSpeed * horizontal;

        _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        Vector3 move = new Vector3(horizontal, 0, vertical) * _currentSpeed;
        Vector3 newVelocity = transform.TransformDirection(move);
        newVelocity.y = _rigidbody.velocity.y; // Сохраняем Y-скорость для гравитации

        _rigidbody.velocity = newVelocity;
    }

    private void HandleRunning()
    {
        if (Input.GetKey(GlobalVars.RunKey))
        {
            _isRunning = true;
            _currentSpeed = _runSpeed;
        }
        else
        {
            _isRunning = false;
            _currentSpeed = _walkSpeed;
        }
    }

    private void HandleJump()
    {       
        if (Input.GetKeyDown(GlobalVars.JumpKey) && _isGrounded && !_isJumping)
        {
            _isJumping = true;
            _jumpRequested = true;
            Debug.Log("JUMP");
            _rigidbody.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
        }       
    }

    private void CheckGroundHandle()
    {
        RaycastHit hit;       
        _isGrounded = Physics.Raycast(_groundCheckPoint.transform.position, Vector3.down, out hit,
                        _groundCheckDistance, _groundMask);

        if (_isGrounded)
            _isJumping = false;
    }   

    private void HandleHeadShake()
    {
        if (!_enableHeadShake || !_isGrounded|| _camera == null)
            return;

        bool isMoving = (Input.GetAxis(HorizontalAxis) != 0 || Input.GetAxis(VerticalAxis) != 0);

        if (isMoving)
        {
            _shakeTimer += Time.deltaTime * (_isRunning ? _shakeFrequency * 1.5f : _shakeFrequency);

            float bobAmount = _isRunning ? _shakeAmplitude * 1.2f : _shakeAmplitude;
            float newY = _defaultCameraY + Mathf.Sin(_shakeTimer) * bobAmount;

            Vector3 cameraPos = _camera.transform.localPosition;
            cameraPos.y = newY;
            _camera.transform.localPosition = cameraPos;
        }
        else
        {           
            _shakeTimer = 0;
            Vector3 cameraPos = _camera.transform.localPosition;
            cameraPos.y = Mathf.Lerp(cameraPos.y, _defaultCameraY, Time.deltaTime * 3f);
            _camera.transform.localPosition = cameraPos;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isGrounded ? Color.green : Color.red;       
        Gizmos.DrawRay(_groundCheckPoint.transform.position, Vector3.down * _groundCheckDistance);
    }
}
