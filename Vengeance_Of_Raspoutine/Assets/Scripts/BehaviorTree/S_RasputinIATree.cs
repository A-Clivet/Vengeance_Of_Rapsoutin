using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;

public class S_RasputinIATree : BehaviorTree.Tree
{
    public S_GameManager gameManager;
    public S_UnitManager unitManager;
    public S_GridManager gridManager;
    public S_UnitCall unitCall;
    public S_UnitCall unit;
    public S_RemoveUnit removeUnit;
    public S_SpecialCapacityManager abilityManager;
    public S_SpecialCapacityStats abilityStats;
    public S_CharacterAdrenaline characterAdrenaline;
    public List<S_Tile> m_Path;

    protected override Node SetupTree()
    {
        Node root = new S_Selector(new List<Node>
        {
            //Allows the AI to check if it can do a combo with the units and if it can it will remove or move a unit
            new S_Sequencer(new List<Node>
                {
                new S_CheckCanComboNode(unitManager),
                new S_CheckLoneUnit(gridManager),
                new S_Selector(new List<Node>
                {
                    new S_ShouldMoveUnit(gridManager),
                    new S_ShouldRemoveUnit(gridManager, removeUnit),
                }),
                }
            ),

            //Allows the AI to call in units and allows to check if there is enough units in its reserve
            new S_Sequencer(new List<Node>
                {
                    new S_CheckNumberUnit(unitCall),
                    new S_UseCallUnit(unitCall),
                }
            ),

            //Allows the AI to check if it can use the ability and allows it to use it or not
            new S_Sequencer(
                new List<Node>
                {
                    new S_CanUseAbility(characterAdrenaline),
                    new S_EnoughAttackingUnitRasputin(unitManager),
                    new S_UseAbility(abilityManager, abilityStats),
                }),

            //Skips the turn
            new S_SkipTurn(gameManager),
        }
        );

        return root;
    }
}
