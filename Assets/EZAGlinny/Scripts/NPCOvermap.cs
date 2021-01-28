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
using CodeMonkey.Utils;

public class NPCOvermap : MonoBehaviour {
    
    public static NPCOvermap instance;

    private const float SPEED = 50f;

    private Player_Base playerBase;
    private State state;
    private Material material;
    private Color materialTintColor;
    private Vector3 targetMovePosition;
    private PlayerOvermap playerOvermap;
    private Character character;
    public bool overrideOvermapRunning;

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

    public void Setup(Character character, PlayerOvermap playerOvermap) {
        this.character = character;
        this.playerOvermap = playerOvermap;
        
        playerBase.GetAnimatedWalker().SetAnimations(GameAssets.UnitAnimTypeEnum.dBareHands_Idle, GameAssets.UnitAnimTypeEnum.dBareHands_Walk, 1f, 1f);

        switch (character.type) {
        case Character.Type.Tank:
            material.mainTexture = GameAssets.i.t_Tank;
            playerBase.GetAnimatedWalker().SetAnimations(GameAssets.UnitAnimTypeEnum.dSwordShield_Idle, GameAssets.UnitAnimTypeEnum.dSwordShield_Walk, 1f, 1f);
            transform.localScale = Vector3.one * 1.2f;
            break;
        case Character.Type.Healer:
            material.mainTexture = GameAssets.i.t_Healer;
            playerBase.GetAnimatedWalker().SetAnimations(GameAssets.UnitAnimTypeEnum.dDualDagger_Idle, GameAssets.UnitAnimTypeEnum.dDualDagger_Walk, 1f, 1f);
            transform.localScale = Vector3.one * 1.0f;
            break;
        case Character.Type.PlayerDoppelganger:
            material.mainTexture = GameAssets.i.t_Player;
            playerBase.GetAnimatedWalker().SetAnimations(GameAssets.UnitAnimTypeEnum.dBareHands_Idle, GameAssets.UnitAnimTypeEnum.dBareHands_Walk, 1f, 1f);
            transform.localScale = Vector3.one * 1.0f;
            break;
        case Character.Type.Shop:
            material.mainTexture = GameAssets.i.t_Vendor;
            break;

        case Character.Type.Villager_1: material.mainTexture = GameAssets.i.t_Villager_1; break;
        case Character.Type.Villager_2: material.mainTexture = GameAssets.i.t_Villager_2; break;
        case Character.Type.Villager_3: material.mainTexture = GameAssets.i.t_Villager_3; break;
        case Character.Type.Villager_4: material.mainTexture = GameAssets.i.t_Villager_4; break;
        case Character.Type.Villager_5: material.mainTexture = GameAssets.i.t_Villager_5; break;

        case Character.Type.Randy:
            material.mainTexture = GameAssets.i.t_Randy;
            playerBase.GetAnimatedWalker().SetAnimations(GameAssets.UnitAnimTypeEnum.dBareHands_Idle, GameAssets.UnitAnimTypeEnum.dBareHands_Walk, 1f, 1f);
            break;
        case Character.Type.TavernAmbush:
            material.mainTexture = GameAssets.i.t_EnemyMinionRed;
            playerBase.GetAnimatedWalker().SetAnimations(GameAssets.UnitAnimTypeEnum.dMinion_Idle, GameAssets.UnitAnimTypeEnum.dMinion_Walk, 1f, 1f);
            break;
        case Character.Type.TavernAmbush_2:
        case Character.Type.TavernAmbush_3:
            material.mainTexture = GameAssets.i.t_EnemyMinionOrange;
            playerBase.GetAnimatedWalker().SetAnimations(GameAssets.UnitAnimTypeEnum.dMinion_Idle, GameAssets.UnitAnimTypeEnum.dMinion_Walk, 1f, 1f);
            break;
        }

        SetTargetMovePosition(GetPosition());

        OvermapHandler.GetInstance().OnOvermapStopped += NPCOvermap_OnOvermapStopped;
    }

    private void NPCOvermap_OnOvermapStopped(object sender, EventArgs e) {
        playerBase.PlayIdleAnim();
    }

    public V_UnitAnimation GetUnitAnimation() {
        return playerBase.GetUnitAnimation();
    }

    public void SaveCharacterPosition() {
        character.position = GetPosition();
    }

    public Character GetCharacter() {
        return character;
    }

    public void SetLastMoveDir(Vector3 lastMoveDir) {
        playerBase.SetLastMoveDir(lastMoveDir);
    }

    private void Update() {
        if (!OvermapHandler.IsOvermapRunning() && !overrideOvermapRunning) {
            //playerBase.PlayIdleAnim();
            return; // Overmap not running
        }

        switch (state) {
        case State.Normal:
            HandleMovement();
            break;
        }

        if (materialTintColor.a > 0) {
            float tintFadeSpeed = 6f;
            materialTintColor.a -= tintFadeSpeed * Time.deltaTime;
            material.SetColor("_Tint", materialTintColor);
        }
    }
    
    private void SetStateNormal() {
        state = State.Normal;
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
            transform.position += moveDir * SPEED * Time.deltaTime;
        }
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

    public void SetTargetMovePosition(Vector3 targetMovePosition) {
        this.targetMovePosition = targetMovePosition;
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

}
