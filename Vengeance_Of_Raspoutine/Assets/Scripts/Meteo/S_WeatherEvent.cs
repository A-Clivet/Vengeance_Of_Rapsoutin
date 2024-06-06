using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class S_WeatherEvent : MonoBehaviour
{

    public static S_WeatherEvent Instance;

    public enum Event
    {
        Earthquake,
        Fog,
        Blizzard
    }

    public EventStocker functionStocker = new EventStocker();
    public Action currentEvent=null;

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

    private int nbTurn = 0;
    [SerializeField] private S_GridManager _player1GridManager;
    [SerializeField] private S_GridManager _player2GridManager;
    [SerializeField] private Image fog;


    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndDestructionOfTheSecondOne);
    }

    public void EventProbability()
    {
        int rndProb=UnityEngine.Random.Range(0, 101);
        if (rndProb > 80)
        {
            int eventChosen= UnityEngine.Random.Range(0, 3);
            if (eventChosen == 0)
            {
                ManageEvent = Event.Earthquake;
            }
            else if (eventChosen == 1)
            {
                ManageEvent= Event.Fog;
            }
            else
            {
                ManageEvent = Event.Blizzard;
            }
            return;
        }
        return;
    }

    public void EarthquakeEvent()
    {
        nbTurn--; ;
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
                _player1GridManager.totalUnitAmount -= 1;
                _player1GridManager.AllUnitPerColumn[u.tileX].Remove(u);
                Destroy(u.gameObject);
                u.StopAllCoroutines();
            }
            unitToRemove.Clear();

            foreach (Unit u in _player2GridManager.unitList.Where(a => a.state == 1))
            {
                unitToRemove.Add(u);  
            }
            foreach (Unit u in unitToRemove)
            {

                u.actualTile.unit = null;
                _player1GridManager.unitList.Remove(u);
                _player1GridManager.totalUnitAmount -= 1;
                _player1GridManager.AllUnitPerColumn[u.tileX].Remove(u);
                Destroy(u.gameObject);
                u.StopAllCoroutines();
            }
            _player1GridManager.AllUnitPerColumn = _player1GridManager.UnitPriorityCheck();
            _player2GridManager.AllUnitPerColumn = _player2GridManager.UnitPriorityCheck();

        }
        return;
    }
    public void FogEvent()
    {
        if (nbTurn <= 2)
        {
            nbTurn++;
        }
        if (nbTurn == 1)
        {
            fog.color = new Color(fog.color.r, fog.color.g, fog.color.b, 0.5f);
        }
        if (nbTurn == 2)
        {
            fog.color = new Color(fog.color.r, fog.color.g, fog.color.b, 1);
        }
        return;
    }

    public void BlizzardEvent()
    {
        nbTurn--; ;
        if (nbTurn < 0)
        {
            nbTurn = 5;
            int listOfIdle = _player1GridManager.unitList.Where(a => a.state == 0).Count();
            int UnitFrozen = 0;
            while (UnitFrozen <= listOfIdle)
            {
                foreach (Unit u in _player1GridManager.unitList.Where(a => a.state == 0))
                {
                    int spawnProbability = UnityEngine.Random.Range(0, 101);
                    if (UnitFrozen >= listOfIdle && spawnProbability <= 25)
                    {
                        u.state = 3;
                        UnitFrozen++;
                    }
                }
            }

            listOfIdle = _player2GridManager.unitList.Where(a => a.state == 0).Count();
            UnitFrozen = 0;
            while (UnitFrozen <= listOfIdle)
            {
                foreach (Unit u in _player2GridManager.unitList.Where(a => a.state == 0))
                {
                    int spawnProbability = UnityEngine.Random.Range(0, 101);
                    if (UnitFrozen <= listOfIdle && spawnProbability <= 25)
                    {
                        u.state = 3;
                        UnitFrozen++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        return;
    }
}
public class EventStocker
{
    private Action _eventToStore { get; set; }
    public Action StockFunction( S_WeatherEvent.Event p_wantedEvent, S_WeatherEvent p_classRef) 
    {
        if(p_wantedEvent==S_WeatherEvent.Event.Earthquake)
        {
            _eventToStore = p_classRef.EarthquakeEvent;
            return _eventToStore;
        }
        else if(p_wantedEvent == S_WeatherEvent.Event.Fog)
        {
            _eventToStore = p_classRef.FogEvent;
            return _eventToStore;
        }
        else if(p_wantedEvent == S_WeatherEvent.Event.Blizzard)
        {
            return _eventToStore;
        }
        Debug.LogError("the event given in parameter does not exist");
        return null;
    }
}
