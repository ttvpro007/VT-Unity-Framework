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
using CodeMonkey.Utils;

public class Dialogue : MonoBehaviour {

    private static Dialogue instance;
    public static Dialogue GetInstance() => instance;

    private Transform leftCharacterTransform;
    private Transform rightCharacterTransform;
    private UIChatBubble leftChatBubble;
    private UIChatBubble rightChatBubble;
    private Text leftCharacterNameText;
    private Text rightCharacterNameText;
    private List<Action> actionList;
    private List<DialogueOption> dialogOptionList;

    private void Awake() {
        instance = this;

        leftCharacterTransform = transform.Find("LeftCharacter");
        rightCharacterTransform = transform.Find("RightCharacter");

        leftChatBubble = transform.Find("LeftChatBubbleUI").GetComponent<UIChatBubble>();
        rightChatBubble = transform.Find("RightChatBubbleUI").GetComponent<UIChatBubble>();

        leftCharacterNameText = transform.Find("LeftCharacterName").GetComponent<Text>();
        rightCharacterNameText = transform.Find("RightCharacterName").GetComponent<Text>();

        ShowLeftCharacterName("");
        ShowRightCharacterName("");

        leftChatBubble.Hide();
        rightChatBubble.Hide();

        HideLeftCharacter();
        HideRightCharacter();

        Hide();
    }

    private void Update() {
        if (dialogOptionList == null && TestInput()) {
            //SoundManager.PlaySound(SoundManager.Sound.ButtonClick);
            PlayNextAction();
        }
    }

    private bool TestInput() {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public void ShowDialogueOptions(List<DialogueOption> dialogOptionList) {
        this.dialogOptionList = dialogOptionList;
        foreach (DialogueOption dialogOption in dialogOptionList) {
            dialogOption.CreateTransform(transform);
        }
    }

    public void ClearDialogueOptions() {
        if (dialogOptionList == null) return;
        foreach (DialogueOption dialogOption in dialogOptionList) {
            dialogOption.DestroySelf();
        }
        dialogOptionList.Clear();
        dialogOptionList = null;
    }

    public void SetDialogueActions(List<Action> actionList, bool autoPlayNextAction) {
        this.actionList = actionList;
        if (autoPlayNextAction) {
            PlayNextAction();
        }
    }

    public void PlayNextAction() {
        Action action = actionList[0];
        actionList.RemoveAt(0);
        action();
    }

    public void ShowLeftCharacterName(string name) {
        leftCharacterNameText.text = name;
    }

    public void ShowRightCharacterName(string name) {
        rightCharacterNameText.text = name;
    }

    public void HideLeftCharacterName() {
        ShowLeftCharacterName("");
    }

    public void HideRightCharacterName() {
        ShowRightCharacterName("");
    }

    public void ShowLeftText(string text) {
        leftChatBubble.ShowText(text);
    }

    public void HideLeftText() {
        leftChatBubble.Hide();
    }

    public void ShowRightText(string text) {
        rightChatBubble.ShowText(text);
    }

    public void HideRightText() {
        rightChatBubble.Hide();
    }

    public void ShowLeftActiveTalkerHideRight(string text) {
        FadeRightCharacter();
        HideRightText();
        UnFadeLeftCharacter();
        ShowLeftText(text);
    }

    public void ShowRightActiveTalkerHideLeft(string text) {
        FadeLeftCharacter();
        HideLeftText();
        UnFadeRightCharacter();
        ShowRightText(text);
    }

    public void ShowLeftCharacter(Sprite characterSprite, bool faded) {
        leftCharacterTransform.gameObject.SetActive(true);
        leftCharacterTransform.GetComponent<Image>().sprite = characterSprite;
        leftCharacterTransform.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        if (faded) {
            leftCharacterTransform.GetComponent<Image>().color = new Color(.4f, .4f, .4f, 1f);
        }
    }

    public void FadeLeftCharacter() {
        leftCharacterTransform.GetComponent<Image>().color = new Color(.4f, .4f, .4f, 1f);
    }

    public void UnFadeLeftCharacter() {
        leftCharacterTransform.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
    }

    public void HideLeftCharacter() {
        leftCharacterTransform.gameObject.SetActive(false);
    }
    
    public void ShowRightCharacter(Sprite characterSprite, bool faded) {
        rightCharacterTransform.gameObject.SetActive(true);
        rightCharacterTransform.GetComponent<Image>().sprite = characterSprite;
        rightCharacterTransform.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
        if (faded) {
            rightCharacterTransform.GetComponent<Image>().color = new Color(.4f, .4f, .4f, 1f);
        }
    }

    public void FadeRightCharacter() {
        rightCharacterTransform.GetComponent<Image>().color = new Color(.4f, .4f, .4f, 1f);
    }

    public void UnFadeRightCharacter() {
        rightCharacterTransform.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
    }

    public void HideRightCharacter() {
        rightCharacterTransform.gameObject.SetActive(false);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }






    /*
     * Single Dialogue Option
     * */
    public class DialogueOption {

        private Transform transform;
        private string text;
        private Action triggerAction;
        private Option option;

        public enum Option {
            _1,
            _2,
            _3,
        }

        public DialogueOption(Option option, string text, Action triggerAction) {
            this.option = option;
            this.text = text;
            this.triggerAction = triggerAction;
        }

        public void CreateTransform(Transform parent) {
            transform = Instantiate(GameAssets.i.pfChatOption, parent);
            Vector2 anchoredPosition;
            switch (option) {
            default:
            case Option._1: anchoredPosition = new Vector2(320, 100); break;
            case Option._2: anchoredPosition = new Vector2(320, 50);  break;
            case Option._3: anchoredPosition = new Vector2(320, 150); break;
            }
            transform.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
            transform.Find("text").GetComponent<Text>().text = text;
            transform.GetComponent<Button_UI>().ClickFunc = Trigger;
        }

        public void Trigger() {
            triggerAction();
        }

        public void DestroySelf() {
            Destroy(transform.gameObject);
        }

    }

}
