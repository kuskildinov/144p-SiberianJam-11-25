using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlenderIndicatorsPanel : MonoBehaviour
{
    [SerializeField] private InteractableObject _button;
    [Header("Indicators")]
    [SerializeField] private Text _fullText;
    [SerializeField] private Color _textActiveColor;
    [SerializeField] private Color _textNoActiveColor;
    [SerializeField] private GameObject _indicatorLight;
    [Header("Panel Sounds")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _buttonSound;

    private BlenderPuzzle _puzzle;

    public void Initialize(BlenderPuzzle puzzle)
    {
        _puzzle = puzzle;

        SubscribeToEvents();
    }

    private void ActivateFullIndecator()
    {
        _button.SetInteractable(true);
        _fullText.color = _textActiveColor;
        _indicatorLight.gameObject.SetActive(true);
    }

    private void DeactivateFullIndicator()
    {       
        _button.SetInteractable(false);
        _fullText.color = _textNoActiveColor;
        _indicatorLight.gameObject.SetActive(false);
    }

    #region >>> SOUNDS

    private void PlayButtonSound()
    {
        _audioSource.PlayOneShot(_buttonSound);
    }

    #endregion

    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _puzzle.BlenderSwitchedOn += OnBlendedSwitchOn;
        _puzzle.BlenderFull += OnBlenderIsFull;
        _button.Interacted += OnBlenderButtonClicked;
    }

    private void UnsubscribeToEvents()
    {
        _puzzle.BlenderSwitchedOn -= OnBlendedSwitchOn;
        _puzzle.BlenderFull -= OnBlenderIsFull;
        _button.Interacted -= OnBlenderButtonClicked;
    }

    private void OnBlenderButtonClicked(Player player)
    {
        PlayButtonSound();
        _puzzle.OnBlenderButtonClicked();
    }

    private void OnBlenderIsFull()
    {
        ActivateFullIndecator();
    }

    private void OnBlendedSwitchOn()
    {       
        DeactivateFullIndicator();
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
