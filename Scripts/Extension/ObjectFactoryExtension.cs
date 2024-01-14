namespace UITemplate.Scripts.Extension
{
    using System;
    using System.Collections.Generic;
    using UITemplate.Scripts.Extension.Base;
    using UnityEngine;

    public static class ObjectFactoryExtension
    {
        private static readonly Dictionary<Type, object> ObjectDictionary = new Dictionary<Type, object>();

        public static void GetService<T>(ref T obj) where T : class, new()
        {
            var objectType = typeof(T);

            if (ObjectDictionary.TryGetValue(objectType, out var existingObject))
            {
                obj = (T)existingObject;
            }
            else
            {
                var newObject = new T();
                ObjectDictionary[objectType] = newObject;

                obj = newObject;
            }
        }
        
        /// <summary>
        /// Can using with interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>() where T : class, new()
        {
            var objectType = typeof(T);

            if (ObjectDictionary.TryGetValue(objectType, out var existingObject))
            {
                return (T)existingObject;
            }
            else
            {
                var newObject = new T();
                ObjectDictionary[objectType] = newObject;

                return  newObject;
            }
        }
        public static T GetService<T>(this T obj) where T : class, new()
        {
            var objectType = typeof(T);

            if (ObjectDictionary.TryGetValue(objectType, out var existingObject))
            {
                return (T)existingObject;
            }
            else
            {
                var newObject = new T();
                ObjectDictionary[objectType] = newObject;
                
                return newObject;
            }
        }
        
        public static void GetMonoService<T>(ref T obj) where T : MonoService, new()
        {
            var objectType = typeof(T);

            if (ObjectDictionary.TryGetValue(objectType, out var existingObject) && existingObject is T typedObject)
            {
                obj = typedObject;
            }
            else
            {
                var newObject = new GameObject(objectType.Name).AddComponent<T>();
                ObjectDictionary[objectType]  = newObject;
                newObject.Init();

                obj = newObject;
            }
        }
        
        public static T GetMonoService<T>() where T : MonoService, new()
        {
            var objectType = typeof(T);

            if (ObjectDictionary.TryGetValue(objectType, out var existingObject))
            {
                return (T)existingObject;
            }
            else
            {
                var newObject = new GameObject(objectType.Name).AddComponent<T>();
                ObjectDictionary[objectType] = newObject;
                newObject.Init();

                return  newObject;
            }
        }
    }
}