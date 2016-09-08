using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {
    
	public Stats selectedObjective { get; set; }

    public enum Type { ROBOT, ELEMENTAL }
    public Type type;
    
    private int objectiveSelected = 0;

	private enum state_machine {SEARCH, MOVE, ATTACK, MOVE_BACK, END }
    private enum enemy_selection_type { HEALTH, RANDOM, ALL }
    private enum enemy_attack_type { PHYSIC, MAGIC }
    private bool moveing = false;
    private bool attackAll = false;

    private state_machine state;
	private bool blockMove;

	public bool stun {set; get;}

	private EnemiesController.enemyTypes AItype;
    
	private Stats stats;

    private enemy_attack_type attack_type;

    public BattleController battle;

    void Awake(){
		blockMove = false;
	}

	void Start(){
		//move.Init (LerpIncrement, LerpRotation, this);

		//attack.Init (this);

		stats = GetComponent<Stats> ();
		AItype = stats.type; 
		state = state_machine.END;
    }

    void Update()
    {
    }
    

    public bool Run(){
		if (state == state_machine.END) {
			StartIA ();
		}
		else EvaluateState ();

		return state == state_machine.END;
	}

	void StartIA(){
		state = state_machine.SEARCH;
		//move.Start();
		//attack.Start();
	}

	void EvaluateState(){
		switch (state) {
			case state_machine.SEARCH:
				SelectObjective ();
                state = state_machine.MOVE;
				break;
			case state_machine.MOVE:
					state = state_machine.ATTACK;
				break;
			case state_machine.ATTACK:
                Attack();
				break;
		}
	}

	void SelectObjective(){
        attackAll = false;
		List<GameObject> playersObj = battle.playersAlive;
		List<Stats> players = new List<Stats> ();
		for (int i = 0; i < playersObj.Count; ++i)  players.Add(playersObj[i].GetComponent<Stats>());

		float rand = Random.value;
		enemy_selection_type selection_type = enemy_selection_type.HEALTH; 

        switch (AItype)
        {
            case EnemiesController.enemyTypes.DPS:
                if (rand < 0.6f)
                    selection_type = enemy_selection_type.HEALTH;
                else
                    selection_type = enemy_selection_type.RANDOM;
                break;
            case EnemiesController.enemyTypes.TANK:
                if (rand < 0.15f)
                    selection_type = enemy_selection_type.HEALTH;
                else
                    selection_type = enemy_selection_type.RANDOM;
                break;
            case EnemiesController.enemyTypes.ASSASIN:
                if (rand < 0.8f)
                    selection_type = enemy_selection_type.HEALTH;
                else
                    selection_type = enemy_selection_type.RANDOM;
                break;
            case EnemiesController.enemyTypes.MAGE:
                if (rand < 0.6f)
                    selection_type = enemy_selection_type.HEALTH;
                else
                    selection_type = enemy_selection_type.RANDOM;
                break;
            case EnemiesController.enemyTypes.BOSS:
                if (rand < 0.3f)//0.3
                    selection_type = enemy_selection_type.ALL;
                else if(rand < 0.8f)//0.8
                    selection_type = enemy_selection_type.RANDOM;
                else
                    selection_type = enemy_selection_type.HEALTH;
                break;

        }

		switch (selection_type) {
			case enemy_selection_type.HEALTH:
				{
					float playerHealth = float.MaxValue; 
					objectiveSelected = 0;
					for (int i = 0; i < players.Count; ++i) {

						if (players[i].HP < playerHealth){
							playerHealth = players [i].HP;
                            objectiveSelected = i;
						}

					}
				}
				break;
			case enemy_selection_type.RANDOM:
				{
					float selected_enemy = Random.value * players.Count;
                    objectiveSelected = 1;
					bool find = false;
					while (objectiveSelected <= players.Count && !find){
					if (selected_enemy < objectiveSelected)
						find = true;
					else
                            objectiveSelected++;
					}
					--objectiveSelected;
				}
				break;
            case enemy_selection_type.ALL:
                attackAll = true;
                break;
		}
        
        selectedObjective = players [objectiveSelected];

        attack_type = enemy_attack_type.PHYSIC;
        rand = Random.value;
        switch (AItype)
        {
            case EnemiesController.enemyTypes.DPS:
                if (rand < 0.006f)
                    attack_type = enemy_attack_type.PHYSIC;
                else
                    attack_type = enemy_attack_type.MAGIC;
                break;
            case EnemiesController.enemyTypes.TANK:
                if (rand < 0.85f)
                    attack_type = enemy_attack_type.PHYSIC;
                else
                    attack_type = enemy_attack_type.MAGIC;
                break;
            case EnemiesController.enemyTypes.ASSASIN:
                if (rand < 0.6f)
                    attack_type = enemy_attack_type.PHYSIC;
                else
                    attack_type = enemy_attack_type.MAGIC;
                break;
            case EnemiesController.enemyTypes.MAGE:
                if (rand < 0.15f)
                    attack_type = enemy_attack_type.PHYSIC;
                else
                    attack_type = enemy_attack_type.MAGIC;
                break;
            case EnemiesController.enemyTypes.BOSS:
                if (rand < 0.5f)
                    attack_type = enemy_attack_type.PHYSIC;
                else
                    attack_type = enemy_attack_type.MAGIC;
                break;
        }
    }

	private bool Move(){
        return true;
	}

	private bool MoveBack(){
        return true;
	}
    

	private void Attack(){
        switch (stats.element)
        {
            case EnemiesController.enemyElements.AIR:
                battle.enemyAbility = 1;
                if (attackAll)
                    battle.enemyAbility = 26;
                break;
            case EnemiesController.enemyElements.FIRE:
                battle.enemyAbility = 8;
                if (attackAll)
                    battle.enemyAbility = 9;
                break;
            case EnemiesController.enemyElements.WATER:
                battle.enemyAbility = 17;
                if (attackAll)
                    battle.enemyAbility = 18;
                break;
            case EnemiesController.enemyElements.EARTH:
                battle.enemyAbility = 25;
                if (attackAll)
                    battle.enemyAbility = 24;
                break;
        }
        if (attack_type == enemy_attack_type.MAGIC) {
  //          Debug.Log("Habilidad");
            battle.doAbilityE(selectedObjective.gameObject);
        }
        else
        {
//            Debug.Log("Fisico");
            if (attackAll)
            {
                for (int i = 0; i < battle.playersAlive.Count; i++)
                {
                    battle.doAttackEP(stats, battle.playersAlive[i].GetComponent<Stats>());
                }
            }
            else
            {
                battle.doAttackEP(stats, selectedObjective);
            }
        }
        state = state_machine.END;
    }


    private int CheckAttackType()
    {
        if(attack_type== enemy_attack_type.PHYSIC)
        {
            if (!attackAll)
                return 1;
            else
                return 2;
        }
        else
        {
            if (!attackAll)
                return 3;
            else
                return 4;
        }
    }
    
}