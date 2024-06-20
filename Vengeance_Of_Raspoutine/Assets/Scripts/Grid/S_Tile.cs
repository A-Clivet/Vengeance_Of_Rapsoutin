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
            grid.unitSelected._shadow.SetActive(false);
            grid.unitSelected.ActionMoveToTile(this);

            
        }
    }
}
