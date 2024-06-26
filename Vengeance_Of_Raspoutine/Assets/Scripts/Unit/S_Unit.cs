using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Movements :")]
    public int speed;
    private bool _isMoving = false;
    public Vector3 _posToMove;

    [Header("References :")]
    public SO_Unit SO_Unit; //Unit.SO_Unit.
    public int attack;
    public int defense;
    public int state = 0;
    public int turnCharge;
    public List<Unit> actualFormation = null;
    public int formationIndex=0;
    //UnitDisplay
    public int unitColor;
    public Sprite unitSprite;
    public List<S_Tile> actualTile=new List<S_Tile>();
    public GameObject highlight;
    public S_GridManager grid;
    public GameObject statsCanvas;
    private S_GridManager enemyGrid;
    private S_UnitManager unitManager;
    private EventReference EventReference;

    public GameObject shadow;
    public GameObject freeze;
    public bool isChecked;
    public bool mustAttack = false;
    private bool _willLoseActionPoints = false;

    S_RemoveUnit _removeUnit;

    private void Awake()
    {
        int randomNumber = Random.Range(0, 1);
        unitColor = SO_Unit.unitColor;
        //gameObject.GetComponent<SpriteRenderer>().sprite = SO_Unit.unitSprite[randomNumber];
        //sizeX = SO_Unit.sizeX;
        //sizeY = SO_Unit.sizeY;
        //attack = SO_Unit.attack;
        //defense = SO_Unit.defense;
        //turnCharge = SO_Unit.unitTurnCharge;
        speed = 10;
    }

    private void Start()
    {
        _removeUnit = S_RemoveUnit.Instance;
        EventReference = FMODEvents.instance.Claws;
        if (SO_Unit.name == "Sniper" ||  SO_Unit.name == "Duelist")
        {
            EventReference = FMODEvents.instance.Impact;
        }
    }

    // destroy the formation in good order to avoid removing itself before the others

    /*This function is the lerp from one position to another
    it also handles the recursive parts of the attack of a formation alternating between moving to the position and
    calling attackAnotherUnit() to see the next position to go.*/
    private IEnumerator LerpMove()
    {
        float t = 0;
        float _distance = Vector3.Distance(transform.position, new Vector3(_posToMove.x, _posToMove.y, -1));

        if (!_isMoving)
        {
            _isMoving = true;
        }

        while (_distance >= 0.1f)
        {

            transform.position = Vector3.Lerp(transform.position, new Vector3(_posToMove.x, _posToMove.y, -1), t);

            // We re-calculate the distance
            _distance = Vector3.Distance(transform.position, new Vector3(_posToMove.x, _posToMove.y, -1));

            yield return new WaitForEndOfFrame();

            if (mustAttack)
            {
                // Speed for one frame divided by the distance left
                t = 2 * Time.deltaTime / _distance;
            }
            else
            {
                t = 5 * Time.deltaTime / _distance;
            }
            
        }
        _isMoving = false;

        transform.position = new Vector3(_posToMove.x, _posToMove.y, -1);

        if (mustAttack && actualFormation[actualFormation.Count - 1] == this)
        {
            AttackPlayer();
        }
    }

    //IS ABSOLUTELY NEEDED TO BE CALLED WHEN A UNIT IS INSTANTIATED
    //AND THE REFERENCE TO HIS OWN TILE IS KNOWN
    public void OnSpawn(S_Tile p_tile)
    {
        actualTile.Clear();
        grid = p_tile.grid;
        grid.unitList.Add(this);
        actualTile.Add(p_tile);
        unitManager = p_tile.grid.unitManager;
        enemyGrid = grid.enemyGrid;

        GetComponent<SpriteRenderer>().sprite = SO_Unit.unitSprite[unitColor];
    }

    public void DestroyFormation()
    {
        unitManager.UnitColumn.Remove(actualFormation); 
        foreach (Unit u in actualFormation)
        {
            if (u != this)
            {
                Destroy(u.gameObject);
                u.StopAllCoroutines();

            }
        }
        actualFormation.Clear();
        StopAllCoroutines();
        for (int i = 0; i < unitManager.UnitColumn.Count; i++)
        {
            if (unitManager.UnitColumn[i][grid.unitManager.UnitColumn[i][0].actualFormation.Count - 1].turnCharge <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }
        for (int i = 0; i < grid.enemyGrid.unitManager.UnitColumn.Count; i++)
        {
            if (grid.enemyGrid.unitManager.UnitColumn[i][grid.enemyGrid.unitManager.UnitColumn[i][0].actualFormation.Count - 1].turnCharge <= 0)
            {
                Destroy(gameObject);
                return;
            }
        }
        S_GameManager.Instance.EndTurn();
        Destroy(gameObject);
        

    }

    private void OnDestroy()
    {
        grid.totalUnitAmount -= 1;
        S_GameManager.Instance.player1UnitCall.CallAmountUpdate();
        S_GameManager.Instance.player2UnitCall.CallAmountUpdate();
    }
    //launch the attack of all formation and begin the recursion of the attack

    public void AttackCharge()
    {
        if (state == 2) turnCharge--;

        if (turnCharge <= 0)
        {
            if (actualFormation.Count > 0)
            {

                //remove virtually the units from their own grid and tile, they do not exists anymore for their grid and respective tiles.
                for (int i = 0; i < actualFormation.Count; i++)
                {
                    foreach (Unit u in actualFormation)
                    {
                        u.mustAttack = true;
                        u.turnCharge = 0;
                        grid.unitList.Remove(u);
                        u._posToMove = new Vector3(transform.position.x, grid.enemyGrid.gridList[actualTile[0].tileX][grid.enemyGrid.gridList[actualTile[0].tileX].Count-1].transform.position.y - transform.localScale.y, -1);
                        u.StartCoroutine(LerpMove());

                    }
                }
            }
        }
    }

    public void AttackPlayer()
    {
        ReducePlayerHp();
        DestroyFormation();
    }

    /* is called by the UnitManager, can be used to define what happens for a collidedUnit if units are kill by the enemy attack*/
    public void ReducePlayerHp(){ // needs rework

        AudioManager.instance.PlayOneShot(EventReference, Camera.main.transform.position);

        if (S_GameManager.Instance.isPlayer1Turn)
        {
            S_GameManager.Instance.player2CharacterHealth.currentHP -=  attack;
        }
        else
        {
            S_GameManager.Instance.player1CharacterHealth.currentHP -= attack;
        }
    }

    /* is called by the collidedUnit that killed it, can be used to check if units are kill by the enemy attack*/
    public void TakeDamage(int p_damage)
    {
        if (actualFormation.Count > 0)
        {
            attack -= p_damage;

            if (attack <= 0)
            {
                // Destruction of all the unit in formation except the leader of the formation (the one who is reading this code)
                for (int j = 0; j < actualFormation.Count; j++)
                {
                    if (actualFormation[j] != this)
                    {
                        _removeUnit.RemoveUnitOnSpecificTile(
                            actualFormation[j].actualTile[0],
                            S_UnitDestructionAnimationManager.UnitDestructionAnimationsEnum.Pak
                        );
                    }
                }

                // Destruction of the unit who leads the formation (the one who is reading this code)
                _removeUnit.RemoveUnitOnSpecificTile(
                    actualTile[0],
                    S_UnitDestructionAnimationManager.UnitDestructionAnimationsEnum.Pak
                );

                unitManager.UnitColumn.Remove(actualFormation);
            }
            return;
        }

        defense -= p_damage;

        if (defense <= 0)
        {
            if (S_GameManager.Instance.isPlayer1Turn)
            {
                S_GameManager.Instance.player1CharacterXP.GainXP(5);
            }
            else
            {
                S_GameManager.Instance.player2CharacterXP.GainXP(5);
            }

            _removeUnit.RemoveUnitOnSpecificTile(
                actualTile[0],
                S_UnitDestructionAnimationManager.UnitDestructionAnimationsEnum.Pak
            );
        }
    }

    // Same as MoveToTile but use a action point
    public void ActionMoveToTile(S_Tile p_tile)
    {
        S_Tile tempoTile = actualTile[0];
        MoveToTile(p_tile);
        if(tempoTile != actualTile[0])
        {
            _willLoseActionPoints = true;
        }
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(true, true);
        }
        else
        {
            S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(false, true);
        }
        grid.unitSelected = null;
        foreach (Unit unit in grid.unitList)
        {
            unit.GetComponent<BoxCollider2D>().enabled = true;
        }
        if (_willLoseActionPoints)
        {
            S_GameManager.Instance.ReduceActionPointBy1();
            _willLoseActionPoints = false;
        }
        highlight.SetActive(false);
    }
    /*Move the collidedUnit to the top of the row of collidedUnit corresponding at the tile clicked if possible
  then deselect the collidedUnit*/
    public void MoveToTile(S_Tile p_tile)
    {
        actualTile.Clear();
        foreach (S_Tile tile in grid.gridList[p_tile.tileX])
        {
            if (!grid.TryFindUnitOntile(tile, out var unit))
            {
                actualTile.Add(tile);
                _posToMove = tile.transform.position;
                StartCoroutine(LerpMove());
                return;
            }
            else
            {
                if (unit == this)
                {
                    actualTile.Add(tile);
                    _posToMove = tile.transform.position;
                    StartCoroutine(LerpMove());
                }
            }
        }
    }
    //Used to reorganize the Units by state
    public void SwitchUnit(S_Tile p_tile)
    {
        foreach (S_Tile tile in grid.gridList[p_tile.tileX])
        {
            if (!grid.TryFindUnitOntile(tile, out var unit))
            {
                actualTile.Add(tile);
                _posToMove = tile.transform.position;
                StartCoroutine(LerpMove());
                return;
            }
        }

    }

    //get the last collidedUnit of the row corresponding to the tile clicked
    public void SelectUnit()
    {
        if (S_GameManager.Instance.isPlayer1Turn)
        {
            S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(true, false);
        }
        else
        {
            S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(false, false);
        }

        if (!grid.isSwapping)
        {
            grid.unitSelected = this;
            foreach (S_Tile tile in grid.gridList[actualTile[0].tileX].Where(x => x.tileX > actualTile[0].tileX))
            {
                if (grid.TryFindUnitOntile(tile, out var unit))
                {
                    grid.unitSelected = unit;
                    AudioManager.instance.PlayOneShot(FMODEvents.instance.UnitDeployed, Camera.main.transform.position);
                }
            }
            foreach (Unit unit in grid.unitList)
            {
                unit.GetComponent<BoxCollider2D>().enabled = false;
            }
            shadow.SetActive(true);

            return;
        }
        {
            if (grid.unitSelected == null)
            {
                grid.unitSelected = this;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.UnitDeployed, Camera.main.transform.position);
            }
            else if (grid.unitSelected != this)
            {
                grid.SwapUnits(grid.unitSelected.actualTile[0], this.actualTile[0], grid.unitSelected, this);
                _posToMove = actualTile[0].transform.position;
                grid.unitSelected._posToMove = grid.unitSelected.actualTile[0].transform.position;
                StartCoroutine(LerpMove());
                StartCoroutine(grid.unitSelected.LerpMove());
                S_UnitCallButtonHandler.Instance.HandleUnitCallButtonInteraction(S_GameManager.Instance.isPlayer1Turn, true);
                grid.unitSelected.highlight.SetActive(false);
                grid.unitSelected = null;
                S_GameManager.Instance.ReduceSwapCounter(S_GameManager.Instance.isPlayer1Turn);
                if (S_GameManager.Instance.isPlayer1Turn)
                {
                    S_SwapButtonsHandler.Instance.HandleSwapUnitButtonEffects(true, false);
                    S_SwapButtonsHandler.Instance.player1ButtonText.text = S_GameManager.Instance.swapCounterP1.ToString();
                    if (S_GameManager.Instance.swapCounterP1 == 0)
                    {
                        S_SwapButtonsHandler.Instance.player1SwapButton.interactable = false;
                    }
                }
                else
                {
                    S_SwapButtonsHandler.Instance.HandleSwapUnitButtonEffects(false, false);
                    S_SwapButtonsHandler.Instance.player2ButtonText.text = S_GameManager.Instance.swapCounterP2.ToString();
                    if (S_GameManager.Instance.swapCounterP2 == 0)
                    {
                        S_SwapButtonsHandler.Instance.player2SwapButton.interactable = false;
                    }
                }
                S_GameManager.Instance.ReduceActionPointBy1();
            }
        }
    }

    public bool CheckUnitInProximity(out Unit p_unit,int p_x, int p_y = 0)
    {
        p_unit = null;
        if(grid.IsOutOfIndex(actualTile[0].tileX + p_x, actualTile[0].tileY + p_y))
        {
            return false;
        }
        if(grid.TryFindUnitOntile(grid.gridList[actualTile[0].tileX + p_x][actualTile[0].tileY + p_y],out var unit))
        {
            p_unit=unit;
            return true;
        }
        else
        {
            return false;
        }
    }

    //Align the Unit with the collumn overed by the mouse to previsualize where you're aiming
    public void VisualizePosition(S_Tile p_tile)
    {
        if (!grid.isSwapping)
        {
            foreach (S_Tile tile in grid.gridList[p_tile.tileX])
            {
                if (!grid.TryFindUnitOntile(tile, out var unit))
                {
                    shadow.transform.position = tile.transform.position;
                    return;
                }
            }
            shadow.transform.position = new Vector3(p_tile.transform.position.x, grid.gridList[p_tile.tileX][Mathf.Abs(grid.height)-1].transform.position.y - transform.localScale.y,-1);

        }
        
    }

    public bool GetIsMoving()
    {
        return _isMoving;
    }

    public void ReturnToBaseTile()
    {
        MoveToTile(actualTile[0]);
    }

    //change sprite of the collidedUnit to the wall png
    public void spriteChange(Sprite img)
    {
        transform.GetComponent<SpriteRenderer>().sprite = img;
    }

    public void ResetBuffs()
    {
        attack = SO_Unit.attack;
        defense = SO_Unit.defense;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Unit collidedUnit = collision.gameObject.GetComponent<Unit>();

        if (collidedUnit.grid != grid)
        {
            if (!collidedUnit.mustAttack)
            {
                // If the collidedUnit is a wall or an idle unit
                if (collidedUnit.actualFormation.Count <= 0)
                {
                    attack -= collidedUnit.defense;
                    collidedUnit.TakeDamage(attack + collidedUnit.defense);
                }
                // If the collidedUnit is in enemy charging formation
                else if (collidedUnit.turnCharge > 0)
                {
                    attack -= collidedUnit.attack;
                    collidedUnit.TakeDamage(attack + collidedUnit.attack);
                }

                // If the first collidedUnit of the attacking formation (the one that collides) has no more attacks point then we destroy the formation
                if (attack <= 0)
                {
                    actualFormation[actualFormation.Count - 1].DestroyFormation();
                }
            }

        }
    }
    private void OnMouseOver()
    {
        if (S_GameManager.Instance.currentTurn != S_GameManager.TurnEmun.TransitionTurn)
        {
            highlight.SetActive(true);
            _removeUnit.hoveringUnit = this;
        }
    }
    private void OnMouseExit()
    {
        highlight.SetActive(false);
        _removeUnit.hoveringUnit = null;
        highlight.SetActive(grid.isSwapping && this == grid.unitSelected);
    }
    private void OnMouseDown()
    {
        if (!grid.isSwapping)
        {
            if (grid.unitSelected == null && state == 0 && S_GameManager.Instance.currentTurn != S_GameManager.TurnEmun.TransitionTurn)
            {
                SelectUnit();
            }
        }
        else
        {
            if (state == 0 && S_GameManager.Instance.currentTurn != S_GameManager.TurnEmun.TransitionTurn)
            {
                SelectUnit();
            }
        }
    }
}
