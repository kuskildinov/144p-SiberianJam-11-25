using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTipsPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private float _tipsDuration;
    [SerializeField] private Text _tipsText; 
    [Header("Tips Texts")]
    [SerializeField] private string _glassSwitchOnTextRus;
    [SerializeField] private string _glassSwitchOnTextEn;
    [Space]
    [SerializeField] private string _walkTextRus;
    [SerializeField] private string _walkTextEn;
    [Space]
    [SerializeField] private string _runTextRus;
    [SerializeField] private string _runTextEn;
    [Space]
    [SerializeField] private string _jumpTextRus;
    [SerializeField] private string _jumpTextEn;

    private Coroutine _showTipsRoutine;

    private void ShowPanel()
    {
        _panel.gameObject.SetActive(true);
    }

    private void HidePanel()
    {
        _tipsText.text = "";
        _panel.gameObject.SetActive(false);
    }

    public void ShowTipByType(PlayerTipsType type)
    {
        string tipsText = "";
        switch (type)
        {
            case PlayerTipsType.GlassSwitch:
                {
                    tipsText = _glassSwitchOnTextRus;
                    break;
                }
            case PlayerTipsType.Walk:
                {
                    tipsText = _walkTextRus;
                    break;
                }
            case PlayerTipsType.Run:
                {
                    tipsText = _runTextRus;
                    break;
                }
            case PlayerTipsType.Jump:
                {
                    tipsText = _jumpTextRus;
                    break;
                }
        }

        ShowTips(tipsText);

    }

    private void ShowTips(string tipText)
    {
        if(_showTipsRoutine != null)
        {
            StopCoroutine(_showTipsRoutine);
            _showTipsRoutine = null;
        }
        _showTipsRoutine = StartCoroutine(ShowTipsRoutine(tipText));
    }

    private IEnumerator ShowTipsRoutine(string tipText)
    {
        ShowPanel();
        _tipsText.text = tipText;
        yield return new WaitForSecondsRealtime(_tipsDuration);
        HidePanel();
    }
}

public enum PlayerTipsType
{
    GlassSwitch,
    Walk,
    Run,
    Jump,
}
