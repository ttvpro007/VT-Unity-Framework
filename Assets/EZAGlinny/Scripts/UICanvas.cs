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

public class UICanvas : MonoBehaviour {

    private static UICanvas instance;

    private void Awake() {
        instance = this;
    }

    public static Transform GetCanvasTransform() {
        return instance.transform;
    }

}
