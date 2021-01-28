/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData {


    public enum State {
        Start,
        FightingHurtMeDaddy,
        DefeatedHurtMeDaddy,
        FightingHurtMeDaddy_2,
        DefeatedHurtMeDaddy_2,
        FightingTank,
        DefeatedTank,
        GoingToTownCenter,
        ArrivedAtTownCenter,
        GoingToAskDoppelGanger,
        GoingToTavern,
        InTavern,
        FightingTavernAmbush,
        SurvivedTavernAmbush,
        HealerJoined,
        LeavingTown,
        GoingToFirstEvilMonsterEncounter,
        GoingToFightEvilMonster,
        FightingEvilMonster_1,
        LostToEvilMonster_1,
        MovingToEvilMonster_2,
        GoingToFightEvilMonster_2,
        FightingEvilMonster_2,
        LostToEvilMonster_2,
        MovingToEvilMonster_3,
        GoingToFightEvilMonster_3,
        FightingEvilMonster_3,
        DefeatedEvilMonster,
        GameOver,
    }

    private static bool isInit = false;
    public static List<Character> characterList;
    public static List<Item> itemList;

    public static State state;
    public static int ftnDewPoints;
    public static int healthPotionCount;
    public static string[] badNamesArray;


    public static void Init() {
        if (isInit) return;
        Debug.Log("Init();");
        isInit = true;
        SoundManager.Initialize();
        state = State.Start;
        ftnDewPoints = 0;
        healthPotionCount = 0;
        badNamesArray = new string[] {
            "Gruff McGruff",
            "That Guy",
            "Waka Waka",
            "Knightsalot Knightingale",
            "Senor Woofs",
            "Dinga Tinga",
            "Generico Nameo",
            "Brody Bro",
            "Dashing Slashing",
            "Cool Maverick",
            "Slick Showers",
            "Coffee Timer",
            "Dirt McGirt",
            "Patrick Folders",
            "Jonathan Discharge",
            "Bobby Tables",
            "Daddy Charger",
        };

        characterList = new List<Character>();

        characterList.Add(
            new Character(Character.Type.Player) {
                position = GameAssets.i.Map.Find("player").position,
            });

        characterList.Add(
            new Character(Character.Type.Tank, Character.SubType.Tank_BeforeJoin) {
                position = GameAssets.i.Map.Find("tank").position,
                enemyEncounter = new EnemyEncounter {
                    enemyBattleArray = new EnemyEncounter.EnemyBattle[] {
                        new EnemyEncounter.EnemyBattle(Character.Type.Tank, BattleHandler.LanePosition.Middle),
                    }
                }
            });
        characterList.Add(
            new Character(Character.Type.Sleezer, Character.SubType.Sleezer_BeforeJoin) {
                position = GameAssets.i.Map.Find("sleezer").position,
            });
        characterList.Add(
            new Character(Character.Type.Healer, Character.SubType.Healer_BeforeJoin) {
                position = GameAssets.i.Map.Find("healer").position,
            });


        foreach (Transform mapSpawn in GameAssets.i.Map) {
            if (!mapSpawn.gameObject.activeSelf) continue;
            CharacterSpawnData characterSpawnData = mapSpawn.GetComponent<CharacterSpawnData>();
            if (characterSpawnData != null) {
                characterList.Add(
                    new Character(characterSpawnData.characterType, characterSpawnData.characterSubType) {
                        position = mapSpawn.position,
                        enemyEncounter = characterSpawnData.enemyEncounter,
                        shopContents = characterSpawnData.shopContents.Clone()
                    }
                );
            }
        }

        itemList = new List<Item>();
        foreach (Transform mapSpawn in GameAssets.i.Map) {
            ItemSpawnData itemSpawnData = mapSpawn.GetComponent<ItemSpawnData>();
            if (itemSpawnData != null) {
                itemList.Add(new Item(itemSpawnData.itemType, itemSpawnData.amount, mapSpawn.position));
            }
        }
    }

    public static string GetCharacterName(Character.Type characterType) {
        Character character = GetCharacter(characterType);
        if (character != null) {
            return character.name;
        } else {
            return "???";
        }
    }

    public static Character GetCharacter(Character.Type characterType) {
        foreach (Character character in characterList) {
            if (character.type == characterType) {
                return character;
            }
        }
        return null;
    }

    public static bool IsNameAvailable(string name) {
        foreach (Character character in characterList) {
            if (character.name == name) {
                return false;
            }
        }
        return true;
    }
    

    public static void AddFtnDewPoints(int amount) {
        ftnDewPoints += amount;
    }

    public static void AddHealthPotion(int amount) {
        healthPotionCount += amount;
    }

    public static bool TrySpendHealthPotion() {
        if (healthPotionCount > 0) {
            healthPotionCount--;
            return true;
        } else {
            return false;
        }
    }





    /*
     * Battle Enemy Encounter
     * */
    [Serializable]
    public class EnemyEncounter {

        public EnemyBattle[] enemyBattleArray;

        [Serializable]
        public struct EnemyBattle {
            public Character.Type characterType;
            public BattleHandler.LanePosition lanePosition;
            public EnemyBattle(Character.Type characterType, BattleHandler.LanePosition lanePosition) {
                this.characterType = characterType;
                this.lanePosition = lanePosition;
            }
        }
    }



    
    [Serializable]
    public class ShopContents {
        public int healthPotions;
        public int ftnDewPoints;
        public bool hasFtnDewArmor;

        public ShopContents Clone() {
            return new ShopContents {
                healthPotions = healthPotions,
                ftnDewPoints = ftnDewPoints,
                hasFtnDewArmor = hasFtnDewArmor,
            };
        }
    }
}
