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

public class GameOverWindow : MonoBehaviour {

    private void Awake() {
        Transform subMain = transform.Find("subMain");

        subMain.Find("quitBtn").GetComponent<Button_UI>().ClickFunc = () => Application.Quit();
        subMain.Find("quitBtn").GetComponent<Button_UI>().AddButtonSounds();

        subMain.Find("codeMonkeyWebsiteBtn").GetComponent<Button_UI>().ClickFunc = () => Application.OpenURL("https://unitycodemonkey.com/");
        subMain.Find("codeMonkeyYoutubeBtn").GetComponent<Button_UI>().ClickFunc = () => Application.OpenURL("https://youtube.com/c/CodeMonkeyUnity");
        subMain.Find("ezaYoutubeBtn").GetComponent<Button_UI>().ClickFunc = () => Application.OpenURL("https://www.youtube.com/channel/UCZrxXp1reP8E353rZsB3jaA");
        
        subMain.Find("codeMonkeyWebsiteBtn").GetComponent<Button_UI>().AddButtonSounds();
        subMain.Find("codeMonkeyYoutubeBtn").GetComponent<Button_UI>().AddButtonSounds();
        subMain.Find("ezaYoutubeBtn").GetComponent<Button_UI>().AddButtonSounds();

        
        subMain.Find("codeMonkeyBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Application.OpenURL("https://www.youtube.com/c/CodeMonkeyUnity");
        };
        subMain.Find("codeMonkeyBtn").GetComponent<Button_UI>().AddButtonSounds();
        
        subMain.Find("codeMonkeyVideoBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Application.OpenURL("https://youtu.be/rmeSwk6AOO4");
        };
        subMain.Find("codeMonkeyVideoBtn").GetComponent<Button_UI>().AddButtonSounds();
        
        subMain.Find("easyAlliesBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Application.OpenURL("https://itch.io/jam/glinnys-cauldron-jam");
        };
        subMain.Find("easyAlliesBtn").GetComponent<Button_UI>().AddButtonSounds();
    }

}
