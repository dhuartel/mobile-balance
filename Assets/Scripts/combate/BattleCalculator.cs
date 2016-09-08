using UnityEngine;
using System.Collections;

public class BattleCalculator {


    // Calculate attack damage the player deals to the enemy and returns it but doesn't process it
    public int getAttackDamagePE(Stats player, Stats enemy, float damageMultiplier)
    {
        return (int) (( (player.getStat(Stats.STATTYPE.PATK) * 4) - (enemy.getStat(Stats.STATTYPE.PDEF) * 2)) * damageMultiplier);
    }

    public int getAttackDamageEP(Stats enemy, Stats player)
    {
        return (enemy.getStat(Stats.STATTYPE.PATK) * 4) - (player.getStat(Stats.STATTYPE.PDEF) * 2);
    }
    
    // Process attack from Player to Enemy and changes enemy's HP, returns true if hit was crit
	public void processAttackPE(Stats player, Stats enemy, bool crit)
    {
        int damageDealt = (player.getStat(Stats.STATTYPE.PATK) * 4) - (enemy.getStat(Stats.STATTYPE.PDEF) * 2);

        if (damageDealt < 0)
        {//Solution for healing attacks
            damageDealt = 1;
        }
        
        if (crit)
        {
            float damageBonus = 50.0f;
            damageBonus += (float)player.getStat(Stats.STATTYPE.CRITDMG) / 5.0f;
            damageBonus /= 100.0f;
            damageBonus *= damageDealt;
            damageDealt += (int)damageBonus;
        }
        enemy.ProcessAttack(damageDealt);
        //return isCrit;
    }

    // Process attack from enemy to player and changes player's HP, returns true if hit was crit
    public void processAttackEP(Stats enemy, Stats player)
    {
        bool isCrit = enemy.getStat(Stats.STATTYPE.CRIT) >= Random.Range(0, 100);
        int damageDealt = (enemy.getStat(Stats.STATTYPE.PATK) * 4) - (player.getStat(Stats.STATTYPE.PDEF) * 2);
        if (damageDealt < 0)
        {//Solution for healing attacks
            damageDealt = 1;
        }

        if (isCrit)
        {
            float damageBonus = 50.0f;
            damageBonus += (float)enemy.getStat(Stats.STATTYPE.CRITDMG) / 5.0f;
            damageBonus /= 100.0f;
            damageBonus *= damageDealt;
            damageDealt += (int)damageBonus;
        }
        player.ProcessAttack(damageDealt);

        if (player.reflect)
        {
            enemy.ProcessAttack(damageDealt * 15 / 100);
            //damageDealt *= (85 / 100);        DON'T THINK SO
        }
        //return isCrit;
    }


    public void processOffensiveAbility(Stats player, Stats enemy, int abilityNumber)
    {
        int damageDealt = 0;
        switch (abilityNumber)
        {
            case 1://Thunder pot:125
                damageDealt = ((player.getStat(Stats.STATTYPE.MATK) * 4) - (enemy.getStat(Stats.STATTYPE.MDEF) * 2)) * 125 / 100;
                if (enemy.GetEnemyElement() == EnemiesController.enemyElements.WATER)
                {
                    damageDealt = (int)(damageDealt * 1.25);
                }
                else if (enemy.GetEnemyElement() == EnemiesController.enemyElements.EARTH)
                {
                    damageDealt = (int)(damageDealt * 0.75);
                }
                enemy.ProcessAttack(damageDealt);
                break;
            case 3://Gale pot:125
                damageDealt = ((player.getStat(Stats.STATTYPE.MATK) * 4) - (enemy.getStat(Stats.STATTYPE.MDEF) * 2)) * 125 / 100;
                if (enemy.GetEnemyElement() == EnemiesController.enemyElements.WATER)
                {
                    damageDealt = (int)(damageDealt * 1.25);
                }
                else if (enemy.GetEnemyElement() == EnemiesController.enemyElements.EARTH)
                {
                    damageDealt = (int)(damageDealt * 0.75);
                }
                enemy.ProcessAttack(damageDealt);
                break;
            case 8://FireBall pot:150
                damageDealt = ((player.getStat(Stats.STATTYPE.MATK) * 4) - (enemy.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                if (enemy.GetEnemyElement() == EnemiesController.enemyElements.EARTH)
                {
                    damageDealt = (int)(damageDealt * 1.25);
                }
                else if (enemy.GetEnemyElement() == EnemiesController.enemyElements.WATER)
                {
                    damageDealt = (int)(damageDealt * 0.75);
                }
                enemy.ProcessAttack(damageDealt);
                break;
            case 9://Lava Mine pot:150
                damageDealt = ((player.getStat(Stats.STATTYPE.MATK) * 4) - (enemy.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                if (enemy.GetEnemyElement() == EnemiesController.enemyElements.EARTH)
                {
                    damageDealt = (int)(damageDealt * 1.25);
                }
                else if (enemy.GetEnemyElement() == EnemiesController.enemyElements.WATER)
                {
                    damageDealt = (int)(damageDealt * 0.75);
                }
                enemy.ProcessAttack(damageDealt);
                break;
            case 10://Meteor rain pot:200
                damageDealt = ((player.getStat(Stats.STATTYPE.MATK) * 4) - (enemy.getStat(Stats.STATTYPE.MDEF) * 2)) * 200 / 100;

                if (enemy.GetEnemyElement() == EnemiesController.enemyElements.EARTH)
                {
                    damageDealt = (int)(damageDealt * 1.25);
                }
                else if (enemy.GetEnemyElement() == EnemiesController.enemyElements.WATER)
                {
                    damageDealt = (int)(damageDealt * 0.75);
                }
                enemy.ProcessAttack(damageDealt);
                break;
            case 16://Iceball pot:150
                damageDealt = ((player.getStat(Stats.STATTYPE.MATK) * 4) - (enemy.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                if (enemy.GetEnemyElement() == EnemiesController.enemyElements.FIRE)
                {
                    damageDealt = (int)(damageDealt * 1.25);
                }
                else if (enemy.GetEnemyElement() == EnemiesController.enemyElements.AIR)
                {
                    damageDealt = (int)(damageDealt * 0.75);
                }
                enemy.ProcessAttack(damageDealt);
                break;
            case 18://Tsunami pot:150
                damageDealt = ((player.getStat(Stats.STATTYPE.MATK) * 4) - (enemy.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                if (enemy.GetEnemyElement() == EnemiesController.enemyElements.FIRE)
                {
                    damageDealt = (int)(damageDealt * 1.25);
                }
                else if (enemy.GetEnemyElement() == EnemiesController.enemyElements.AIR)
                {
                    damageDealt = (int)(damageDealt * 0.75);
                }
                enemy.ProcessAttack(damageDealt);
                break;
            case 24://Earthquake pot:150
                damageDealt = ((player.getStat(Stats.STATTYPE.MATK) * 4) - (enemy.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                if (enemy.GetEnemyElement() == EnemiesController.enemyElements.AIR)
                {
                    damageDealt = (int)(damageDealt * 1.25);
                }
                else if (enemy.GetEnemyElement() == EnemiesController.enemyElements.FIRE)
                {
                    damageDealt = (int)(damageDealt * 0.75);
                }
                enemy.ProcessAttack(damageDealt);
                break;
            case 25://Ivy pot:150
                damageDealt = ((player.getStat(Stats.STATTYPE.MATK) * 4) - (enemy.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                if (enemy.GetEnemyElement() == EnemiesController.enemyElements.AIR)
                {
                    damageDealt = (int)(damageDealt * 1.25);
                }
                else if (enemy.GetEnemyElement() == EnemiesController.enemyElements.FIRE)
                {
                    damageDealt = (int)(damageDealt * 0.75);
                }
                enemy.ProcessAttack(damageDealt);
                break;
        }
    }

    public void processHealAbilityP(Stats player, Stats targetPlayer, int abilityNumber)
    {
        int healDealt = 0;
        switch (abilityNumber)
        {
            case 15://Healing Drop pot150
                healDealt = (player.getStat(Stats.STATTYPE.MATK) * 3)* 150 / 100;
                targetPlayer.ProcessHeal(healDealt);
                break;
            case 17://Healing Rain pot125
                healDealt = (player.getStat(Stats.STATTYPE.MATK) * 3) * 125 / 100;
                targetPlayer.ProcessHeal(healDealt);
                break;
        }
    }


    public void processOffensiveAbilityEP(Stats enemy, Stats player, int abilityNumber)
    {
        int damageDealt = 0;
        switch (abilityNumber)
        {
            case 1://Thunder pot:125
                damageDealt = ((enemy.getStat(Stats.STATTYPE.MATK) * 4) - (player.getStat(Stats.STATTYPE.MDEF) * 2)) * 125 / 100;
                break;
            case 3://Gale pot:125
                damageDealt = ((enemy.getStat(Stats.STATTYPE.MATK) * 4) - (player.getStat(Stats.STATTYPE.MDEF) * 2)) * 125 / 100;
                break;
            case 8://FireBall pot:150
                damageDealt = ((enemy.getStat(Stats.STATTYPE.MATK) * 4) - (player.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                break;
            case 9://Meteor rain pot:200
                damageDealt = ((enemy.getStat(Stats.STATTYPE.MATK) * 4) - (player.getStat(Stats.STATTYPE.MDEF) * 2)) * 200 / 100;
                break;
            case 10://Lava Mine pot:150
                damageDealt = ((enemy.getStat(Stats.STATTYPE.MATK) * 4) - (player.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                break;
            case 17://Iceball pot:150
                damageDealt = ((enemy.getStat(Stats.STATTYPE.MATK) * 4) - (player.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                break;
            case 18://Tsunami pot:150
                damageDealt = ((enemy.getStat(Stats.STATTYPE.MATK) * 4) - (player.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                break;
            case 24://Earthquake pot:150
                damageDealt = ((enemy.getStat(Stats.STATTYPE.MATK) * 4) - (player.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                break;
            case 25://Ivy pot:150
                damageDealt = ((enemy.getStat(Stats.STATTYPE.MATK) * 4) - (player.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                break;
            case 26://Great electric ball pot:150
                damageDealt = ((enemy.getStat(Stats.STATTYPE.MATK) * 4) - (player.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                break;
        }
        if (damageDealt < 0)
        {//Solution for healing attacks
            damageDealt = 1;
        }
        Debug.Log(damageDealt);
        player.ProcessAttack(damageDealt);
    }

    public void processHealAbilityE(Stats player, Stats targetPlayer, int abilityNumber)
    {
        int healDealt = 0;
        switch (abilityNumber)
        {
            case 15://Healing Drop pot150
                healDealt = ((player.getStat(Stats.STATTYPE.MATK) * 4) - (targetPlayer.getStat(Stats.STATTYPE.MDEF) * 2)) * 150 / 100;
                targetPlayer.ProcessHeal(healDealt);
                break;
            case 16://Healing Rain pot125
                healDealt = ((player.getStat(Stats.STATTYPE.MATK) * 4) - (targetPlayer.getStat(Stats.STATTYPE.MDEF) * 2)) * 125 / 100;
                targetPlayer.ProcessHeal(healDealt);
                break;
        }
    }
}
