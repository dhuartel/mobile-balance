using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class BattleUI : MonoBehaviour {
    
    private bool input;
    private float time=0;

    public BattleController battle;


    public GameObject[] actionButtons = new GameObject[4];
    public enum JoystickInputStates { NONE=0,ACTIONS,ABILITIES,AIR,FIRE,WATER,EARTH }
    private JoystickInputStates current_state;
    private uint currentButton = 0;
    EventSystem eventSystem;

    public List<Text> names;
    public List<Text> enemiesHP;
    public List<Text> enemiesMP;
    public List<Text> alliesHP;
    public List<Text> alliesMP;

    void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    void OnEnable()
    {
        input = false;
    }
    
    
    public void ChangeState(JoystickInputStates newState) {
        current_state = newState;
        switch (newState)
        {
            case JoystickInputStates.NONE:
                eventSystem.SetSelectedGameObject(null);
                break;
            default:
                int i = 0;
                for (i=0 ; i < 4; i++)
                {
                    if (actionButtons[i].GetComponent<Button>().interactable)
                    {
                        break;
                    }
                }
                //Debug.Log(i);
                eventSystem.SetSelectedGameObject(actionButtons[i]);
                currentButton = (uint)i;
                break;
        }
    }
    

    public void ChangeButtonsText(int type)
    {
        bool abilitiesUnlocked;

        switch (type)
        {

            case 0:
                eventSystem.SetSelectedGameObject(null);
                actionButtons[0].GetComponentInChildren<Text>().text = "";
                actionButtons[1].GetComponentInChildren<Text>().text = "";
                actionButtons[2].GetComponentInChildren<Text>().text = "";
                actionButtons[3].GetComponentInChildren<Text>().text = "";
                break;
            case 1:
                actionButtons[0].GetComponentInChildren<Text>().text = "ATTACK";
                actionButtons[1].GetComponentInChildren<Text>().text = "MAGIC";
                actionButtons[2].GetComponentInChildren<Text>().text = "DEFEND";
                actionButtons[3].GetComponentInChildren<Text>().text = "RUN";
                
                    for (int i = 0; i < 4; i++)
                    {
                        actionButtons[i].GetComponent<Button>().interactable = true;
                        actionButtons[i].GetComponentInChildren<Text>().color = Color.black;
                    }
                    abilitiesUnlocked = false;
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (battle.useOfAbilities.AbleToUseAbility(i, j + 1))
                                abilitiesUnlocked = true;
                        }
                    }
                    if (!abilitiesUnlocked)
                    {
                        actionButtons[1].GetComponent<Button>().interactable = false;
                        actionButtons[1].GetComponentInChildren<Text>().color = Color.red;
                    }
                
                break;
            case 2:
                actionButtons[0].GetComponentInChildren<Text>().text = "AIR";
                actionButtons[1].GetComponentInChildren<Text>().text = "FIRE";
                actionButtons[2].GetComponentInChildren<Text>().text = "WATER";
                actionButtons[3].GetComponentInChildren<Text>().text = "EARTH";

                for (int i = 0; i < 4; i++)
                {
                    abilitiesUnlocked = false;
                    for(int j = 0; j < 4; j++)
                    {
                        if (battle.useOfAbilities.AbleToUseAbility(i, j + 1))
                            abilitiesUnlocked = true;
                    }
                    if (abilitiesUnlocked)
                    {
                        actionButtons[i].GetComponent<Button>().interactable = true;
                        actionButtons[i].GetComponentInChildren<Text>().color = Color.black;
                    }
                    else
                    {
                        actionButtons[i].GetComponent<Button>().interactable = false;
                        actionButtons[i].GetComponentInChildren<Text>().color = Color.red;
                    }
                }
                break;
            case 3:
                actionButtons[0].GetComponentInChildren<Text>().text = "THUNDER";
                actionButtons[1].GetComponentInChildren<Text>().text = "ELECTRIC FIELD";
                actionButtons[2].GetComponentInChildren<Text>().text = "GALE";
                actionButtons[3].GetComponentInChildren<Text>().text = "HASTE";
                for(int i = 0; i < 4; i++)
                {
                    if (battle.useOfAbilities.AbleToUseAbility(0, i+1))
                    {
                        actionButtons[i].GetComponent<Button>().interactable = true;
                        actionButtons[i].GetComponentInChildren<Text>().color = Color.black;
                    }
                    else
                    {
                        actionButtons[i].GetComponent<Button>().interactable = false;
                        actionButtons[i].GetComponentInChildren<Text>().text = "";
                    }
                }
                break;
            case 4:
                actionButtons[0].GetComponentInChildren<Text>().text = "FIRE WHEEL";
                actionButtons[1].GetComponentInChildren<Text>().text = "LAVA MINE";
                actionButtons[2].GetComponentInChildren<Text>().text = "METEOR RAIN";
                actionButtons[3].GetComponentInChildren<Text>().text = "BERSERK";
                for (int i = 0; i < 4; i++)
                {
                    if (battle.useOfAbilities.AbleToUseAbility(1, i + 1))
                    {
                        actionButtons[i].GetComponent<Button>().interactable = true;
                        actionButtons[i].GetComponentInChildren<Text>().color = Color.black;
                    }
                    else
                    {
                        actionButtons[i].GetComponent<Button>().interactable = false;
                        actionButtons[i].GetComponentInChildren<Text>().text = "";
                    }
                }
                break;
            case 5:
                actionButtons[0].GetComponentInChildren<Text>().text = "HEALING DROP";
                actionButtons[1].GetComponentInChildren<Text>().text = "ICE BALL";
                actionButtons[2].GetComponentInChildren<Text>().text = "HEALING RAIN";
                actionButtons[3].GetComponentInChildren<Text>().text = "ICE SPIKES";
                for (int i = 0; i < 4; i++)
                {
                    if (battle.useOfAbilities.AbleToUseAbility(2, i + 1))
                    {
                        actionButtons[i].GetComponent<Button>().interactable = true;
                        actionButtons[i].GetComponentInChildren<Text>().color = Color.black;
                    }
                    else
                    {
                        actionButtons[i].GetComponent<Button>().interactable = false;
                        actionButtons[i].GetComponentInChildren<Text>().text = "";
                    }
                }
                break;
            case 6:
                actionButtons[0].GetComponentInChildren<Text>().text = "ROOTS";
                actionButtons[1].GetComponentInChildren<Text>().text = "EARTH ARMOR";
                actionButtons[2].GetComponentInChildren<Text>().text = "EARTHQUAKE";
                actionButtons[3].GetComponentInChildren<Text>().text = "LEAF TWISTER";
                for (int i = 0; i < 4; i++)
                {
                    if (battle.useOfAbilities.AbleToUseAbility(3, i + 1))
                    {
                        actionButtons[i].GetComponent<Button>().interactable = true;
                        actionButtons[i].GetComponentInChildren<Text>().color = Color.black;
                    }
                    else
                    {
                        actionButtons[i].GetComponent<Button>().interactable = false;
                        actionButtons[i].GetComponentInChildren<Text>().text = "";
                    }
                }
                break;
        }
    }

    public void UpdateHPMP()
    {
        for(int i = 0; i < battle.enemies.enemies.Count; i++)
        {
            enemiesHP[i].text = battle.enemies.enemies[i].GetComponent<Stats>().currentHP + " / " + battle.enemies.enemies[i].GetComponent<Stats>().HP;
            enemiesMP[i].text = battle.enemies.enemies[i].GetComponent<Stats>().currentMP + " / " + battle.enemies.enemies[i].GetComponent<Stats>().MP;
        }
        for (int i=0; i<battle.players.Count;i++)
        {
            alliesHP[i].text = battle.players[i].GetComponent<Stats>().currentHP + " / " + battle.players[i].GetComponent<Stats>().HP;
            alliesMP[i].text = battle.players[i].GetComponent<Stats>().currentMP + " / " + battle.players[i].GetComponent<Stats>().MP;
        }
    }
}
