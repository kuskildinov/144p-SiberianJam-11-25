using UnityEngine;

public class SecureCam : MonoBehaviour
{
    [SerializeField] private bool _isActive = true;
    [Header("Movment Settings")]
    [SerializeField] private Transform[] _targets;
    [SerializeField] private float _rotationSpeed = 3f;
    [SerializeField] private float _holdDuration = 2f;
    [SerializeField] private Transform _eye;

    [Header("Spotlight Settings")]
    [SerializeField] private Light _eyeSpotlight;
    [SerializeField] private float _spotlightRange = 50f;
    [SerializeField] private float _spotlightAngle = 50f;

    [Header("Player Detection")]      
    [SerializeField] private float _detectionRange = 8f;
    
    private int _currentIndex = 0;
    private bool _movingForward = true;
    private float _holdTimer = 0f;
    private bool _isLooking = true;

    private bool _playerInSight = false;
    private Transform _playerTransform;
    private Player _currentDetectedPlayer;

    private void Update()
    {
        if (_targets == null || _targets.Length == 0 || _isActive == false) return;

        if (_isLooking && _playerInSight == false)
        {
            HandleCamMovment();
        }
        else
        {
            // Немедленно переходим к следующей цели
            _isLooking = true;
        }

        if (_eyeSpotlight == null) return;

        CheckPlayerDetection();
    }

    #region Movment

    private void HandleCamMovment()
    {
        // Плавно поворачиваемся к цели
        Vector3 direction = (_targets[_currentIndex].position - _eye.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(direction);
        _eye.rotation = Quaternion.Slerp(_eye.rotation, targetRot, _rotationSpeed * Time.deltaTime);

        // Если смотрим на цель, начинаем задержку
        if (Quaternion.Angle(_eye.rotation, targetRot) < 2f)
        {
            _holdTimer += Time.deltaTime;
            if (_holdTimer >= _holdDuration)
            {
                _holdTimer = 0f;
                _isLooking = false;
                GetNextTarget();
            }
        }
    }

    private void GetNextTarget()
    {
        if (_movingForward)
        {
            _currentIndex++;
            if (_currentIndex >= _targets.Length)
            {
                _currentIndex = _targets.Length - 2;
                _movingForward = false;
            }
        }
        else
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = 1;
                _movingForward = true;
            }
        }
    }

    #endregion

    #region Player Detection

    private void CheckPlayerDetection()
    {       
        Collider[] hitColliders = Physics.OverlapSphere(_eye.position, _detectionRange);
        bool detected = false;

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToPlayer = (hitCollider.transform.position - _eye.position).normalized;
            float angle = Vector3.Angle(_eye.forward, directionToPlayer);

            if (angle <= _spotlightAngle / 2f)
            {              
                RaycastHit hit;
                if (Physics.Raycast(_eye.position, directionToPlayer, out hit, _detectionRange))
                {
                    if (hit.transform.TryGetComponent<Player>(out Player player))
                    {
                        if (player.CheckCanBeDetected() == false)
                        {
                            Debug.Log("TEST");
                            break;
                        }

                        _currentDetectedPlayer = player;
                        detected = true;
                        _playerTransform = hitCollider.transform;
                        LookAtPlayer(_playerTransform);
                        if (!_playerInSight)
                        {
                            OnPlayerDetected();
                            _currentDetectedPlayer.DetectedBySecure(_eye);
                        }
                        break;
                    }                   
                }
            }
        }

        if (!detected && _playerInSight)
        {
            OnPlayerLost();
            _currentDetectedPlayer.LostDetectionBySecure();
        }
    }

    private void OnPlayerDetected()
    {
        _playerInSight = true;      
      
        if (_eyeSpotlight != null)
        {
            _eyeSpotlight.color = Color.red;
        }
    }

    private void OnPlayerLost()
    {
        _playerInSight = false;       
        if (_eyeSpotlight != null)
        {
            _eyeSpotlight.color = Color.white;
        }
    }

    private void LookAtPlayer(Transform player)
    {
        _eye.LookAt(player);
    }

    #endregion
}
