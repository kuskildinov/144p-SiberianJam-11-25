using UnityEngine;
using Cinemachine;

public class PlayerMovment : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";
    private const string MouseX = "Mouse X";
    private const string MouseY = "Mouse Y";

    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 5.0f;
    [SerializeField] private float _runSpeed = 8.0f;
    [SerializeField] private float _slowWalkSpeed = 1f;
    [SerializeField] private float _jumpSpeed = 7.0f;
    [SerializeField] private float _gravity = 9.81f;

    [Header("Ground Detection")]  
    [SerializeField] private LayerMask _groundMask;   
    [SerializeField] private Transform _groundCheckPoint;

    [Header("Mouse Look")]
    [SerializeField] private float _mouseSensitivity = 2.0f;
    [SerializeField] private float _verticalLookLimit = 80.0f;

    [Header("Links")]
    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    [SerializeField] private CharacterController _characterController;   
   
    private Player _player;  
    private Vector3 _targetDirection = Vector3.zero;
    private float _rotationX = 0;
    private float _currentSpeed;
    private bool _isGrounded = true;
    private bool _isJumping = false;   
    private bool _isRunning = false;
    private bool _isMouseActive = true;
    private bool _isUnderControl = false;
    private bool _canRun => _player.CurrentPlayerState == PlayerState.DEFAULT;
    private bool _canJump => _player.CurrentPlayerState == PlayerState.DEFAULT;

    private float _horizontalInput;
    private float _verticalInput;
    private float _mouseX;
    private float _mouseY;
    private float _verticalVelocity = 0f;           
    private bool _runKeyInput;
    private bool _jumpKeyInput;
   

    public void initialize(Player player)
    {
        _player = player;                
        _currentSpeed = _walkSpeed;
        _slowWalkSpeed = _walkSpeed / 2;
    }

    private void Update()
    {
        if (_player.IsActive == false)
            return;

        ReadInput();
        HandleMouseLook();      
        HandleMovement();       
        HandleRunning();
        CheckGroundHandle();
        if (_isUnderControl)
            LockCameraToTarget();       
    }

    #region >>> MOVMENT
    private void HandleMovement()
    {       
        // Получаем направления движения относительно поворота объекта
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Вычисляем горизонтальный вектор движения
        Vector3 moveDirection = (forward * _verticalInput) + (right * _horizontalInput);

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

    private void HandleRunning()
    {
        if (!_canRun) 
            return;

        if (_runKeyInput)
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
    #endregion
    #region >>> JUMP
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
        if (_jumpKeyInput && _isGrounded && !_isJumping && _canJump)
        {
            _verticalVelocity = _jumpSpeed;
            _isJumping = true;
        }
    }

    private void CheckGroundHandle()
    {
        _isGrounded = Physics.CheckSphere(_groundCheckPoint.transform.position, .5f, _groundMask);

        if (_isGrounded)
            _isJumping = false;
    }

    #endregion
    #region >>> LOOK DIRECTION
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
        Quaternion targetRotation = Quaternion.LookRotation(_targetDirection - _virtualCam.transform.position);
        _virtualCam.transform.rotation = Quaternion.Slerp(_virtualCam.transform.rotation, targetRotation, 2f * Time.deltaTime);    
    }

    private void HandleMouseLook()
    {
        if (_isMouseActive == false)
            return;
             
        _player.transform.Rotate(0, _mouseX, 0);               
        _rotationX -= _mouseY;
        _rotationX = Mathf.Clamp(_rotationX, -_verticalLookLimit, _verticalLookLimit);
        _virtualCam.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
    }

    #endregion

    private void ReadInput()
    {       
        _horizontalInput = Input.GetAxis(HorizontalAxis);
        _verticalInput = Input.GetAxis(VerticalAxis);
        _runKeyInput = Input.GetKey(GlobalVars.RunKey);
        _jumpKeyInput = Input.GetKeyDown(GlobalVars.JumpKey);
        _mouseX = Input.GetAxis(MouseX) * _mouseSensitivity;
        _mouseY = Input.GetAxis(MouseY) * _mouseSensitivity;
    }

    private void OnDestroy()
    {
        
    }
}
