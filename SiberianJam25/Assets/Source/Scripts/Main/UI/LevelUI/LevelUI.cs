using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private BlackFadePanel _blackFade;

    private LevelRoot _root;

    public void Initialize(LevelRoot root)
    {
        _root = root;
    }

    #region >>> BLACK FADE

    public void ShowBlackFadeOff()
    {
        _blackFade.PlayFadeOffAnimation();
    }

    #endregion
}
