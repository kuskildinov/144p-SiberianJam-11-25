using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private PhrasePanel _dialogPanel;
   public void SetDialogPhrase(DialogComponent dialogComponent)
    {
        string name = dialogComponent.CharacterName;
        DialogPhrase currentPhrase = dialogComponent.Phrases[Random.Range(0, dialogComponent.Phrases.Count)];
        string dialogText = currentPhrase.PhraseTextRus;

        _dialogPanel.ShowPhrase(dialogText, name);
    }
}
