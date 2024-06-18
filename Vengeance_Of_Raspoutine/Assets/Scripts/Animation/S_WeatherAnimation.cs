using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_WeatherAnimation : MonoBehaviour
{
    public static S_WeatherAnimation Instance;

    [SerializeField] private TextMeshProUGUI _playerWeatherText;
    [SerializeField] private S_WeatherEvent _currentEvent;
    [SerializeField] private Animator _animatorWeather;
    [SerializeField] private Animator _animatorPlayerTurn;
    [SerializeField] private GameObject _animPlayerCanvas;

    [HideInInspector] public bool hasPlayedPlayer = false;
    [HideInInspector] public bool hasPlayedWeather = false;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }

    private void Start()
    {
        _animPlayerCanvas.SetActive(false);
    }

    void Update()
    {
        PlayAnimAfterTheOther();
    }

    public void PlayWeatherAnimation() // function for play the weather animation 
    {
        _playerWeatherText.text = "Weather : " + _currentEvent.ManageEvent;
        _animatorWeather.Play(0, 0, 0);
    }

    public void PlayAnimAfterTheOther()
    {
        if (!hasPlayedWeather) // check if the first animation has already play 
        {
            _animatorWeather.Play("WeatherAnimation");
            hasPlayedWeather = true;
        }

        // Check if the first animation is finished and play the second one.
        if (hasPlayedWeather && !hasPlayedPlayer && _animatorWeather.GetCurrentAnimatorStateInfo(0).IsName("WeatherAnimation")&& _animatorWeather.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !_animatorWeather.IsInTransition(0))
        {
            _animPlayerCanvas.SetActive(true);
            _animatorPlayerTurn.Play("PlayerTurnAnimation");
            hasPlayedPlayer = true;
        }

        // Check that the second animation is complete and deactivate the Animator
        if (hasPlayedPlayer && _animatorPlayerTurn.GetCurrentAnimatorStateInfo(0).IsName("PlayerTurnAnimation") && _animatorPlayerTurn.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !_animatorPlayerTurn.IsInTransition(0))
        {
            _animPlayerCanvas.SetActive(false);
        }
    }
    public void SkipAnimation(InputAction.CallbackContext ctx) //skip the animation speed to make a kind of skip
    {
        if (ctx.performed)
        {
            _animatorPlayerTurn.speed = 100f;
        }

        else if (ctx.canceled)
        {
            _animatorPlayerTurn.speed = 1f;
        }
    }
}
