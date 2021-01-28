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
using UnityEngine.UI;

public class UIChatBubble : MonoBehaviour {

    private static UIChatBubble instance;

    public static void Create(Vector2 position, string text) {
        Transform chatBubbleUITransform = Object.Instantiate(GameAssets.i.pfChatBubbleUI, instance.transform);
    }

    private Text uiText;

    private void Awake() {
        instance = this;
        uiText = transform.Find("Text").GetComponent<Text>();
    }

    public void ShowText(string text) {
        Show();
        uiText.text = "";
        //uiText.text = text;
        Text_Writer.RemoveWriter(uiText);
        Text_Writer.AddWriter(uiText, text, .02f, true);
        SoundManager.PlaySound(SoundManager.Sound.Talking, text.Length * .02f);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

}
