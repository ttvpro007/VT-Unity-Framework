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

public class OvermapFtnDewHandler {

    private static OvermapFtnDewHandler instance;
    private static bool isInit;
    private static float nextSpawnTimer;

    private static void Init() {
        if (isInit) return;
        isInit = true;
        nextSpawnTimer = 1f;
    }

    private System.Action destroyAllChatBubbles;

    public OvermapFtnDewHandler() {
        instance = this;
        destroyAllChatBubbles = () => { };
        Init();
    }

    public void Update() {
        if (!OvermapHandler.IsOvermapRunning()) {
            return;
        }

        if ((int)GameData.state >= (int)GameData.State.DefeatedTank && !OvermapHandler.GetInstance().sleezerActive) {
            // Count down timer to next popup
            nextSpawnTimer -= Time.deltaTime;
            if (nextSpawnTimer <= 0f) {
                nextSpawnTimer = Random.Range(7f, 15f);
                SpawnChatBubble(GetCharacterTransformToSayChatBubble());
            }
        }
    }

    private void SpawnChatBubble(Transform parent) {
        Transform chatBubbleTransform = Object.Instantiate(GetChatBubbleTransform(), parent);
        chatBubbleTransform.localPosition = new Vector3(3.5f, 5f);

        chatBubbleTransform.Find("Bubble").GetComponent<SpriteRenderer>().sortingOrder = 10000 + 1;
        chatBubbleTransform.Find("Handle").GetComponent<SpriteRenderer>().sortingOrder = 10000 + 2;
        chatBubbleTransform.Find("Text").GetComponent<MeshRenderer>().sortingOrder = 10000 + 2;
        chatBubbleTransform.Find("FountainDewIcon").GetComponent<SpriteRenderer>().sortingOrder = 10000 + 2;

        System.Action destroyChatBubble = () => { 
            if (chatBubbleTransform != null && chatBubbleTransform.gameObject != null) {
                Object.Destroy(chatBubbleTransform.gameObject);
            }
        };
        destroyAllChatBubbles += destroyChatBubble;
        FunctionTimer.Create(() => {
            destroyChatBubble();
        }, 2.4f);
    }

    private Transform GetChatBubbleTransform() {
        List<Transform> list = new List<Transform> {
            GameAssets.i.pfChatBubble_FtnDew,
            GameAssets.i.pfChatBubble_FtnDew_2,
            GameAssets.i.pfChatBubble_FtnDew_3,
            GameAssets.i.pfChatBubble_FtnDew_4,
        };
        return list[Random.Range(0, list.Count)];
    }

    private Transform GetCharacterTransformToSayChatBubble() {
        List<Transform> list = new List<Transform> {
            OvermapHandler.GetInstance().GetPlayer().transform,
        };
        if (OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Sleezer)) != null) {
            list.Add(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Sleezer)).transform);
        }
        if (OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Tank)) != null) {
            list.Add(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Tank)).transform);
        }
        if (OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Healer)) != null) {
            list.Add(OvermapHandler.GetInstance().GetFollower(GameData.GetCharacter(Character.Type.Healer)).transform);
        }
        return list[Random.Range(0, list.Count)];
    }

    private void DestroyAllChatBubbles() {
        destroyAllChatBubbles();
    }

    public static void DestroyAllChatBubbles_Static() {
        instance.DestroyAllChatBubbles();
    }
}
