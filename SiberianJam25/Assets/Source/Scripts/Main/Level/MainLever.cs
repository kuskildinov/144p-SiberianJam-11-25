using UnityEngine;

public class MainLever : InteractableObject
{
    [SerializeField] private int _index;
    [SerializeField] private Animator _animator;

    private LevelRoot _root;

    public void Initialize(LevelRoot root)
    {
        _root = root;
        CheckCanInteract();
        _animator.SetBool("Activate", false);
    }

    public override void TryInteract(Player player = null)
    {
        base.TryInteract();

        SwitchOn();
        _root.OnPuzzleComplited(_index);

    }

    private void CheckCanInteract()
    {
        if((_index == 0 && GlobalVars.PuzzleOneReady) || (_index == 1 && GlobalVars.PuzzleTwoReady) || (_index == 2 && GlobalVars.PuzzleTreeReady))
        {
            SwitchOn();          
        }
     }

    private void SwitchOn()
    {
        // анимация срабатывания
        CanInteract = false;
        _animator.SetBool("Activate", true);
    }
}
