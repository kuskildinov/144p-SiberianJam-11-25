using UnityEngine;

public class LevelPuzzle : MonoBehaviour
{
    protected LevelPuzzlesHandler _puzzleHandler;

    public virtual void Initialize(LevelPuzzlesHandler puzzleHandler)
    {
        _puzzleHandler = puzzleHandler;
    }
}
