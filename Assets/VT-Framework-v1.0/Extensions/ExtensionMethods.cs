using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System;

namespace VT.Extensions
{
    public static class ExtensionMethods
    {
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

        private static int Convert2DIndexesTo1DIndex(int outerIndex, int innerIndex, int length)
        {
            return outerIndex * length + innerIndex;
        }
        #endregion

        #region LIST
        public static List<T> Shuffle<T>(this List<T> listToShuffle)
        {
            if (listToShuffle == null || listToShuffle.Count <= 1)
            {
                return listToShuffle;
            }

            int randomInRange;
            int count = listToShuffle.Count;
            T temp;

            for (int i = 0; i < count; i++)
            {
                randomInRange = UnityEngine.Random.Range(0, count);
                temp = listToShuffle[count - 1];
                listToShuffle[count - 1] = listToShuffle[randomInRange];
                listToShuffle[randomInRange] = temp;
                count--;
            }

            return listToShuffle;
        }

        public static Stack<T> ShuffleToStack<T>(this List<T> listToShuffle)
        {
            if (listToShuffle == null || listToShuffle.Count <= 1)
            {
                return new Stack<T>(listToShuffle);
            }

            int randomInRange;
            int count = listToShuffle.Count;
            T temp;

            for (int i = 0; i < count; i++)
            {
                randomInRange = UnityEngine.Random.Range(0, count);
                temp = listToShuffle[count - 1];
                listToShuffle[count - 1] = listToShuffle[randomInRange];
                listToShuffle[randomInRange] = temp;
                count--;
            }

            return new Stack<T>(listToShuffle);
        }
        #endregion

        #region IENUMERABLE
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || source.Count() == 0;
        }
        #endregion

        #region ENUM
        public static IEnumerable<T> GetEnumValues<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
        #endregion

        #region TRANSFORM
        public static void ResetLocalTransform(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static T GetFirstComponentOfTypeInHierachy<T>(this Transform transform, T component = null, bool addRoot = false) where T : UnityEngine.Object
        {
            if (component != null)
                return component;

            if (addRoot)
            {
                component = transform.GetComponent<T>();

                if (component != null)
                    return component;
            }

            foreach (Transform child in transform)
            {
                component = GetFirstComponentOfTypeInHierachy(child, component, true);

                if (component != null)
                    return component;
            }

            return component;
        }

        public static List<T> GetComponentsOfTypeInHierachy<T>(this Transform root, List<T> genericList = null, bool addRoot = true) where T : UnityEngine.Object
        {
            if (genericList == null)
                genericList = new List<T>();

            if (addRoot)
            {
                T rootComponent = root.GetComponent<T>();

                if (rootComponent)
                    genericList.Add(rootComponent);
            }

            foreach (Transform child in root)
            {
                GetComponentsOfTypeInHierachy(child, genericList, true);
            }

            return genericList;
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