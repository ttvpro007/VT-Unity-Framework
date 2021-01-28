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
using CodeMonkey.Utils;

public class Window_Pause : MonoBehaviour {

    private static Window_Pause instance;

    private void Awake() {
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        instance = this;

        transform.Find("resumeBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Hide();
        };

        transform.Find("volumeIncreaseBtn").GetComponent<Button_UI>().ClickFunc = () => {
            SoundManager.masterVolume++;
            SoundManager.masterVolume = Mathf.Clamp(SoundManager.masterVolume, 0, 10);
            RefreshVolume();
        };

        transform.Find("volumeDecreaseBtn").GetComponent<Button_UI>().ClickFunc = () => {
            SoundManager.masterVolume--;
            SoundManager.masterVolume = Mathf.Clamp(SoundManager.masterVolume, 0, 10);
            RefreshVolume();
        };

        SoundManager.AddButtonSounds(transform.Find("resumeBtn").GetComponent<Button_UI>());
        SoundManager.AddButtonSounds(transform.Find("volumeIncreaseBtn").GetComponent<Button_UI>());
        SoundManager.AddButtonSounds(transform.Find("volumeDecreaseBtn").GetComponent<Button_UI>());

        Hide();
    }

    private void RefreshVolume() {
        transform.Find("volumeText").GetComponent<Text>().text = SoundManager.masterVolume.ToString();
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

    public static void Show_Static() {
        instance.Show();
    }

}
