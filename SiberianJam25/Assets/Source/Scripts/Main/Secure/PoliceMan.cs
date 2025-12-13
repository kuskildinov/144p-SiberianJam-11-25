using UnityEngine;

public class PoliceMan : NPC
{
    [Header("Player Detection")]
    [SerializeField] private float _detectionRange = 8f;
    [SerializeField] private float _detectionAngle = 50f;
    [SerializeField] private Transform _headPoint;

    private bool _playerDetected = false;
    private Transform _playerTransform;
    private Player _currentDetectedPlayer;

    public override void Update()
    {
        base.Update();

        CheckPlayerDetection();
    }

    private void CheckPlayerDetection()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _detectionRange);
        bool detected = false;

        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToPlayer = (hitCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToPlayer);

            if (angle <= _detectionAngle / 2f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, _detectionRange))
                {
                    if (hit.transform.TryGetComponent<Player>(out Player player))
                    {
                        if (player.CheckCanBeDetected() == false)
                        {                           
                            break;
                        }                          
                        _currentDetectedPlayer = player;
                        detected = true;
                        _playerTransform = hitCollider.transform;
                        LookAtPlayer(_playerTransform);
                        if (!_playerDetected)
                        {
                            OnPlayerDetected();
                            _currentDetectedPlayer.DetectedBySecure(_headPoint);
                        }
                        break;
                    }
                }
            }
        }

        if (!detected && _playerDetected)
        {
            OnPlayerLost();
            _currentDetectedPlayer.LostDetectionBySecure();
        }
    }

    private void OnPlayerDetected()
    {
        _playerDetected = true;       
    }

    private void OnPlayerLost()
    {
        _playerDetected = false;       
    }

    private void LookAtPlayer(Transform player)
    {
        transform.LookAt(player);
    }
}
