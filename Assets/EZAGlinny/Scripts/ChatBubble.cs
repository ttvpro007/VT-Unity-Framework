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

public class ChatBubble {

    public static void Create(Transform parent, Vector3 position, string text) {
        InitIfNeeded();
        Transform chatBubbleTransform = Object.Instantiate(GameAssets.i.pfChatBubble, parent);
        chatBubbleTransform.localPosition = position;
        chatBubbleTransform.localScale = Vector3.one * (1f / parent.localScale.x);

        ChatBubble chatBubble = new ChatBubble(chatBubbleTransform, text);

        chatBubbleList.Add(chatBubble);
    }
    
    private static GameObject initGameObject; // Global game object used for initializing class, is destroyed on scene change
    private static List<ChatBubble> chatBubbleList;
    private static int sortingOrder;
    
    private static void InitIfNeeded() {
        if (initGameObject == null) {
            initGameObject = new GameObject("ChatBubble");
            chatBubbleList = new List<ChatBubble>();
            FunctionUpdater.Create(() => Update_Static());
            sortingOrder = 10000;
        }
    }

    private static void Update_Static() {
        for (int i = 0; i < chatBubbleList.Count; i++) {
            chatBubbleList[i].Update();
            if (chatBubbleList[i].isDestroyed) {
                chatBubbleList.RemoveAt(i);
                i--;
            }
        }
    }



    private Transform transform;
    private bool isDestroyed;
    private float timer;

    private ChatBubble(Transform transform, string text) {
        this.transform = transform;

        transform.Find("Text").GetComponent<TextMesh>().text = text;
        transform.Find("Text").GetComponent<MeshRenderer>().sortingOrder = sortingOrder + 1;
        transform.Find("Bubble").GetComponent<SpriteRenderer>().sortingOrder = sortingOrder + 0;
        transform.Find("Handle").GetComponent<SpriteRenderer>().sortingOrder = sortingOrder + 1;
        sortingOrder += 2;

        float textWidth = transform.Find("Text").GetComponent<MeshRenderer>().bounds.size.x * 2f + 8f;
        transform.Find("Bubble").GetComponent<SpriteRenderer>().size = new Vector2(textWidth, 11.5f);
        transform.Find("Bubble").localPosition = new Vector3(textWidth * .25f + 2.5f, 3.9f);

        timer = 5f;
    }

    public void Update() {
        if (isDestroyed) return;
        timer -= Time.deltaTime;
        if (timer <= 0f) {
            isDestroyed = true;
            Object.Destroy(transform.gameObject);
        }
    }

}
