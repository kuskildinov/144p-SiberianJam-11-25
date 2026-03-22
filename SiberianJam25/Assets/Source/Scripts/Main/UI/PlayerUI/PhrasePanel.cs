using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhrasePanel : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _phraseText;

    [Header("Typing Settings")]
    [SerializeField] private float _typingSpeed = 0.05f;
    [SerializeField] private float _fastForwardSpeed = 0.01f;
    [SerializeField] private float _endDialogTime;

    [Header("Appearance")]
    [SerializeField] private Color _textColor = Color.white;

    private string _currentPhrase;
    private Coroutine _typingCoroutine;
    private bool _isTyping = false;
    private bool _isFastForward = false;

    private void Awake()
    {
        _phraseText.color = _textColor;
    }

    /// <summary>
    /// Показать панель диалога
    /// </summary>
    public void Show()
    {
        if (_panel != null)
            _panel.SetActive(true);
    }

    /// <summary>
    /// Скрыть панель диалога
    /// </summary>
    public void Hide()
    {
        if (_panel != null)
            _panel.SetActive(false);

        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }

        _isTyping = false;
        _isFastForward = false;
    }

    public void ShowPhrase(string phrase, string name)
    {
        if (_phraseText == null) return;

        Show();

        // Останавливаем предыдущую корутину
        if (_typingCoroutine != null)
        {           
            SkipTyping();
            return;
        }

        _currentPhrase = phrase;
        _phraseText.text = "";
        _nameText.text = name;
        _isTyping = true;
        _isFastForward = false;

        _typingCoroutine = StartCoroutine(TypeText(phrase));
    }

    private IEnumerator TypeText(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            _phraseText.text += text[i];

            // Используем разную скорость в зависимости от режима
            float speed = _isFastForward ? _fastForwardSpeed : _typingSpeed;
            yield return new WaitForSecondsRealtime(speed);
        }

        yield return new WaitForSecondsRealtime(_endDialogTime);

        _isTyping = false;
        _typingCoroutine = null;
        Hide();
    }

    public void FastForward()
    {
        if (_isTyping)
        {
            _isFastForward = true;
        }
    }

    public void SkipTyping()
    {
        if (_typingCoroutine != null)
        {
            StopCoroutine(_typingCoroutine);
            _typingCoroutine = null;
        }

        if (_phraseText != null && _currentPhrase != null)
        {
            _phraseText.text = _currentPhrase;
        }

        _isTyping = false;
        _isFastForward = false;
    }

    public bool IsTyping()
    {
        return _isTyping;
    }

    public void ClearText()
    {
        if (_phraseText != null)
            _phraseText.text = "";
        if (_nameText != null)
            _nameText.text = "";
        _currentPhrase = "";
        _isTyping = false;
        _isFastForward = false;
    }
}
