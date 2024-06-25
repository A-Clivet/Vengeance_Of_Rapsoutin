using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class S_WeatherEvent : MonoBehaviour
{
    public static S_WeatherEvent Instance;

    public enum Event
    {
        None,
        Earthquake,
        Fog,
        Blizzard
    }

    public EventStocker functionStocker = new EventStocker();
    public Action currentEvent = null;

    Event _ManageEvent;
    public Event ManageEvent
    {
        get { return _ManageEvent; }
        private set
        {
            _ManageEvent = value;

            currentEvent = functionStocker.StockFunction(value, this);
        }
    }

    [SerializeField] private S_GridManager _player1GridManager;
    [SerializeField] private S_GridManager _player2GridManager;
    [SerializeField] private Image fog;
    [SerializeField] private GameObject _mainCam;
    [SerializeField] private S_SnowStorm snowStormManager;
    [SerializeField] private TextMeshProUGUI weatherInfo;

    private int nbTurn = 0;
    private int fogOpacityState = 1;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }

    private IEnumerator CameraShake()
    {
        float timer = 0;
        Vector3 originalPos = _mainCam.transform.localPosition;
        while (timer < 2)
        {
            _mainCam.transform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere*0.1f;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _mainCam.transform.position = originalPos;

        yield return null;
    }
    public void EventProbability()
    {
        nbTurn = 0;
        fog.color = new Color(fog.color.r, fog.color.g, fog.color.b, 0);
        fogOpacityState = 1;

        int rndProb = UnityEngine.Random.Range(0, 101);

        if (rndProb > 80)
        {
            int eventChosen = UnityEngine.Random.Range(0, 3);

            if (eventChosen == 0)
            {
                nbTurn = 4;
                ManageEvent = Event.Earthquake;
            }
            else if (eventChosen == 1)
            {
                ManageEvent = Event.Fog;
            }
            else
            {
                nbTurn = 5;
                ManageEvent = Event.Blizzard;
            }
        }
        else
        {
            ManageEvent = Event.None;
        }
    }

    public void EarthquakeEvent()
    {
        nbTurn--;
        if (nbTurn < 0)
        {
            nbTurn = 4;
            StartCoroutine(CameraShake());
            List<Unit> unitToRemove = new List<Unit>();
            foreach (Unit u in _player1GridManager.unitList.Where(a => a.state == 1))
            {
                unitToRemove.Add(u);
            }
            foreach (Unit u in unitToRemove)
            {

                _player1GridManager.unitList.Remove(u);
                Destroy(u.gameObject);
            }
            unitToRemove.Clear();

            foreach (Unit u in _player2GridManager.unitList.Where(a => a.state == 1))
            {
                unitToRemove.Add(u);
            }
            foreach (Unit u in unitToRemove)
            {
                _player2GridManager.unitList.Remove(u);
                Destroy(u.gameObject);
            }
            _player1GridManager.UnitPriorityCheck();
            _player2GridManager.UnitPriorityCheck();

        }
        return;
    }
    public void FogEvent()
    {
        nbTurn += fogOpacityState;

        if (nbTurn <= 0)
        {
            fog.color = new Color(fog.color.r, fog.color.g, fog.color.b, 0);
            fogOpacityState = -fogOpacityState;
        }
        if (nbTurn == 1)
        {
            fog.color = new Color(fog.color.r, fog.color.g, fog.color.b, 0.5f);
        }
        if (nbTurn >= 2)
        {
            fog.color = new Color(fog.color.r, fog.color.g, fog.color.b, 1);
            fogOpacityState = -fogOpacityState;
        }
        return;
    }

    public void BlizzardEvent()
    {
        nbTurn--;
        if (nbTurn < 0)
        {
            snowStormManager.StartSnowstorm();
            nbTurn = 5;
            List<Unit> listOfIdle;
            listOfIdle = new List<Unit>();
            foreach (Unit u in _player1GridManager.unitList.Where(a => a.state == 0))
            {
                listOfIdle.Add(u);
            }

            int nbMaxOfUnitFrozen = listOfIdle.Count() / 2;
            for (int i = 0; i < nbMaxOfUnitFrozen; i++)
            {
                int unitToFreeze = UnityEngine.Random.Range(0, listOfIdle.Count());
                listOfIdle[unitToFreeze].freeze.SetActive(true);
                listOfIdle[unitToFreeze].state = 3;
            }

            listOfIdle.Clear();
            foreach (Unit u in _player2GridManager.unitList.Where(a => a.state == 0))
            {
                listOfIdle.Add(u);
            }

            nbMaxOfUnitFrozen = listOfIdle.Count() / 2;
            for (int i = 0; i < nbMaxOfUnitFrozen; i++)
            {
                int unitToFreeze = UnityEngine.Random.Range(0, listOfIdle.Count());
                listOfIdle[unitToFreeze].freeze.SetActive(true);
                listOfIdle[unitToFreeze].state = 3;
            }
        }
        return;
    }
}

public class EventStocker
{
    private Action _eventToStore { get; set; }
    public Action StockFunction(S_WeatherEvent.Event p_wantedEvent, S_WeatherEvent p_classRef)
    {
        if (p_wantedEvent == S_WeatherEvent.Event.Earthquake)
        {
            _eventToStore = p_classRef.EarthquakeEvent;
            return _eventToStore;
        }
        else if (p_wantedEvent == S_WeatherEvent.Event.Fog)
        {
            _eventToStore = p_classRef.FogEvent;
            return _eventToStore;
        }
        else if (p_wantedEvent == S_WeatherEvent.Event.Blizzard)
        {
            _eventToStore = p_classRef.BlizzardEvent;
            return _eventToStore;
        }
        else if (p_wantedEvent == S_WeatherEvent.Event.None)
        {
            _eventToStore = null;
            return _eventToStore;
        }
        Debug.LogError("the event given in parameter does not exist");
        return null;
    }
}