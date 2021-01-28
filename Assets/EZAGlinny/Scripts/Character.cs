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

/*
 * Represents a single character: Player, Companion, Enemy
 * */
public class Character {

    public static bool IsUniqueCharacterType(Type type) {
        switch (type) {
        default:
        case Type.Player:
        case Type.Sleezer:
        case Type.Tank:
        case Type.Healer:

        case Type.Randy:
        case Type.EvilMonster:
        case Type.EvilMonster_2:
        case Type.EvilMonster_3:
            return true;
        case Type.Enemy_MinionOrange:
        case Type.Enemy_MinionRed:
        case Type.Enemy_Ogre:
        case Type.Enemy_Zombie:
            return false;
        }
    }


    public class Stats {

        public int healthMax;
        public int health;
        public int speedMax;
        public int speed;
        public int attack;
        public int specialMax;
        public int special;

    }

    public enum Type {
        Player,
        Sleezer,
        Tank,
        Healer,
        PlayerDoppelganger,

        Enemy_MinionOrange,
        Enemy_MinionRed,
        Enemy_Ogre,
        Enemy_Zombie,

        TavernAmbush,

        Randy,
        EvilMonster,

        Shop,
        Villager_1,
        Villager_2,
        Villager_3,
        Villager_4,
        Villager_5,
        
        EvilMonster_2,
        EvilMonster_3,
        
        TavernAmbush_2,
        TavernAmbush_3,
    }

    public enum SubType {
        None,

        Enemy_HurtMeDaddy,
        Enemy_HurtMeDaddy_2,
        
        Tank_BeforeJoin,
        Tank_Friendly,
        
        Sleezer_BeforeJoin,
        Sleezer_Friendly,
        
        Healer_BeforeJoin,
        Healer_Friendly,

        EvilMonster_1,
        EvilMonster_2,
        EvilMonster_3,
    }

    public Type type;
    public SubType subType;
    public Stats stats;
    public string name;
    public Vector3 position;
    public GameData.EnemyEncounter enemyEncounter;
    public GameData.ShopContents shopContents;
    public bool isDead;
    public bool isInPlayerTeam;
    public bool hasFtnDewArmor;
    public bool hasSwordThousandTruths;

    public Character(Type type, SubType subType = SubType.None) {
        this.type = type;
        this.subType = subType;
        name = type.ToString();
        
        stats = new Stats {
            attack = 10,
            health = 100,
            healthMax = 100,
            special = 1,
            specialMax = 1,
            speed = 1,
            speedMax = 1,
        };

        switch (type) {
        default:
            Debug.Log("###### Default Type: " + type);
            break;
        case Type.Player:
            stats = new Stats {
                attack = 35,
                health = 120,
                healthMax = 120,
                special = 0,
                specialMax = 2 + 1,
                speed = 1,
                speedMax = 1,
            };
            //hasFtnDewArmor = true;
            //hasSwordThousandTruths = true;
            isInPlayerTeam = true;
            break;
        case Type.Healer:
            stats = new Stats {
                attack = 20,
                health = 90,
                healthMax = 90,
                special = 1,
                specialMax = 2 + 1,
                speed = 1,
                speedMax = 1,
            };
            break;
        case Type.Tank:
            stats = new Stats {
                attack = 25,
                health = 150,
                healthMax = 150,
                special = 1,
                specialMax = 2 + 1,
                speed = 1,
                speedMax = 1,
            };
            break;
        case Type.Sleezer:
            name = "Sleezer";
            stats = new Stats {
                attack = 10,
                health = 100,
                healthMax = 100,
                special = 1,
                specialMax = 1,
                speed = 1,
                speedMax = 1,
            };
            break;
        case Type.Enemy_MinionOrange:
            stats = new Stats {
                attack = 16,
                health = 50,
                healthMax = 50,
                special = 1,
                specialMax = 1,
                speed = 1,
                speedMax = 1,
            };
            break;
        case Type.Enemy_MinionRed:
            stats = new Stats {
                attack = 20,
                health = 80,
                healthMax = 80,
                special = 1,
                specialMax = 1,
                speed = 1,
                speedMax = 1,
            };
            break;
        case Type.Enemy_Zombie:
            stats = new Stats {
                attack = 30,
                health = 120,
                healthMax = 120,
                special = 1,
                specialMax = 1,
                speed = 1,
                speedMax = 1,
            };
            break;
        case Type.Enemy_Ogre:
            stats = new Stats {
                attack = 25,
                health = 200,
                healthMax = 200,
                special = 1,
                specialMax = 1,
                speed = 1,
                speedMax = 1,
            };
            break;
        case Type.EvilMonster:
            name = "Evil Monster";
            stats = new Stats {
                attack = 40,
                health = 300,
                healthMax = 300,
                special = 1,
                specialMax = 1,
                speed = 1,
                speedMax = 1,
            };
            break;
        case Type.EvilMonster_2:
        case Type.EvilMonster_3:
            name = "Evil Monster";
            stats = new Stats {
                attack = 60,
                health = 300,
                healthMax = 300,
                special = 1,
                specialMax = 1,
                speed = 1,
                speedMax = 1,
            };
            break;
        case Type.PlayerDoppelganger:
        case Type.TavernAmbush:
        case Type.TavernAmbush_2:
        case Type.TavernAmbush_3:
            break;
        case Type.Villager_1:
        case Type.Villager_2:
        case Type.Villager_3:
        case Type.Villager_4:
        case Type.Villager_5:
            name = "Villager";
            break;
        case Type.Randy:
            name = "Randy";
            break;
        case Type.Shop:
            name = "Vendor";
            break;
        }
        isDead = false;
    }

    public bool IsEnemy() {
        switch (type) {
        default:
        case Type.Player:
        case Type.Sleezer:
        case Type.Tank:
        case Type.Healer:
        case Type.PlayerDoppelganger:
        case Type.TavernAmbush:
        case Type.TavernAmbush_2:
        case Type.TavernAmbush_3:
            return false;
        case Type.Enemy_MinionOrange:
        case Type.Enemy_MinionRed:
        case Type.Enemy_Ogre:
        case Type.Enemy_Zombie:
        case Type.EvilMonster:
        case Type.EvilMonster_2:
        case Type.EvilMonster_3:
            return true;
        }
    }

}
