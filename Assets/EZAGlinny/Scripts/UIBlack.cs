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

public class UIBlack : MonoBehaviour {

    private static UIBlack instance;

    private void Awake() {
        instance = this;
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        Hide();
    }

    public static void Hide() {
        instance.gameObject.SetActive(false);
    }
    
    public static void Show() {
        instance.gameObject.SetActive(true);
    }

}
