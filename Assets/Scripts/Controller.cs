using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    public GameObject MainMenu;
    public GameObject CharSel;
    public GameObject ValueChange;
    public GameObject Combat;
    public int selectedOpt;

    public int numberAllies;
    public Text numAlly; 
    public List<int> enemiesSelected;

    public Container container;
    public EnemiesController enemies;
    public BattleController battle;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ButtonMenu(int selection)
    {
        if (selection == 1)
        {
            MainMenu.SetActive(false);
            CharSel.SetActive(true);
        }

    }

    public void AddCharacter(int selection)
    {
        enemiesSelected.Add(selection);
    }

    public void RemoveSelected()
    {
        if (enemiesSelected.Count != 0)
            enemiesSelected.RemoveAt(enemiesSelected.Count - 1);
    }

    public void NumberOfAllies(int allyCount)
    {
        numberAllies = allyCount;
        numAlly.text ="Num of allies: "+ allyCount.ToString();
    }

    public void StartBattle()
    {
        for (int i = 0; i < enemiesSelected.Count; i++)
        {
            enemies.AddEnemy(container.enemies[enemiesSelected[i] - 1]);
            enemies.enemies[i].GetComponent<Stats>().BeginBattle();
            enemies.enemies[i].GetComponent<Stats>().SetInitiatives(true);
            enemies.enemies[i].GetComponent<Stats>().ReStartHPandMP();
            battle.bInput.names[i].text = enemies.enemies[i].name;
        }
        for (int i = 0; i < numberAllies + 1; i++)
        {
            battle.AddPlayer(container.allies[i]);
            battle.players[i].GetComponent<Stats>().BeginBattle();
            battle.players[i].GetComponent<Stats>().SetInitiatives(true);
            battle.players[i].GetComponent<Stats>().ReStartHPandMP();
        }
        battle.bInput.UpdateHPMP();
        CharSel.SetActive(false);
        Combat.SetActive(true);
        battle.enabled = true;

    }

    public void EndBattle()
    {
        Combat.SetActive(false);
        MainMenu.SetActive(true);
        battle.enabled = false;

    }
}
