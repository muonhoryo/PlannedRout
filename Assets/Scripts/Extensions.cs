

using UnityEngine;

namespace PlannedRout
{
    public static class Extensions
    {
        public static Vector2Int GetIntegerPosition(this Vector2 position)
        {
            return new Vector2Int((int)Mathf.Round(position.x), (int)Mathf.Round(position.y));
        }
        public static Vector2Int GetIntegerPosition(this Vector3 position) =>
            GetIntegerPosition((Vector2)position);

        public static Vector3 GetPhysicsPosition(this Vector2Int integerPosition)
        {
            return new Vector3(integerPosition.x, integerPosition.y, 0);
        }
    }
}