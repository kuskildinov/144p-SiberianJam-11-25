using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogComponent : MonoBehaviour
{
    public string CharacterName;
    public List<DialogPhrase> Phrases;
    public float DialogTime = 5f;
}

[Serializable]
public struct DialogPhrase
{
    public string PhraseTextRus;
    public string PhraseTextEn;
}
