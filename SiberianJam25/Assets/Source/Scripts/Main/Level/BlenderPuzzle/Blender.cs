using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blender : MonoBehaviour
{
    [Header("Mesh variants")]
    [SerializeField] private GameObject _goodMesh;
    [SerializeField] private GameObject _badMesh;
    [Header("Blender Settings")]
    [SerializeField] private GameObject _blenderBlood;
    [SerializeField] private GameObject _throughBlood;
    [SerializeField] private List<ParticleSystem> _bloodVFX;
    [SerializeField] private float _timeToFullCarete = 10f;
    [SerializeField] private Transform _blenderSpawnPoint;
    [Header("Blood Settings")]
    [SerializeField] private float _bloodSpeed;
    [SerializeField] private float _blenderBloodLowerY;
    [SerializeField] private float _troughBloodMaxY;
    [Header("Interactors")]
    [SerializeField] private InteractableObject _button;
    [SerializeField] private InteractableObject _carete;
    [Header("Indicators")]
    [SerializeField] private Text _fullText;
    [SerializeField] private Color _textActiveColor;
    [SerializeField] private Color _textNoActiveColor;
    [SerializeField] private GameObject _indicatorLight;
    [Header("other")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private int _maxPiecesCount = 1;
    
    private LevelRoot _levelRoot;
    private bool _switchOn;
    private int _currentPiecesCount;

    public void Initialize(LevelRoot levelRoot)
    {
        _levelRoot = levelRoot;
        _currentPiecesCount = 0;
        HideBlood();      
        SubscribeToEvents();
    }

    private void Update()
    {
        if (!_switchOn)
            return;

        MoveDownBlenderBlood();
        MoveUpTroughBlood();
    }

    #region >>> VISUAL

    private void ShowGoodMesh()
    {
        _goodMesh.gameObject.SetActive(true);
        _badMesh.gameObject.SetActive(false);
    }

    private void ShowBadMesh()
    {
        _goodMesh.gameObject.SetActive(false);
        _badMesh.gameObject.SetActive(true);
    }

    #endregion
    #region >>> SWITCH ON OFF

    private void SwitchOn()
    {
        _switchOn = true;

        // HideAllPieces
        ShowBlood();
        StartCoroutine(BloodCollectionRoutine());
    }

    #endregion  
    #region >>> BLOOD BEHAVIOUR

    private void ShowBlood()
    {
        _blenderBlood.gameObject.SetActive(true);
        _throughBlood.gameObject.SetActive(true);
        ShowBloodVFX();
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
            _blenderBlood.transform.localPosition = Vector3.MoveTowards(_blenderBlood.transform.localPosition,Vector3.down,_bloodSpeed * Time.deltaTime);
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
    }

    #endregion
    #region >>> INTERACTIONS

    private bool CheckCanClickButton()
    {
        if(_currentPiecesCount < _maxPiecesCount)
        {
            return false;
        }

        return true;
    }

    private void AddNewPiece(BodyPieces piece)
    {
        piece.SetParent(transform);
        piece.transform.position = _blenderSpawnPoint.position;
        _currentPiecesCount++;
        CheckPiecesCount();
    }

    private void CheckPiecesCount()
    {
        if (_currentPiecesCount >= _maxPiecesCount)
        {
            OnPiecesFull();
        }
    }

    private void OnPiecesFull()
    {
        _button.CanInteract = true;
        ActivateFullIndecator();
    }

    #endregion
    #region >>> INDICATORS

    private void ActivateFullIndecator()
    {
        _fullText.color = _textActiveColor;
        _indicatorLight.gameObject.SetActive(true);
    }

    private void DeactivateFullIndicator()
    {
        _fullText.color = _textNoActiveColor;
        _indicatorLight.gameObject.SetActive(false);
    }

    #endregion
    #region >>> EVENTS
    private void SubscribeToEvents()
    {
        _levelRoot.WorldStateChanged += OnWorldStateChanged;
        _button.Interacted += OnButtonClicked;
        _carete.Interacted += OnPLayerDropBodyPiece;
    }

    private void UnsubscribeToEvents()
    {
        _levelRoot.WorldStateChanged -= OnWorldStateChanged;
        _button.Interacted -= OnButtonClicked;
        _carete.Interacted -= OnPLayerDropBodyPiece;
    }

    private void OnWorldStateChanged(WorldState newState)
    {
        if (newState == WorldState.PINK)
        {
            ShowGoodMesh();
        }
        else if (newState == WorldState.BAD)
        {
            ShowBadMesh();
        }
    }

    private void OnButtonClicked(Player player)
    {
        _audioSource.PlayOneShot(_buttonSound);

        if (CheckCanClickButton())
        {
            SwitchOn();
        }       
    }

    private void OnPLayerDropBodyPiece(Player player)
    {
        Item piece = player.CurrentItemOnHand;
        if(piece!= null && piece.TryGetComponent<BodyPieces>(out BodyPieces bodyPiece))
        {          
            player.DropItem();
            AddNewPiece(bodyPiece);
        }
        else
            return;

      
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
