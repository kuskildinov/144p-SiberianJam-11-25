using UnityEngine;

public class PoliceMan : NPC
{
    [Header("Player Detection")]
    [SerializeField] private float _detectionRange = 8f;
    [SerializeField] private float _detectionAngle = 50f;
    [SerializeField] private Transform _headPoint;
    [Header("Signs")]
    [SerializeField] private GameObject _num;
    [SerializeField] private GameObject _symbol;

    private bool _playerDetected = false;
    private Transform _playerTransform;
    private Player _currentDetectedPlayer;

    public override void Update()
    {
        base.Update();

        CheckPlayerDetection();
    }

    #region >>> PLAYER DETECTION
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
                        if (!player.IsActive)
                            return;
                                              
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
    #endregion
    #region >>> SIGNS SETTINGS

    private void ShowSymbol()
    {
        _num.gameObject.SetActive(false);
        _symbol.gameObject.SetActive(true);
    }

    private void ShowNum()
    {
        _num.gameObject.SetActive(true);
        _symbol.gameObject.SetActive(false);
    }


    #endregion

    protected override void OnWorldStateChanged(WorldState newState)
    {
        if(newState == WorldState.PINK)
        {
            ShowSymbol();
        }
        else if(newState == WorldState.BAD)
        {
            ShowNum();
        }

    }
}
