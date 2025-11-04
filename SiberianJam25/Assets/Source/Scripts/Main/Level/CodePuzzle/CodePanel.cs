using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodePanel : MonoBehaviour
{
    [SerializeField] private List<CodePanelButton> _butttons;
    [SerializeField] private CodePanelButton _enterButton;
    [SerializeField] private Text _passwordText;
    [Header("Indicator")]
    [SerializeField] private MeshRenderer _indicator;
    [SerializeField] private Material _correctColor;
    [SerializeField] private Material _incorrectColor;
    [SerializeField] private float _indicatorShowTime = 2f;
    [SerializeField] private AudioSource _indicatorSource;
    [SerializeField] private AudioClip _correctCLip;
    [SerializeField] private AudioClip _incorrectClip;

    private string _currentEnteredPassword = string.Empty;
    private CodeGatePuzzle _codeGate;
    private Material _indicatorDefaultMaterial;

    public void Initialize(CodeGatePuzzle codeGate)
    {
        _codeGate = codeGate;
        InitializeButtons();
        _indicatorDefaultMaterial = _indicator.material;
    }

    public void EnterNewNumber(string num)
    {
        if(num == "0")
        {
            CheckIsCorrectPassword();
            return;
        }

        if (_currentEnteredPassword.Length < 4)
            _currentEnteredPassword += num;

        if (_currentEnteredPassword.Length > 4)
            CheckIsCorrectPassword();

        _passwordText.text = _currentEnteredPassword;

    }

    private void CheckIsCorrectPassword()
    {
        if(_currentEnteredPassword == GlobalVars.CodePanelCode)
        {           
            PlayCorrectSound();
            ShowIndicator(true);
            DeactivateButtons();
            _codeGate.OpenGate();
            
        }
        else
        {
            ShowIndicator(false);
            PlayIncorrectSound();            
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

    private void PlayCorrectSound()
    {
        _indicatorSource.PlayOneShot(_correctCLip);
    }

    private void PlayIncorrectSound()
    {
        _indicatorSource.PlayOneShot(_incorrectClip);
    }

    private void ShowIndicator(bool correct)
    {
        StartCoroutine(ShowIndicatorRoutine(correct));
    }

    private void DeactivateButtons()
    {
        foreach (CodePanelButton button in _butttons)
        {
            button.CanInteract = false;
        }
    }

    private IEnumerator ShowIndicatorRoutine(bool correction)
    {
        if (correction)
            _indicator.material = _correctColor;
        else
            _indicator.material = _incorrectColor;

        yield return new WaitForSecondsRealtime(_indicatorShowTime);

        _indicator.material = _indicatorDefaultMaterial;
    }
}
