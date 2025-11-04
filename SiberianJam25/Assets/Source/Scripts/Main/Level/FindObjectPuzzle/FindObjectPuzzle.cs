using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjectPuzzle : MonoBehaviour
{
    [SerializeField] private List<KeyLock> _locks;
    [SerializeField] private float rotationDuration = 1f;
    [SerializeField] private Animator _doorAnimator;

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
        RotateKeys();

        StartCoroutine(OpenDoorRoutine());
    }

    private void RotateKeys()
    {
        foreach (KeyLock keyLock in _locks)
        {
            StartCoroutine(RotateKeyRoutine(keyLock.transform));
        }
    }

    private IEnumerator RotateKeyRoutine(Transform key)
    {      
        Quaternion startRotation = key.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, 90);
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;
            key.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        key.rotation = targetRotation;       
    }

    private IEnumerator OpenDoorRoutine()
    {
        yield return new WaitForSecondsRealtime(2f);

        _doorAnimator.SetTrigger("Open");
    }
}
