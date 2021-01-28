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
using CodeMonkey.Utils;

public class Cinematic_SleezerWin : MonoBehaviour {

    private void Start() {
        FunctionTimer.Create(() => {
            OvermapHandler.LoadBackToOvermap();
        }, 20f);
    }

}
