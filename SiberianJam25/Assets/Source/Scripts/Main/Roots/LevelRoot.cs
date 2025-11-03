using UnityEngine;

public class LevelRoot : CompositeRoot
{
    [SerializeField] private EnviernmentSwitcher _enviernemtSwitcher;
    private WorldState _currentWorldState;

    public override void Compose()
    {
        _currentWorldState = WorldState.PINK;
    }

    public void TryShowPinkWorld()
    {
        _enviernemtSwitcher.ShowPinkWorld();
    }

    public void TryShowBadWorld()
    {
        _enviernemtSwitcher.ShowBadWorld();
    }
}

public enum WorldState
{
    PINK,
    BAD,
}

