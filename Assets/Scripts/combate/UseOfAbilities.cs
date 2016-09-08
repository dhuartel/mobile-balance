using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UseOfAbilities : MonoBehaviour {

    /*public GameObject thunderPrefab;
    public AudioClip thunderSound;
    public GameObject electricFieldPrefab;
    public AudioClip electricFieldSound;
    public GameObject galePrefab;
    public AudioClip galeSound;
    public GameObject hastePrefab;
    public AudioClip hasteSound;
    public GameObject fireBallPrefab;
    public AudioClip fireBallSound;
    public GameObject LavaMinePrefab;
    public AudioClip LavaMineSound;
    public GameObject meteorRainPrefab;
    public AudioClip meteorRainSound;
    public GameObject berserkPrefab;
    public AudioClip berserkSound;
    public GameObject healDropPrefab;
    public AudioClip healDropSound;
    //public GameObject healRainPrefab;
    public GameObject iceBallPrefab;
    public AudioClip iceBallSound;
    public GameObject tsunamiPrefab;
    public AudioClip tsunamiSound;
    public GameObject rootsPrefab;
    public AudioClip rootsSound;
    public GameObject earthQuakePrefab;
    public AudioClip earthQuakeSound;
    public GameObject earthArmorPrefab;
    public AudioClip earthArmorSound;
    public GameObject leafTwisterPrefab;
    public AudioClip leafTwisterSound;
    public GameObject cannonPrefab;*/

    //private AudioSource source;

    //private ParticleSystem activeSystem;
    GameObject[] abilityTarget = new GameObject[4];
    BattleCalculator calculator;
    int numberOfAffectedEnemies;

    Stats playerAbilityUser;
    Stats enemyAbilityUser;

    public BattleController battle;
    public EnemiesController enemies;

    // Use this for initialization
    void OnEnable () {
        calculator = new BattleCalculator();
    }

    public void CheckUseOfAbilityP(int ability)
    {
        playerAbilityUser = battle.GetCurrentCharacter();
        //It will return 1 for finished ability, 2 when it must return to selectability, 3 when the movement is in process(gale, ivy, tsunami)
        switch (ability)
        {
            case 1:
                ThunderP();
                break;
            case 2:
                ElectricFieldP();
                break;
            case 3:
                GaleP();
                break;
            case 4:
                TeleportP();
                break;
            case 8:
                FireBallP();
                break;
            case 9:
                LavamineP();
                break;
            case 10:
                MeteorRainP();
                break;
            case 11:
                BerserkP();
                break;
            case 15:
                HealingDropP();
                break;
            case 16:
                IceBallP();
                break;
            case 17:
                HealingRainP();
                break;
            case 18:
                TsunamiP();
                break;
            case 22:
                RootsP();
                break;
            case 23:
                EarthArmorP();
                break;
            case 24:
                EarthquakeP();
                break;
            case 25:
                LeafTwisterP();
                break;
        }
    }

    public void CheckUseOfAbilityE(int ability, GameObject objective=null)
    {
        abilityTarget[0] = objective;
        enemyAbilityUser = battle.GetCurrentEnemy();
        //It will return 1 for finished ability, 2 when it must return to selectability, 3 when the movement is in process(gale, ivy, tsunami)
        switch (ability)
        {
            case 1:
                ThunderE();
                break;
            case 2:
                ElectricFieldP();
                break;
            case 4:
                TeleportP();
                break;
            case 8:
                FireBallE();
                break;
            case 9:
                MeteorRainE();
                break;
            case 11:
                BerserkP();
                break;
            case 16:
                HealingRainP();
                break;
            case 17:
                IceBallE();
                break;
            case 18:
                TsunamiE();
                break;
            case 22:
                RootsP();
                break;
            case 23:
                EarthArmorP();
                break;
            case 24:
                EarthquakeE();
                break;
            case 25:
                LeafTwisterE();
                break;
            case 26:
                ElectricBallE();
                break;
        }
    }

    private void ThunderP()
    {
        battle.attackFinished = 1;
           abilityTarget[0] = enemies.GetObjective();
        
        calculator.processOffensiveAbility(playerAbilityUser, abilityTarget[0].GetComponent<Stats>(), 1);



        playerAbilityUser.useMana(1);
        
        abilityTarget[0] = null;
    }

    private void ElectricFieldP()
    {
        battle.attackFinished = 1;
           abilityTarget[0] = battle.GetTargetPlayer();
        abilityTarget[0].GetComponent<Stats>().ActivateElectricField();
        playerAbilityUser.useMana(2);

        
        abilityTarget[0] = null;
        battle.attackFinished = 0;
    }

    private void GaleP()
    {
        battle.attackFinished = 1;
           abilityTarget[0] = enemies.GetObjective();
        playerAbilityUser.useMana(3);
        
        //instance.transform.Rotate(270, 0, 0);
        calculator.processOffensiveAbility(playerAbilityUser, abilityTarget[0].GetComponent<Stats>(), 3);
        if (abilityTarget[0])
        {
            abilityTarget[0].GetComponent<Stats>().DelayInitiatives();
            battle.FillBattleOrder();
        }
        abilityTarget[0] = null;
    }

    private void TeleportP()
    {
        battle.attackFinished = battle.playersAlive.Count;
        for (int i = 0; i < battle.playersAlive.Count; i++)
        {
            battle.playersAlive[i].GetComponent<Stats>().ActivateHaste();
        }
        battle.FillBattleOrder();
        abilityTarget[0] = null;
        battle.attackFinished = 0;
    }

    private void FireBallP()
    {
        battle.attackFinished = 1;

        abilityTarget[0] = enemies.GetObjective();

        playerAbilityUser.useMana(8);
        
        calculator.processOffensiveAbility(playerAbilityUser, abilityTarget[0].GetComponent<Stats>(), 8);
        abilityTarget[0] = null;
    }

    private void LavamineP()
    {
        battle.attackFinished = 1;
        abilityTarget[0] = enemies.GetObjective();
        calculator.processOffensiveAbility(playerAbilityUser, abilityTarget[0].GetComponent<Stats>(), 9);
        abilityTarget[0].GetComponent<Stats>().Burn();
        

        playerAbilityUser.useMana(9);
    }

    private void MeteorRainP()
    {
        int numberOfEnemies= enemies.GetNumberOfEnemies();
        battle.attackFinished = numberOfEnemies;
                enemies.MarkEnemy(0);
            for (int i = 0; i < numberOfEnemies; i++)
            {
                abilityTarget[i] = enemies.GetObjective();
            }
            enemies.Unmark();
        
        for (int i=0; i< numberOfEnemies; i++)
        {
            calculator.processOffensiveAbility(playerAbilityUser, abilityTarget[i].GetComponent<Stats>(), 10);
        }

        playerAbilityUser.useMana(10);
    }

    private void BerserkP()
    {
        battle.attackFinished = 1;
        abilityTarget[0] = battle.GetTargetPlayer();
        abilityTarget[0].GetComponent<Stats>().ActivateBerserk();
        playerAbilityUser.useMana(11);
        
        abilityTarget[0] = null;
        battle.attackFinished = 0;
    }

    private void HealingDropP()
    {
        battle.attackFinished = 1;
        abilityTarget[0] = battle.GetTargetPlayer();
        
        calculator.processHealAbilityP(playerAbilityUser, abilityTarget[0].GetComponent<Stats>(), 15);
        
        abilityTarget[0] = null;
        playerAbilityUser.useMana(15);
    }

    private void HealingRainP()
    {
        battle.attackFinished = battle.playersAlive.Count;
        for (int i = 0; i < battle.playersAlive.Count; i++)
        {
            calculator.processHealAbilityP(playerAbilityUser, battle.playersAlive[i].GetComponent<Stats>(), 17);
        }
        
        playerAbilityUser.useMana(17);
    }

    private void IceBallP()//IceBall
    {
        battle.attackFinished = 1;
        abilityTarget[0] = enemies.GetObjective();
        playerAbilityUser.useMana(16);
        
        calculator.processOffensiveAbility(playerAbilityUser, abilityTarget[0].GetComponent<Stats>(), 16);
        abilityTarget[0] = null;
    }

    private void TsunamiP()
    {
        int numberOfEnemies = enemies.GetNumberOfEnemies();
        battle.attackFinished = numberOfEnemies;

                enemies.MarkEnemy(0);
            for (int i = 0; i < numberOfEnemies; i++)
            {
                abilityTarget[i] = enemies.GetObjective();
                enemies.MarkEnemy(1);
            }
            enemies.Unmark();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            calculator.processOffensiveAbility(playerAbilityUser, abilityTarget[i].GetComponent<Stats>(), 18);
        }
        playerAbilityUser.useMana(18);
        


        /* HOW TO CAST 1 TO EVERY ENEMY
        GameObject aux=new UnityEngine.GameObject();
        GameObject instance;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            aux.transform.position = abilityTarget[i].transform.position - (abilityUser.gameObject.transform.position);
            aux.transform.position.Normalize();
            aux.transform.position = new Vector3(aux.transform.position.x / 10.0f, 0, aux.transform.position.z / 10.0f);
            instance = Instantiate(tsunamiPrefab, abilityUser.gameObject.transform.position + new Vector3(0, 0.1f, 1), Quaternion.identity) as GameObject;
            instance.GetComponent<MagicalFXBFE.FX_SpawnDirection>().Direction = aux.transform.position;
        }
        }*/
    }

    private void RootsP()
    {
        battle.attackFinished = 1;
        abilityTarget[0] = enemies.GetObjective();
        abilityTarget[0].GetComponent<Stats>().Root();
        //target.GetComponent<IA>().applyRoots(); //I think it should go into IA since enemies don't have such thing as character manager were you can apply altered states

        playerAbilityUser.useMana(22);
        abilityTarget[0] = null;
        battle.attackFinished = 0;
    }

    private void EarthArmorP() {

        battle.attackFinished = 1;
        abilityTarget[0] = battle.GetTargetPlayer();
        abilityTarget[0].GetComponent<Stats>().ActivateEarthArmor();
        playerAbilityUser.useMana(23);
        
      
        abilityTarget[0] = null;
        battle.attackFinished = 0;
    }

    private void EarthquakeP()
    {
        int numberOfEnemies = enemies.GetNumberOfEnemies();
        battle.attackFinished = numberOfEnemies;


                enemies.MarkEnemy(0);
            for (int i = 0; i < numberOfEnemies; i++)
            {
                abilityTarget[i] = enemies.GetObjective();
                enemies.MarkEnemy(1);
            }
            enemies.Unmark();

        for (int i = 0; i < numberOfEnemies; i++)
        {
            calculator.processOffensiveAbility(playerAbilityUser, abilityTarget[i].GetComponent<Stats>(), 24);
        }
        playerAbilityUser.useMana(24);
    }


    private void LeafTwisterP()
    {
        battle.attackFinished = 1;
        abilityTarget[0] = enemies.GetObjective();
        calculator.processOffensiveAbility(playerAbilityUser, abilityTarget[0].GetComponent<Stats>(), 25);

        playerAbilityUser.useMana(25);
        
        abilityTarget[0] = null;
    }



    private void ThunderE()
    {
        Debug.Log("Ja mon"+ abilityTarget[0].gameObject.name);
        calculator.processOffensiveAbilityEP(enemyAbilityUser, abilityTarget[0].GetComponent<Stats>(), 1);



        enemyAbilityUser.useMana(1);
        
        abilityTarget[0] = null;
    }


   

    private void FireBallE()
    {
        enemyAbilityUser.useMana(8);
        
        
        calculator.processOffensiveAbilityEP(enemyAbilityUser, abilityTarget[0].GetComponent<Stats>(), 8);
        abilityTarget[0] = null;
    }

    private void MeteorRainE()
    {
        int numberOfEnemies = battle.playersAlive.Count;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            abilityTarget[i] = battle.playersAlive[i];
        }
        for (int i = 0; i < numberOfEnemies; i++)
        {
            calculator.processOffensiveAbilityEP(enemyAbilityUser, abilityTarget[i].GetComponent<Stats>(), 9);
        }

        enemyAbilityUser.useMana(9);
    }
    

    

    private void IceBallE()//IceBall
    {
        enemyAbilityUser.useMana(17);
        
        calculator.processOffensiveAbilityEP(enemyAbilityUser, abilityTarget[0].GetComponent<Stats>(), 17);
        abilityTarget[0] = null;
    }

    private void TsunamiE()
    {
        int numberOfEnemies = battle.playersAlive.Count;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            abilityTarget[i] = battle.playersAlive[i];
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            calculator.processOffensiveAbilityEP(enemyAbilityUser, abilityTarget[i].GetComponent<Stats>(), 18);
        }
        enemyAbilityUser.useMana(18);
    }

    private void EarthquakeE()
    {
        int numberOfEnemies = battle.playersAlive.Count;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            abilityTarget[i] = battle.playersAlive[i];
        }

        for (int i = 0; i < numberOfEnemies; i++)
        {
            calculator.processOffensiveAbilityEP(enemyAbilityUser, abilityTarget[i].GetComponent<Stats>(), 24);
        }
        enemyAbilityUser.useMana(24);
    }

    private void LeafTwisterE()
    {
        calculator.processOffensiveAbilityEP(enemyAbilityUser, abilityTarget[0].GetComponent<Stats>(), 25);

        enemyAbilityUser.useMana(25);
    }

    private void ElectricBallE()
    {
        int numberOfEnemies = battle.playersAlive.Count;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            abilityTarget[i] = battle.playersAlive[i];
        }

        calculator.processOffensiveAbilityEP(enemyAbilityUser, abilityTarget[0].GetComponent<Stats>(), 25);

        enemyAbilityUser.useMana(25);
        
    }

    public bool AbleToUseAbility(int branch, int ability)
    {
        if (battle.GetCurrentCharacter().checkMana(branch * 7 + ability))
            return true;
        else
            return false;
    }
}
