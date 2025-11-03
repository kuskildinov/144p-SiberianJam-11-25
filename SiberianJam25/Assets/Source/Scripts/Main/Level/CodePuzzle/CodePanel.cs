using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodePanel : MonoBehaviour
{
    [SerializeField] private List<CodePanelButton> _butttons;

    private string _currentEnteredPassword = string.Empty;
    private CodeGatePuzzle _codeGate;

    public void Initialize(CodeGatePuzzle codeGate)
    {
        InitializeButtons();
    }

    public void EnterNewNumber(string num)
    {
        if (_currentEnteredPassword.Length < 4)
            _currentEnteredPassword += num;

        if (_currentEnteredPassword.Length >= 4)
            CheckIsCorrectPassword();

    }

    private void CheckIsCorrectPassword()
    {
        if(_currentEnteredPassword == GlobalVars.CodePanelCode)
        {
            Debug.Log("ֿאנמכü גונםûי");
            _codeGate.OpenGate();
            
        }
        else
        {
            Debug.Log("ְֿ׀־ֻÜ ֵֽ ֲֵ׀ֽÛֹ!");
        }

        _currentEnteredPassword = string.Empty;
    }

    private void InitializeButtons()
    {
        foreach (CodePanelButton button in _butttons)
        {
            button.Initialize(this);
        }
    }
}
