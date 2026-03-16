using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderMonster : NPC
{
    [SerializeField] private GameObject _goodMesh;
    [SerializeField] private GameObject _badMesh;

    private BlenderPuzzle _puzzle;

    public void Initialize(BlenderPuzzle puzzle)
    {
        _puzzle = puzzle;
    }

    public void ShowGood()
    {
        _goodMesh.gameObject.SetActive(true);
        _badMesh.gameObject.SetActive(false);
    }

    public void ShowBad()
    {
        _goodMesh.gameObject.SetActive(false);
        _badMesh.gameObject.SetActive(true);
    }
}
