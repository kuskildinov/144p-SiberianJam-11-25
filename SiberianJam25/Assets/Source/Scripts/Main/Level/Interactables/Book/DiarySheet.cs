using System;
using UnityEngine;
using UnityEngine.UI;

public class DiarySheet : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Button _nextPageButton;
    [SerializeField] private Button _previousPageButton;

    private DiaryBook _book;

    public void Initialize(DiaryBook book)
    {
        _book = book;
        SubscribeToButtonsEvents();
    }


    public void SetData(SheetData data)
    {
        _text.text = data.SheetText;
    }

    public void Clear()
    {
        _text.text = "";
    }

    private void OnNextPageButtonClicked()
    {
        Debug.Log($"NEXT {gameObject.name}");
        _book.TryOpenNextPage();
    }

    private void OnPreviuosPageButtonClicked()
    {
        Debug.Log($"PREVIOUS {gameObject.name}");
        _book.TryOpenPreviousPage();
    }

    #region >>> BUTTONS EVENTS

    private void SubscribeToButtonsEvents()
    {
        _nextPageButton.onClick.AddListener(OnNextPageButtonClicked);
        _previousPageButton.onClick.AddListener(OnPreviuosPageButtonClicked);
    }

    private void UnsubscribeToButtonsEvents()
    {
        _nextPageButton.onClick.RemoveAllListeners();
        _previousPageButton.onClick.RemoveAllListeners();
    }

    #endregion

    private void OnDestroy()
    {
        UnsubscribeToButtonsEvents();
    }
}

[Serializable]
public struct SheetData
{
    [TextArea] public string SheetText;
}
