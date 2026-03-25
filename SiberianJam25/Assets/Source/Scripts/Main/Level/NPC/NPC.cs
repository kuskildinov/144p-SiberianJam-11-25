using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : InteractableObject
{
    [SerializeField] private Animator _animator;
    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _rotationSpeed = 120f;
    [SerializeField] private float _waitTimeAtPoint = 2f;
    [SerializeField] private bool _loopPatrol;
    [SerializeField] private bool _resetMovePoint;
    [SerializeField] private bool _pingPongMove;
    [SerializeField] private bool _playOnAwake;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> _waypoints = new List<Transform>();

    private LevelNpcHandler _npcHandler;
    private enum NPCState { Moving, Waiting, Idle }
    private NPCState _currentState = NPCState.Idle;
    private int _currentWaypointIndex = 0;
    private bool _movingForward = true;
    private Quaternion _defaultRotation;
    private Coroutine _dialogCoroutine;

    // Properties
    public bool HasWaypoints => _waypoints != null && _waypoints.Count > 0;
    public bool IsMoving => _currentState == NPCState.Moving;

    public event Action OnLastPointReached;
    
    public void Initialize(LevelNpcHandler npcHandler)
    {
        _npcHandler = npcHandler;

        if (_playOnAwake)
            Activate();

        _defaultRotation = transform.rotation;

        SubscribeToEvents();
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

    #region >>> INTERACTION

    public override void TryInteract(Player player = null)
    {
        base.TryInteract(player);

        TryStartDialogWithPlayer(player);
    }

    #endregion
    #region >>> DIALOG

    private void TryStartDialogWithPlayer(Player player)
    {
        if (TryGetComponent<DialogComponent>(out DialogComponent dialogComponent))
        {
            if(_dialogCoroutine != null)
            {
                StopCoroutine(_dialogCoroutine);
            }
            _dialogCoroutine = StartCoroutine(DialogWithPlayerRoutine(player, dialogComponent));
        }
    }

    private IEnumerator DialogWithPlayerRoutine(Player player, DialogComponent dialogComponent)
    {
        transform.LookAt(new Vector3(player.transform.position.x, 1f, player.transform.position.z));
        player.TryActivateDialog(dialogComponent);

        yield return new WaitForSecondsRealtime(dialogComponent.DialogTime);

        ResetRotation();
    }

    #endregion
    #region >>> MOVMENT
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
               
        //if (_currentState == NPCState.Idle && _waypoints.Count == 1)
        //{
        //    StartPatrol();
        //}
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
        RotateToMovePoint(targetWaypoint);

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

        if(_resetMovePoint)
        {
            ClearWaypoints();
        }
        else
        {
            GoToNextWaypoint();
        }
       
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
                    OnLastPointReached?.Invoke();
                }
            }
            else if(_pingPongMove)
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
        if (_animator == null) return;
               
        if(_currentState == NPCState.Idle || _currentState == NPCState.Waiting)
        {
            _animator.SetBool("Walk", false);
        }
        else if(_currentState == NPCState.Moving)
        {
            _animator.SetBool("Walk", true);
        }
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
    #endregion
    #region >>> ROTATION
    private void RotateToMovePoint(Transform targetWaypoint)
    {
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        direction.y = 0; // »гнорируем разницу по высоте

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void ResetRotation()
    {
        transform.rotation = _defaultRotation;
    }
    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _npcHandler.WorldStateChanged += OnWorldStateChanged;
    }

    private void UnsubscribeToEvents()
    {
        _npcHandler.WorldStateChanged -= OnWorldStateChanged;
    }

    protected virtual void OnWorldStateChanged(WorldState newState)
    {
       
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
