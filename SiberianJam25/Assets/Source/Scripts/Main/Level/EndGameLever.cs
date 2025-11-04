using UnityEngine;

public class EndGameLever : InteractableObject
{
    [SerializeField] private LevelRoot _levelRoot;

    public override void TryInteract(Player player = null)
    {
        base.TryInteract(player);

        _levelRoot.OnWinGame();
    }
}
