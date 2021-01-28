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

public class Window_KeyContinue : MonoBehaviour {

    private static Window_KeyContinue instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        if (GameData.state == GameData.State.Start) {
            FunctionTimer.Create(() => { 
                Show();
            }, 1f);
        }
        Hide();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
    
    private void Show() {
        gameObject.SetActive(true);
    }

    public static void Hide_Static() {
        instance.Hide();
    }

}
