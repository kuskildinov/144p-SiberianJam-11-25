using System.Collections;
using UnityEngine;

public class CodeGatePuzzle : LevelPuzzle
{
    private const string OpenGateAnimationParam = "Open";
    private const float OpenGateDeley = 2f;

    [SerializeField] private CodePanel _codePanel;
    [SerializeField] private Animator _doorAnimator;

    public override void Initialize(LevelPuzzlesHandler puzzleHandler)
    {
        base.Initialize(puzzleHandler);

        _codePanel.Initialize(this);
    }

    public void OpenGate()
    {
        StartCoroutine(OpenDoorRoutine());
    }

    private IEnumerator OpenDoorRoutine()
    {
        yield return new WaitForSecondsRealtime(OpenGateDeley);

        _doorAnimator.SetTrigger(OpenGateAnimationParam);
    }
}
