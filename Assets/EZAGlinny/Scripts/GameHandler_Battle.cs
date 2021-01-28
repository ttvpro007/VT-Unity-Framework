/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;
using GridPathfindingSystem;

public class GameHandler_Battle : MonoBehaviour {

    [SerializeField] private CameraFollow cameraFollow;

    private void Awake() {
        new BattleHandler();

        if (BattleHandler.enemyEncounter == null) {
            Debug.Log("#### Debug Enemy Encounter");
            GameData.EnemyEncounter enemyEncounter = new GameData.EnemyEncounter {
                enemyBattleArray = new GameData.EnemyEncounter.EnemyBattle[] { 
                    new GameData.EnemyEncounter.EnemyBattle { characterType = Character.Type.Enemy_MinionOrange, lanePosition = BattleHandler.LanePosition.Middle },
                    new GameData.EnemyEncounter.EnemyBattle { characterType = Character.Type.Enemy_MinionOrange, lanePosition = BattleHandler.LanePosition.Up },
                    new GameData.EnemyEncounter.EnemyBattle { characterType = Character.Type.Enemy_MinionOrange, lanePosition = BattleHandler.LanePosition.Down },
                },
            };
            BattleHandler.LoadEnemyEncounter(new Character(Character.Type.Enemy_MinionOrange), enemyEncounter);
        }
    }

    private void Start() {
        // Set up Battle Scene
        //BattleHandler.SpawnCharacter(BattleHandler.LanePosition.Middle, true);
        //BattleHandler.SpawnCharacter(BattleHandler.LanePosition.Up, true);
        //BattleHandler.SpawnCharacter(BattleHandler.LanePosition.Down, true);

        /*
        GameData.EnemyEncounter enemyEncounter = BattleHandler.enemyEncounter;
        for (int i = 0; i < enemyEncounter.enemyBattleArray.Length; i++) {
            GameData.EnemyEncounter.EnemyBattle enemyBattle = enemyEncounter.enemyBattleArray[i];
            BattleHandler.SpawnCharacter(enemyBattle, false);
        }
        */

        BattleHandler.GetInstance().Start(cameraFollow);
    }

    private void Update() {
        BattleHandler.GetInstance().Update();
    }

}
