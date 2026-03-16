using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderBloodHandler : MonoBehaviour
{
    [Header("Blood Objects")]
    [SerializeField] private GameObject _blenderBlood;
    [SerializeField] private GameObject _throughBlood;
    [SerializeField] private List<ParticleSystem> _bloodVFX;
    [Header("Blood Settings")]
    [SerializeField] private float _bloodSpeed;
    [SerializeField] private float _timeToFullCarete = 10f;
    [SerializeField] private float _blenderBloodLowerY;
    [SerializeField] private float _troughBloodMaxY;

    private BlenderPuzzle _puzzle;
    private bool _bloodIsActive;

    public event Action BloodTroughEnded;

   public void Initialize(BlenderPuzzle puzzle)
    {
        _puzzle = puzzle;

        HideBlood();
        SubscribeToEvents();
    }

    private void Update()
    {
        if (!_bloodIsActive)
            return;

        MoveDownBlenderBlood();
        MoveUpTroughBlood();
    }

    #region >>> BLOOD BEHAVIOUR

    private void TryShowBloodEffects()
    {
        ShowBlood();
    }

    private void ShowBlood()
    {
        _blenderBlood.gameObject.SetActive(true);
        _throughBlood.gameObject.SetActive(true);
        _bloodIsActive = true;
        ShowBloodVFX();

        StartCoroutine(BloodCollectionRoutine());
    }

    private void HideBlood()
    {
        _blenderBlood.gameObject.SetActive(false);
        _throughBlood.gameObject.SetActive(false);
        HideBloodVFX();
    }

    private void MoveDownBlenderBlood()
    {
        if (_blenderBlood.transform.localPosition.y >= _blenderBloodLowerY)
            _blenderBlood.transform.localPosition = Vector3.MoveTowards(_blenderBlood.transform.localPosition, Vector3.down, _bloodSpeed * Time.deltaTime);
    }

    private void MoveUpTroughBlood()
    {
        if (_throughBlood.transform.localPosition.y <= _troughBloodMaxY)
            _throughBlood.transform.localPosition = Vector3.MoveTowards(_throughBlood.transform.localPosition, Vector3.up, _bloodSpeed * Time.deltaTime);
    }

    private void ShowBloodVFX()
    {
        foreach (ParticleSystem bloodParticle in _bloodVFX)
        {
            bloodParticle.Play();
        }
    }

    private void HideBloodVFX()
    {
        foreach (ParticleSystem bloodParticle in _bloodVFX)
        {
            bloodParticle.Stop();
        }
    }

    private IEnumerator BloodCollectionRoutine()
    {
        yield return new WaitForSecondsRealtime(_timeToFullCarete);
        HideBloodVFX();

        BloodTroughEnded?.Invoke();
    }

    #endregion
    #region >>> EVENTS

    private void SubscribeToEvents()
    {
        _puzzle.BlenderSwitchedOn += TryShowBloodEffects;
    }

    private void UnsubscribeToEvents()
    {
        _puzzle.BlenderSwitchedOn -= TryShowBloodEffects;
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
