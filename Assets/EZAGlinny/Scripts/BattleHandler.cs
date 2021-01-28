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
using V_AnimationSystem;
using CodeMonkey.Utils;
using CodeMonkey.MonoBehaviours;

public class BattleHandler {

    private static BattleHandler instance;
    public static BattleHandler GetInstance() => instance;

    public static Character character;
    public static GameData.EnemyEncounter enemyEncounter;

    public static void LoadEnemyEncounter(Character character, GameData.EnemyEncounter enemyEncounter) {
        OvermapHandler.SaveAllCharacterPositions();
        BattleHandler.character = character;
        BattleHandler.enemyEncounter = enemyEncounter;
        Loader.Load(Loader.Scene.BattleScene);
    }


    private int turn;
    private CameraFollow cameraFollow;
    private List<CharacterBattle> characterBattleList;
    private CharacterBattle activeCharacterBattle;
    private CharacterBattle selectedTargetCharacterBattle;

    private LanePosition lastPlayerActiveLanePosition;
    private LanePosition lastEnemyActiveLanePosition;

    private State state;

    public enum LanePosition {
        Middle,
        Up,
        Down,
        Top,
        Bottom,
    }

    private enum State {
        WaitingForPlayer,
        Busy,
    }

    public BattleHandler() {
        instance = this;
        turn = 0;
        characterBattleList = new List<CharacterBattle>();
        state = State.Busy;
    }

    public static void SpawnCharacter(Character.Type characterType, BattleHandler.LanePosition lanePosition, bool isPlayerTeam, Character.Stats stats) {
        Transform characterTransform = Object.Instantiate(GameAssets.i.pfCharacterBattle, GetPosition(lanePosition, isPlayerTeam), Quaternion.identity);
        CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(characterType, lanePosition, GetPosition(lanePosition, isPlayerTeam), isPlayerTeam, stats);
        instance.characterBattleList.Add(characterBattle);
    }

    public static Vector3 GetPosition(LanePosition lanePosition, bool isPlayerTeam) {
        float playerTeamMultiplier = isPlayerTeam ? -1f : 1f;

        switch (lanePosition) {
        default:
        case LanePosition.Middle:   return new Vector3(playerTeamMultiplier * 50f, 0);
        case LanePosition.Up:       return new Vector3(playerTeamMultiplier * 60f, +20);
        case LanePosition.Down:     return new Vector3(playerTeamMultiplier * 60f, -20);
        case LanePosition.Top:      return new Vector3(playerTeamMultiplier * 70f, +35);
        case LanePosition.Bottom:   return new Vector3(playerTeamMultiplier * 70f, -35);
        }

    }

    public static LanePosition GetNextLanePosition(LanePosition lanePosition, bool moveUp) {
        if (moveUp) {
            switch (lanePosition) {
            default:
            case LanePosition.Middle:   return LanePosition.Up;
            case LanePosition.Up:       return LanePosition.Top;
            case LanePosition.Top:      return LanePosition.Bottom;
            case LanePosition.Bottom:   return LanePosition.Down;
            case LanePosition.Down:     return LanePosition.Middle;
            }
        } else {
            switch (lanePosition) {
            default:
            case LanePosition.Middle:   return LanePosition.Down;
            case LanePosition.Down:     return LanePosition.Bottom;
            case LanePosition.Bottom:   return LanePosition.Top;
            case LanePosition.Top:      return LanePosition.Up;
            case LanePosition.Up:       return LanePosition.Middle;
            }
        }
    }

    public void Start(CameraFollow cameraFollow) {
        this.cameraFollow = cameraFollow;
        cameraFollow.Setup(() => Vector3.zero, () => 50f, true, true);
        cameraFollow.SetCameraMoveSpeed(10f);
        cameraFollow.SetCameraZoomSpeed(10f);
        ResetCamera();

        // Set up Battle Scene
        foreach (Character character in GameData.characterList) {
            if (character.isInPlayerTeam) {
                LanePosition lanePosition;
                switch (character.type) {
                default:
                case Character.Type.Player: lanePosition = LanePosition.Middle; break;
                case Character.Type.Tank:   lanePosition = LanePosition.Down;   break;
                case Character.Type.Healer: lanePosition = LanePosition.Up;     break;
                }
                SpawnCharacter(character.type, lanePosition, true, character.stats);
            }
        }

        GameData.EnemyEncounter enemyEncounter = BattleHandler.enemyEncounter;
        for (int i = 0; i < enemyEncounter.enemyBattleArray.Length; i++) {
            GameData.EnemyEncounter.EnemyBattle enemyBattle = enemyEncounter.enemyBattleArray[i];
            SpawnCharacter(enemyBattle.characterType, enemyBattle.lanePosition, false, new Character(enemyBattle.characterType).stats);
        }

        SetActiveCharacterBattle(GetAliveTeamCharacterBattleList(true)[0]);
        SetSelectedTargetCharacterBattle(GetAliveTeamCharacterBattleList(false)[0]);
        
        lastPlayerActiveLanePosition = GetAliveTeamCharacterBattleList(true)[0].GetLanePosition();
        lastEnemyActiveLanePosition = GetNextLanePosition(GetAliveTeamCharacterBattleList(false)[0].GetLanePosition(), true);

        state = State.WaitingForPlayer;
    }

    private void ResetCamera() {
        cameraFollow.SetCameraFollowPosition(Vector3.zero);
        cameraFollow.SetCameraZoom(50f);
    }

    private void SetCamera(Vector3 position, float zoom) {
        cameraFollow.SetCameraFollowPosition(position);
        cameraFollow.SetCameraZoom(zoom);
    }

    private void SetActiveCharacterBattle(CharacterBattle characterBattle) {
        if (activeCharacterBattle != null) {
            activeCharacterBattle.HideSelectionCircle();
        }
        if (!characterBattle.IsPlayerTeam()) {
            // Enemy Active
            selectedTargetCharacterBattle.HideSelectionCircle();
        }
        activeCharacterBattle = characterBattle;
        activeCharacterBattle.ShowSelectionCircle(new Color(0, 1, 0, 1));

    }

    private void SetSelectedTargetCharacterBattle(CharacterBattle characterBattle) {
        if (selectedTargetCharacterBattle != null) {
            selectedTargetCharacterBattle.HideSelectionCircle();
        }
        selectedTargetCharacterBattle = characterBattle;
        selectedTargetCharacterBattle.ShowSelectionCircle(new Color(1, 0, 0, 1));
    }

    private CharacterBattle GetNextCharacterBattle(LanePosition lanePosition, bool moveUp, bool isPlayerTeam) {
        List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(isPlayerTeam);
        if (characterBattleList.Count == 0) {
            return null;
        }

        CharacterBattle characterBattle = null;
        do {
            // Get next lane position
            lanePosition = GetNextLanePosition(lanePosition, moveUp);
            // Find character at that position
            characterBattle = GetCharacterBattleAt(lanePosition, isPlayerTeam);
            if (characterBattle != null && characterBattle.IsDead()) characterBattle = null; // Ignore dead
        } while (characterBattle == null);

        return characterBattle;
    }

    private CharacterBattle GetCharacterBattleAt(LanePosition lanePosition, bool isPlayerTeam) {
        foreach (CharacterBattle characterBattle in characterBattleList) {
            if (characterBattle.IsPlayerTeam() == isPlayerTeam && characterBattle.GetLanePosition() == lanePosition) {
                return characterBattle;
            }
        }
        return null;
    }

    private List<CharacterBattle> GetTeamCharacterBattleList(bool isPlayerTeam) {
        List<CharacterBattle> ret = new List<CharacterBattle>();
        foreach (CharacterBattle characterBattle in characterBattleList) {
            if (characterBattle.IsPlayerTeam() == isPlayerTeam) {
                ret.Add(characterBattle);
            }
        }
        return ret;
    }

    private List<CharacterBattle> GetAliveTeamCharacterBattleList(bool isPlayerTeam) {
        List<CharacterBattle> ret = new List<CharacterBattle>();
        foreach (CharacterBattle characterBattle in characterBattleList) {
            if (characterBattle.IsDead()) continue; // Character is Dead
            if (characterBattle.IsPlayerTeam() == isPlayerTeam) {
                ret.Add(characterBattle);
            }
        }
        return ret;
    }

    public void Update() {
        switch (state) {
        case State.WaitingForPlayer:
            // Player Turn

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
                // Select Target Up
                SetSelectedTargetCharacterBattle(GetNextCharacterBattle(selectedTargetCharacterBattle.GetLanePosition(), true, false));
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
                // Select Target Down
                SetSelectedTargetCharacterBattle(GetNextCharacterBattle(selectedTargetCharacterBattle.GetLanePosition(), false, false));
            }
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                // Attack
                state = State.Busy;
                SetCamera(selectedTargetCharacterBattle.GetPosition() + new Vector3(-5f, 0), 30f);
                activeCharacterBattle.AttackTarget(selectedTargetCharacterBattle.GetPosition(), () => {
                    SoundManager.PlaySound(SoundManager.Sound.CharacterHit);
                    int damageBase = activeCharacterBattle.GetAttack();
                    int damageMin = (int)(damageBase * 0.8f);
                    int damageMax = (int)(damageBase * 1.2f);
                    selectedTargetCharacterBattle.Damage(activeCharacterBattle, UnityEngine.Random.Range(damageMin, damageMax));
                    UtilsClass.ShakeCamera(.75f, .1f);
                    if (selectedTargetCharacterBattle.IsDead()) {
                        TestEvilMonsterKilled();
                    }
                }, () => {
                    ResetCamera();
                    activeCharacterBattle.SlideBack(() => FunctionTimer.Create(ChooseNextActiveCharacter, .2f));
                });
            }
            if (Input.GetKeyDown(KeyCode.R)) {
                // Special
                if (activeCharacterBattle.TrySpendSpecial()) {
                    // Spend Special
                    switch (activeCharacterBattle.GetCharacterType()) {
                    default:
                    case Character.Type.Player:
                        state = State.Busy;
                        SetCamera(selectedTargetCharacterBattle.GetPosition() + new Vector3(-5f, 0), 30f);
                        Vector3 slideToPosition = selectedTargetCharacterBattle.GetPosition() + new Vector3(-8f, 0);
                        activeCharacterBattle.SlideToPosition(slideToPosition, () => {
                            activeCharacterBattle.PlayAnim(UnitAnim.GetUnitAnim("dSwordShield_SpartanKickRight"), (UnitAnim u) => {
                                ResetCamera();
                                activeCharacterBattle.SlideBack(() => FunctionTimer.Create(ChooseNextActiveCharacter, .2f));
                            }, (string trigger) => {
                                // Massive Single Enemy Damage
                                SoundManager.PlaySound(SoundManager.Sound.CharacterHit);
                                UtilsClass.ShakeCamera(2f, .15f);
                                int damageAmount = 100;
                                selectedTargetCharacterBattle.Damage(activeCharacterBattle, damageAmount);
                                if (selectedTargetCharacterBattle.IsDead()) {
                                    TestEvilMonsterKilled();
                                }
                            });
                        });
                        break;
                    case Character.Type.Tank:
                        state = State.Busy;
                        activeCharacterBattle.SlideToPosition(GetPosition(LanePosition.Middle, false) + new Vector3(-15, 0), () => {
                            activeCharacterBattle.PlayAnim(UnitAnim.GetUnitAnim("dDagger_GroundPound"), (UnitAnim u) => {
                                activeCharacterBattle.SlideBack(() => FunctionTimer.Create(ChooseNextActiveCharacter, .2f));
                            }, (string trigger) => {
                                // Damage All Enemies
                                SoundManager.PlaySound(SoundManager.Sound.GroundPound);
                                UtilsClass.ShakeCamera(2f, .15f);
                                int damageAmount = 30;
                                List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(false);
                                foreach (CharacterBattle characterBattle in characterBattleList) {
                                    characterBattle.Damage(activeCharacterBattle, damageAmount);
                                }
                                if (selectedTargetCharacterBattle.IsDead()) {
                                    TestEvilMonsterKilled();
                                }
                            });
                        });
                        break;
                    case Character.Type.Healer:
                        state = State.Busy;
                        SetCamera(activeCharacterBattle.GetPosition(), 30f);
                        activeCharacterBattle.PlayAnim(UnitAnim.GetUnitAnim("dBareHands_Victory"), (UnitAnim u) => {
                            activeCharacterBattle.PlayIdleAnim();
                            ResetCamera();
                            FunctionTimer.Create(ChooseNextActiveCharacter, .2f);
                        }, (string trigger) => {
                        });

                        FunctionTimer.Create(() => {
                            // Heal all
                            SoundManager.PlaySound(SoundManager.Sound.Heal);
                            List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(true);
                            foreach (CharacterBattle characterBattle in characterBattleList) {
                                characterBattle.Heal(50);
                            }
                        }, 1.2f);
                        break;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.T)) {
                // Heal
                if (GameData.TrySpendHealthPotion()) {
                    state = State.Busy;
                    activeCharacterBattle.Heal(100);
                    FunctionTimer.Create(ChooseNextActiveCharacter, .2f);
                }
            }
            break;
        case State.Busy:
            break;
        }
    }

    public CharacterBattle GetActiveCharacterBattle() {
        return activeCharacterBattle;
    }

    private int GetAliveCount(bool isPlayerTeam) {
        return GetAliveTeamCharacterBattleList(isPlayerTeam).Count;
    }

    private void TestEvilMonsterKilled() {
        switch (GameData.state) {
        case GameData.State.FightingEvilMonster_1:
            // Player Lost to Evil Monster
            character.isDead = true;
            GameData.GetCharacter(Character.Type.Player).stats.health = 1;
            GameData.GetCharacter(Character.Type.Tank).stats.health = 1;
            GameData.GetCharacter(Character.Type.Healer).stats.health = 1;
            GameData.state = GameData.State.LostToEvilMonster_1;

            Loader.Load(Loader.Scene.Cinematic_1);
            //OvermapHandler.LoadBackToOvermap();
            break;
        case GameData.State.FightingEvilMonster_2:
            // Player Lost to Evil Monster
            character.isDead = true;
            GameData.GetCharacter(Character.Type.Player).stats.health = 1;
            GameData.GetCharacter(Character.Type.Tank).stats.health = 1;
            GameData.GetCharacter(Character.Type.Healer).stats.health = 1;
            GameData.state = GameData.State.LostToEvilMonster_2;

            Loader.Load(Loader.Scene.Cinematic_1);
            //OvermapHandler.LoadBackToOvermap();
            break;
        case GameData.State.FightingEvilMonster_3:
            // Player Defeated Evil Monster
            character.isDead = true;
            GameData.state = GameData.State.DefeatedEvilMonster;
            
            Loader.Load(Loader.Scene.Cinematic_SleezerWin);
            //OvermapHandler.LoadBackToOvermap();
            break;
        }
    }

    private void ChooseNextActiveCharacter() {
        // Choose the next character to act
        if (GetAliveTeamCharacterBattleList(false).Count == 0) {
            // Battle Over, Player Wins!
            Debug.Log("Battle Over, Player Wins!");
            // Kill Enemy Character in Overmap
            character.isDead = true;
            // Update Player Team Values
            foreach (CharacterBattle characterBattle in GetTeamCharacterBattleList(true)) {
                if (Character.IsUniqueCharacterType(characterBattle.GetCharacterType())) {
                    // Unique Character
                    Character uniqueCharacter = GameData.GetCharacter(characterBattle.GetCharacterType());
                    if (uniqueCharacter != null) {
                        uniqueCharacter.stats.health = characterBattle.GetHealthAmount();
                        if (uniqueCharacter.isInPlayerTeam) {
                            if (uniqueCharacter.stats.health < 1) uniqueCharacter.stats.health = 1;
                        }
                    }
                }
            }
            Debug.Log("GameData.state: " + GameData.state);
            switch (GameData.state) {
            case GameData.State.Start:
                break;
            case GameData.State.FightingHurtMeDaddy:
                GameData.state = GameData.State.DefeatedHurtMeDaddy;
                break;
            case GameData.State.FightingHurtMeDaddy_2:
                GameData.state = GameData.State.DefeatedHurtMeDaddy_2;
                break;
            case GameData.State.FightingTank:
                GameData.state = GameData.State.DefeatedTank;
                // Heal Tank
                character.isDead = false;
                character.isInPlayerTeam = true;
                character.stats.health = character.stats.healthMax;
                character.subType = Character.SubType.Tank_Friendly;
                GameData.GetCharacter(Character.Type.Sleezer).subType = Character.SubType.Sleezer_Friendly;
                // Heal Player
                Character uniqueCharacter = GameData.GetCharacter(Character.Type.Player);
                uniqueCharacter.stats.health = uniqueCharacter.stats.healthMax;
                break;
            case GameData.State.FightingTavernAmbush:
                GameData.state = GameData.State.SurvivedTavernAmbush;
                break;
            case GameData.State.FightingEvilMonster_1:
                // Player Lost to Evil Monster
                GameData.GetCharacter(Character.Type.Player).stats.health = 1;
                GameData.GetCharacter(Character.Type.Tank).stats.health = 1;
                GameData.GetCharacter(Character.Type.Healer).stats.health = 1;
                GameData.state = GameData.State.LostToEvilMonster_1;
                break;
            case GameData.State.FightingEvilMonster_2:
                // Player Lost to Evil Monster
                GameData.state = GameData.State.LostToEvilMonster_2;
                break;
            case GameData.State.FightingEvilMonster_3:
                // Player Defeated Evil Monster
                GameData.state = GameData.State.DefeatedEvilMonster;
                break;
            }
            //SoundManager.PlaySound(SoundManager.Sound.BattleWin);
            FunctionTimer.Create(OvermapHandler.LoadBackToOvermap, .7f);
            return;
        }
        if (GetAliveTeamCharacterBattleList(true).Count == 0) {
            // Battle Over, Enemy Wins!
            Debug.Log("#### Battle Over, Enemy Wins!");
            
            foreach (CharacterBattle characterBattle in GetTeamCharacterBattleList(true)) {
                if (Character.IsUniqueCharacterType(characterBattle.GetCharacterType())) {
                    // Unique Character
                    Character uniqueCharacter = GameData.GetCharacter(characterBattle.GetCharacterType());
                    if (uniqueCharacter != null) {
                        uniqueCharacter.stats.health = characterBattle.GetHealthAmount();
                    }
                }
            }

            FunctionTimer.Create(OvermapHandler.LoadBackToOvermap, .7f);
            return;
        }

        // Choose Next Character
        if (activeCharacterBattle.IsPlayerTeam()) {
            // Next is Enemy
            List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(false);
            SetActiveCharacterBattle(
                GetNextCharacterBattle(lastEnemyActiveLanePosition, false, false)
            );
            lastEnemyActiveLanePosition = activeCharacterBattle.GetLanePosition();

            FunctionTimer.Create(() => {
                CharacterBattle aiTargetCharacterBattle = GetAliveTeamCharacterBattleList(true)[Random.Range(0, GetAliveTeamCharacterBattleList(true).Count)];
                SetCamera(aiTargetCharacterBattle.GetPosition() + new Vector3(+5f, 0), 30f);
                activeCharacterBattle.AttackTarget(aiTargetCharacterBattle.GetPosition(), () => {
                    SoundManager.PlaySound(SoundManager.Sound.CharacterHit);
                    int damageBase = activeCharacterBattle.GetAttack();
                    int damageMin = (int)(damageBase * 0.8f);
                    int damageMax = (int)(damageBase * 1.2f);
                    int damageAmount = Random.Range(damageMin, damageMax);
                    int damageChance = 75;
                    if (GetAliveCount(true) == 1) {
                        // Only one character alive on the Players Team
                        CharacterBattle lastSurvivingCharacterBattle = GetAliveTeamCharacterBattleList(true)[0];
                        if (lastSurvivingCharacterBattle.GetHealthAmount() <= damageAmount) {
                            // This hit would kill the last player character, miss
                            damageChance = 0;
                        }
                    }
                    if (Random.Range(0, 100) < damageChance) {
                        // Hit
                        aiTargetCharacterBattle.Damage(activeCharacterBattle, damageAmount);
                        UtilsClass.ShakeCamera(.75f, .1f);
                    } else {
                        // Miss
                        DamagePopup.Create(activeCharacterBattle.GetPosition(), "MISS", UtilsClass.GetColorFromString("00B4FF"));
                    }
                }, () => {
                    ResetCamera();
                    activeCharacterBattle.SlideBack(() => FunctionTimer.Create(ChooseNextActiveCharacter, .2f));
                });
            }, .3f);
        } else {
            // Next is player character
            List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(true);
            //SetActiveCharacterBattle(characterBattleList[Random.Range(0, characterBattleList.Count)]);

            //Debug.Log(lastPlayerActiveLanePosition);
            SetActiveCharacterBattle(
                GetNextCharacterBattle(lastPlayerActiveLanePosition, false, true)
            );
            lastPlayerActiveLanePosition = activeCharacterBattle.GetLanePosition();
            //Debug.Log("2 " + lastPlayerActiveLanePosition);

            activeCharacterBattle.TickSpecialCooldown();

            RefreshSelectedTargetCharacterBattle();
            state = State.WaitingForPlayer;
        }
    }

    private void RefreshSelectedTargetCharacterBattle() {
        if (selectedTargetCharacterBattle != null && selectedTargetCharacterBattle.IsDead()) {
            selectedTargetCharacterBattle.HideSelectionCircle();
            selectedTargetCharacterBattle = null;
        }
        if (selectedTargetCharacterBattle == null) {
            // Select next valid target
            List<CharacterBattle> characterBattleList = GetAliveTeamCharacterBattleList(false);
            if (characterBattleList.Count == 0) {
                // No more targets available
                return;
            } else {
                // There are still targets available
                CharacterBattle newTargetCharacterBattle = characterBattleList[Random.Range(0, characterBattleList.Count)];
                SetSelectedTargetCharacterBattle(newTargetCharacterBattle);
            }
        } else {
            SetSelectedTargetCharacterBattle(selectedTargetCharacterBattle);
        }
    }

}
