using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTower : MonoBehaviour
{
    [SerializeField] private MainDoorIndicators _doorIndicators;    
    [Header("Gate")]
    [SerializeField] private Animator _gate;

    private LevelRoot _root;

    public void Initialize(LevelRoot root)
    {
        _root = root;

        _doorIndicators.Initialize(this);
    }

    public void OnPuzzleComplited(int index)
    {
        _doorIndicators.UpdateLights();
    }

    public void OpenGate()
    {
        _gate.SetTrigger("Open");
    }
}
