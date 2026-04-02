using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogComponent : MonoBehaviour
{
    public string CharacterNameRus;
    public string CharacterNameEn;
    public List<DialogPhrase> Phrases;
    public float DialogTime = 5f;   
}

[Serializable]
public struct DialogPhrase
{
    public string PhraseTextRus;
    public string PhraseTextEn;
}
