using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractionInfoPanel : MonoBehaviour
{
    [SerializeField] private GameObject _interactionPanel;
    [SerializeField] private Text _interactionInfoText;
    [SerializeField] private Text _cantTakeItemText;
    [SerializeField] private float _infoDuration;

    private Coroutine _showCoroutine;

    public void Show(string infoText)
    {
        if(_showCoroutine!= null)
        {
            StopCoroutine(_showCoroutine);
            _showCoroutine = null;
        }
        _showCoroutine = StartCoroutine(ShowInfoRoutine(infoText));
    }

    public void Hide()
    {
        _interactionPanel.gameObject.SetActive(false);
    }

    public void ShowCantInteractText()
    {
        StartCoroutine(ShowCantInteractTextRoutine());
    }

    private IEnumerator ShowInfoRoutine(string infoText)
    {
        _interactionPanel.gameObject.SetActive(true);
        _interactionInfoText.text = infoText;

        yield return new WaitForSecondsRealtime(_infoDuration);

        Hide();
    }

    private IEnumerator ShowCantInteractTextRoutine()
    {
        _cantTakeItemText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(_infoDuration * 5);
        _cantTakeItemText.gameObject.SetActive(false);
    }
}
