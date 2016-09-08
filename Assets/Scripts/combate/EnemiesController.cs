using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemiesController : MonoBehaviour {


    public enum enemyTypes { DPS, TANK, ASSASIN, MAGE, BOSS };
    public enum enemyElements { AIR, FIRE, WATER, EARTH };

    public List<GameObject> enemies;
    public List<GameObject> enemiesInBattle;
    private List<Stats> enemiesStats;

    public int objectiveEnemy;
    //private GameObject marker;
    public Transform cameraPos;

    public BattleController battle;

    void Start()
    {
        enemies = new List<GameObject>();
        enemiesInBattle = new List<GameObject>();
        enemiesStats = new List<Stats>();
        /*marker = GameObject.Find("Marker");
        Unmark();*/
    }


    void Update() {
        /*for (int i = 0; i < enemiesStats.Count; ++i)
			if (enemiesStats [i].destroy)
				DeleteEnemy (enemiesInBattle [i]);*/
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
        enemiesInBattle.Add(enemy);
        enemiesStats.Add(enemy.GetComponent<Stats>());
    }

    public void DeleteEnemy(GameObject enemy)
    {

        enemiesStats.RemoveAt(enemiesInBattle.IndexOf(enemy));
        enemiesInBattle.Remove(enemy);
        Destroy(enemy);
        battle.FillBattleOrder();
    }

    public void MarkEnemy(int enemy)
    {
        if (objectiveEnemy < enemiesInBattle.Count)
            enemiesInBattle[objectiveEnemy].transform.FindChild("Canvas").FindChild("Marker").gameObject.SetActive(false);
        if (enemy == 0)
        {
            objectiveEnemy = enemy;
        }
        else if (enemy == 1)
        {
            if (objectiveEnemy + 1 > enemiesInBattle.Count - 1)
            {
                objectiveEnemy = 0;
            }
            else
            {
                objectiveEnemy = objectiveEnemy + 1;
            }
        }
        else
        {
            if (objectiveEnemy - 1 < 0)
            {
                objectiveEnemy = enemiesInBattle.Count - 1;
            }
            else
            {
                objectiveEnemy = objectiveEnemy - 1;
            }
        }
        enemiesInBattle[objectiveEnemy].transform.FindChild("Canvas").FindChild("Marker").gameObject.SetActive(true);
        /*int enemyPos = enemies.IndexOf(enemiesInBattle[objectiveEnemy]);
        switch (enemyPos)
        {
            case 0:
                marker.transform.localPosition = new Vector3(34.6f, 38.2f, 1.8f);
                break;
            case 1:
                marker.transform.localPosition = new Vector3(32.2f, 38.2f, 4.3f);
                break;
            case 2:
                marker.transform.localPosition = new Vector3(28.8f, 38.2f, 3f);
                break;
        }*/

    }

    public void Unmark()
    {
        enemiesInBattle[objectiveEnemy].transform.FindChild("Canvas").FindChild("Marker").gameObject.SetActive(false);
    }

    public GameObject GetObjective()
    {
        return enemiesInBattle[objectiveEnemy-1];
    }

    public Stats GetObjectiveStats()
    {
        return enemiesStats[objectiveEnemy-1];
    }

    public int GetNumberOfEnemies()
    {
        return enemiesInBattle.Count;
    }

    public void EndOfBattle()
    {
        for (int i = 0; i < enemies.Count; i++)
            Destroy(enemies[i]);
        enemies.Clear();
        enemiesInBattle.Clear();
    }
    /*public static Vector3 TileToPosition(Vector2 tilePos, float y_pos){
		string tile_name = "Tile(" + tilePos.x + "," + tilePos.y + ")";
		GameObject tile = GameObject.Find (tile_name);
		Vector3 return_pos = new Vector3 (tile.transform.position.x, y_pos, tile.transform.position.z);

		return return_pos;
	}*/

    public void SelectObjective(int objective)
    {
        if(enemiesInBattle.Count==1)
            objectiveEnemy = 1;
        else if (enemiesInBattle.Count == 2)
        {
            if (enemies.Count == 3)
            {
                if (objective == 3)
                    objectiveEnemy = 2;
                else if ((enemiesInBattle[0] != enemies[0]) && (objective == 2))
                {
                    objectiveEnemy = 1;
                }
                else
                {
                    objectiveEnemy = objective;
                }
            }
            else
            {
                objectiveEnemy = objective;
                Debug.Log("2 y 2"+objectiveEnemy);
            }
        }
        else
        {
            objectiveEnemy = objective;
            Debug.Log("Hay 3");
        }
    }
}
