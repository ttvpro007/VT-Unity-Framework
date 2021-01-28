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
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey;
using CodeMonkey.Utils;

public class FollowerOvermap : MonoBehaviour {
    
    public static FollowerOvermap instance;

    private float speed = 50f;

    private Player_Base playerBase;
    private State state;
    private Material material;
    private Color materialTintColor;
    private Vector3 targetMovePosition;
    private PlayerOvermap playerOvermap;
    private Vector3 followOffset;
    private Character character;
    private HealthSystem healthSystem;
    private World_Bar healthWorldBar;

    private float sleezerTripUpTimer;

    private enum State {
        Normal,
    }

    private void Awake() {
        instance = this;
        playerBase = gameObject.GetComponent<Player_Base>();
        material = transform.Find("Body").GetComponent<MeshRenderer>().material;
        materialTintColor = new Color(1, 0, 0, 0);
        SetStateNormal();
    }

    private void Start() {
        //playerBase.GetAnimatedWalker().SetAnimations(UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Idle"), UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Walk"), 1f, 1f);
    }

    public void Setup(Character character, PlayerOvermap playerOvermap, Vector3 followOffset) {
        this.character = character;
        this.playerOvermap = playerOvermap;
        this.followOffset = followOffset;

        switch (character.type) {
        case Character.Type.Tank:
            material.mainTexture = GameAssets.i.t_Tank;
            playerBase.GetAnimatedWalker().SetAnimations(GameAssets.UnitAnimTypeEnum.dSwordShield_Idle, GameAssets.UnitAnimTypeEnum.dSwordShield_Walk, 1f, 1f);
            transform.localScale = Vector3.one * 1.2f;
            break;
        case Character.Type.Sleezer:
            material.mainTexture = GameAssets.i.t_Sleezer;
            playerBase.GetAnimatedWalker().SetAnimations(GameAssets.UnitAnimTypeEnum.dBareHands_Idle, GameAssets.UnitAnimTypeEnum.dBareHands_Walk, 1f, 1f);
            transform.localScale = Vector3.one * 0.7f;
            sleezerTripUpTimer = 5f;
            if (GameData.state == GameData.State.DefeatedTank) {
                sleezerTripUpTimer = 2f;
            }
            speed = 65f;
            break;
        case Character.Type.Healer:
            material.mainTexture = GameAssets.i.t_Healer;
            playerBase.GetAnimatedWalker().SetAnimations(GameAssets.UnitAnimTypeEnum.dDualDagger_Idle, GameAssets.UnitAnimTypeEnum.dDualDagger_Walk, 1f, 1f);
            transform.localScale = Vector3.one * 1.0f;
            break;
        }
        
        healthSystem = new HealthSystem(character.stats.healthMax);
        healthSystem.SetHealthAmount(character.stats.health);
        healthWorldBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(15, 2), Color.grey, Color.red, healthSystem.GetHealthPercent(), UnityEngine.Random.Range(10000, 11000), new World_Bar.Outline { color = Color.black, size = .6f });
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        RefreshHealthBar();

        SetTargetMovePosition(playerOvermap.GetPosition() + followOffset);
    }

    public void SaveCharacterPosition() {
        character.position = GetPosition();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        RefreshHealthBar();
    }

    public void RefreshHealthBar() {
        healthWorldBar.SetSize(healthSystem.GetHealthPercent());
        if (healthSystem.GetHealthPercent() >= 1f) {
            // Full health
            healthWorldBar.Hide();
        } else {
            healthWorldBar.Show();
        }
    }

    public Character GetCharacter() {
        return character;
    }

    private void Update() {
        if (materialTintColor.a > 0) {
            float tintFadeSpeed = 6f;
            materialTintColor.a -= tintFadeSpeed * Time.deltaTime;
            material.SetColor("_Tint", materialTintColor);
        }

        if (!OvermapHandler.IsOvermapRunning()) {
            playerBase.PlayIdleAnim();
            return; // Overmap not running
        }

        switch (character.type) {
        default:
            switch (state) {
            case State.Normal:
                HandleTargetMovePosition();
                HandleMovement();
                break;
            }
            break;
        case Character.Type.Sleezer:
            switch (state) {
            case State.Normal:
                HandleTargetMovePosition();
                HandleSleezer();
                HandleMovement();
                break;
            }
            break;
        }
    }
    
    private void SetStateNormal() {
        state = State.Normal;
    }
    
    private void HandleSleezer() {
        sleezerTripUpTimer -= Time.deltaTime;
        if (sleezerTripUpTimer < 0f) {
            SetTargetMovePosition(playerOvermap.GetPosition());
            float tripUpDistance = 5f;
            if (Vector3.Distance(GetPosition(), playerOvermap.GetPosition()) < tripUpDistance) {
                playerOvermap.SleezerTripUp();
                SoundManager.PlaySound(SoundManager.Sound.CharacterHit);
                string chatText;
                switch (UnityEngine.Random.Range(0, 4)) {
                default:
                case 0: chatText = "Hihihi, sorry about that!"; break;
                case 1: chatText = "Hihihi, don't mind me!";    break;
                case 2: chatText = "Hihihi, I'm so clumsy!";    break;
                case 3: chatText = "Hihihi, silly me!";         break;
                }
                ChatBubble.Create(transform, new Vector3(3.5f, 4), chatText);
                sleezerTripUpTimer = UnityEngine.Random.Range(20f, 50f);

                // Other Message
                Transform randomCharacterTransform = OvermapHandler.GetInstance().GetRandomCharacterTransform(false);
                string responseMessage;
                switch (UnityEngine.Random.Range(0, 2)) {
                default:
                case 0: responseMessage = "Oh Sleezer! You're so silly!"; break;
                case 1: responseMessage = "Oh Sleezer! You're adorable!"; break;
                }
                FunctionTimer.Create(() => {
                    ChatBubble.Create(randomCharacterTransform, new Vector3(3.5f, 5), responseMessage);
                }, 5f);

                OvermapFtnDewHandler.DestroyAllChatBubbles_Static();
                OvermapHandler.GetInstance().sleezerActive = true;
                FunctionTimer.Create(() => { OvermapHandler.GetInstance().sleezerActive = false; }, 7f);
            }
        }
    }

    private void HandleTargetMovePosition() {
        float tooFarDistance = 50f;
        if (Vector3.Distance(GetPosition(), playerOvermap.GetPosition()) > tooFarDistance) {
            SetTargetMovePosition(playerOvermap.GetPosition() + followOffset);
        }
        float tooCloseDistance = 20f;
        if (Vector3.Distance(GetPosition(), playerOvermap.GetPosition()) < tooCloseDistance) {
            SetTargetMovePosition(GetPosition());
        }
    }

    private void HandleMovement() {
        float minMoveDistance = 5f;
        Vector3 moveDir = new Vector3(0, 0);
        if (Vector3.Distance(GetPosition(), targetMovePosition) > minMoveDistance) {
            moveDir = (targetMovePosition - GetPosition()).normalized;
        }

        bool isIdle = moveDir.x == 0 && moveDir.y == 0;
        if (isIdle) {
            playerBase.PlayIdleAnim();
        } else {
            playerBase.PlayMoveAnim(moveDir);
            transform.position += moveDir * speed * Time.deltaTime;
        }
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

    public void SetTargetMovePosition(Vector3 targetMovePosition) {
        this.targetMovePosition = targetMovePosition;
    }

}
