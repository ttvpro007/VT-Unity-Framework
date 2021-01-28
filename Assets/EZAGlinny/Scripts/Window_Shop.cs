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

public class Window_Shop : MonoBehaviour {

    private static Window_Shop instance;

    private Action onHide;
    private GameData.ShopContents shopContents;

    private void Awake() {
        instance = this;
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        transform.Find("healthPotionBtn").GetComponent<Button_UI>().ClickFunc = Buy_HealthPotion;
        transform.Find("ftnDewBtn").GetComponent<Button_UI>().ClickFunc = Buy_FtnDewPoints;
        transform.Find("ftnDewArmorBtn").GetComponent<Button_UI>().ClickFunc = Buy_FtnDewArmor;

        transform.Find("closeBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Hide();
        };

        transform.Find("healthPotionBtn").GetComponent<Button_UI>().AddButtonSounds();
        transform.Find("ftnDewBtn").GetComponent<Button_UI>().AddButtonSounds();
        transform.Find("ftnDewArmorBtn").GetComponent<Button_UI>().AddButtonSounds();
        transform.Find("closeBtn").GetComponent<Button_UI>().AddButtonSounds();

        Hide();
    }

    private void Buy_HealthPotion() {
        if (shopContents.healthPotions > 0) {
            shopContents.healthPotions--;
            GameData.AddHealthPotion(1);
            Refresh();
        }
    }

    private void Buy_FtnDewPoints() {
        if (shopContents.ftnDewPoints > 0) {
            shopContents.ftnDewPoints--;
            GameData.AddFtnDewPoints(1);
            Refresh();
        }
    }

    private void Buy_FtnDewArmor() {
        if (shopContents.hasFtnDewArmor) {
            shopContents.hasFtnDewArmor = false;
            GameData.GetCharacter(Character.Type.Player).hasFtnDewArmor = true;
            OvermapHandler.SaveAllCharacterPositions();
            Loader.Load(Loader.Scene.GameScene);
        }
    }

    private void Refresh() {
        transform.Find("healthPotionBtn").Find("text").GetComponent<Text>().text = shopContents.healthPotions.ToString();
        transform.Find("healthPotionBtn").Find("blocker").gameObject.SetActive(shopContents.healthPotions <= 0);
        
        transform.Find("ftnDewBtn").Find("text").GetComponent<Text>().text = shopContents.ftnDewPoints.ToString();
        transform.Find("ftnDewBtn").Find("blocker").gameObject.SetActive(shopContents.ftnDewPoints <= 0);

        transform.Find("ftnDewArmorBtn").Find("blocker").gameObject.SetActive(!shopContents.hasFtnDewArmor);
    }

    public static void Hide_Static() => instance.Hide();

    private void Hide() {
        gameObject.SetActive(false);
        if (onHide != null) {
            onHide();
        }
    }

    public static void Show_Static(GameData.ShopContents shopContents, Action onHide) => instance.Show(shopContents, onHide);

    private void Show(GameData.ShopContents shopContents, Action onHide) {
        this.shopContents = shopContents;
        this.onHide = onHide;
        gameObject.SetActive(true);
        Refresh();
    }




}
