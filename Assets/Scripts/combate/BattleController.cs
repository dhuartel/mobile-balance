using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class BattleController : MonoBehaviour
{
    public enum generalBattleState { STARTING = 0, NONE}
    public enum BattleStates { PLAYERTURN = 0, SELECTATTACKOBJECTIVE, ATTACKING, SELECTABILITYTYPE, SELECTABILITY, SELECTABILITYOBJECTIVE, ABILITYINPROCESS, DEFEND, ENEMYTURN, WAITING }

    private generalBattleState currentGeneralState;
    private BattleStates currentBattleState;
    public BattleUI bInput;

    public UseOfAbilities useOfAbilities;

    public EnemiesController enemies;
    public Controller controller;

    public List<GameObject> players;
    public List<GameObject> playersAlive;
    private int targetPlayer;

    private bool offensiveAbility;
    private bool globalAbility;
    
    private float time = 0;

    public struct InitiativeStruct
    {
        public int initiative;
        public GameObject character;
    }

    public InitiativeStruct[] battleOrder;
    private int abilityBranchSelected;
    private int abilitySelected;
    public int enemyAbility { get; set; }

    BattleCalculator calculator;

    private AIController currentEnemy;

    public int attackFinished { get; set; }
    private bool startAttack;

    private bool attack = false;

    public GameObject exit;


    public GameObject loseWinText;


    void OnEnable()
    {
        useOfAbilities = gameObject.GetComponent<UseOfAbilities>();

        currentGeneralState = generalBattleState.STARTING;
        calculator = new BattleCalculator();
        //marker = GameObject.Find("Marker");
        attackFinished = 0;
        startAttack = false;
    }

    void Update()
    {
        switch (currentGeneralState)
        {
            case generalBattleState.STARTING:
                loseWinText.SetActive(false);
                FillBattleOrder();
                bInput.ChangeButtonsText(1);
                exit.SetActive(false);
                currentGeneralState = generalBattleState.NONE;
                break;
            case generalBattleState.NONE:
                switch (currentBattleState)
                {
                    case BattleStates.PLAYERTURN:
                        break;
                    case BattleStates.SELECTATTACKOBJECTIVE:
                        attack = true;
                        break;
                    case BattleStates.ATTACKING:
                            ManageAttacking();
                        break;
                    case BattleStates.SELECTABILITYTYPE:
                        break;
                    case BattleStates.SELECTABILITY:
                        break;
                    case BattleStates.SELECTABILITYOBJECTIVE:
                        break;
                    case BattleStates.ABILITYINPROCESS:
                        doAbilityP();
                        break;
                    case BattleStates.DEFEND:
                        ManageDefend();
                        break;
                    case BattleStates.ENEMYTURN:
                        if (currentEnemy.Run())
                        {
                            currentEnemy.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
                            EndTurn(false);
                        }
                        break;
                    case BattleStates.WAITING:
                        break;
                }
                break;
        }
    }


    private void ManageDefend()
    {
        attackFinished = 1;
        GetCurrentCharacter().defendChar();
        currentBattleState = BattleStates.WAITING;
        EndTurn(false, true);
    }

    private void RevivePlayers()
    {
        for (int i = 0; i < players.Count; ++i)
        {
            {
                players[i].GetComponent<Stats>().Revive();//We have to decide how to manage dead players
            }
        }
    }

    private void ManageSelectAbilityType()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            currentBattleState = BattleStates.PLAYERTURN;
            bInput.ChangeButtonsText(1);
            bInput.ChangeState(BattleUI.JoystickInputStates.ACTIONS);
        }
    }

    private void ManageSelectAbility()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            currentBattleState = BattleStates.SELECTABILITYTYPE;
            bInput.ChangeButtonsText(2);
            bInput.ChangeState(BattleUI.JoystickInputStates.ABILITIES);
        }
    }

    private void ManageSelectAbilityObjective()
    {
        if (globalAbility)
        {
            currentBattleState = BattleStates.ABILITYINPROCESS;
        }
        else
        {
            if (offensiveAbility)
            {
                //ManageSelectEnemyObjective(true);
            }
            else
            {
                //ManageSelectPlayerObjective(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            attackFinished = 0;
            currentBattleState = BattleStates.SELECTABILITY;
            switch (abilityBranchSelected)
            {
                case 1:
                    bInput.ChangeButtonsText(3);
                    bInput.ChangeState(BattleUI.JoystickInputStates.AIR);
                    break;
                case 2:
                    bInput.ChangeButtonsText(4);
                    bInput.ChangeState(BattleUI.JoystickInputStates.FIRE);
                    break;
                case 3:
                    bInput.ChangeButtonsText(5);
                    bInput.ChangeState(BattleUI.JoystickInputStates.WATER);
                    break;
                case 4:
                    bInput.ChangeButtonsText(6);
                    bInput.ChangeState(BattleUI.JoystickInputStates.EARTH);
                    break;
            }
            abilitySelected = 0;
        }
    }

    private void ManageSelectAttackObjective()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            bInput.ChangeButtonsText(1);
            bInput.ChangeState(BattleUI.JoystickInputStates.ACTIONS);
            //enemies.Unmark();
            currentBattleState = BattleStates.PLAYERTURN;
        }
    }


    private void ManageAttacking()
    {
        currentBattleState = BattleStates.WAITING;
        attackFinished = 1;
        GameObject target = enemies.GetObjective();
        doAttackPE(battleOrder[0].character.GetComponent<Stats>(), target.GetComponent<Stats>());
        EndTurn(true);
    }

    private void ManageAbilityInProcess()
    {
        doAbilityP();
    }
    

    public void EndTurn(bool meleeAttack, bool defend = false)
    {
        battleOrder[0].character.GetComponent<Stats>().SetInitiatives(false);
        if (enemies.enemiesInBattle.Count == 0)
        {
            loseWinText.SetActive(true);
            loseWinText.GetComponent<Text>().text = "You Win";
            currentBattleState = BattleStates.WAITING;
            EndBattle();
        }
        else if (playersAlive.Count == 0)
        {
            loseWinText.SetActive(true);
            loseWinText.GetComponent<Text>().text = "You lose";
            currentBattleState = BattleStates.WAITING;
            EndBattle();
        }
        else
        {
            if (battleOrder[0].character.GetComponent<Stats>().enemy)
            {
                bInput.ChangeButtonsText(0);
                bInput.ChangeState(BattleUI.JoystickInputStates.NONE);
                currentBattleState = BattleStates.ENEMYTURN;
                GetCurrentCharacter().BeginTurn();
                currentEnemy = battleOrder[0].character.GetComponent<AIController>();
            }
            else
            {
                bInput.ChangeButtonsText(1);
                bInput.ChangeState(BattleUI.JoystickInputStates.ACTIONS);
                GetCurrentCharacter().BeginTurn();
                currentBattleState = BattleStates.PLAYERTURN;
                
            }
        }
    }

    private void EndBattle()
    {
        players.Clear();
        playersAlive.Clear();
        enemies.EndOfBattle();
        exit.SetActive(true);
        //Aqui saldre
    }

    public void ExitBatlle()
    {
        controller.EndBattle();
    }

    public void doAttackPE(Stats player, Stats enemy)
    {
        bool crit = player.getStat(Stats.STATTYPE.CRIT) >= UnityEngine.Random.Range(0.0f, 100.0f);
        calculator.processAttackPE(player, enemy, crit);

        bInput.UpdateHPMP();

    }

    public void doAttackEP(Stats enemy, Stats player)
    {
        calculator.processAttackEP(enemy, player);


        bInput.UpdateHPMP();
    }
    

    public void doAbilityP()
    {

        useOfAbilities.CheckUseOfAbilityP((abilityBranchSelected - 1) * 7 + abilitySelected);
        bInput.UpdateHPMP();
        //EndTurn(false);
    }

    public void doAbilityE(GameObject objective)
    {

        useOfAbilities.CheckUseOfAbilityE(enemyAbility, objective);
        //StartCoroutine(EndTurn(false));
        bInput.UpdateHPMP();
    }

    public void AddPlayer(GameObject player)
    {
        players.Add(player);
        playersAlive.Add(player);
    }

    public void DeletePlayer(GameObject deadPlayer)
    {
        playersAlive.Remove(deadPlayer);
        FillBattleOrder();
    }

    public Stats GetCurrentCharacter()
    {
        return battleOrder[0].character.GetComponent<Stats>();
    }

    public Stats GetCurrentEnemy()
    {
        Debug.Log(battleOrder[0].character.name);
        return battleOrder[0].character.GetComponent<Stats>();
    }

    public GameObject GetTargetPlayer()
    {
        return players[targetPlayer];
    }

    public void FillBattleOrder()
    {
        battleOrder = new InitiativeStruct[(enemies.enemiesInBattle.Count + playersAlive.Count) * 3];
        for (int i = 0; i < playersAlive.Count; i++)
        {
            battleOrder[i * 3].initiative = playersAlive[i].GetComponent<Stats>().initiative1;
            battleOrder[i * 3].character = playersAlive[i];
            battleOrder[i * 3 + 1].initiative = playersAlive[i].GetComponent<Stats>().initiative2;
            battleOrder[i * 3 + 1].character = playersAlive[i];
            battleOrder[i * 3 + 2].initiative = playersAlive[i].GetComponent<Stats>().initiative3;
            battleOrder[i * 3 + 2].character = playersAlive[i];
        }
        for (int i = 0; i < enemies.enemiesInBattle.Count; i++)
        {
            battleOrder[(i + playersAlive.Count) * 3].initiative = enemies.enemiesInBattle[i].GetComponent<Stats>().initiative1;
            battleOrder[(i + playersAlive.Count) * 3].character = enemies.enemiesInBattle[i];
            battleOrder[(i + playersAlive.Count) * 3 + 1].initiative = enemies.enemiesInBattle[i].GetComponent<Stats>().initiative2;
            battleOrder[(i + playersAlive.Count) * 3 + 1].character = enemies.enemiesInBattle[i];
            battleOrder[(i + playersAlive.Count) * 3 + 2].initiative = enemies.enemiesInBattle[i].GetComponent<Stats>().initiative3;
            battleOrder[(i + playersAlive.Count) * 3 + 2].character = enemies.enemiesInBattle[i];
        }
        SortInitiatives();
    }

    public void AddNewInitiative(int initiative, GameObject character)
    {
        battleOrder[0].initiative = initiative;
        battleOrder[0].character = character;
        SortInitiatives();
    }

    public void SortInitiatives()
    {
        for (int i = 0; i < battleOrder.Length; i++)
        {
            int j = i;
            while (j > 0 && battleOrder[j - 1].initiative > battleOrder[j].initiative)
            {
                InitiativeStruct aux = battleOrder[j - 1];
                battleOrder[j - 1] = battleOrder[j];
                battleOrder[j] = aux;
                j = j - 1;
            }
        }
    }

    public void ShowTypeAbilities(int type)
    {
        abilityBranchSelected = type;
        switch (type)
        {
            case 1:
                bInput.ChangeButtonsText(3);
                bInput.ChangeState(BattleUI.JoystickInputStates.AIR);
                break;
            case 2:
                bInput.ChangeButtonsText(4);
                bInput.ChangeState(BattleUI.JoystickInputStates.FIRE);
                break;
            case 3:
                bInput.ChangeButtonsText(5);
                bInput.ChangeState(BattleUI.JoystickInputStates.WATER);
                break;
            case 4:
                bInput.ChangeButtonsText(6);
                bInput.ChangeState(BattleUI.JoystickInputStates.EARTH);
                break;
        }
        currentBattleState = BattleStates.SELECTABILITY;
    }

    public void SelectAbility(int abilityNumber)
    {
        abilitySelected = abilityNumber;

        switch ((abilityBranchSelected - 1) * 7 + abilitySelected)
        {
            case 1://Thunder
                    enemies.MarkEnemy(0);
                offensiveAbility = true;
                globalAbility = false;
                break;
            case 2://Electric Field
                offensiveAbility = false;
                globalAbility = false;
                break;
            case 3://Gale
                    enemies.MarkEnemy(0);
                offensiveAbility = true;
                globalAbility = false;
                break;
            case 4://Teleport(Haste)
                offensiveAbility = false;
                globalAbility = true;
                break;
            case 8://FireBall
                    enemies.MarkEnemy(0);
                offensiveAbility = true;
                globalAbility = false;
                break;
            case 9://Lava mine
                    enemies.MarkEnemy(0);
                offensiveAbility = true;
                globalAbility = false;
                break;
            case 10://Meteor rain
                offensiveAbility = true;
                globalAbility = true;
                break;
            case 11://Berserk
                offensiveAbility = false;
                globalAbility = false;
                break;
            case 15://Healing drop
                offensiveAbility = false;
                globalAbility = false;
                break;
            case 16://Ice ball
                    enemies.MarkEnemy(0);
                offensiveAbility = true;
                globalAbility = false;
                break;
            case 17://Healing rain
                offensiveAbility = false;
                globalAbility = true;
                break;
            case 18://Tsunami
                offensiveAbility = true;
                globalAbility = true;
                break;
            case 22://Roots
                    enemies.MarkEnemy(0);
                offensiveAbility = true;
                globalAbility = false;
                break;
            case 23://Earth armor
                offensiveAbility = false;
                globalAbility = false;
                break;
            case 24://Earthquake
                offensiveAbility = true;
                globalAbility = true;
                break;
            case 25://Ivy(To be seen)
                    enemies.MarkEnemy(0);
                offensiveAbility = true;
                globalAbility = false;
                break;
        }
        //bInput.ChangeButtonsText(0);
        bInput.ChangeState(BattleUI.JoystickInputStates.NONE);
        currentBattleState = BattleStates.SELECTABILITYOBJECTIVE;
    }
    private bool isEnemyTurn()
    {
        return battleOrder[0].character.CompareTag("Enemy");
    }
    
    

    public void SelectBattleAction(int action)
    {
        if (currentGeneralState == generalBattleState.NONE)
        {
            if (Time.time > time + 0.25f)
            {
                time = Time.time;
                switch (currentBattleState)
                {
                    case BattleStates.PLAYERTURN:
                        switch (action)
                        {
                            case 1:
                                time = Time.time;
                                //bInput.ChangeButtonsText(0);
                                bInput.ChangeState(BattleUI.JoystickInputStates.NONE);
                                currentBattleState = BattleStates.SELECTATTACKOBJECTIVE;
                                //bInput.HighlightSelected(0);
                                break;
                            case 2:
                                currentBattleState = BattleStates.SELECTABILITYTYPE;
                                bInput.ChangeButtonsText(2);
                                bInput.ChangeState(BattleUI.JoystickInputStates.ABILITIES);
                                break;
                            case 3:
                                currentBattleState = BattleStates.DEFEND;
                                break;
                            case 4:
                                enemies.EndOfBattle();
                                RevivePlayers();
                                break;
                        }
                        break;
                    case BattleStates.SELECTABILITYTYPE:
                        ShowTypeAbilities(action);
                        currentBattleState = BattleStates.SELECTABILITY;
                        break;
                    case BattleStates.SELECTABILITY:
                        SelectAbility(action);
                        //currentBattleState = BattleStates.SELECTABILITYOBJECTIVE;
                        break;
                }
            }
        }
    }

    public void SelectPlayerObjective(int objective)
    {
        targetPlayer = objective;
    }

    public void SelectObjectiveEnemy(int objective)
    {
        enemies.SelectObjective(objective);
        if (attack)
            currentBattleState = BattleStates.ATTACKING;
        else
            currentBattleState = BattleStates.ABILITYINPROCESS;
    }
}