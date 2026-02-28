using System.Collections;
using UnityEngine;

public class PlayerGlassSwitcher : MonoBehaviour
{     
    private const string BlinkAnimationTriggerParam = "Blink";

    [SerializeField] private GameObject _glassFade;
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
       
    }

    public void OnEyesClosed()
    {
        if(_glassOn)
        {
            _player.OnGlassesOff();
        }
        else
        {            
            _player.OnGlassesOn();
        }
    }
    
    private IEnumerator SwitchGlassesRoutine()
    {        
        // активируем аниматор камеры                
        yield return null;
               
        if (_glassOn)
        {           
            // ћы снимаем очки, поэтому сначала в руке их нет
            _glasses.gameObject.SetActive(false);           
        }
        else
        {
            // ћы надеваем очки -> показываем их в руке
            _glasses.gameObject.SetActive(true);
            Debug.Log("Ќадеваем");
        }
       
        yield return null;       

        _player.PlaySwitchGlassesAnimation();
        // ƒожидаемс€ середины надевани€ очков
        yield return new WaitForSecondsRealtime(1f);

        if (_glassOn)
        {
            // ћы снимаем очки, поэтому показываем, после того как рука приблизилась к лицу
            _glasses.gameObject.SetActive(true);
        }
        else
        {
            // ћы надеваем очки -> скрываем, так как оставл€ем очки на глазах
            _glasses.gameObject.SetActive(false);
        }

        _glassOn = !_glassOn;
        _eyeBlinkAnimator.SetTrigger(BlinkAnimationTriggerParam);
        yield return null;
    }
}
