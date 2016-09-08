using UnityEngine;
using System.Collections;

public class Stats : MonoBehaviour {

    public enum STATTYPE { BASEHP = 0, CHP, HP, BASEMP, CMP, MP, STR, INT, DEX, VIT, PATK, PDEF, MATK, MDEF, CRIT, CRITDMG, SPD };

    public EnemiesController.enemyTypes type;
    public EnemiesController.enemyElements element;

    public int initiative1;
    public int initiative2;
    public int initiative3;

    private bool defenseUP = false;
    private int prevDef;

    private bool electricField = false;
    private bool berserk = false;
    private bool haste = false;
    private bool earthArmor = false;
    private bool burnt = false;
    private bool rooted = false;
    private int electricFieldTurns;
    private int berserkTurns;
    private int hasteTurns;
    private int earthArmorTurns;
    private int burntTurns;
    private int rootTurns;
    public bool reflect;

    public int HP { get; set;}
    public int currentHP { get; set; }

    public int MP { get; set; }
    public int currentMP { get; set; }
    public int STR;
    public int DEX;
    public int INT;
    public int VIT;
    public int PATK { get; set; }
    public int PDEF { get; set; }
    public int MATK { get; set; }
    public int MDEF { get; set; }
    public int CRIT { get; set; }
    public int CRITDMG { get; set; }
    public int SPD { get; set; }


    // ----- base stats -------
    public int base_HP;
    public int base_MP;
    public int base_PATK;
    public int base_PDEF;
    public int base_MATK;
    public int base_MDEF;
    public int base_CRIT;
    public int base_CRITDMG;
    public int base_SPD;
    /*public int base_STR;
    public int base_DEX;
    public int base_INT;
    public int base_VIT;*/
    // ------------------------

    public BattleController battle;

    public bool enemy;


    // Use this for initialization
    void Start () {
        RecalculateStats();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BeginBattle()
    {
        if (defenseUP)
        {
            //attr.setStat(Stats.STATTYPE.PDEF, prevDef);
            PDEF = prevDef;
            defenseUP = false;
        }
        if (electricField)
        {
            electricField = false;
            DEX -= 10;
            reflect = false;
            /*attr.setStat(Stats.STATTYPE.DEX, attr.getStat(Stats.STATTYPE.DEX) - 10);
            attr.SetReflect(false);*/
        }
        if (berserk)
        {
            berserk = false;
            STR -= 30;
            PDEF = PDEF * (100 / 70);
            /*attr.setStat(Stats.STATTYPE.STR, attr.getStat(Stats.STATTYPE.STR) - 30);
            attr.setStat(Stats.STATTYPE.PDEF, attr.getStat(Stats.STATTYPE.PDEF) * (100 / 70));*/
        }
        if (haste)
        {
            SPD = SPD - 10;
            //attr.setStat(Stats.STATTYPE.SPD, attr.getStat(Stats.STATTYPE.SPD) - 10);
            haste = false;
        }
        if (earthArmor)
        {
            VIT = VIT - 15;
            //attr.setStat(Stats.STATTYPE.VIT, attr.getStat(Stats.STATTYPE.VIT) - 15);
            earthArmor = false;
        }
        if (burnt)
        {
            burnt = false;
        }
        if (rooted)
        {
            rooted = false;
        }
        RecalculateStats();
        //attr.RecalculateStats();
    }

    public void BeginTurn()
    {
        if (defenseUP)
        {
            PDEF = prevDef;
            //attr.setStat(Stats.STATTYPE.PDEF, prevDef);
            defenseUP = false;
        }
        if (electricField)
        {
            if (electricFieldTurns == 0)
            {
                DEX -= 10;
                reflect = false;
                /*attr.setStat(Stats.STATTYPE.DEX, attr.getStat(Stats.STATTYPE.DEX) - 10);
                attr.SetReflect(false);
                attr.RecalculateStats();*/
                electricField = false;

            }
            else
            {
                --electricFieldTurns;
            }
        }
        if (berserk)
        {
            if (berserkTurns == 0)
            {
                STR -= 30;
                PDEF = PDEF * (100 / 70);
                /*attr.setStat(Stats.STATTYPE.STR, attr.getStat(Stats.STATTYPE.STR) - 30);
                attr.setStat(Stats.STATTYPE.PDEF, attr.getStat(Stats.STATTYPE.PDEF) * (100 / 70));
                attr.RecalculateStats();*/
                berserk = false;
            }
            else
            {
                --berserkTurns;
            }
        }
        if (haste)
        {
            if (hasteTurns == 0)
            {
                SPD = SPD - 10;
                /*attr.setStat(Stats.STATTYPE.SPD, attr.getStat(Stats.STATTYPE.SPD) - 10);
                attr.RecalculateStats();*/
                haste = false;
            }
            else
            {
                --hasteTurns;
            }
        }
        if (earthArmor)
        {
            if (earthArmorTurns == 0)
            {
                VIT -= 15;
                /*attr.setStat(Stats.STATTYPE.VIT, attr.getStat(Stats.STATTYPE.VIT) - 15);
                attr.RecalculateStats();*/
                earthArmor = false;
            }
            else
            {
                --earthArmorTurns;
            }
        }
        if (burnt)
        {
            if (burntTurns == 0)
            {
                burnt = false;
            }
            else
            {
                ProcessAttack(50);
                burntTurns--;
            }
        }
        if (rooted)
        {
            if (rootTurns == 0)
            {
                rooted = false;
            }
            else
            {
                rootTurns--;
            }
        }
        /*if(current_state!=State.DEAD)
            current_state = State.NONE;*/

        //con.menu.combatUIController.HighlightCharacter(gameObject.name, true);
        RecalculateStats();
    }

    public void ProcessHeal(int heal)
    {
        if ((currentHP + heal) >= HP)
        {
            currentHP = HP;
        }
        else
        {
            currentHP += heal;
        }

        //UpdateHPandMPUI();
        battle.attackFinished--;
    }

    public void ProcessAttack(int damage)
    {
        //Play get hit animation
        currentHP -= damage;
        if (currentHP < 0)
        {
            if (enemy)
                battle.enemies.DeleteEnemy(gameObject);
            else
                battle.DeletePlayer(gameObject);
        }
    }

    public void defendChar()
    {
        defenseUP = true;
        prevDef = PDEF;
        PDEF=(int)(PDEF * 1.5);
        battle.attackFinished = 0;
    }

    public void ActivateElectricField()
    {
        if (electricField)//Additive fields or just one
        {
            electricFieldTurns = 3;
        }
        else
        {
            DEX=DEX + 10;
            reflect= true;
            RecalculateStats();
            electricField = true;
            electricFieldTurns = 3;
        }
    }

    public void ActivateEarthArmor()
    {
        if (earthArmor)//Additive fields or just one
        {
            earthArmorTurns = 3;
        }
        else
        {
            VIT=VIT + 15;
            RecalculateStats();
            earthArmor = true;
            earthArmorTurns = 3;
        }
    }

    public void ActivateBerserk()
    {
        if (berserk)//Additive fields or just one
        {
            berserkTurns = 3;
        }
        else
        {
            STR=STR + 30;
            PDEF=PDEF * (70 / 100);
            RecalculateStats();
            berserk = true;
            berserkTurns = 3;
        }
    }

    public void ActivateHaste()
    {
        if (haste)//Additive fields or just one
        {
            hasteTurns = 2;
        }
        else
        {
            SPD=SPD + 10;
            initiative1 = initiative1 - 10;
            initiative2 = initiative2 - 20;
            initiative3 = initiative3 - 30;
            haste = true;
            hasteTurns = 3;
        }
    }

    public void Burn()
    {
        burnt = true;
        burntTurns = 2;
    }

    public void Root()
    {
        rooted = true;
        rootTurns = 1;
    }

    public void Revive()
    {
        if (currentHP <= 0)
            currentHP = 20;
    }

    public int getStat(STATTYPE t)
    {
        switch (t)
        {
            case STATTYPE.STR:
                return STR;
            case STATTYPE.INT:
                return INT;
            case STATTYPE.DEX:
                return DEX;
            case STATTYPE.VIT:
                return VIT;
            case STATTYPE.CHP:
                return currentHP;
            case STATTYPE.HP:
                return HP;
            case STATTYPE.CMP:
                return currentMP;
            case STATTYPE.MP:
                return MP;
            case STATTYPE.PATK:
                return PATK;
            case STATTYPE.PDEF:
                return PDEF;
            case STATTYPE.CRIT:
                return CRIT;
            case STATTYPE.MATK:
                return MATK;
            case STATTYPE.MDEF:
                return MDEF;
            case STATTYPE.CRITDMG:
                return CRITDMG;
            case STATTYPE.SPD:
                return SPD;
            default:
                return -5;
        }
    }
    public void RecalculateStats()
    {
        HP = base_HP + (VIT * 10);
        MP = base_MP + (INT * 5);
        PATK = base_PATK + (STR * 8);
        PDEF = base_PDEF + (VIT * 8);
        MATK = base_MATK + (INT * 8);
        MDEF = base_MDEF + (INT * 8);
        CRIT = base_CRIT + (DEX / 2);
        CRITDMG = base_CRITDMG + STR * 5;
        SPD = base_SPD + DEX;
    }

    public void ReStartHPandMP()
    {
        currentHP = HP;
        currentMP = MP;
    }


    public void SetInitiatives(bool firstTime)
    {
        if (firstTime)
        {
            initiative1 = 50 - SPD;
            initiative2 = initiative1 + 50 - SPD;
            initiative3 = initiative2 + 50 - SPD;
        }
        else
        {
            initiative1 = initiative2;
            initiative2 = initiative3;
            initiative3 = initiative2 + 50 - SPD;
            battle.AddNewInitiative(initiative3, gameObject);
        }
    }

    public EnemiesController.enemyElements GetEnemyElement()
    {
        return element;
    }

    public float LifePercentage()
    {
        return (float)currentHP / (float)HP;
    }

    public void useMana(int ability)
    {
        switch (ability)
        {
            case 1:
                currentMP -= 20;
                break;
            case 2:
                currentMP -= 30;
                break;
            case 3:
                currentMP -= 15;
                break;
            case 4:
                currentMP -= 25;
                break;
            case 8:
                currentMP -= 20;
                break;
            case 9:
                currentMP -= 30;
                break;
            case 10:
                currentMP -= 50;
                break;
            case 11:
                currentMP -= 20;
                break;
            case 15:
                currentMP -= 15;
                break;
            case 16:
                currentMP -= 20;
                break;
            case 17:
                currentMP -= 30;
                break;
            case 18:
                currentMP -= 45;
                break;
            case 22:
                currentMP -= 15;
                break;
            case 23:
                currentMP -= 40;
                break;
            case 24:
                currentMP -= 20;
                break;
            case 25:
                currentMP -= 25;
                break;
        }
    }

    public bool checkMana(int ability)
    {
        /*if (godMode)
            return true;*/
        switch (ability)
        {
            case 1:
                return currentMP > 20;
            case 2:
                return currentMP > 30;
            case 3:
                return currentMP > 15;
            case 4:
                return currentMP > 25;
            case 8:
                return currentMP > 20;
            case 9:
                return currentMP > 30;
            case 10:
                return currentMP > 50;
            case 11:
                return currentMP > 20;
            case 15:
                return currentMP > 15;
            case 16:
                return currentMP > 20;
            case 17:
                return currentMP > 30;
            case 18:
                return currentMP > 45;
            case 22:
                return currentMP > 15;
            case 23:
                return currentMP > 20;
            case 24:
                return currentMP > 40;
            case 25:
                return currentMP > 25;
            default:
                return false;
        }
    }

    public void DelayInitiatives()
    {
        initiative1 += 10;
        initiative2 += 10;
        initiative3 += 10;
    }
}
