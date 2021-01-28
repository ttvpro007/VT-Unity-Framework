/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using UnityEngine;
using System.Reflection;
using V_AnimationSystem;

public class GameAssets : MonoBehaviour {

    private static GameAssets _i;

    public static GameAssets i {
        get {
            if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }




    public Sprite codeMonkeyHeadSprite;
    public Transform Map;

    public Sprite s_ShootFlash;
    public Sprite s_Bubble;
    public Sprite s_BubbleHandle;
    public Sprite s_PlayerDialogueSprite;
    public Sprite s_PlayerPortrait;
    public Sprite s_TankPortrait;
    public Sprite s_HealerPortrait;
    public Sprite s_SleezerPortrait;
    public Sprite s_EnemyMinionOrangePortrait;
    public Sprite s_EnemyMinionRedPortrait;
    public Sprite s_EnemyOgrePortrait;
    public Sprite s_EvilMonsterPortrait;
    public Sprite s_RandyPortrait;
    public Sprite s_VendorPortrait;

    public Sprite s_Cross;
    public Sprite s_Arrow;
    public Sprite s_ExclamationPoint;
    public Sprite s_Rope;
    
    public Transform pfSwordSlash;
    public Transform pfEnemy;
    public Transform pfEnemyFlyingBody;
    public Transform pfImpactEffect;
    public Transform pfFollowerOvermap;
    public Transform pfEnemyOvermap;
    public Transform pfNPCOvermap;
    public Transform pfCharacterBattle;
    public Transform pfChatBubble;
    public Transform pfChatBubbleUI;
    public Transform pfDamagePopup;
    public Transform pfChatOption;
    public Transform pfFtnDewItemOvermap;
    public Transform pfHealthPotionItemOvermap;
    public Transform pfObjectTractor;
    public Transform pfChatBubble_FtnDew;
    public Transform pfChatBubble_FtnDew_2;
    public Transform pfChatBubble_FtnDew_3;
    public Transform pfChatBubble_FtnDew_4;
    public Transform pfSmokePuff;
    public Transform pfKey;

    public Material m_WeaponTracer;
    public Material m_MarineSpriteSheet;

    public Texture2D t_Player;
    public Texture2D t_Tank;
    public Texture2D t_Sleezer;
    public Texture2D t_Healer;
    public Texture2D t_EnemyMinionOrange;
    public Texture2D t_EnemyMinionRed;
    public Texture2D t_Ogre;
    public Texture2D t_Zombie;
    public Texture2D t_Vendor;
    public Texture2D t_EvilMonster;
    public Texture2D t_Randy;
    public Texture2D t_Villager_1;
    public Texture2D t_Villager_2;
    public Texture2D t_Villager_3;
    public Texture2D t_Villager_4;
    public Texture2D t_Villager_5;

    public Texture2D t_FtnDewArmor;
    public Texture2D t_SwordThousandTruths;


    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
        [Range(0f, 2f)]
        public float volume;
    }


    
    public static class UnitAnimTypeEnum {

        static UnitAnimTypeEnum() {
            V_Animation.Init();
            FieldInfo[] fieldInfoArr = typeof(UnitAnimTypeEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in fieldInfoArr) {
                if (fieldInfo != null) {
                    fieldInfo.SetValue(null, UnitAnimType.GetUnitAnimType(fieldInfo.Name));
                }
            }
        }

        public static UnitAnimType dSwordTwoHandedBack_Idle;
        public static UnitAnimType dSwordTwoHandedBack_Walk;
        public static UnitAnimType dSwordTwoHandedBack_Sword;
        public static UnitAnimType dSwordTwoHandedBack_Sword2;

        public static UnitAnimType dDualDagger_Idle;
        public static UnitAnimType dDualDagger_Walk;
        public static UnitAnimType dDualDagger_Attack;
        
        public static UnitAnimType dSpartan_Attack;
        public static UnitAnimType dBareHands_Hit;

        public static UnitAnimType dMinion_Idle;
        public static UnitAnimType dMinion_Walk;
        public static UnitAnimType dMinion_Attack;

        public static UnitAnimType dShielder_Idle;
        public static UnitAnimType dShielder_Walk;

        public static UnitAnimType dSwordShield_Idle;
        public static UnitAnimType dSwordShield_Walk;
        public static UnitAnimType dSwordShield_Attack;

        public static UnitAnimType dMarine_Idle;
        public static UnitAnimType dMarine_Walk;
        public static UnitAnimType dMarine_Attack;

        public static UnitAnimType dBareHands_Idle;
        public static UnitAnimType dBareHands_Walk;
        public static UnitAnimType dBareHands_Punch;
        
        public static UnitAnimType dZombie_Idle;
        public static UnitAnimType dZombie_Walk;
        public static UnitAnimType dZombie_Attack;

        public static UnitAnimType dOgre_Idle;
        public static UnitAnimType dOgre_Walk;
        public static UnitAnimType dOgre_Attack;
        

    }




    public static class UnitAnimEnum {

        static UnitAnimEnum() {
            V_Animation.Init();
            FieldInfo[] fieldInfoArr = typeof(UnitAnimEnum).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fieldInfo in fieldInfoArr) {
                if (fieldInfo != null) {
                    fieldInfo.SetValue(null, UnitAnim.GetUnitAnim(fieldInfo.Name));
                }
            }
        }
        
        public static UnitAnim dMarine_AimWeaponRight;
        public static UnitAnim dMarine_AimWeaponRightInvertV;
        public static UnitAnim dMarine_ShootWeaponRight;
        public static UnitAnim dMarine_ShootWeaponRightInvertV;
        
    }

}
