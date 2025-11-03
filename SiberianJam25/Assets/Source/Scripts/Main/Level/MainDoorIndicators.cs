using UnityEngine;

public class MainDoorIndicators : MonoBehaviour
{
    [SerializeField] private MeshRenderer _lightOne;
    [SerializeField] private MeshRenderer _lightTwo;
    [SerializeField] private MeshRenderer _lighTree;
    [Header("Colors")]
    [SerializeField] private Material _greenColorMaterial;
    [SerializeField] private Material _redColorMaterial;
    
    public void Initialize()
    {
       UpdateLights();
    }
  
    public void OnPuzzleComplited()
    {       
        UpdateLights();
    }

    private void UpdateLights()
    {
        if (GlobalVars.PuzzleOneReady)
            _lightOne.material = _greenColorMaterial;
        else
            _lightOne.material = _redColorMaterial;

        if (GlobalVars.PuzzleTwoReady)
            _lightTwo.material = _greenColorMaterial;
        else
            _lightTwo.material = _redColorMaterial;

        if (GlobalVars.PuzzleTreeReady)
            _lighTree.material = _greenColorMaterial;
        else
            _lighTree.material = _redColorMaterial;
    }       
}
