using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VT.Utilities.Factory;

namespace VT.Utilities
{
    public static class Utils
    {
        #region INT
        /// <summary>
        /// Return a wrapped integer in range 0 (inclusive) and max (exclusive)
        /// </summary>
        public static int WrapIndex(int max, int current)
        {
            return WrapIndex(0, max, current);
        }

        /// <summary>
        /// Return a wrapped integer in range min (inclusive) and max (exclusive)
        /// </summary>
        public static int WrapIndex(int min, int max, int current)
        {
            int range = max - min;
            return ((current - min) % range + range) % range + min;
        }
        #endregion

        #region STRING
        public static string ConvertToTMPColorString(string htmlColor, string source)
        {
            return $"<color=#{htmlColor}>{source}</color>";
        }

        public static string ConvertToTMPColorString(Color color, string source)
        {
            return ConvertToTMPColorString(ColorUtility.ToHtmlStringRGB(color), source);
        }

        public static string LineBreak(int amount)
        {
            string lineBreaks = string.Empty;
            for (int i = 0; i < amount; i++)
            {
                lineBreaks += "\n";
            }
            return lineBreaks;
        }

        public static string Tab(int amount)
        {
            string tabs = string.Empty;
            for (int i = 0; i < amount; i++)
            {
                tabs += "\t";
            }
            return tabs;
        }

#if UNITY_EDITOR
        public static string GetNiceName(string name)
        {
            string clipName = ObjectNames.NicifyVariableName(name).Replace(" ", "");

            string[] splitName = clipName.Split('-');
            if (splitName.Length > 1)
            {
                clipName = splitName[0];
            }

            return clipName;
        }
#endif

        #endregion

        #region ENUM
        public static IEnumerable<T> GetEnumValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static int GetEnumIndex<T>(object enumValue)
        {
            return Array.IndexOf(Enum.GetValues(typeof(T)), enumValue);
        }

        public static T GetEnumValueByIndex<T>(int enumIndex)
        {
            return (T)Enum.GetValues(typeof(T)).GetValue(enumIndex);
        }
        #endregion

        #region FLOAT
        public static float ReplaceIfZero(float numberToCheck, float replaceValue)
        {
            return numberToCheck == 0f ? replaceValue : numberToCheck;
        }
        #endregion

        #region VECTOR 3
        public static Vector3 GetXYRandomDirection()
        {
            return GetXYRandomUnitPosition().normalized;
        }

        public static Vector3 GetXYRandomUnitPosition()
        {
            return UnityEngine.Random.insideUnitCircle;
        }
        #endregion

        #region RUNTIME MONO BEHAVIOUR
        public static RuntimeMonoBehaviour DoCameraShake(float intensity, float duration)
        {
            Vector3 originalCameraPosition = Camera.main.transform.position;
            Vector3 lastCameraPosition = Vector3.zero;
            return RuntimeMonoBehaviour.Create
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

        /// <summary>
        /// Create a delegate action that runs every interval within executionTimes
        /// </summary>
        /// <param name="action">The action to be executed</param>
        /// <param name="interval">The interval that the action will be executed</param>
        /// <param name="executionTimes">The times that the action will be executed (if = 0, execute unlimitedly)</param>
        public static RuntimeMonoBehaviour CreateFunctionTimer(Action action, float interval, int executionTimes = 0)
        {
            executionTimes = Mathf.Max(0, executionTimes);
            float executeTimer = Time.time + interval;
            int count = 0;
            return RuntimeMonoBehaviour.Create
            (
                "Function Timer",
                () =>
                {
                    if (Time.time >= executeTimer)
                    {
                        action?.Invoke();
                        executeTimer = Time.time + interval;
                        count++;
                    }
                    return executionTimes > 0 ? count >= executionTimes : false;
                }
            );
        }
        #endregion

#if UNITY_EDITOR
        //#region MONKEY COMMANDS
        //[Command("Copy All Components",
        //    Help = "Copies all components of the first selected ofject",
        //    QuickName = "CAC", DefaultValidation = DefaultValidation.AT_LEAST_ONE_GAME_OBJECT)]
        //public static void CopyComponents()
        //{
        //    GameObject go = Selection.activeGameObject;
        //    Component[] components = go.GetComponents<Component>();
        //    string[] serializedData = new string[components.Length];
        //    char separator = ':';

        //    for (int i = 0; i < components.Length; i++)
        //    {
        //        serializedData[i] = components[i].GetType().AssemblyQualifiedName + separator + EditorJsonUtility.ToJson(components[i]);
        //    }

        //    EditorGUIUtility.systemCopyBuffer = string.Join("\n", serializedData);
        //}

        //[Command("Paste All Components",
        //    Help = "Paste all previously copied components to the selected ofject",
        //    QuickName = "PAC", DefaultValidation = DefaultValidation.AT_LEAST_ONE_GAME_OBJECT)]
        //public static void PasteComponents()
        //{
        //    GameObject go = Selection.activeGameObject;
        //    string[] serializedData = EditorGUIUtility.systemCopyBuffer.Split('\n');
        //    char[] separator = { ':' };
        //    foreach (var data in serializedData)
        //    {
        //        string[] typeAndJson = data.Split(separator, 2);
        //        Type type = Type.GetType(typeAndJson[0]);
        //        if (type.FullName == "UnityEngine.Transform")
        //            EditorJsonUtility.FromJsonOverwrite(typeAndJson[1], go.transform);
        //        else
        //            EditorJsonUtility.FromJsonOverwrite(typeAndJson[1], go.AddComponent(type));
        //    }
        //}
        //#endregion
#endif

#if UNITY_EDITOR
        #region SPRITE
        [Serializable]
        public struct SpriteConstructor
        {
            public SpriteConstructor(Sprite sprite, Color color, Vector3 size, Vector3 position, Transform parent, Material material, string sortingLayerName, int orderInLayer, string name)
            {
                Sprite = sprite;
                Color = color;
                Size = size;
                Position = position;
                Parent = parent;
                Material = material;
                SortingLayerName = sortingLayerName;
                OrderInLayer = orderInLayer;
                Name = name;
            }

            public Sprite Sprite;
            public Color Color;
            public Vector3 Size;
            public Vector3 Position;
            public Transform Parent;
            public Material Material;
            public string SortingLayerName;
            public int OrderInLayer;
            public string Name;
        }

        public static SpriteRenderer DrawSprite(SpriteConstructor spriteConstructor)
        {
            return DrawSprite
            (
                spriteConstructor.Sprite,
                spriteConstructor.Color,
                spriteConstructor.Size,
                spriteConstructor.Position,
                spriteConstructor.Parent,
                spriteConstructor.Material,
                spriteConstructor.SortingLayerName,
                spriteConstructor.OrderInLayer,
                spriteConstructor.Name
            );
        }

        public static SpriteRenderer DrawSprite(Sprite sprite, Color color, Vector3 size, Vector3 position, Transform parent, Material material = null, string sortingLayerName = default, int orderInLayer = default, string name = "Sprite")
        {
            SpriteRenderer sr = VTObjectFactory.CreateEditorGameObject<SpriteRenderer>(name);
            sr.sprite = sprite;
            sr.color = color;
            sr.sortingLayerName = sortingLayerName;
            sr.sortingOrder = orderInLayer;

            Transform t = sr.transform;
            t.transform.SetParent(parent);
            t.transform.localPosition = position;
            t.transform.localScale = size;

            if (material)
                sr.material = material;

            return sr;
        }

        public static Image DrawImage(Color color, Vector2 sizeDelta, Vector2 anchoredPosition, RectTransform parent, Material material = null, string name = "Sprite")
        {
            return DrawImage(null, color, sizeDelta, anchoredPosition, parent, material, name);
        }

        public static Image DrawImage(Sprite sprite, Color color, Vector2 sizeDelta, Vector2 anchoredPosition, RectTransform parent, Material material = null, string name = "Sprite")
        {
            Image i = VTObjectFactory.CreateEditorGameObject<Image>(name, typeof(Image));
            i.gameObject.layer = parent.gameObject.layer;

            i.sprite = sprite;
            i.color = color;

            RectTransform rt = i.rectTransform;
            rt.SetParent(parent);
            rt.sizeDelta = sizeDelta;
            rt.anchoredPosition = anchoredPosition;
            rt.localScale = Vector3.one;

            if (material)
                i.material = material;

            return i;
        }
        #endregion
#endif

        #region DEBUG UTILS
        public static void Log(object caller, string message)
        {
            Debug.Log(caller != null ? $"[{caller}]: {message}" : message);
        }

        public static void LogWarning(object caller, string message)
        {
            Debug.LogWarning(caller != null ? $"[{caller}]: {message}" : message);
        }

        public static void LogError(object caller, string message)
        {
            Debug.LogError(caller != null ? $"[{caller}]: {message}" : message);
        }

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