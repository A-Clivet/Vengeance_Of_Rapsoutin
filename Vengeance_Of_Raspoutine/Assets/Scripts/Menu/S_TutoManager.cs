using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_TutoManager : MonoBehaviour
{

    [HideInInspector] public int intTuto;
    private Canvas _currentTuto;
    private int _heightMaxListTuto;

    public GameObject buttonPrevious;
    public GameObject buttonNext;
    public List<Canvas> tutoSelection = new(new Canvas[10]);
    [SerializeField] private Canvas _currentSprite;

    public static S_TutoManager Instance;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.Warning);
    }

    private void Start()//Settings variables
    {
        DesactivateButtonTuto();
        intTuto = 0;
        ResetTutoOrder(intTuto);
        _heightMaxListTuto = tutoSelection.Count;
    }

    /// <summary> Function to display the next tutorial image. </summary>
    public void NextTutoButton()
    {
        if (intTuto >= 0 && intTuto < _heightMaxListTuto - 1)
        {
            intTuto += 1;
            ResetTutoOrder(intTuto);
            DesactivateButtonTuto();
        }
    }

    /// <summary> Function to display previous tutorial image. </summary>
    public void PreviousTutoButton()
    {
        if (intTuto > 0 && intTuto <= _heightMaxListTuto - 1)
        {
            intTuto -= 1;
            ResetTutoOrder(intTuto);
            DesactivateButtonTuto();
        }
    }

    /// <summary> Function in which the previousTuto button is disabled when the first img is reached or the nextTuto button is disabled when the last img is reached. </summary>
   public void DesactivateButtonTuto()
    {
        if (intTuto >= 1)
        {
            buttonPrevious.SetActive(true);
        }
        else if (intTuto <= 0)
        {
            buttonPrevious.SetActive(false);
        }

        if (intTuto == _heightMaxListTuto - 1)
        {
            buttonNext.SetActive(false);
        }
        else if (intTuto < 9)
        {
            buttonNext.SetActive(true);
        }
    }

    public void ResetTutoOrder(int x)  // set the current tuto on start and set the current sprite on start
    {
        if (x > tutoSelection.Count || x < 0)
        {
            return;
        }
        _currentTuto = tutoSelection[x];
        _currentSprite = tutoSelection[x];   
    }
}
