using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjectPuzzle : LevelPuzzle
{
    private const string OpenGateAnimatorParam = "Open";
    private const float OpenDoorDeley = 2f;

    [SerializeField] private List<KeyLock> _locks;
    [SerializeField] private float _keyRotationDuration = 1f;
    [SerializeField] private Animator _doorAnimator;
       
    public override void Initialize(LevelPuzzlesHandler puzzleHandler)
    {
        base.Initialize(puzzleHandler);

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

        while (elapsedTime < _keyRotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _keyRotationDuration;
            key.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        key.rotation = targetRotation;       
    }

    private IEnumerator OpenDoorRoutine()
    {
        yield return new WaitForSecondsRealtime(OpenDoorDeley);

        _doorAnimator.SetTrigger(OpenGateAnimatorParam);
    }
}
