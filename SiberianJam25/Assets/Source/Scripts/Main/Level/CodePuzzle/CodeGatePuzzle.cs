using System.Collections;
using UnityEngine;

public class CodeGatePuzzle : MonoBehaviour
{
    private const string OpenGateAnimation = "Open";
    [SerializeField] private CodePanel _codePanel;
    [SerializeField] private Animator _doorAnimator;

    public void Initialize()
    {
        _codePanel.Initialize(this);
    }

    public void OpenGate()
    {
        StartCoroutine(OpenDoorRoutine());
    }

    private IEnumerator OpenDoorRoutine()
    {
        yield return new WaitForSecondsRealtime(2f);

        _doorAnimator.SetTrigger("Open");
    }
}
