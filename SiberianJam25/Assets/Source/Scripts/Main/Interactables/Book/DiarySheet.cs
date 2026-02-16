using System;
using UnityEngine;
using UnityEngine.UI;

public class DiarySheet : MonoBehaviour
{
    [SerializeField] private Text _text;

    public void SetData(SheetData data)
    {
        _text.text = data.SheetText;
    }

    public void Clear()
    {
        _text.text = "";
    }
}

[Serializable]
public struct SheetData
{
    [TextArea] public string SheetText;
}
