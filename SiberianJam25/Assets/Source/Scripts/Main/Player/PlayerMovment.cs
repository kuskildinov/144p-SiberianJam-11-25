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
    [SerializeField] private float _gravity = 9.81f;

    [Header("Ground Detection")]
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _slopeLimit = 45f;
    [SerializeField] private Transform _groundCheckPoint;

    [Header("Mouse Look")]
    [SerializeField] private float _mouseSensitivity = 2.0f;
    [SerializeField] private float _verticalLookLimit = 80.0f;

    [Header("Links")]    
    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CharacterController _characterController;

    [Header("Head Shake")]
    [SerializeField] private bool _enableHeadShake = true;
    [SerializeField] private float _shakeFrequency = 1.5f;
    [SerializeField] private float _shakeAmplitude = 0.1f;
   
    private Player _player;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _targetDirection = Vector3.zero;
    private float _rotationX = 0;
    private float _currentSpeed;
    private bool _isGrounded = true;
    private bool _isJumping = false;
    private bool _jumpRequested = false;
    private bool _isRunning = false;
    private bool _isMouseActive = true;
    private bool _isUnderControl = false;

    private float _defaultCameraY;
    private float _shakeTimer = 0;
    private float _verticalVelocity = 0f;

    public bool IsMouseActive { get => _isMouseActive; set => _isMouseActive = value; }
    public bool IsUnderControl { get => _isUnderControl; set => _isUnderControl = value; }
    public Vector3 TargetDirection { get => _targetDirection; set => _targetDirection = value; }

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
        HandleMovement();
        HandleHeadShake();
        HandleRunning();
        CheckGroundHandle();
        if (_isUnderControl)
            LockCameraToTarget();       
    }

    public void OnLostControl(Transform secureCam)
    {
        _targetDirection = secureCam.position;
        _isMouseActive = false;
        _isUnderControl = true;
    }

    public void OnReturnControl()
    {
        _isMouseActive = true;
        _isUnderControl = false;
    }

    private void LockCameraToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_targetDirection - _camera.transform.position);
        _camera.transform.rotation = Quaternion.Slerp(_camera.transform.rotation, targetRotation, 2f * Time.deltaTime);    
    }

    private void HandleMouseLook()
    {
        if (_isMouseActive == false)
            return;

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

        // Получаем направления движения относительно поворота объекта
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Вычисляем горизонтальный вектор движения
        Vector3 moveDirection = (forward * vertical) + (right * horizontal);

        // Нормализуем, если длина больше 1 (для диагонального движения)
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        // Умножаем на скорость
        moveDirection *= _currentSpeed;

        // Обработка гравитации и прыжка
        HandleGravityAndJump();

        // Комбинируем горизонтальное движение с вертикальной скоростью
        Vector3 finalMove = new Vector3(moveDirection.x, _verticalVelocity, moveDirection.z);

        // Двигаем персонажа
        _characterController.Move(finalMove * Time.deltaTime);
    }

    private void HandleGravityAndJump()
    {
        // Проверяем, находится ли персонаж на земле
        bool wasGrounded = _isGrounded;
        _isGrounded = _characterController.isGrounded;

        // Обработка приземления
        if (!wasGrounded && _isGrounded)
        {
            _verticalVelocity = -2f; // Небольшая отрицательная скорость для лучшего прилипания к земле
            _isJumping = false;
        }

        // Применяем гравитацию
        if (!_isGrounded)
        {
            _verticalVelocity -= _gravity * Time.deltaTime;
        }
        else
        {
            // Небольшая отрицательная скорость когда на земле для лучшего прилипания
            if (_verticalVelocity < 0)
                _verticalVelocity = -2f;
        }

        // Обработка прыжка
        if (Input.GetKeyDown(GlobalVars.JumpKey) && _isGrounded && !_isJumping)
        {
            _verticalVelocity = _jumpSpeed;
            _isJumping = true;
        }
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

    private void CheckGroundHandle()
    {
        _isGrounded = Physics.CheckSphere(_groundCheckPoint.transform.position, .5f, _groundMask);

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
}
