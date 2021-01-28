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

public class Window_Info : MonoBehaviour {

    private Text ftnDewText;
    private Text healthPotionText;
    private Text timePlayedText;
    private float secondsPlayed;
    private GameObject keyGameObject;

    private void Awake() {
        ftnDewText = transform.Find("ftnDew").Find("text").GetComponent<Text>();
        healthPotionText = transform.Find("healthPotion").Find("text").GetComponent<Text>();
        keyGameObject = transform.Find("key").gameObject;
        keyGameObject.SetActive(false);

        ftnDewText.text = GameData.ftnDewPoints.ToString();

        if (GameData.ftnDewPoints > 0) {
            ShowFtnDew();
        } else {
            HideFtnDew();
        }

        FunctionTimer.Create(() => {
            if (GameData.healthPotionCount > 0 && OvermapHandler.GetInstance().GetPlayer().GetHealth() < 100) {
                keyGameObject.SetActive(true);
                FunctionTimer.Create(() => { keyGameObject.SetActive(false); }, 0.5f);
                FunctionTimer.Create(() => { keyGameObject.SetActive(true);  }, 1.0f);
                FunctionTimer.Create(() => { keyGameObject.SetActive(false); }, 1.5f);
                FunctionTimer.Create(() => { keyGameObject.SetActive(true);  }, 2.0f);
                FunctionTimer.Create(() => { keyGameObject.SetActive(false); }, 2.5f);
            }
        }, Random.Range(3f, 10f));

        transform.Find("optionsBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Window_Pause.Show_Static();
        };
        transform.Find("optionsBtn").GetComponent<Button_UI>().AddButtonSounds();

        transform.Find("optionsText").gameObject.SetActive(false);
        if (GameData.state == GameData.State.Start) {
            FunctionTimer.Create(() => transform.Find("optionsText").gameObject.SetActive(true),  4.0f);
            /*
            FunctionTimer.Create(() => transform.Find("optionsText").gameObject.SetActive(false), 2.5f);
            FunctionTimer.Create(() => transform.Find("optionsText").gameObject.SetActive(true),  3.0f);
            FunctionTimer.Create(() => transform.Find("optionsText").gameObject.SetActive(false), 3.5f);
            FunctionTimer.Create(() => transform.Find("optionsText").gameObject.SetActive(true),  4.0f);
            */
        }

        timePlayedText = transform.Find("timePlayedText").GetComponent<Text>();
        secondsPlayed = Random.Range(0f, 60f * 30);
        RefreshTimePlayed();
    }

    private void Update() {
        healthPotionText.text = GameData.healthPotionCount.ToString();
        
        ftnDewText.text = GameData.ftnDewPoints.ToString();

        if (GameData.ftnDewPoints > 0) {
            ShowFtnDew();
        } else {
            HideFtnDew();
        }

        secondsPlayed += Time.deltaTime * 1f;
        RefreshTimePlayed();
    }

    private void RefreshTimePlayed() {
        int seconds = Mathf.RoundToInt(secondsPlayed);
        int minutes = Mathf.FloorToInt(seconds / 60f);
        seconds -= (minutes * 60);

        timePlayedText.text = "Time Played: " + minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    private void HideFtnDew() {
        transform.Find("ftnDew").gameObject.SetActive(false);
    }

    private void ShowFtnDew() {
        transform.Find("ftnDew").gameObject.SetActive(true);
    }

}
