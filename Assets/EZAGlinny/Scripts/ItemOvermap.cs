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

public class ItemOvermap : MonoBehaviour {

    private PlayerOvermap playerOvermap;
    private Item item;

    public void Setup(Item item, PlayerOvermap playerOvermap) {
        this.item = item;
        this.playerOvermap = playerOvermap;
    }

    private void Update() {
        float tractorDistance = 40f;
        if (Vector3.Distance(GetPosition(), playerOvermap.GetPosition()) < tractorDistance) {
            // Within Tractor distance, get pulled towards player
            float tractorSpeed = Vector3.Distance(GetPosition(), playerOvermap.GetPosition()) * 8f;
            transform.position += (playerOvermap.GetPosition() - GetPosition()).normalized * tractorSpeed * Time.deltaTime;

            float grabDistance = 2f;
            if (Vector3.Distance(GetPosition(), playerOvermap.GetPosition()) < grabDistance) {
                // Within Grab distance, grab and destroy

                switch (item.GetItemType()) {
                default:
                case Item.ItemType.FtnDew:
                    GameData.AddFtnDewPoints(item.GetAmount());
                    SoundManager.PlaySound(SoundManager.Sound.Coin);
                    break;
                case Item.ItemType.HealthPotion:
                    GameData.AddHealthPotion(item.GetAmount());
                    SoundManager.PlaySound(SoundManager.Sound.Coin);
                    break;
                }

                item.DestroySelf();
                Destroy(gameObject);
            }
        }
    }


    private Vector3 GetPosition() {
        return transform.position;
    }

}
