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
using TMPro;
using CodeMonkey.Utils;

public class DamagePopup : MonoBehaviour {

    // Create a Damage Popup
    public static DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit) {
        Transform damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(damageAmount, isCriticalHit);

        return damagePopup;
    }

    public static DamagePopup Create(Vector3 position, string text, Color color) {
        DamagePopup damagePopup = Create(position, 0, false);
        damagePopup.SetText(text);
        damagePopup.SetTextColor(color);
        return damagePopup;
    }

    private const int SORTING_ORDER_BASE = 10000;
    private static int sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    private Vector3 moveVector;

    private void Awake() {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damageAmount, bool isCriticalHit) {
        textMesh.SetText(damageAmount.ToString());
        if (!isCriticalHit) {
            // Normal hit
            textMesh.fontSize = 36;
            textColor = UtilsClass.GetColorFromString("FFC500");
        } else {
            // Critical hit
            textMesh.fontSize = 45;
            textColor = UtilsClass.GetColorFromString("FF2B00");
        }
        textMesh.color = textColor;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        textMesh.sortingOrder = SORTING_ORDER_BASE + sortingOrder;

        moveVector = new Vector3(.7f, 1) * 60f;
    }

    public void SetText(string text) {
        textMesh.SetText(text);
    }

    public void SetTextColor(Color color) {
        textColor = color;
        textMesh.color = textColor;
    }

    private void Update() {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f) {
            // First half of the popup lifetime
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        } else {
            // Second half of the popup lifetime
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0) {
            // Start disappearing
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0) {
                Destroy(gameObject);
            }
        }
    }

}
