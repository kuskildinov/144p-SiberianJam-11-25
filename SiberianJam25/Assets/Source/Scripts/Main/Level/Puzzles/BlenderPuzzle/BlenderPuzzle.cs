using System;
using UnityEngine;

public class BlenderPuzzle : LevelPuzzle
{
    [Header("Mesh variants")]
    [SerializeField] private GameObject _goodMesh;
    [SerializeField] private GameObject _badMesh;
    [Header("Blender Components")]
    [SerializeField] private BlenderBloodHandler _bloodHandler;
    [SerializeField] private BlenderIndicatorsPanel _indicatorsPanel;
    [SerializeField] private InteractableObject _carete;
    [SerializeField] private BlenderMonster _monster;
    [SerializeField] private Transform _blenderSpawnPoint;    
    [Header("other")]   
    [SerializeField] private int _maxPiecesCount;
    [SerializeField] private AudioSource _blenderSoundSource;
    [SerializeField] private AudioSource _mosterSoundSource;
       
    private bool _isSwitchOn;
    private bool _isMonsterEating;
    private int _currentPiecesCount = 0;

    public event Action BlenderSwitchedOn;
    public event Action BlenderFull;
       
    public override void Initialize(LevelPuzzlesHandler puzzleHandler)
    {
        base.Initialize(puzzleHandler);

        _bloodHandler.Initialize(this);
        _indicatorsPanel.Initialize(this);

        SubscribeToEvents();
    }

    #region >>> VISUAL

    private void ShowGoodMesh()
    {
        _goodMesh.gameObject.SetActive(true);
        _badMesh.gameObject.SetActive(false);

        _monster.ShowGood();
    }

    private void ShowBadMesh()
    {
        _goodMesh.gameObject.SetActive(false);
        _badMesh.gameObject.SetActive(true);

        _monster.ShowBad();
    }

    #endregion
    #region >>> SWITCH ON OFF

    private void SwitchOn()
    {
        _isSwitchOn = true;
        PlayMainBlenderSound();
        // HideAllPieces
        
        BlenderSwitchedOn?.Invoke();
    }

    #endregion
    #region >>> SOUNDS

    private void PlayMainBlenderSound()
    {
        _blenderSoundSource.Play();
    }

    private void StopMainBlenderSound()
    {
        _blenderSoundSource.Stop();
    }

    #endregion
    #region >>> INTERACTABLES

    public void OnBlenderButtonClicked()
    {
        SwitchOn();
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
        BlenderFull?.Invoke();
    }

    #endregion   
    #region >>> EVENTS
    private void SubscribeToEvents()
    {
        _puzzleHandler.WorlsStateChanged += OnWorldStateChanged;
        _carete.Interacted += OnPLayerDropBodyPiece;
        _bloodHandler.BloodTroughEnded += OnBloodTroughEnded;
        _monster.OnLastPointReached += OnMonsterEndWay;        
    }

    private void UnsubscribeToEvents()
    {
        _puzzleHandler.WorlsStateChanged -= OnWorldStateChanged;
        _carete.Interacted -= OnPLayerDropBodyPiece;
        _bloodHandler.BloodTroughEnded -= OnBloodTroughEnded;
        _monster.OnLastPointReached -= OnMonsterEndWay;
    }

    public void OnWorldStateChanged(WorldState newState)
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

    private void OnBloodTroughEnded()
    {       
        StopMainBlenderSound();
        _monster.Activate();
    }

    private void OnMonsterEndWay()
    {
        _mosterSoundSource.Play();
    }
    #endregion

    private void OnDestroy()
    {
        UnsubscribeToEvents();
    }
}
