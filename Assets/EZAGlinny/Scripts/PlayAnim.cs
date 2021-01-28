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

public class PlayAnim : MonoBehaviour {

    [SerializeField] private string animName;

    private void Start() {
        GetComponent<Player_Base>().GetUnitAnimation().PlayAnimForced(UnitAnim.GetUnitAnim(animName), 1f, null);
    }

}
