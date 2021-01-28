/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey;
using CodeMonkey.Utils;

public class Window_PickName : MonoBehaviour {

    private static Window_PickName instance;

    private Transform optionsTransform;
    private Transform optionSingleTemplateTransform;
    private List<Transform> optionSingleTransformList;

    private void Awake() {
        instance = this;
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        optionsTransform = transform.Find("options");
        optionSingleTemplateTransform = optionsTransform.Find("optionTemplate");
        optionSingleTemplateTransform.gameObject.SetActive(false);

        optionSingleTransformList = new List<Transform>();

        Hide();
    }

    private void DestroyOptions() {
        foreach (Transform optionSingleTransform in optionSingleTransformList) {
            Destroy(optionSingleTransform.gameObject);
        }
        optionSingleTransformList.Clear();
    }

    public static void AddOption_Static(string text, Action onClick) => instance.AddOption(text, onClick);

    private void AddOption(string text, Action onClick) {
        Transform optionSingleTransform = Instantiate(optionSingleTemplateTransform, optionsTransform);
        optionSingleTransform.gameObject.SetActive(true);
        optionSingleTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -25 * optionSingleTransformList.Count);
        optionSingleTransform.GetComponent<Button_UI>().ClickFunc = () => {
            //DestroyOptions();
            //Hide();
            onClick();
        };
        optionSingleTransform.Find("text").GetComponent<Text>().text = text;
        optionSingleTransformList.Add(optionSingleTransform);
    }

    public static void Hide_Static() => instance.Hide();

    private void Hide() {
        gameObject.SetActive(false);
    }

    public static void Show_Static() => instance.Show();

    private void Show() {
        gameObject.SetActive(true);
    }

    public static void ShowAvailableNames(Action<string> onSelected) {
        instance.DestroyOptions();
        Window_PickName.Show_Static();
        Window_PickName.AddOption_Static("Custom Name (Max Length 4)", () => { /* DISABLED */});
        for (int i=0; i<GameData.badNamesArray.Length; i++) {
            string badName = GameData.badNamesArray[i];
            if (GameData.IsNameAvailable(badName)) {
                Window_PickName.AddOption_Static(badName, () => {
                    instance.Hide();
                    onSelected(badName);
                });
            }
        }
    }

}
