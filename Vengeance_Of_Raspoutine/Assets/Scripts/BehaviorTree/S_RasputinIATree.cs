using S_BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class S_RasputinIATree : S_BehaviorTree.S_Tree
{
    public static S_RasputinIATree Instance;

    public S_GameManager gameManager;
    public S_UnitManager unitManager;
    public S_GridManager gridManager;
    public S_UnitCall unitCall;
    public S_RemoveUnit removeUnit;

    protected S_SpecialCapacityManager pr_abilityManager;
    protected S_SpecialCapacityStats pr_abilityStats;
    protected S_CharacterAdrenaline pr_characterAdrenaline;

    private GameObject _player2CharacterGameObject;

    private void Awake()
    {
        Instance = S_Instantiator.Instance.ReturnInstance(this, Instance, S_Instantiator.InstanceConflictResolutions.WarningAndPause);
    }

    protected override Node SetupTree()
    {
        _player2CharacterGameObject = S_CharacterManager.Instance.player2CharacterGameObject;

        pr_abilityManager = _player2CharacterGameObject.GetComponent<S_SpecialCapacityManager>();
        pr_characterAdrenaline = _player2CharacterGameObject.GetComponent<S_CharacterAdrenaline>();
        pr_abilityStats = pr_characterAdrenaline.specialCapacity;


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
                    new S_CanUseAbility(pr_characterAdrenaline),
                    new S_EnoughAttackingUnitRasputin(unitManager),
                    new S_UseAbility(pr_abilityManager, pr_abilityStats),
                }),

            //Skips the turn
            new S_SkipTurn(gameManager),
        }
        );

        StartCoroutine(CooldownPerAction());
        return root;
    }

    public IEnumerator CooldownPerAction()
    {
        print("Waiting amongus");
        yield return new WaitForSeconds(2);
    }
}