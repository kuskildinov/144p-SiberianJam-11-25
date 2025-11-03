using UnityEngine;

public class CodeGatePuzzle : MonoBehaviour
{
    private const string OpenGateAnimation = "Open";
    [SerializeField] private CodePanel _codePanel;
    [SerializeField] private Animator _animator;

    public void Initialize()
    {
        _codePanel.Initialize(this);
    }

    public void OpenGate()
    {
        _animator.SetBool(OpenGateAnimation,true);
    }
}
