using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBlinkPanel : MonoBehaviour
{
    [SerializeField] private PlayerGlassSwitcher _glassSwitcher;

    public void EyesClosed()
    {
        _glassSwitcher.OnEyesClosed();
    }
}
