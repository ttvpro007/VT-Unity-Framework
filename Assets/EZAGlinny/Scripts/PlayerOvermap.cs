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
using System.Collections.Generic;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;

/*
 * Player movement with Arrow keys
 * */
public class PlayerOvermap : MonoBehaviour {
    
    public static PlayerOvermap instance;

    private const float SPEED = 50f;
    
    [SerializeField] private LayerMask wallLayerMask;
    private Player_Base playerBase;
    private State state;
    private Material material;
    private Color materialTintColor;
    private HealthSystem healthSystem;
    private World_Bar healthWorldBar;
    private Character character;

    private enum State {
        Normal,
        Tripped,
    }

    private void Awake() {
        instance = this;
        playerBase = gameObject.GetComponent<Player_Base>();
        material = transform.Find("Body").GetComponent<MeshRenderer>().material;
        materialTintColor = new Color(1, 0, 0, 0);
        SetStateNormal();
        
    }

    public void Setup(Character character) {
        this.character = character;
        transform.position = character.position;
        healthSystem = new HealthSystem(character.stats.healthMax);
        healthSystem.SetHealthAmount(character.stats.health);
        healthWorldBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(15, 2), Color.grey, Color.red, healthSystem.GetHealthPercent(), UnityEngine.Random.Range(10000, 11000), new World_Bar.Outline { color = Color.black, size = .6f });
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        RefreshHealthBar();

        RefreshTexture();

        playerBase.GetAnimatedWalker().SetAnimations(UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Idle"), UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Walk"), 1f, .7f);

        OvermapHandler.GetInstance().OnOvermapStopped += NPCOvermap_OnOvermapStopped;
    }

    private void NPCOvermap_OnOvermapStopped(object sender, EventArgs e) {
        playerBase.PlayIdleAnim();
    }

    public void RefreshTexture() {
        if (character.hasFtnDewArmor) {
            Texture2D newSpritesheetTexture = new Texture2D(material.mainTexture.width, material.mainTexture.height, TextureFormat.ARGB32, true);
            newSpritesheetTexture.SetPixels((material.mainTexture as Texture2D).GetPixels());
            Color[] ftnDewArmorPixels = GameAssets.i.t_FtnDewArmor.GetPixels(0, 0, 512, 128);
            newSpritesheetTexture.SetPixels(0, 256, 512, 128, ftnDewArmorPixels);
            newSpritesheetTexture.Apply();
            material.mainTexture = newSpritesheetTexture;
        }
        
        if (character.hasSwordThousandTruths) {
            Texture2D newSpritesheetTexture = new Texture2D(material.mainTexture.width, material.mainTexture.height, TextureFormat.ARGB32, true);
            newSpritesheetTexture.SetPixels((material.mainTexture as Texture2D).GetPixels());
            Color[] swordThousandTruthsPixels = GameAssets.i.t_SwordThousandTruths.GetPixels(0, 0, 128, 128);
            newSpritesheetTexture.SetPixels(0, 128, 128, 128, swordThousandTruthsPixels);
            newSpritesheetTexture.Apply();
            material.mainTexture = newSpritesheetTexture;
        }
    }

    public V_UnitAnimation GetUnitAnimation() {
        return playerBase.GetUnitAnimation();
    }

    public void SaveCharacterPosition() {
        character.position = GetPosition();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        RefreshHealthBar();
    }

    private void RefreshHealthBar() {
        healthWorldBar.SetSize(healthSystem.GetHealthPercent());
        if (healthSystem.GetHealthPercent() >= 1f) {
            // Full health
            healthWorldBar.Hide();
        } else {
            healthWorldBar.Show();
        }
    }

    private void Update() {
        if (materialTintColor.a > 0) {
            float tintFadeSpeed = 6f;
            materialTintColor.a -= tintFadeSpeed * Time.deltaTime;
            material.SetColor("_Tint", materialTintColor);
        }

        if (!OvermapHandler.IsOvermapRunning()) {
            //playerBase.PlayIdleAnim();
            return; // Overmap not running
        }

        switch (state) {
        case State.Normal:
            HandleMovement();
            HandleInteract();
            break;
        }
    }
    
    private void SetStateNormal() {
        state = State.Normal;
    }

    private bool spawnedTankInteractKey;

    private void HandleInteract() {
        if (((int)GameData.state) < ((int)GameData.State.DefeatedTank)) {
            if (!spawnedTankInteractKey) {
                Character tankCharacter = GameData.GetCharacter(Character.Type.Tank);
                if (Vector3.Distance(GetPosition(), tankCharacter.position) < 12f) {
                    spawnedTankInteractKey = true;
                    Instantiate(GameAssets.i.pfKey, tankCharacter.position + new Vector3(0, 15), Quaternion.identity);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            NPCOvermap npcOvermap = OvermapHandler.GetInstance().GetClosestNPC(GetPosition(), 12f);
            if (npcOvermap != null) {
                switch (npcOvermap.GetCharacter().type) {
                case Character.Type.Tank:
                    // Tank is only a NPC before he joins
                    Cutscenes.Play_Tank_BeforeJoin();
                    break;
                case Character.Type.Healer:
                    // Healer is only a NPC before he joins
                    //Cutscenes.Play_Healer_BeforeJoin();
                    break;
                case Character.Type.PlayerDoppelganger:
                    if (GameData.state == GameData.State.GoingToAskDoppelGanger) {
                        Cutscenes.Play_DoppelGanger();
                    }
                    break;
                case Character.Type.Shop:
                    Cutscenes.Play_Shop(npcOvermap.GetCharacter());
                    break;
                }
            } else {
                // No NPC in range
                // Consume Health Potion
                if (GameData.TrySpendHealthPotion()) {
                    // Heal all Team members
                    int healAmount = 40;
                    Heal(healAmount);
                    FollowerOvermap tankOvermap = OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Tank));
                    if (tankOvermap != null) tankOvermap.Heal(healAmount);
                    FollowerOvermap healerOvermap = OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Healer));
                    if (healerOvermap != null) healerOvermap.Heal(healAmount);
                }
            }
            /*
            if (OvermapHandler.GetInstance().TryPlayerInteract()) {
                Dialogue dialogue = Dialogue.GetInstance();
                dialogue.SetDialogueActions(new List<Action>() {
                    () => {
                        dialogue.Show();
                        dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite);
                        dialogue.ShowLeftText("Hello there General Kenobi");
                        dialogue.ShowRightCharacter(GameAssets.i.s_PlayerDialogueSprite);
                        dialogue.FadeRightCharacter();
                    },
                    () => {
                        dialogue.ShowRightActiveTalkerHideLeft("Yes yes hello there...");
                    },
                    () => {
                        dialogue.ShowRightText("What brings you here?");
                    },
                    () => {
                        dialogue.ShowLeftActiveTalkerHideRight("Oh I don't know");
                    },
                    () => {
                        dialogue.ShowLeftText("Lets battle!");
                    },
                    () => {
                        Loader.Load(Loader.Scene.BattleScene);
                    },
                });
                dialogue.PlayNextAction();
                ///
                dialogue.SetDialogueActions(new List<Action>() {
                    () => {
                        dialogue.Show();
                        dialogue.ShowLeftCharacter(GameAssets.i.s_PlayerDialogueSprite, false);
                        dialogue.ShowRightCharacter(GameAssets.i.s_PlayerDialogueSprite, true);
                        dialogue.ShowDialogueOptions(new List<Dialogue.DialogueOption> {
                            new Dialogue.DialogueOption(Dialogue.DialogueOption.Option._1, "Hello there!", () => {
                                dialogue.ClearDialogueOptions();
                                dialogue.ShowRightActiveTalkerHideLeft("Yes yes hello there...");
                            }),
                            new Dialogue.DialogueOption(Dialogue.DialogueOption.Option._2, "General Kenobi!", () => {
                                dialogue.ClearDialogueOptions();
                                dialogue.ShowRightActiveTalkerHideLeft("Yes yes hello there...");
                            }),
                        });
                    },
                }, true);
            }
            */
        }
    }

    private void HandleMovement() {
        float moveX = 0f;
        float moveY = 0f;
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            moveX = +1f;
        }

        Vector3 moveDir = new Vector3(moveX, moveY).normalized;
        bool isIdle = moveX == 0 && moveY == 0;
        if (isIdle) {
            playerBase.PlayIdleAnim();
        } else {
            if (CanMoveTo(moveDir, SPEED * Time.deltaTime)) {
                playerBase.PlayMoveAnim(moveDir);
                transform.position += moveDir * SPEED * Time.deltaTime;
            } else {
                playerBase.PlayMoveAnim(moveDir);
            }
        }
    }

    private bool IsOnTopOfWall() {
        RaycastHit2D raycastHit = Physics2D.BoxCast(GetPosition(), new Vector2(5, 9), 0f, Vector2.zero, 0f, wallLayerMask);
        return raycastHit.collider != null;
    }

    private bool CanMoveTo(Vector3 dir, float distance) {
        if (IsOnTopOfWall()) return true;
        RaycastHit2D raycastHit = Physics2D.BoxCast(GetPosition(), new Vector2(5, 9), 0f, dir, distance, wallLayerMask);
        return raycastHit.collider == null;
    }

    private void TryMoveTo(Vector3 dir, float distance) {
        RaycastHit2D raycastHit = Physics2D.BoxCast(GetPosition(), new Vector2(5, 9), 0f, dir, distance, wallLayerMask);
        if (raycastHit.collider == null) {
            transform.position += dir * distance;
        } else {
            transform.position += dir * (raycastHit.distance - .1f);
        }
    }


    public void SleezerTripUp() {
        // Get tripped up by Sleezer
        state = State.Tripped;
        playerBase.GetUnitAnimation().PlayAnimForced(UnitAnim.GetUnitAnim("LyingUp"), 1f, null);
        /*
        FunctionTimer.Create(() => {
            ChatBubble.Create(transform, new Vector3(3.5f, 5), "Oh Sleezer! You're so silly!");
        }, 5f);
        */
        FunctionTimer.Create(() => {
            state = State.Normal;
            playerBase.PlayIdleAnim();
        }, 5f);
    }

    public int GetHealth() {
        return character.stats.health;
    }

    public void Heal(int healAmount) {
        materialTintColor = new Color(0, 1, 0, 1f);
        material.SetColor("_Tint", materialTintColor);
        healthSystem.Heal(healAmount);
        character.stats.health = healthSystem.GetHealthAmount();
    }

    private void DamageFlash() {
        materialTintColor = new Color(1, 0, 0, 1f);
        material.SetColor("_Tint", materialTintColor);
    }

    public void DamageKnockback(Vector3 knockbackDir, float knockbackDistance) {
        transform.position += knockbackDir * knockbackDistance;
        DamageFlash();
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }
        
}
