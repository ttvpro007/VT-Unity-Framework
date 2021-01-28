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
using CodeMonkey.Utils;

/*
 * Handles the Overmap
 * */
public class OvermapHandler {

    private static OvermapHandler instance;
    public  static OvermapHandler GetInstance() => instance;

    public static void LoadBackToOvermap() { 
        Loader.Load(Loader.Scene.GameScene);
    }

    public static void SaveAllCharacterPositions() {
        if (instance == null) return;
        instance.playerOvermap.SaveCharacterPosition();
        foreach (NPCOvermap npcOvermap in instance.npcList) { npcOvermap.SaveCharacterPosition(); }
        foreach (FollowerOvermap followerOvermap in instance.followerList) { followerOvermap.SaveCharacterPosition(); }
        foreach (EnemyOvermap enemyOvermap in instance.enemyList) { enemyOvermap.SaveCharacterPosition(); }
    }

    public event EventHandler OnOvermapStopped;

    public bool sleezerActive;

    private PlayerOvermap playerOvermap;
    private OvermapFtnDewHandler overmapFtnDewHandler;

    private bool overmapRunning;
    private List<EnemyOvermap> enemyList;
    private List<NPCOvermap> npcList;
    private List<FollowerOvermap> followerList;

    public OvermapHandler(PlayerOvermap playerOvermap) {
        instance = this;
        this.playerOvermap = playerOvermap;
        followerList = new List<FollowerOvermap>();
        npcList = new List<NPCOvermap>();
        enemyList = new List<EnemyOvermap>();
        overmapRunning = true;
    }

    public void Start(Transform transform) {
        overmapFtnDewHandler = new OvermapFtnDewHandler();
        UIBlack.Show();
        StopOvermapRunning();
        Window_PickName.Show_Static();
        Window_PickName.AddOption_Static("Custom Name (Length 4)", () => { });
        foreach (string badName in GameData.badNamesArray) {
            Window_PickName.AddOption_Static(badName, () => { Debug.Log(badName); });
        }
        Window_PickName.Hide_Static();
        StartOvermapRunning();
        UIBlack.Hide();


        foreach (Character character in GameData.characterList) {
            if (character.isDead) continue;
            if (character.IsEnemy()) {
                SpawnEnemy(character);
                continue;
            }

            switch (character.type) {
            case Character.Type.Player:
                playerOvermap.Setup(character);
                break;
            case Character.Type.Tank:
                if (character.subType == Character.SubType.Tank_BeforeJoin) {
                    // Before Tank joined, make him an NPC
                    SpawnNPC(character);
                } else {
                    // After Tank joined, make him a Follower
                    SpawnFollower(character, new Vector3(10, 0));
                }
                break;
            case Character.Type.Sleezer:
                if (character.subType == Character.SubType.Sleezer_Friendly) {
                    // After Tank joined, make him a Follower
                    SpawnFollower(character, new Vector3(10, -5));
                }
                break;
            case Character.Type.Healer:
                if (character.subType == Character.SubType.Healer_BeforeJoin) {
                    // Before Healer joined, make him an NPC
                    SpawnNPC(character);
                } else {
                    // After Healer joined, make him a Follower
                    SpawnFollower(character, new Vector3(10, 0));
                }
                break;
            case Character.Type.PlayerDoppelganger:
            case Character.Type.Shop:
            case Character.Type.Randy:
            case Character.Type.TavernAmbush:
            case Character.Type.TavernAmbush_2:
            case Character.Type.TavernAmbush_3:
                SpawnNPC(character);
                break;
            case Character.Type.Villager_1:
            case Character.Type.Villager_2:
            case Character.Type.Villager_3:
            case Character.Type.Villager_4:
            case Character.Type.Villager_5:
                NPCOvermap npcOvermap = SpawnNPC(character);
                foreach (Transform child in transform.Find("Map")) {
                    if (Vector3.Distance(npcOvermap.GetPosition(), child.position) < 1f) {
                        CharacterSetLastMoveDirData characterSetLastMoveDirData = child.GetComponent<CharacterSetLastMoveDirData>();
                        npcOvermap.SetLastMoveDir(characterSetLastMoveDirData.lastMoveDir);
                    }
                }
                break;
            }
        }


        foreach (Item item in GameData.itemList) {
            if (item.IsDestroyed()) continue;
            SpawnItem(item);
        }

        //OvermapHandler.SpawnNPC(new Vector3(0, 70));

        // Starting state
        switch (GameData.state) {
        case GameData.State.Start:
            Cutscenes.Play_Start();
            break;
        case GameData.State.DefeatedHurtMeDaddy:
        case GameData.State.DefeatedHurtMeDaddy_2:
            Window_QuestPointer.Create(GetNPC(GameData.GetCharacter(Character.Type.Tank)).GetPosition() + new Vector3(0, 10), Color.yellow, Color.yellow, crossSprite:GameAssets.i.s_ExclamationPoint);
            break;
        case GameData.State.DefeatedTank:
            Cutscenes.Play_Tank_AfterJoin();
            break;
        case GameData.State.SurvivedTavernAmbush:
            Cutscenes.Play_SurvivedTavernAmbush();
            break;
        case GameData.State.HealerJoined:
            Window_QuestPointer.Create(GameAssets.i.Map.Find("shop").position, Color.white, Color.white);
            break;
        case GameData.State.LostToEvilMonster_1:
            playerOvermap.SetPosition(GameAssets.i.Map.Find("dungeonPlayer").position);
            GetFollower(GameData.GetCharacter(Character.Type.Tank)).SetPosition(GameAssets.i.Map.Find("dungeonTank").position);
            GetFollower(GameData.GetCharacter(Character.Type.Sleezer)).SetPosition(GameAssets.i.Map.Find("dungeonSleezer").position);
            GetFollower(GameData.GetCharacter(Character.Type.Healer)).SetPosition(GameAssets.i.Map.Find("dungeonHealer").position);

            Cutscenes.Play_LostToEvilMonster_1();
            break;
        case GameData.State.MovingToEvilMonster_2:
            Window_QuestPointer.Create(GameAssets.i.Map.Find("evilMonster_2").position, Color.white, Color.white);
            break;
        case GameData.State.LostToEvilMonster_2:
            playerOvermap.SetPosition(GameAssets.i.Map.Find("dungeonPlayer").position);
            GetFollower(GameData.GetCharacter(Character.Type.Tank)).SetPosition(GameAssets.i.Map.Find("dungeonTank").position);
            GetFollower(GameData.GetCharacter(Character.Type.Sleezer)).SetPosition(GameAssets.i.Map.Find("dungeonSleezer").position);
            GetFollower(GameData.GetCharacter(Character.Type.Healer)).SetPosition(GameAssets.i.Map.Find("dungeonHealer").position);

            Cutscenes.Play_LostToEvilMonster_2();
            break;
        case GameData.State.MovingToEvilMonster_3:
            Window_QuestPointer.Create(GameAssets.i.Map.Find("evilMonster_3").position, Color.white, Color.white);
            break;
        case GameData.State.DefeatedEvilMonster:
            Cutscenes.Play_DefeatedEvilMonster();
            break;
        }

        transform.Find("Map").Find("Hitboxes").Find("TavernEntryHitbox").gameObject.SetActive(((int)GameData.state) < ((int)GameData.State.DefeatedTank));
        transform.Find("Map").Find("Hitboxes").Find("HurtMeDaddyHitbox").gameObject.SetActive(((int)GameData.state) < ((int)GameData.State.DefeatedHurtMeDaddy));
        transform.Find("Map").Find("Hitboxes").Find("HurtMeDaddy2Hitbox").gameObject.SetActive(((int)GameData.state) < ((int)GameData.State.DefeatedHurtMeDaddy_2));
        transform.Find("Map").Find("Hitboxes").Find("CastleBlockageHitbox").gameObject.SetActive(((int)GameData.state) < ((int)GameData.State.LostToEvilMonster_2));
        
        transform.Find("Map").Find("VillagerBlocker").gameObject.SetActive(((int)GameData.state) < ((int)GameData.State.DefeatedTank));

        transform.Find("Map").Find("Blockage").gameObject.SetActive(((int)GameData.state) < ((int)GameData.State.LostToEvilMonster_2));

        //transform.Find("Map").Find("Hitboxes").Find("HurtMeDaddyHitbox").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        //transform.Find("Map").Find("Hitboxes").Find("HurtMeDaddy2Hitbox").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);

        foreach (Transform hitboxTransform in transform.Find("Map").Find("Hitboxes")) {
            hitboxTransform.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        }

        //transform.Find("Map").Find("Hitboxes").Find("CastleBlockageHitbox").GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, .5f);

        //Window_QuestPointer.Create(new Vector3(150, 0), Color.white, Color.white);
        //ChatBubble.Create(playerOvermap.transform, new Vector3(3.5f, 5), "Oh Sleezer! You're so silly!");
        //ChatBubble.Create(playerOvermap.transform, new Vector3(3.5f, 5), "Hihihi");
    }

    public void Update() {
        overmapFtnDewHandler.Update();

        switch (GameData.state) {
        case GameData.State.GoingToTownCenter:
            //Vector3 townCenterPosition = GameAssets.i.Map.Find("townCenter").position;
            List<Vector3> positionList = new List<Vector3> {
                GameAssets.i.Map.Find("townCenter").position,
                GameAssets.i.Map.Find("townCenter_2").position,
                GameAssets.i.Map.Find("townCenter_3").position,
            };
            bool reachedPosition = false;
            foreach (Vector3 position in positionList) {
                if (Vector3.Distance(playerOvermap.GetPosition(), position) < 30f) {
                    reachedPosition = true;
                }
            }
            if (reachedPosition) {
                GameData.state = GameData.State.ArrivedAtTownCenter;
                Cutscenes.Play_ArrivedAtTownCenter();
            }
            break;
        case GameData.State.GoingToTavern:
            Vector3 tavernPosition = GameAssets.i.Map.Find("tavern").position;
            if (Vector3.Distance(playerOvermap.GetPosition(), tavernPosition) < 20f) {
                GameData.state = GameData.State.InTavern;
                Cutscenes.Play_Tavern();
            }
            break;
        case GameData.State.LeavingTown:
            //Vector3 leaveTownPosition = GameAssets.i.Map.Find("letsLeaveTown").position;
            positionList = new List<Vector3> {
                GameAssets.i.Map.Find("letsLeaveTown").position,
                GameAssets.i.Map.Find("letsLeaveTown_2").position,
                GameAssets.i.Map.Find("letsLeaveTown_3").position,
            };
            reachedPosition = false;
            foreach (Vector3 position in positionList) {
                if (Vector3.Distance(playerOvermap.GetPosition(), position) < 30f) {
                    reachedPosition = true;
                }
            }
            if (reachedPosition) {
                GameData.state = GameData.State.GoingToFirstEvilMonsterEncounter;
                Window_QuestPointer.DestroyPointer(GameAssets.i.Map.Find("letsLeaveTown").position);

                Window_QuestPointer.Create(GameAssets.i.Map.Find("evilMonster").position, Color.white, Color.white);
            }
            break;
        case GameData.State.GoingToFirstEvilMonsterEncounter:
            Vector3 evilMonsterPosition = GetEnemy(Character.Type.EvilMonster).GetPosition();
            if (Vector3.Distance(playerOvermap.GetPosition(), evilMonsterPosition) < 90f) {
                GameData.state = GameData.State.GoingToFightEvilMonster;
                Window_QuestPointer.DestroyPointer(GameAssets.i.Map.Find("evilMonster").position);

                Cutscenes.Play_SpottedEvilMonster();
            }
            break;
        case GameData.State.MovingToEvilMonster_2:
            evilMonsterPosition = GetEnemy(Character.Type.EvilMonster_2).GetPosition();
            if (Vector3.Distance(playerOvermap.GetPosition(), evilMonsterPosition) < 80f) {
                GameData.state = GameData.State.GoingToFightEvilMonster_2;
                Window_QuestPointer.DestroyPointer(GameAssets.i.Map.Find("evilMonster_2").position);
            }
            break;
        case GameData.State.MovingToEvilMonster_3:
            evilMonsterPosition = GetEnemy(Character.Type.EvilMonster_3).GetPosition();
            if (Vector3.Distance(playerOvermap.GetPosition(), evilMonsterPosition) < 80f) {
                GameData.state = GameData.State.GoingToFightEvilMonster_3;
                Window_QuestPointer.DestroyPointer(GameAssets.i.Map.Find("evilMonster_3").position);
            }
            break;
        }
    }

    public void SpawnSmoke(Vector3 position, float timer, Vector3 localScale) {
        FunctionTimer.Create(() => {
            Transform smokeTransform = UnityEngine.Object.Instantiate(GameAssets.i.pfSmokePuff, position, Quaternion.identity);
            smokeTransform.localScale = localScale;
        }, timer);
    }

    public Transform GetRandomCharacterTransform(bool includeSleezer) {
        List<Transform> list = new List<Transform> {
            GetPlayer().transform,
        };
        if (includeSleezer && GetFollower(GameData.GetCharacter(Character.Type.Sleezer)) != null) {
            list.Add(GetFollower(GameData.GetCharacter(Character.Type.Sleezer)).transform);
        }
        if (GetFollower(GameData.GetCharacter(Character.Type.Tank)) != null) {
            list.Add(GetFollower(GameData.GetCharacter(Character.Type.Tank)).transform);
        }
        if (GetFollower(GameData.GetCharacter(Character.Type.Healer)) != null) {
            list.Add(GetFollower(GameData.GetCharacter(Character.Type.Healer)).transform);
        }
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public NPCOvermap GetClosestNPC(Vector3 position, float maxDistance = float.MaxValue) {
        NPCOvermap closest = null;
        foreach (NPCOvermap npcOvermap in npcList) {
            if (Vector3.Distance(position, npcOvermap.GetPosition()) > maxDistance) continue; // Too far
            if (closest == null) {
                closest = npcOvermap;
            } else {
                if (Vector3.Distance(position, npcOvermap.GetPosition()) < Vector3.Distance(position, closest.GetPosition())) {
                    closest = npcOvermap;
                }
            }
        }
        return closest;
    }

    public PlayerOvermap GetPlayer() {
        return playerOvermap;
    }

    public NPCOvermap GetNPC(Character character) {
        foreach (NPCOvermap npc in npcList) {
            if (npc.GetCharacter() == character) {
                return npc;
            }
        }
        return null;
    }

    public FollowerOvermap GetFollower(Character character) {
        foreach (FollowerOvermap follower in followerList) {
            if (follower.GetCharacter() == character) {
                return follower;
            }
        }
        return null;
    }

    public EnemyOvermap GetEnemy(Character.Type characterType) {
        foreach (EnemyOvermap enemyOvermap in enemyList) {
            if (enemyOvermap.GetCharacter().type == characterType) {
                return enemyOvermap;
            }
        }
        return null;
    }

    public bool TryPlayerInteract() {
        NPCOvermap npcOvermap = GetClosestNPC(playerOvermap.GetPosition(), 20f);
        if (npcOvermap != null) {
            UIBlack.Show();
            StopOvermapRunning();
            return true;
        }
        return false;
    }

    public static bool IsOvermapRunning() {
        return instance.overmapRunning;
    }

    public static void StartOvermapRunning() {
        instance.overmapRunning = true;
    }

    public static void StopOvermapRunning() {
        instance.overmapRunning = false;
        if (instance.OnOvermapStopped != null) instance.OnOvermapStopped(instance, EventArgs.Empty);
    }
    
    public static void SpawnFollower(Character character, Vector3 followOffset) {
        Transform followerTransform = UnityEngine.Object.Instantiate(GameAssets.i.pfFollowerOvermap, character.position, Quaternion.identity);
        FollowerOvermap followerOvermap = followerTransform.GetComponent<FollowerOvermap>();
        followerOvermap.Setup(character, instance.playerOvermap, followOffset);
        instance.followerList.Add(followerOvermap);
    }
    
    public static void SpawnEnemy(Character character) {
        Transform enemyTransform = UnityEngine.Object.Instantiate(GameAssets.i.pfEnemyOvermap, character.position, Quaternion.identity);
        EnemyOvermap enemyOvermap = enemyTransform.GetComponent<EnemyOvermap>();
        enemyOvermap.Setup(character, instance.playerOvermap);
        instance.enemyList.Add(enemyOvermap);
    }
    
    public static NPCOvermap SpawnNPC(Character character) {
        Transform npcTransform = UnityEngine.Object.Instantiate(GameAssets.i.pfNPCOvermap, character.position, Quaternion.identity);
        NPCOvermap npcOvermap = npcTransform.GetComponent<NPCOvermap>();
        npcOvermap.Setup(character, instance.playerOvermap);
        instance.npcList.Add(npcOvermap);
        return npcOvermap;
    }
    
    public static void SpawnItem(Item item) {
        Transform prefab;
        switch (item.GetItemType()) {
        default:
            Debug.Log("########## Default Item Type: " + item.GetItemType());
            prefab = GameAssets.i.pfFtnDewItemOvermap;
            break;
        case Item.ItemType.FtnDew:
            prefab = GameAssets.i.pfFtnDewItemOvermap;
            break;
        case Item.ItemType.HealthPotion:
            prefab = GameAssets.i.pfHealthPotionItemOvermap;
            break;
        }
        Transform itemTransform = UnityEngine.Object.Instantiate(prefab, item.GetPosition(), Quaternion.identity);
        ItemOvermap itemOvermap = itemTransform.GetComponent<ItemOvermap>();
        itemOvermap.Setup(item, instance.playerOvermap);
        //instance.itemList.Add(itemOvermap);
    }

}
