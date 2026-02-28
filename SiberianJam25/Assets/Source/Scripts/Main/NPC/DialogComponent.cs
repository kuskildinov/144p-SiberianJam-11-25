using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogComponent : MonoBehaviour
{
    [SerializeField] private string _characterName;
    [SerializeField] private List<DialogPhrase> _phrases;
}

[Serializable]
public struct DialogPhrase
{
    public string PhraseText;
}
