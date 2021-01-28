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
using CodeMonkey;
using CodeMonkey.Utils;
using V_AnimationSystem;

public static class Cutscenes {


    public static void Play_Start() {
        OvermapHandler.StopOvermapRunning();
        UIBlack.Show();
        Character playerCharacter = GameData.GetCharacter(Character.Type.Player);
        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, false);
                dialogue.ShowLeftText("What happened?");
                dialogue.ShowLeftCharacterName("???");
                dialogue.HideRightCharacter();
                dialogue.HideRightText();
                dialogue.HideRightCharacterName();
            },
            () => {
                dialogue.ShowLeftText("What was I doing?");
            },
            () => {
                dialogue.ShowLeftText("Oh right, I was going to defeat the Evil Monster in the Evil Castle");
            },
            () => {
                dialogue.ShowLeftText("What was my name again?");
            },
            () => {
                dialogue.Hide();
                Window_KeyContinue.Hide_Static();
                Window_PickName.ShowAvailableNames((string selectedName) => {
                    playerCharacter.name = selectedName;
                    dialogue.Show();
                    dialogue.PlayNextAction();
                });
            },
            () => {
                dialogue.ShowLeftText("Oh that's right, I'm " + playerCharacter.name);
                dialogue.ShowLeftCharacterName(playerCharacter.name);
            },
            () => {
                dialogue.ShowLeftText("Alright let's go!");
            },
            () => {
                dialogue.Hide();
                UIBlack.Hide();
                OvermapHandler.StartOvermapRunning();
                //GameData.state = GameData.State.GoingToTownCenter;
            },
        }, true);
    }

    public static void Play_HurtMeDaddy(Character character) {
        // First Minion
        OvermapHandler.StopOvermapRunning();
        UIBlack.Show();
        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, true);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.HideLeftText();
                dialogue.ShowRightCharacter(GameAssets.i.s_EnemyMinionOrangePortrait, false);
                dialogue.ShowRightText("Myah! Hurt me daddy!");
                dialogue.HideRightCharacterName();
            },
            () => {
                dialogue.ShowLeftActiveTalkerHideRight("What?");
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("Hurt me daddy!");
            },
            () => {
                dialogue.ShowLeftActiveTalkerHideRight("That's weird...");
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("Hurt me daddy!");
            },
            () => {
                GameData.state = GameData.State.FightingHurtMeDaddy;
                BattleHandler.LoadEnemyEncounter(character, character.enemyEncounter);
            },
        }, true);
    }

    public static void Play_HurtMeDaddy_2(Character character) {
        // Second Minion
        OvermapHandler.StopOvermapRunning();
        UIBlack.Show();
        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, true);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowRightCharacter(GameAssets.i.s_EnemyMinionRedPortrait, false);
                dialogue.ShowRightText("Hey you! Have you seen a short guy around here?");
                dialogue.HideRightCharacterName();
            },
            () => {
                dialogue.ShowLeftActiveTalkerHideRight("Hum...");
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("Wait a second!");
            },
            () => {
                dialogue.ShowRightText("Did you do it?!");
            },
            () => {
                dialogue.ShowRightText("You did!");
            },
            () => {
                dialogue.ShowRightText("How dare you hurt him!");
            },
            () => {
                GameData.state = GameData.State.FightingHurtMeDaddy_2;
                BattleHandler.LoadEnemyEncounter(character, character.enemyEncounter);
            },
        }, true);
    }

    public static void Play_Tank_BeforeJoin() {
        // First meeting with Tank
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Dialogue dialogue = Dialogue.GetInstance();
        Character tankCharacter = GameData.GetCharacter(Character.Type.Tank);
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, false);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowRightCharacter(GameAssets.i.s_TankPortrait, true);
                //dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Tank));
                dialogue.ShowRightCharacterName("???");
                dialogue.ShowLeftText("Greetings!");
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("Entrance to the town is restricted");
            },
            () => {
                dialogue.ShowLeftActiveTalkerHideRight("But I need to get past to get to the Evil Castle to defeat the Evil Monster!");
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("There are reports of Evil Minions roaming these lands, no one is allowed in until they are taken care of");
            },
            () => {
                dialogue.ShowLeftActiveTalkerHideRight("Some short guys? I took care of them!");
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("I don't believe you!");
            },
            () => {
                dialogue.ShowRightText("Show me your skills!");
            },
            () => {
                GameData.state = GameData.State.FightingTank;
                BattleHandler.LoadEnemyEncounter(tankCharacter, tankCharacter.enemyEncounter);
            },
        }, true);
    }

    public static void Play_Tank_AfterJoin() {
        // After Defeating the Tank, Tank Joins team
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Character playerCharacter = GameData.GetCharacter(Character.Type.Player);
        Character tankCharacter = GameData.GetCharacter(Character.Type.Tank);

        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, true);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowRightCharacter(GameAssets.i.s_TankPortrait, false);
                dialogue.ShowRightCharacterName("???");
                dialogue.ShowRightText("Ok ok you win!");
            },
            () => { dialogue.ShowRightText("You are such a strong Warrior, my apologies for doubting you"); },
            () => { dialogue.ShowLeftActiveTalkerHideRight("That's okay, you're quite strong yourself!"); },
            () => { dialogue.ShowRightActiveTalkerHideLeft("Please allow me to redeem myself by joining you on your quest to defeat the Evil Monster!"); },
            () => { dialogue.ShowLeftActiveTalkerHideRight("Sure, help is always needed, my name is " + playerCharacter.name); },
            () => { dialogue.ShowRightActiveTalkerHideLeft("And I'm..."); },
            () => {
                dialogue.Hide();
                Window_PickName.ShowAvailableNames((string selectedName) => {
                    tankCharacter.name = selectedName;
                    dialogue.Show();
                    dialogue.PlayNextAction();
                });
            },
            () => { 
                dialogue.ShowRightActiveTalkerHideLeft("I'm " + tankCharacter.name);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Tank));
            },
            () => { dialogue.ShowRightText("Oh hey and this is my buddy Sleezer!"); },
            () => { dialogue.ShowRightText("He goes everywhere with me"); },
            () => { dialogue.ShowRightText("He's a bit of a goofball but he's good company"); },
            () => { 
                dialogue.ShowRightCharacter(GameAssets.i.s_SleezerPortrait, false);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Sleezer));
                //dialogue.ShowRightCharacterName("???");
                dialogue.ShowRightText("Hihihi that's me!"); 
            },
            () => { dialogue.ShowLeftActiveTalkerHideRight("Oh Sleezer! You're adorable!"); },
            () => { dialogue.ShowLeftText("Alright, onwards on our Quest!"); },
            () => {
                dialogue.Hide();
                UIBlack.Hide();
                OvermapHandler.StartOvermapRunning();
                GameData.state = GameData.State.GoingToTownCenter;
                Window_QuestPointer.Create(GameAssets.i.Map.Find("townCenter").position, Color.white, Color.white);
            },
        }, true);
    }

    public static void Play_ArrivedAtTownCenter() {
        Window_QuestPointer.DestroyPointer(GameAssets.i.Map.Find("townCenter").position);
        GameData.state = GameData.State.GoingToAskDoppelGanger;
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, true);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowRightCharacter(GameAssets.i.s_TankPortrait, false);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Tank));
                dialogue.HideLeftText();
                dialogue.ShowRightText("We should get some refreshments before going on our Quest");
            },
            () => { dialogue.ShowLeftActiveTalkerHideRight("Good idea, let's find the tavern"); },
            () => {
                dialogue.Hide();
                UIBlack.Hide();
                OvermapHandler.StartOvermapRunning();
                Window_QuestPointer.Create(OvermapHandler.GetInstance().GetNPC(GameData.GetCharacter(Character.Type.PlayerDoppelganger)).GetPosition() + new Vector3(0, 10), Color.yellow, Color.yellow, crossSprite:GameAssets.i.s_ExclamationPoint);
            },
        }, true);
    }

    public static void Play_DoppelGanger() {
        Window_QuestPointer.DestroyPointer(OvermapHandler.GetInstance().GetNPC(GameData.GetCharacter(Character.Type.PlayerDoppelganger)).GetPosition() + new Vector3(0, 10));
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, false);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowRightCharacter(GameAssets.i.s_PlayerDialogueSprite, true);
                dialogue.ShowLeftText("Greetings, can you tell me where the Tavern is?");
                dialogue.HideRightText();
                dialogue.HideRightCharacterName();
            },
            () => { dialogue.ShowRightActiveTalkerHideLeft("It's just over there"); },
            () => { dialogue.ShowLeftActiveTalkerHideRight("Ok thank you!"); },
            () => {
                dialogue.Hide();
                UIBlack.Hide();
                OvermapHandler.StartOvermapRunning();
                Window_QuestPointer.Create(GameAssets.i.Map.Find("tavern").position, Color.white, Color.white);
                GameData.state = GameData.State.GoingToTavern;
            },
        }, true);
    }

    public static void Play_Tavern() {
        Window_QuestPointer.DestroyPointer(GameAssets.i.Map.Find("tavern").position);
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Dialogue dialogue = Dialogue.GetInstance();
        Character tavernAmbushCharacter = GameData.GetCharacter(Character.Type.TavernAmbush);
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, false);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowLeftText("Greetings!");
                dialogue.HideRightCharacter();
                dialogue.HideRightText();
                dialogue.HideRightCharacterName();
            },
            () => { dialogue.ShowLeftText("May we have some refreshments?"); },
            () => {
                UIBlack.Hide();
                dialogue.Hide();
                
                Vector3 playerPosition = OvermapHandler.GetInstance().GetPlayer().GetPosition();
                OvermapHandler.GetInstance().GetPlayer().GetUnitAnimation().PlayAnimForced(UnitAnim.GetUnitAnim("dSwordTwoHandedBack_IdleDown"), 1f, null);
                
                NPCOvermap enemyOvermap = OvermapHandler.GetInstance().GetNPC(tavernAmbushCharacter);
                enemyOvermap.transform.position = playerPosition + new Vector3(0, -17f);
                enemyOvermap.GetUnitAnimation().PlayAnimForced(UnitAnim.GetUnitAnim("dMinion_IdleUp"), 1f, null);
                OvermapHandler.GetInstance().SpawnSmoke(enemyOvermap.GetPosition(), .3f, Vector3.one);

                NPCOvermap enemyOvermap_2 = OvermapHandler.GetInstance().GetNPC(GameData.GetCharacter(Character.Type.TavernAmbush_2));
                enemyOvermap_2.transform.position = playerPosition + new Vector3(14f, -12f);
                enemyOvermap_2.GetUnitAnimation().PlayAnimForced(UnitAnim.GetUnitAnim("dMinion_IdleUp"), 1f, null);
                OvermapHandler.GetInstance().SpawnSmoke(enemyOvermap_2.GetPosition(), .3f, Vector3.one);

                NPCOvermap enemyOvermap_3 = OvermapHandler.GetInstance().GetNPC(GameData.GetCharacter(Character.Type.TavernAmbush_3));
                enemyOvermap_3.transform.position = playerPosition + new Vector3(-14, -12f);
                enemyOvermap_3.GetUnitAnimation().PlayAnimForced(UnitAnim.GetUnitAnim("dMinion_IdleUp"), 1f, null);
                OvermapHandler.GetInstance().SpawnSmoke(enemyOvermap_3.GetPosition(), .3f, Vector3.one);
                
                enemyOvermap.gameObject.SetActive(false);
                enemyOvermap_2.gameObject.SetActive(false);
                enemyOvermap_3.gameObject.SetActive(false);
                FunctionTimer.Create(() => {
                    enemyOvermap.gameObject.SetActive(true);
                    enemyOvermap_2.gameObject.SetActive(true);
                    enemyOvermap_3.gameObject.SetActive(true);
                }, .45f);

                FunctionTimer.Create(() => {
                    UIBlack.Show();
                    dialogue.Show();
                    dialogue.PlayNextAction();
                }, 1.2f);
            },
            () => {
                dialogue.HideLeftText();
                dialogue.FadeLeftCharacter();
                dialogue.ShowRightCharacter(GameAssets.i.s_EnemyMinionRedPortrait, false);
                dialogue.ShowRightText("That's the one! Get him!"); 
            },
            () => {
                GameData.GetCharacter(Character.Type.TavernAmbush_2).isDead = true;
                GameData.GetCharacter(Character.Type.TavernAmbush_3).isDead = true;
                GameData.state = GameData.State.FightingTavernAmbush;
                BattleHandler.LoadEnemyEncounter(tavernAmbushCharacter, tavernAmbushCharacter.enemyEncounter);
            },
        }, true);
    }

    public static void Play_SurvivedTavernAmbush() {
        // Healer after Tavern ambush
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Dialogue dialogue = Dialogue.GetInstance();
        Character healerCharacter = GameData.GetCharacter(Character.Type.Healer);
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, true);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.HideLeftText();
                dialogue.ShowRightCharacter(GameAssets.i.s_HealerPortrait, false);
                dialogue.ShowRightCharacterName("???");
                dialogue.ShowRightText("You've saved us all!");
            },
            () => { dialogue.ShowRightText("Oh no, you're hurt, let me help you"); },
            () => {
                UIBlack.Hide();
                dialogue.Hide();
                // Player Heal
                FunctionTimer.Create(() => {
                    OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Tank)).Heal(200);
                    OvermapHandler.GetInstance().GetPlayer().Heal(200);
                }, .2f);
                FunctionTimer.Create(() => {
                    /*
                    Character uniqueCharacter = GameData.GetCharacter(Character.Type.Player);
                    uniqueCharacter.stats.health = uniqueCharacter.stats.healthMax;
                    uniqueCharacter = GameData.GetCharacter(Character.Type.Tank);
                    uniqueCharacter.stats.health = uniqueCharacter.stats.healthMax;
                    */

                    UIBlack.Show();
                    dialogue.Show();
                    dialogue.ShowRightText("You look like a strong warrior on a dangerous quest, need help?");
                }, .5f);
            },
            () => { dialogue.ShowLeftActiveTalkerHideRight("Sure thing, the path will be dangerous so it will be nice to have a healer"); },
            () => { dialogue.ShowLeftText("What to they call you?"); },
            () => { dialogue.ShowRightActiveTalkerHideLeft("My name is..."); },
            () => {
                dialogue.Hide();
                Window_PickName.ShowAvailableNames((string selectedName) => {
                    healerCharacter.name = selectedName;
                    dialogue.Show();
                    dialogue.PlayNextAction();
                });
            },
            () => { 
                dialogue.ShowRightText("My name is " + healerCharacter.name); 
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Healer));
            },
            () => { dialogue.ShowLeftActiveTalkerHideRight("Nice to meet you " + healerCharacter.name); },
            () => { dialogue.ShowRightActiveTalkerHideLeft("We should pick up some supplies before we go"); },
            () => { dialogue.ShowLeftActiveTalkerHideRight("Alright, let's go"); },
            () => {
                GameData.state = GameData.State.HealerJoined;
                healerCharacter.isInPlayerTeam = true;
                healerCharacter.subType = Character.SubType.Healer_Friendly;
                OvermapHandler.SaveAllCharacterPositions();
                Loader.Load(Loader.Scene.GameScene);
            },
        }, true);
    }

    public static void Play_Shop(Character shopCharacter) {
        if (GameData.state == GameData.State.HealerJoined) {
            Window_QuestPointer.DestroyPointer(GameAssets.i.Map.Find("shop").position);
            GameData.state = GameData.State.LeavingTown;
            Window_QuestPointer.Create(GameAssets.i.Map.Find("letsLeaveTown").position, Color.white, Color.white);
        }


        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, true);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.HideLeftText();
                dialogue.ShowRightCharacter(GameAssets.i.s_VendorPortrait, false);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Shop));
                dialogue.ShowRightText("Greetings! Care to browse my wares?");
            },
            () => {
                dialogue.Hide();

                Window_Shop.Show_Static(shopCharacter.shopContents, () => {
                    UIBlack.Hide();
                    OvermapHandler.StartOvermapRunning();
                });
            },
        }, true);
    }

    public static void Play_SpottedEvilMonster() {
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, false);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowLeftText("Wait a second...");
                dialogue.HideRightCharacter();
                dialogue.HideRightText();
            },
            () => {
                dialogue.ShowLeftText("That's him!");
            },
            () => {
                dialogue.Hide();
                UIBlack.Hide();
                OvermapHandler.StartOvermapRunning();
            },
        }, true);
    }

    public static void Play_EvilMonster_1(Character evilMonsterCharacter) {
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, false);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowLeftText("It's you!");
                dialogue.ShowRightCharacter(GameAssets.i.s_EvilMonsterPortrait, true);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.EvilMonster));
                dialogue.HideRightText();
            },
            () => {
                dialogue.ShowLeftText("I've come to rid you of this world!");
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("Mwahahaha let's see you try!");
            },
            () => {
                GameData.state = GameData.State.FightingEvilMonster_1;
                BattleHandler.LoadEnemyEncounter(evilMonsterCharacter, evilMonsterCharacter.enemyEncounter);
            },
        }, true);
    }

    public static void Play_LostToEvilMonster_1() {
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();

        List<World_Sprite> ropeWorldSpriteList = new List<World_Sprite>();
        ropeWorldSpriteList.Add(World_Sprite.Create(OvermapHandler.GetInstance().GetPlayer().GetPosition(), Vector3.one * 1f, GameAssets.i.s_Rope, Color.white, 100));
        ropeWorldSpriteList.Add(World_Sprite.Create(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Healer)).GetPosition(), Vector3.one * 1f, GameAssets.i.s_Rope, Color.white, 100));
        ropeWorldSpriteList.Add(World_Sprite.Create(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Tank)).GetPosition(), Vector3.one * 1.2f, GameAssets.i.s_Rope, Color.white, 100));
        ropeWorldSpriteList.Add(World_Sprite.Create(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Sleezer)).GetPosition(), Vector3.one * 0.7f, GameAssets.i.s_Rope, Color.white, 100));
        
        NPCOvermap randyOvermap = OvermapHandler.GetInstance().GetNPC(GameData.GetCharacter(Character.Type.Randy));
        randyOvermap.GetUnitAnimation().PlayAnimForced(UnitAnim.GetUnitAnim("dBareHands_IdleUp"), 1f, null);
        Vector3 randyStartingPosition = randyOvermap.GetPosition();

        randyOvermap.overrideOvermapRunning = true;

        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, false);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowLeftText("Oh come on, we had him! What happened?");
                dialogue.ShowRightCharacter(GameAssets.i.s_TankPortrait, true);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Tank));
                dialogue.HideRightText();
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("Seems he overpowered us somehow...");
            },
            () => {
                dialogue.ShowRightCharacter(GameAssets.i.s_RandyPortrait, false);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Randy));
                dialogue.ShowRightActiveTalkerHideLeft("Oh my, aren't you precious");
            },
            () => { dialogue.ShowRightText("You boys are quite fancy huhuhu"); },
            () => { dialogue.ShowLeftActiveTalkerHideRight("Who are you? Are you here to torture us?"); },
            () => { dialogue.ShowRightActiveTalkerHideLeft("Oh my, I could never torture such delicate specimens"); },
            () => { dialogue.ShowRightText("You are too pretty to be tied up"); },
            () => { dialogue.ShowRightText("Allow me to help you huhuhu"); },
            () => {
                // Randy Disappears in Smoke
                UIBlack.Hide();
                dialogue.Hide();

                // Untie Player
                randyOvermap.SetTargetMovePosition(OvermapHandler.GetInstance().GetPlayer().GetPosition() + new Vector3(0, -5));
                FunctionTimer.Create(() => { 
                    ropeWorldSpriteList[0].DestroySelf();
                }, 1f);

                // Untie Healer
                FunctionTimer.Create(() => { 
                    randyOvermap.SetTargetMovePosition(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Healer)).GetPosition() + new Vector3(+5, 0));
                }, 1f);
                FunctionTimer.Create(() => { 
                    ropeWorldSpriteList[1].DestroySelf();
                }, 2f);

                // Untie Tank
                FunctionTimer.Create(() => { 
                    randyOvermap.SetTargetMovePosition(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Tank)).GetPosition() + new Vector3(-5, 0));
                }, 2f);
                FunctionTimer.Create(() => { 
                    ropeWorldSpriteList[2].DestroySelf();
                }, 3f);

                // Untie Sleezer
                FunctionTimer.Create(() => { 
                    randyOvermap.SetTargetMovePosition(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Sleezer)).GetPosition() + new Vector3(-5, 0));
                }, 3f);
                FunctionTimer.Create(() => { 
                    ropeWorldSpriteList[3].DestroySelf();
                }, 4f);
                
                // Go to disappear
                FunctionTimer.Create(() => { 
                    randyOvermap.SetTargetMovePosition(randyStartingPosition);
                }, 4f);
                FunctionTimer.Create(() => { 
                    OvermapHandler.GetInstance().SpawnSmoke(randyOvermap.GetPosition(), .0f, Vector3.one);
                }, 5f);
                FunctionTimer.Create(() => {
                    randyOvermap.Hide();
                }, 5.5f);

                FunctionTimer.Create(() => {
                    UIBlack.Show();
                    dialogue.Show();
                    dialogue.PlayNextAction();
                }, 7.5f);
            },
            () => { 
                dialogue.ShowLeftActiveTalkerHideRight("Well that was weird...");
                dialogue.HideRightCharacterName();
                dialogue.HideRightCharacter();
            },
            () => { 
                dialogue.ShowLeftText("But hey lets keep going! He's not getting away this time");
            },
            /*
            () => {
                NPCOvermap npcOvermap = OvermapHandler.GetInstance().GetNPC(GameData.GetCharacter(Character.Type.Randy));
                npcOvermap.SetTargetMovePosition();
                dialogue.Hide();
                UIBlack.Hide();
                OvermapHandler.StartOvermapRunning();
            },*/
            () => {
                dialogue.Hide();
                UIBlack.Hide();
                OvermapHandler.StartOvermapRunning();
                GameData.state = GameData.State.MovingToEvilMonster_2;
                Window_QuestPointer.Create(GameAssets.i.Map.Find("evilMonster_2").position, Color.white, Color.white);
            },
        }, true);
    }

    
    public static void Play_EvilMonster_2(Character evilMonsterCharacter) {
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, false);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowLeftText("You're not getting away this time!");
                dialogue.ShowRightCharacter(GameAssets.i.s_EvilMonsterPortrait, true);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.EvilMonster));
                dialogue.HideRightText();
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("Mwahahaha let's see!");
            },
            () => {
                GameData.state = GameData.State.FightingEvilMonster_2;
                BattleHandler.LoadEnemyEncounter(evilMonsterCharacter, evilMonsterCharacter.enemyEncounter);
            },
        }, true);
    }

    public static void Play_LostToEvilMonster_2() {
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        
        List<World_Sprite> ropeWorldSpriteList = new List<World_Sprite>();
        ropeWorldSpriteList.Add(World_Sprite.Create(OvermapHandler.GetInstance().GetPlayer().GetPosition(), Vector3.one * 1f, GameAssets.i.s_Rope, Color.white, 100));
        ropeWorldSpriteList.Add(World_Sprite.Create(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Healer)).GetPosition(), Vector3.one * 1f, GameAssets.i.s_Rope, Color.white, 100));
        ropeWorldSpriteList.Add(World_Sprite.Create(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Tank)).GetPosition(), Vector3.one * 1.2f, GameAssets.i.s_Rope, Color.white, 100));
        ropeWorldSpriteList.Add(World_Sprite.Create(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Sleezer)).GetPosition(), Vector3.one * 0.7f, GameAssets.i.s_Rope, Color.white, 100));
        
        NPCOvermap randyOvermap = OvermapHandler.GetInstance().GetNPC(GameData.GetCharacter(Character.Type.Randy));
        randyOvermap.GetUnitAnimation().PlayAnimForced(UnitAnim.GetUnitAnim("dBareHands_IdleUp"), 1f, null);
        Vector3 randyStartingPosition = randyOvermap.GetPosition();

        randyOvermap.overrideOvermapRunning = true;

        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, true);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.HideLeftText();
                dialogue.ShowRightCharacter(GameAssets.i.s_RandyPortrait, false);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Randy));
                dialogue.ShowRightText("Oh my, this is my lucky day");
            },
            () => {
                dialogue.ShowLeftActiveTalkerHideRight("Oh no, you again");
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("What's wrong, not happy to see me?");
            },
            () => { dialogue.ShowLeftActiveTalkerHideRight("It seems we miraculously lost again"); },
            () => { dialogue.ShowLeftText("It's very annoying to get the Evil Monster almost near death and suddenly he wins"); },
            () => { dialogue.ShowRightActiveTalkerHideLeft("Oh dear that is a real bother"); },
            () => { dialogue.ShowLeftActiveTalkerHideRight("Yes it is, are you going to let us go?"); },
            () => { dialogue.ShowRightActiveTalkerHideLeft("Huhuhu maybe"); },
            () => { dialogue.ShowRightText("My oh my, you are truly precious"); },
            () => { dialogue.ShowRightText("If you really want to defeat him you need the Sword of Thousand Truths"); },
            () => { dialogue.ShowRightText("And would you look at that, I have it right here!"); },
            () => { dialogue.ShowRightText("Have fun! Huhuhu!"); },
            () => {
                // Randy Disappears in Smoke
                // Drop Sword
                /*
                NPCOvermap npcOvermap = OvermapHandler.GetInstance().GetNPC(GameData.GetCharacter(Character.Type.Randy));
                Transform objectTractorTransform = UnityEngine.Object.Instantiate(GameAssets.i.pfObjectTractor, npcOvermap.GetPosition(), Quaternion.identity);
                objectTractorTransform.GetComponent<TractorBehaviour>().Setup(OvermapHandler.GetInstance().GetPlayer(), 20f, 8f, () => {
                    GameData.GetCharacter(Character.Type.Player).hasSwordThousandTruths = true;
                    OvermapHandler.GetInstance().GetPlayer().RefreshTexture();
                });

                npcOvermap.Hide();
                UIBlack.Hide();
                dialogue.Hide();
                FunctionTimer.Create(() => {
                    UIBlack.Show();
                    dialogue.Show();
                    dialogue.PlayNextAction();
                }, 1f);
                */

                
                Transform objectTractorTransform = UnityEngine.Object.Instantiate(GameAssets.i.pfObjectTractor, randyOvermap.GetPosition(), Quaternion.identity);
                objectTractorTransform.localEulerAngles = new Vector3(0, 0, -45);
                objectTractorTransform.GetComponent<TractorBehaviour>().Setup(OvermapHandler.GetInstance().GetPlayer(), 20f, 8f, () => {
                    GameData.GetCharacter(Character.Type.Player).hasSwordThousandTruths = true;
                    OvermapHandler.GetInstance().GetPlayer().RefreshTexture();
                    SoundManager.PlaySound(SoundManager.Sound.Coin);
                });

                // Randy Disappears in Smoke
                UIBlack.Hide();
                dialogue.Hide();

                // Untie Player
                randyOvermap.SetTargetMovePosition(OvermapHandler.GetInstance().GetPlayer().GetPosition() + new Vector3(0, -5));
                FunctionTimer.Create(() => { 
                    ropeWorldSpriteList[0].DestroySelf();
                }, 1f);

                // Untie Healer
                FunctionTimer.Create(() => { 
                    randyOvermap.SetTargetMovePosition(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Healer)).GetPosition() + new Vector3(+5, 0));
                }, 1f);
                FunctionTimer.Create(() => { 
                    ropeWorldSpriteList[1].DestroySelf();
                }, 2f);

                // Untie Tank
                FunctionTimer.Create(() => { 
                    randyOvermap.SetTargetMovePosition(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Tank)).GetPosition() + new Vector3(-5, 0));
                }, 2f);
                FunctionTimer.Create(() => { 
                    ropeWorldSpriteList[2].DestroySelf();
                }, 3f);

                // Untie Sleezer
                FunctionTimer.Create(() => { 
                    randyOvermap.SetTargetMovePosition(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Sleezer)).GetPosition() + new Vector3(-5, 0));
                }, 3f);
                FunctionTimer.Create(() => { 
                    ropeWorldSpriteList[3].DestroySelf();
                }, 4f);
                
                // Go to disappear
                FunctionTimer.Create(() => { 
                    randyOvermap.SetTargetMovePosition(randyStartingPosition);
                }, 4f);
                FunctionTimer.Create(() => { 
                    OvermapHandler.GetInstance().SpawnSmoke(randyOvermap.GetPosition(), .0f, Vector3.one);
                }, 5f);
                FunctionTimer.Create(() => {
                    randyOvermap.Hide();
                }, 5.5f);

                FunctionTimer.Create(() => {
                    UIBlack.Show();
                    dialogue.Show();
                    dialogue.PlayNextAction();
                }, 6.5f);

            },
            () => { 
                dialogue.ShowLeftActiveTalkerHideRight("He's very odd but somehow very helpful"); 
                dialogue.HideRightCharacter();
                dialogue.HideRightCharacterName();
            },
            () => { dialogue.ShowLeftText("Why would the Evil Monster keep him around?"); },
            () => { dialogue.ShowLeftText("Anyways who cares we have the Sword of a Thousand Truths!"); },
            () => { 
                dialogue.ShowLeftText("Let's get him once and for all!");
            },
            () => {
                dialogue.Hide();
                UIBlack.Hide();
                OvermapHandler.StartOvermapRunning();
                GameData.state = GameData.State.MovingToEvilMonster_3;
                Window_QuestPointer.Create(GameAssets.i.Map.Find("evilMonster_3").position, Color.white, Color.white);
            },
        }, true);
    }

    public static void Play_EvilMonster_3(Character evilMonsterCharacter) {
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, false);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowLeftText("Back again!");
                dialogue.ShowRightCharacter(GameAssets.i.s_EvilMonsterPortrait, true);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.EvilMonster));
                dialogue.HideRightText();
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("Now you're becoming a nuisance!");
            },
            () => {
                dialogue.ShowRightText("This time I'll end you!");
            },
            () => {
                GameData.state = GameData.State.FightingEvilMonster_3;
                BattleHandler.LoadEnemyEncounter(evilMonsterCharacter, evilMonsterCharacter.enemyEncounter);
            },
        }, true);
    }

    public static void Play_DefeatedEvilMonster() {
        UIBlack.Show();
        OvermapHandler.StopOvermapRunning();
        Dialogue dialogue = Dialogue.GetInstance();
        dialogue.SetDialogueActions(new List<Action>() {
            () => {
                dialogue.Show();
                dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, false);
                dialogue.ShowLeftCharacterName(GameData.GetCharacterName(Character.Type.Player));
                dialogue.ShowLeftText("We did it!");
                dialogue.ShowRightCharacter(GameAssets.i.s_SleezerPortrait, true);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Sleezer));
                dialogue.HideRightText();
            },
            () => {
                dialogue.ShowLeftText("Sleezer, you defeated him!");
            },
            () => {
                dialogue.ShowRightActiveTalkerHideLeft("Hihihi that's me!");
            },
            () => {
                dialogue.ShowLeftActiveTalkerHideRight("It's finally over, we defeated the Evil Monster!");
            },
            () => {
                dialogue.ShowLeftText("Thank you all for coming with me on this quest");
            },
            () => {
                dialogue.FadeLeftCharacter();
                dialogue.HideLeftText();
                dialogue.ShowRightCharacter(GameAssets.i.s_TankPortrait, false);
                dialogue.ShowRightText("It was an honor to fight alongside you!");
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Tank));
            },
            () => {
                dialogue.ShowRightCharacter(GameAssets.i.s_HealerPortrait, false);
                dialogue.ShowRightText("Yes, I'm glad we joined you on this quest");
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Healer));
            },
            () => {
                dialogue.ShowRightText("If you ever need help we'll be right here");
            },
            () => {
                dialogue.ShowLeftActiveTalkerHideRight("Here's to the savior of the world, Sleezer!");
            },
            () => {
                dialogue.FadeLeftCharacter();
                dialogue.HideLeftText();
                dialogue.ShowRightCharacter(GameAssets.i.s_SleezerPortrait, false);
                dialogue.ShowRightCharacterName(GameData.GetCharacterName(Character.Type.Sleezer));
                dialogue.ShowRightText("Hihihi that's me!");
            },
            () => {
                GameData.state = GameData.State.GameOver;
                Loader.Load(Loader.Scene.GameOver);
            },
        }, true);
    }


}
