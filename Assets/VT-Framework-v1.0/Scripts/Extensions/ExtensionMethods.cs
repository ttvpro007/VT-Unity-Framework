using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEditor;
using System.Linq.Expressions;

namespace VT.Extensions
{
    public static class ExtensionMethods
    {
        #region STRING
        public static string RemoveSpaces(this string source)
        {
            return source.Replace(" ", string.Empty);
        }

        //https://stackoverflow.com/questions/155303/net-how-can-you-split-a-caps-delimited-string-into-an-array
        public static string AddSpaceInterCappedString(string text)
        {
            return Regex.Replace(text, $"([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }

        //https://stackoverflow.com/questions/155303/net-how-can-you-split-a-caps-delimited-string-into-an-array
        public static string[] SplitInterCappedString(string text)
        {
            return Regex.Split(text, $"/([A-Z]+(?=$|[A-Z][a-z])|[A-Z]?[a-z]+)/g");
        }

        //https://stackoverflow.com/a/7265336
        public static string Replace(this string source, char[] characters, string replacement)
        {
            string pattern = $"[{new string(characters)}]";
            return Regex.Replace(source, pattern, replacement);
        }

        public static string ConvertToNamespaceQualifiedName(this string source)
        {
            return source.Replace(new char[] { ' ', '-' }, string.Empty);
        }

        public static string ToShortenFormFloored(this float number, int decimalPlaces = 1)
        {
            string shortenForm = number.ToShortenForm();
            if (float.TryParse(shortenForm, out float result))
            {
                return ((int)result).ToString();
            }
            return shortenForm;
        }

        public static string ToShortenForm(this float number, int decimalPlaces = 1)
        {
            List<string> suffix = new List<string>
            {
                "",
                "K",
                "M",
                "B"
            };

            int suffixIndex = 0;
            while (number >= 1e3f)
            {
                suffixIndex++;
                number /= 1e3f;
            }

            string numberFormat = "N" + decimalPlaces;
            return $"{number.ToString(number % 1 == 0 ? "N0" : numberFormat)}{suffix[suffixIndex]}";
        }

        public static string ToShortenForm(this int number, int decimalPlaces = 1)
        {
            return ToShortenForm((float)number, decimalPlaces);
        }

        public static string ToShortenFormFloored(this int number, int decimalPlaces = 1)
        {
            return ToShortenFormFloored((float)number, decimalPlaces);
        }
        #endregion

        #region FLOAT
        public static float Round(this float source)
        {
            return Mathf.Round(source);
        }
        #endregion

        #region ARRAY
        public static IEnumerable<T> Where<T>(this T[,] multiDArray, Func<T, bool> predicate)
        {
            List<T> list = new List<T>();

            foreach (var item in multiDArray)
            {
                if (predicate(item))
                {
                    list.Add(item);
                }
            }

            return list;
        }
        #endregion

        #region ENUM
        //https://stackoverflow.com/a/26289874
        public static class EnumConverter<TEnum> where TEnum : Enum
        {
            public static readonly Func<ulong, TEnum> Convert = GenerateConverter();

            private static Func<ulong, TEnum> GenerateConverter()
            {
                var parameter = Expression.Parameter(typeof(ulong));
                var dynamicMethod = Expression.Lambda<Func<ulong, TEnum>>
                (
                    Expression.Convert(parameter, typeof(TEnum)), parameter
                );
                return dynamicMethod.Compile();
            }
        }

        public static bool HasFlag<T>(this T key, T flag) where T : Enum
        {
            var keyFlagValue = ConvertToLongValue(key, flag);
            return (keyFlagValue.Item1 & keyFlagValue.Item2) == keyFlagValue.Item2;
        }

        public static T SetFlag<T>(this T key, T flag) where T : Enum
        {
            var keyFlagValue = ConvertToLongValue(key, flag);
            //return (T)Enum.Parse(typeof(T), (keyFlagValue.Item1 | keyFlagValue.Item2).ToString());
            return EnumConverter<T>.Convert(keyFlagValue.Item1 | keyFlagValue.Item2);
        }

        public static T UnsetFlag<T>(this T key, T flag) where T : Enum
        {
            var keyFlagValue = ConvertToLongValue(key, flag);
            //return (T)Enum.Parse(typeof(T), (keyFlagValue.Item1 & (~keyFlagValue.Item2)).ToString());
            return EnumConverter<T>.Convert(keyFlagValue.Item1 & (~keyFlagValue.Item2));
        }

        public static Enum ToggleFlag<T>(this T key, T flag) where T : Enum
        {
            var keyFlagValue = ConvertToLongValue(key, flag);
            //return (T)Enum.Parse(typeof(T), (keyFlagValue.Item1 ^ keyFlagValue.Item2).ToString());
            return EnumConverter<T>.Convert(keyFlagValue.Item1 ^ keyFlagValue.Item2);
        }

        private static (ulong, ulong) ConvertToLongValue<T>(T a, T b)
        {
            return (Convert.ToUInt64(a), Convert.ToUInt64(b));
        }
        #endregion

        #region LIST
        public static List<T> CreateNewIfNull<T>(this List<T> source)
        {
            if (source == null)
                source = new List<T>();

            return source;
        }

        public static Stack<T> ShuffleToStack<T>(this List<T> source)
        {
            if (source == null || source.Count <= 1)
            {
                return new Stack<T>(source);
            }

            int randomInRange;
            int count = source.Count;
            T temp;

            for (int i = 0; i < count; i++)
            {
                randomInRange = UnityEngine.Random.Range(0, count);
                temp = source[count - 1];
                source[count - 1] = source[randomInRange];
                source[randomInRange] = temp;
                count--;
            }

            return new Stack<T>(source);
        }
        #endregion

        #region DICTIONARY
        public static Dictionary<TK, TV> CreateNewIfNull<TK, TV>(this Dictionary<TK, TV> source)
        {
            if (source == null)
                source = new Dictionary<TK, TV>();

            return source;
        }
        #endregion

        #region IENUMERABLE
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || source.Count() == 0;
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            int count = source.Count();

            if (source == null || count <= 1)
            {
                yield return source.FirstOrDefault();
            }

            UnityEngine.Random.InitState((int)DateTime.Now.Ticks ^ 17 ^ 23);

            int randomInRange;
            IList<T> sourceList = source.ToList();

            for (int i = 0; i < count; i++)
            {
                randomInRange = UnityEngine.Random.Range(i, count);
                yield return sourceList[randomInRange];
                sourceList[randomInRange] = sourceList[i];
            }
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            return new Queue<T>(source);
        }

        public static Stack<T> ToStack<T>(this IEnumerable<T> source)
        {
            return new Stack<T>(source);
        }
        #endregion

        #region VECTOR 3
        public static float GetAngleFromDirection(this Vector3 direction)
        {
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        #endregion

        #region TRANSFORM
        public static void ResetLocalTransform(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static T GetFirstComponentOfTypeInHierachy<T>(this Transform root, Func<T, bool> predicate = null, bool checkRoot = true) where T : UnityEngine.Object
        {
            var components = GetComponentsOfTypeInHierachy(root, predicate, null, checkRoot, true);
            return !components.IsNullOrEmpty() ? components[0] : null;
        }

        public static List<T> GetComponentsOfTypeInHierachy<T>(this Transform root, Func<T, bool> predicate = null, List<T> genericList = null, bool checkRoot = true, bool getFirstOnly = false) where T : UnityEngine.Object
        {
            if (genericList == null)
                genericList = new List<T>();

            if (checkRoot)
            {
                T[] rootComponents = root.GetComponents<T>();

                if (rootComponents != null && rootComponents.Count() > 0)
                {
                    if (predicate != null)
                    {
                        foreach (var component in rootComponents)
                        {
                            if (predicate(component))
                                genericList.Add(component);

                            if (genericList.Count > 0 && getFirstOnly)
                                return genericList;
                        }
                    }
                    else
                    {
                        genericList.AddRange(rootComponents);
                    }
                }
            }

            foreach (Transform child in root)
            {
                GetComponentsOfTypeInHierachy(child, predicate, genericList);
            }

            return genericList;
        }

        public static void SetSizeX(this Transform source, float x)
        {
            source.localScale = new Vector3(x, source.localScale.y, source.localScale.z);
        }
        
        public static void SetSizeY(this Transform source, float y)
        {
            source.localScale = new Vector3(source.localScale.x, y, source.localScale.z);
        }

        public static void SetSizeZ(this Transform source, float z)
        {
            source.localScale = new Vector3(source.localScale.x, source.localScale.y, z);
        }
        #endregion

        #region GAME OBJECT
        public static GameObject CreateNewIfNull(this GameObject source, string name)
        {
            if (!source)
                source = new GameObject(name);

            return source;
        }

#if UNITY_EDITOR
        public static GameObject Focus(this GameObject source)
        {
            Selection.activeObject = source;
            return source;
        }

        public static GameObject SetParentAndAlign(this GameObject source, GameObject parent)
        {
            GameObjectUtility.SetParentAndAlign(source, parent);
            return source;
        }
#endif

        #endregion

        #region SCRIPTABLE OBJECT
        public static T ResourcesLoadObjectWithClassAsName<T>() where T : UnityEngine.Object
        {
            return Resources.Load<T>(typeof(T).Name);
        }

        public static UnityEngine.Object ResourcesLoadObjectWithClassAsName(Type type)
        {
            return Resources.Load(type.Name);
        }
        #endregion

        #region UNITY COMPONENT
        public static T DeepCopy<T>(this T original, GameObject destination) where T : Component
        {
            Type type = original.GetType();
            Component clone = destination.AddComponent(type);
            BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Default | BindingFlags.DeclaredOnly;

            FieldInfo[] fields = type.GetFields(flags);
            foreach (var field in fields)
            {
                field.SetValue(clone, field.GetValue(original));
            }

            PropertyInfo[] properties  = type.GetProperties(flags);
            foreach (var property in properties)
            {
                property.SetValue(clone, property.GetValue(original));
            }

            return (T) clone;
        }
        #endregion

        #region EXPERIMENTAL
        private static void RearrangeChildrenVerticalPositions<T>(this Transform transform, float rangeX, List<Transform> childrenTransform = null)
        {
            if (childrenTransform == null)
            {
                childrenTransform = new List<Transform>();

                foreach (Transform t in transform)
                {
                    childrenTransform.Add(t);
                }
            }

            for (int i = 0; i < childrenTransform.Count; i++)
            {
                float left = transform.position.x - rangeX / 2f;
                float half = rangeX / (childrenTransform.Count * 2);
                Vector3 newPosition = new Vector3(left + half * (2 * i + 1), transform.position.y, transform.position.z);
                childrenTransform[i].transform.position = newPosition;
            }
        }

        private static void RearrangeChildrenHorizontalPositions<T>(this Transform transform, float rangeY, List<Transform> childrenTransform = null)
        {
            if (childrenTransform == null)
            {
                childrenTransform = new List<Transform>();

                foreach (Transform t in transform)
                {
                    childrenTransform.Add(t);
                }
            }

            for (int i = 0; i < childrenTransform.Count; i++)
            {
                float bottom = transform.position.y - rangeY / 2f;
                float half = rangeY / (childrenTransform.Count * 2);
                Vector3 newPosition = new Vector3(transform.position.x, bottom + half * (2 * i + 1), transform.position.z);
                childrenTransform[i].transform.position = newPosition;
            }
        }
        #endregion
    }
}