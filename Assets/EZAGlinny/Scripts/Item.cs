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

public class Item : MonoBehaviour {

    public enum ItemType {
        FtnDew,
        HealthPotion,
    }

    private ItemType itemType;
    private int amount;
    private Vector3 position;
    private bool isDestroyed;

    public Item(ItemType itemType, int amount, Vector3 position) {
        this.itemType = itemType;
        this.amount = amount;
        this.position = position;
        isDestroyed = false;
    }

    public ItemType GetItemType() {
        return itemType;
    }

    public int GetAmount() {
        return amount;
    }

    public Vector3 GetPosition() {
        return position;
    }

    public bool IsDestroyed() {
        return isDestroyed;
    }

    public void DestroySelf() {
        isDestroyed = true;
    }

}
