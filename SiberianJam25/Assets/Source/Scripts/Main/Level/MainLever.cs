using System.Collections;
using UnityEngine;

public class MainLever : InteractableObject
{
    [SerializeField] private int _index;
    [SerializeField] private Animator _animator;
    [Header("VFX")]
    [SerializeField] private GameObject _sparksVFX;
    [Header("Sound")]
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _switchOffClip;
    [SerializeField] private float _playSoundDeley = 1f;

    private LevelRoot _root;
    
    public void Initialize(LevelRoot root)
    {
        _root = root;
        CheckCanInteract();
        DeactivateSparks();
        _animator.SetBool("Activate", false);
        
    }

    #region >>> INTERACTION

    public override void TryInteract(Player player = null)
    {
        base.TryInteract();

        SwitchOff();
    }

    private void CheckCanInteract()
    {
        if((_index == 0 && GlobalVars.PuzzleOneReady) || (_index == 1 && GlobalVars.PuzzleTwoReady) || (_index == 2 && GlobalVars.PuzzleTreeReady))
        {
            SwitchOff();          
        }
     }

    #endregion
    #region >>> SWITCH ON OFF
    private void SwitchOff()
    {
        StartCoroutine(SwitchOffRoutine());
    }

    private void SwitchOn()
    {
        CanInteract = true;
        _animator.SetBool("Activate", false);

        DeactivateSparks();
    }

    private IEnumerator SwitchOffRoutine()
    {       
        CanInteract = false;
        PlaySwitchOffAnimation();
        ActivateSparks();
        yield return new WaitForSecondsRealtime(_playSoundDeley);
        PlaySwitchOffSound();
        _root.OnPuzzleComplited(_index);
        yield return null;
    }

    #endregion
    #region >>> ANIMATIONS
    private void PlaySwitchOffAnimation()
    {
        _animator.SetBool("Activate", true);
    }

    #endregion
    #region >>> VFX

    private void ActivateSparks()
    {
        _sparksVFX.gameObject.SetActive(true);
    }

    private void DeactivateSparks()
    {
        _sparksVFX.gameObject.SetActive(false);
    }
    #endregion
    #region >>> SOUNDS

    private void PlaySwitchOffSound()
    {
        _source.PlayOneShot(_switchOffClip);
    }

    #endregion
}
