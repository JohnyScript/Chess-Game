namespace UnityEngine
{
    public static class Vector2IntUtils
    {
        public static Vector2Int Add (this Vector2Int value, int valueToAdd)
        {
            return new Vector2Int(value.x + valueToAdd, value.y + valueToAdd);
        }


        public static Vector2Int Subtract (this Vector2Int value, int valueToAdd)
        {
            return new Vector2Int(value.x - valueToAdd, value.y - valueToAdd);
        }
    }
}