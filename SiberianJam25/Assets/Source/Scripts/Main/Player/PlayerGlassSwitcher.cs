using System.Collections;
using UnityEngine;

public class PlayerGlassSwitcher : MonoBehaviour
{  
    private const string CameraDownAnimationParam = "Down";
    private const string BlinkAnimationTriggerParam = "Blink";

    [SerializeField] private GameObject _glassFade;
    [SerializeField] private Animator _camAnimator;
    [SerializeField] private Animator _eyeBlinkAnimator;
    [SerializeField] private GameObject _glasses;

    private Player _player;
    private bool _glassOn;

    public bool GlassOn => _glassOn;

    public void Initialize(Player player)
    {
        _player = player;
        _glasses.gameObject.SetActive(false);
    }

    public void TrySwitchGlasses()
    {
        StartCoroutine(SwitchGlassesRoutine());
    }    

    public void OnEndSwitchGlasses()
    {
        _camAnimator.enabled = false;
        _camAnimator.SetBool(CameraDownAnimationParam, false);      
    }

    
    private IEnumerator SwitchGlassesRoutine()
    {        
        // активируем аниматор камеры
        _camAnimator.enabled = true;                   
        yield return null;

        // Включаем анимацию камеры с ожиданием в зависимости от длительности анимации
        _camAnimator.SetBool(CameraDownAnimationParam,true);
        if (_glassOn)
        {           
            // Мы снимаем очки, поэтому сначала в руке их нет
            _glasses.gameObject.SetActive(false);
            Debug.Log("Снимаем");
        }
        else
        {
            // Мы надеваем очки -> показываем их в руке
            _glasses.gameObject.SetActive(true);
            Debug.Log("Надеваем");
        }
       
        yield return null;
        AnimatorStateInfo stateInfo = _camAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;
        yield return new WaitForSecondsRealtime(animationLength / 2);

        _player.PlaySwitchGlassesAnimation();
        // Дожидаемся середины надевания очков
        yield return new WaitForSecondsRealtime(1f);

        if (_glassOn)
        {
            // Мы снимаем очки, поэтому показываем, после того как рука приблизилась к лицу
            _glasses.gameObject.SetActive(true);
        }
        else
        {
            // Мы надеваем очки -> скрываем, так как оставляем очки на глазах
            _glasses.gameObject.SetActive(false);
        }

        _eyeBlinkAnimator.SetTrigger(BlinkAnimationTriggerParam);
        _glassOn = !_glassOn;
        yield return null;
    }
}
