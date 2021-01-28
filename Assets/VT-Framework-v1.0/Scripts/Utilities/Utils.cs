using System;
using UnityEngine;
using UnityEngine.UI;

namespace VT.Utilities
{
    public static class Utils
    {
        #region FLOAT
        public static float ReplaceIfZero(float numberToCheck, float replaceValue)
        {
            return numberToCheck == 0f ? replaceValue : numberToCheck;
        }
        #endregion

        #region CAMERA FX
        public static void DoCameraShake(float intensity, float duration)
        {
            Vector3 originalCameraPosition = Camera.main.transform.position;
            Vector3 lastCameraPosition = Vector3.zero;
            RuntimeMonoBehaviour.Create
            (
                "Camera Shake",
                () =>
                {
                    duration -= Time.unscaledDeltaTime;
                    Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized * intensity;
                    Camera.main.transform.position = Camera.main.transform.position - lastCameraPosition + randomPosition;
                    lastCameraPosition = randomPosition;

                    if (duration <= 0f)
                    {
                        Camera.main.transform.position = originalCameraPosition;
                    }

                    return duration <= 0f;
                }
            );
        }
        #endregion

        #region SPRITE
        public static Transform DrawSprite(Sprite sprite, Color color, Vector3 size, Vector3 position, Transform parent, string name = "Sprite")
        {
            GameObject go = new GameObject(name, typeof(SpriteRenderer));

            Transform t = go.transform;
            t.SetParent(parent);
            t.localPosition = position;
            t.localScale = size;

            SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = color;

            return t;
        }

        public static RectTransform DrawSpriteUI(Color color, Vector2 sizeDelta, Vector2 anchoredPosition, RectTransform parent, string name = "Sprite")
        {
            return DrawSpriteUI(null, color, sizeDelta, anchoredPosition, parent, name);
        }

        public static RectTransform DrawSpriteUI(Sprite sprite, Color color, Vector2 sizeDelta, Vector2 anchoredPosition, RectTransform parent, string name = "Sprite")
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent);
            rt.sizeDelta = sizeDelta;
            rt.anchoredPosition = anchoredPosition;
            rt.localScale = Vector3.one;

            Image i = go.GetComponent<Image>();
            i.sprite = sprite;
            i.color = color;

            return rt;
        }
        #endregion

        #region DEBUG UTILS
        public static void DebugMethod(string signature, string description, int preLineBreak, params string[] parameters)
        {
            DebugEntry($"method call", "", preLineBreak);
            DebugLabel($"signature", signature, 1);
            DebugLabel($"description", description, 1);

            foreach (var parameter in parameters)
            {
                DebugLabel("parameter", parameter, 1);
            }
        }

        public static void DebugCode<T>(Func<T> codeAction, string code, string resultPrefix, string resultSuffix)
        {
            if (codeAction == null) return;

            DebugLabel($"code", code, 1);
            DebugLabel($"result", resultPrefix, 1);
            DebugText($"{codeAction.Invoke()}", 0);
            DebugText($"{resultSuffix}", 0);
        }

        public static void DebugCodeEntry<T>(Func<T> codeAction, string entryName, string description, string code, string resultPrefix, string resultSuffix, int preLineBreak = 1)
        {
            if (entryName != "")
            {
                DebugEntry($"{entryName}", description, preLineBreak);
            }

            DebugCode(codeAction, code, resultPrefix, resultSuffix);
        }

        public static void DebugLineBreak(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Debug.Log("\n");
            }
        }

        public static void DebugEntry(string entryName, string description, int preLineBreak = 1)
        {
            DebugLineBreak(preLineBreak);
            DebugText($"[ENTRY - {entryName.ToUpper()}] ");

            if (description != string.Empty)
            {
                DebugLabel("description", description, 1);
            }
        }

        public static void DebugLabel(string labelName, string description, int preLineBreak = 1)
        {
            DebugLineBreak(preLineBreak);
            DebugText($"[{labelName.ToUpper()}] " + description);
        }

        public static void DebugText(string text, int preLineBreak = 0)
        {
            DebugLineBreak(preLineBreak);
            Debug.Log(text);
        }
        #endregion
    }
}