using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlackFadePanel : MonoBehaviour
{
    private const string FadeOnParam = "FadeOn";
    private const string FadeOffParam = "FadeOff";
    private const float FadeAnimationDurationPerSec = 1f;

    [SerializeField] private Animator _animator;
    [SerializeField] private Image _image;

    private Coroutine _fadeCoroutine;

    public void Show()
    {
        _image.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _image.gameObject.SetActive(false);
    }

    public void PlayFadeOnAnimation()
    {
        if(_fadeCoroutine != null)
        {
            _fadeCoroutine = null;
            return;
        }
        _fadeCoroutine = StartCoroutine(FadeRoutine(FadeOnParam));
    }
    
    public void PlayFadeOffAnimation()
    {
        if (_fadeCoroutine != null)
        {
            _fadeCoroutine = null;
            return;
        }
        _fadeCoroutine = StartCoroutine(FadeRoutine(FadeOffParam));
    }

    public void PlayOnAndOffAnimation()
    {
        if (_fadeCoroutine != null)
        {
            _fadeCoroutine = null;
            return;
        }
        _fadeCoroutine = StartCoroutine(OnOffFadeRoutine());
    }

    private IEnumerator FadeRoutine(string param)
    {
        _animator.SetTrigger(param);
        yield return new WaitForSecondsRealtime(FadeAnimationDurationPerSec);
        _animator.ResetTrigger(param);
    }

    private IEnumerator OnOffFadeRoutine()
    {
        PlayFadeOnAnimation();
        yield return new WaitForSecondsRealtime(FadeAnimationDurationPerSec);
        PlayFadeOffAnimation();
    }
}
