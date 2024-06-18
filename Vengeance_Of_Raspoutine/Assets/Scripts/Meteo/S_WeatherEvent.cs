using System;
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
    [SerializeField] private TextMeshProUGUI weatherInfo;

    private int nbTurn = 0;
    private int fogOpacityState = 1;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
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
                nbTurn = 8;
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

        weatherInfo.text = "Weather : " + ManageEvent;
    }

    public void EarthquakeEvent()
    {
        nbTurn--;
        if (nbTurn < 0)
        {
            nbTurn = 8;
            List<Unit> unitToRemove = new List<Unit>();
            foreach (Unit u in _player1GridManager.unitList.Where(a => a.state == 1))
            {
                unitToRemove.Add(u);
            }
            foreach (Unit u in unitToRemove)
            {

                u.actualTile.unit = null;
                _player1GridManager.unitList.Remove(u);
                _player1GridManager.AllUnitPerColumn[u.tileX].Remove(u);
                Destroy(u.gameObject);
            }
            unitToRemove.Clear();

            foreach (Unit u in _player2GridManager.unitList.Where(a => a.state == 1))
            {
                unitToRemove.Add(u);
            }
            foreach (Unit u in unitToRemove)
            {

                u.actualTile.unit = null;
                _player2GridManager.unitList.Remove(u);
                _player2GridManager.AllUnitPerColumn[u.tileX].Remove(u);
                Destroy(u.gameObject);
            }
            _player1GridManager.AllUnitPerColumn = _player1GridManager.UnitPriorityCheck();
            _player2GridManager.AllUnitPerColumn = _player2GridManager.UnitPriorityCheck();

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
                listOfIdle[unitToFreeze].GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
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
                listOfIdle[unitToFreeze].GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
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