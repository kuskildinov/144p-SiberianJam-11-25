using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : InteractableObject
{
    [SerializeField] private Animator animator;
    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _rotationSpeed = 120f;
    [SerializeField] private float _waitTimeAtPoint = 2f;
    [SerializeField] private bool _loopPatrol = true;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> _waypoints = new List<Transform>();

    private enum NPCState { Moving, Waiting, Idle }
    private NPCState _currentState = NPCState.Idle;
    private int _currentWaypointIndex = 0;
    private bool _movingForward = true;

    // Properties
    public bool HasWaypoints => _waypoints != null && _waypoints.Count > 0;
    public bool IsMoving => _currentState == NPCState.Moving;

    private void Start()
    {
        Activate();
    }

    public void Activate()
    {
        // Ќачинаем патрулирование если есть точки
        if (HasWaypoints)
        {
            StartPatrol();
        }
        else
        {
            SetIdle();
        }
    }

   public virtual void Update()
    {
        switch (_currentState)
        {
            case NPCState.Moving:
                MoveToWaypoint();
                break;
            case NPCState.Waiting:
                // ќжидание обрабатываетс€ в корутине
                break;
            case NPCState.Idle:
                // NPC просто стоит на месте
                UpdateIdleBehavior();
                break;
        }

        UpdateAnimator();
    }
       
    public void StartPatrol()
    {
        if (!HasWaypoints) return;

        _currentState = NPCState.Moving;
        _currentWaypointIndex = 0;
        _movingForward = true;
    }
       
    public void StopPatrol()
    {
        _currentState = NPCState.Idle;
        StopAllCoroutines();
    }
      
    public void AddWaypoint(Transform waypoint)
    {
        _waypoints.Add(waypoint);
               
        if (_currentState == NPCState.Idle && _waypoints.Count == 1)
        {
            StartPatrol();
        }
    }
        
    public void ClearWaypoints()
    {
        _waypoints.Clear();
        StopPatrol();
    }
   
    private void MoveToWaypoint()
    {
        if (!HasWaypoints) return;

        Transform targetWaypoint = _waypoints[_currentWaypointIndex];

        if (targetWaypoint == null)
        {
            Debug.LogWarning($"Waypoint {_currentWaypointIndex} is null!");
            GoToNextWaypoint();
            return;
        }

        // ѕоворот в сторону цели
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        direction.y = 0; // »гнорируем разницу по высоте

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        // ƒвижение к точке
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, _movementSpeed * Time.deltaTime);

        // ѕроверка достижени€ точки
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        if (distance < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }
        
    private IEnumerator WaitAtWaypoint()
    {
        _currentState = NPCState.Waiting;

        yield return new WaitForSeconds(_waitTimeAtPoint);

        GoToNextWaypoint();
    }
       
    private void GoToNextWaypoint()
    {
        if (!HasWaypoints) return;

        if (_loopPatrol)
        {          
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Count;
        }
        else
        {            
            if (_movingForward)
            {
                _currentWaypointIndex++;
                if (_currentWaypointIndex >= _waypoints.Count - 1)
                {
                    _movingForward = false;
                }
            }
            else
            {
                _currentWaypointIndex--;
                if (_currentWaypointIndex <= 0)
                {
                    _movingForward = true;
                }
            }
        }

        _currentState = NPCState.Moving;
    }
   
    protected virtual void UpdateIdleBehavior()
    {
        //TODO: добавить действие на месте
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;
               
        animator.SetFloat("Speed", _currentState == NPCState.Moving ? _movementSpeed : 0f);       
    }
  
    private void SetIdle()
    {
        _currentState = NPCState.Idle;
        StopAllCoroutines();
    }

    public void SetMovementSpeed(float speed)
    {
        _movementSpeed = Mathf.Max(0, speed);
    }

    public void SetWaitTime(float waitTime)
    {
        _waitTimeAtPoint = Mathf.Max(0, waitTime);
    }

    public void SetLoopPatrol(bool loop)
    {
        _loopPatrol = loop;
    }
}
