namespace AXitUnityTemplate.ObjectPool
{
    using UnityEngine;

    public static class ExtensionMethod
    {
        public static string GetPath(this Transform current)
        {
            if (current.parent == null)
                return current.name;

            return current.parent.GetPath() + "/" + current.name;
        }

        public static string Path(this Component component) { return ExtensionMethod.GetPath(component.transform); }

        public static string Path(this GameObject gameObject) { return ExtensionMethod.GetPath(gameObject.transform); }

        public static Vector2 AsUnityVector2(this System.Numerics.Vector2 v) { return new Vector2(v.X, v.Y); }

        public static Vector3 AsUnityVector3(this System.Numerics.Vector3 v) { return new Vector3(v.X, v.Y, v.Z); }
    }
}