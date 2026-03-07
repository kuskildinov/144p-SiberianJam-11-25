using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMonster : MonoBehaviour
{
    [Header("Mesh variants")]
    [SerializeField] private GameObject _goodMesh;
    [SerializeField] private GameObject _badMesh;
    [Header("Queue Settings")]
    [SerializeField] private List<Transform> _queuePoints;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _npcMoveDelay = 5f;
    [SerializeField] private Transform _centerPoint;
    [SerializeField] private List<NPC> _npcPrefabs;

    private LevelRoot _levelRoot;
    private bool _isActive = true;
    private Queue<NPC> _currentQueue = new Queue<NPC>();

    public void Initialize(LevelRoot levelRoot)
    {
        _levelRoot = levelRoot;

        InitializeNpc();
        SubscribeToEvents();
        StartCoroutine(ActiveRoutine());
    }
   
    private IEnumerator ActiveRoutine()
    {
        while (_isActive)
        {
            NPC currentNpc = _currentQueue.Peek();
            currentNpc.AddWaypoint(_centerPoint);
            currentNpc.StartPatrol();
            _currentQueue.Dequeue();
            yield return new WaitForEndOfFrame();
            MoveAllNpcForward();
            yield return new WaitForSecondsRealtime(5f);
            Destroy(currentNpc.gameObject);
            SpawnNewNpc();
            yield return new WaitForSecondsRealtime(15f);
        }

        yield break;
    }

    #region >>> VISUAL

    private void ShowGoodMesh()
    {
        _goodMesh.gameObject.SetActive(true);
        _badMesh.gameObject.SetActive(false);
    }

    private void ShowBadMesh()
    {
        _goodMesh.gameObject.SetActive(false);
        _badMesh.gameObject.SetActive(true);
    }

    #endregion
    #region >>> QUEUE SETTINGS

    private void InitializeNpc()
    {
        foreach (Transform point in _queuePoints)
        {
            NPC npcPrefab = GetRandomNpc();
            if(npcPrefab == null)
            {
                Debug.LogError("Ошибка спавна нпс у монстра");
                return;
            }
            NPC npc = Instantiate(npcPrefab, point.position,point.rotation);
            npc.transform.SetParent(this.transform);
            _currentQueue.Enqueue(npc);
            npc.Activate();
        }
    }

    #endregion
    #region >>> NPC SETTINGS

    private NPC GetRandomNpc()
    {
        if(_npcPrefabs == null || _npcPrefabs.Count <= 0)
            return null;

        var rand = Random.Range(0, _npcPrefabs.Count);
        return _npcPrefabs[rand];
    }

    private void MoveAllNpcForward()
    {
        NPC[] npcArray = _currentQueue.ToArray();

        for (int i = 0; i < _queuePoints.Count - 1; i++)
        {
            npcArray[i].AddWaypoint(_queuePoints[i]);
            npcArray[i].StartPatrol();
        }
    }

    private void SpawnNewNpc()
    {
        NPC npcPrefab = GetRandomNpc();
        NPC newNpc = Instantiate(npcPrefab, _spawnPoint.position,_spawnPoint.rotation);
        newNpc.AddWaypoint(_queuePoints[_queuePoints.Count - 1]);
        newNpc.StartPatrol();
        _currentQueue.Enqueue(newNpc);
    }

    #endregion
    #region >>> EVENTS
    private void SubscribeToEvents()
    {
        _levelRoot.WorldStateChanged += OnWorldStateChanged;      
    }

    private void UnsubscribeToEvents()
    {
        _levelRoot.WorldStateChanged -= OnWorldStateChanged;
    }

    private void OnWorldStateChanged(WorldState newState)
    {
        if (newState == WorldState.PINK)
        {
            ShowGoodMesh();
        }
        else if (newState == WorldState.BAD)
        {
            ShowBadMesh();
        }
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
