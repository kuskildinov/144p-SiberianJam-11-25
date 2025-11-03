using System.Collections.Generic;
using UnityEngine;

public class FindObjectPuzzle : MonoBehaviour
{
    [SerializeField] private List<KeyLock> _locks;
    
    public void Initialize()
    {
        InitializeLocks();
    }

    private void InitializeLocks()
    {
        foreach (KeyLock keyLock in _locks)
        {
            keyLock.Initialize(this);
        }
    }

    public void OnKeyInserted()
    {
        if(CheckAllLocks())
        {
            OpenDoor();
        }
    }

    private bool CheckAllLocks()
    {
        foreach (KeyLock keyLock in _locks)
        {
            if (keyLock.IsEmpty)
                return false;
        }

        return true;
    }

    private void OpenDoor()
    {
        Debug.Log("Дверь открыта");
    }
}
