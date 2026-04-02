using System.Collections;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [Header("Dialog")]
    [SerializeField] private PhrasePanel _dialogPanel;
    [Header("Target")]
    [SerializeField] private GameObject _targetImage;
    [Header("Interaction Info")]
    [SerializeField] private InteractionInfoPanel _interactionInfoPanel;
    [Header("PLayer Tips")]
    [SerializeField] private PlayerTipsPanel _tipsPanel;

    private PlayerRoot _root;
    private Coroutine _dialogCoroutine;

    public void Initialize(PlayerRoot root)
    {
        _root = root;
    }

    #region >>>  DIALOG
    public void SetDialogPhrase(DialogComponent dialogComponent)
    {
        if (_dialogCoroutine != null)
        {
            StopCoroutine(_dialogCoroutine);
        }

        _dialogCoroutine = StartCoroutine(ShowRandomDialogRoutine(dialogComponent));
    }   

    public void SetDialogPhrase(DialogComponent dialogComponent, int dialogIndex)
    {
        if (_dialogCoroutine != null)
        {
            StopCoroutine(_dialogCoroutine);
        }

        _dialogCoroutine = StartCoroutine(ShowDialogRoutine(dialogComponent, dialogIndex));
    }

    private IEnumerator ShowRandomDialogRoutine(DialogComponent dialogComponent)
    {
        string name = "";

        if (GlobalVars.CurrentLang == Lang.RU)
            name = dialogComponent.CharacterNameRus;
        else if(GlobalVars.CurrentLang == Lang.EN)
            name = dialogComponent.CharacterNameEn;

        DialogPhrase currentPhrase = dialogComponent.Phrases[Random.Range(0, dialogComponent.Phrases.Count)];
        string dialogText = "";

        if (GlobalVars.CurrentLang == Lang.RU)
            dialogText = currentPhrase.PhraseTextRus;
        else if (GlobalVars.CurrentLang == Lang.EN)
            dialogText = currentPhrase.PhraseTextEn;

        _dialogPanel.ShowPhrase(dialogText, name);
        yield return new WaitForSecondsRealtime(dialogComponent.DialogTime);
        _dialogPanel.Hide();
    }

    private IEnumerator ShowDialogRoutine(DialogComponent dialogComponent, int dialogIndex)
    {
        string name = "";

        if (GlobalVars.CurrentLang == Lang.RU)
            name = dialogComponent.CharacterNameRus;
        else if (GlobalVars.CurrentLang == Lang.EN)
            name = dialogComponent.CharacterNameEn;

        DialogPhrase currentPhrase = dialogComponent.Phrases[dialogIndex];
        string dialogText = "";

        if (GlobalVars.CurrentLang == Lang.RU)
            dialogText = currentPhrase.PhraseTextRus;
        else if (GlobalVars.CurrentLang == Lang.EN)
            dialogText = currentPhrase.PhraseTextEn;

        _dialogPanel.ShowPhrase(dialogText, name);
        yield return new WaitForSecondsRealtime(dialogComponent.DialogTime);
        _dialogPanel.Hide();
    }
    #endregion
    #region >>> TARGET IMAGE

    public void ShowTargetImage()
    {
        _targetImage.gameObject.SetActive(true);
    }

    public void HideTargetImage()
    {
        _targetImage.gameObject.SetActive(false);
    }

    #endregion
    #region >>> INTERACTION INFO

    public void TryShowInteractionInfo(string infoText)
    {
        _interactionInfoPanel.Show(infoText);
    }

    public void ShowCantInteractText()
    {
        _interactionInfoPanel.ShowCantInteractText();
    }

    #endregion
    #region >>> PLAYER TIPS

    public void ShowTipByType(PlayerTipsType type)
    {
        _tipsPanel.ShowTipByType(type);
    }

    #endregion
}
