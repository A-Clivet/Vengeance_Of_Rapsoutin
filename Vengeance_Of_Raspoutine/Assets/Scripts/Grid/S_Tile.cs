using UnityEngine;

public class S_Tile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public Unit unit;
    public S_GridManager grid;

    //get the parent gridManager for future uses
    private void Awake()
    {
        grid=transform.parent.gameObject.GetComponent<S_GridManager>();
    }
    private void OnMouseOver()
    {
        if (grid.unitSelected != null)
        {
            grid.unitSelected.VisualizePosition(this);
        }
    }
    //Move the unit if one is selected
    private void OnMouseDown()
    {
        if (grid.unitSelected != null && S_GameManager.Instance.currentTurn!=S_GameManager.TurnEmun.TransitionTurn)
        {
            if ((grid.gridList[tileX][Mathf.Abs(grid.height) - grid.unitSelected.sizeY].unit != null) /*|| (grid.gridList[p_tile.tileX][4] != this && sizeY == 2)*/)
            {
                return;
            }
            grid.unitSelected.ActionMoveToTile(grid.gridList[tileX][Mathf.Abs(grid.height) - grid.unitSelected.sizeY]);
        }
    }
}
