using UnityEngine;

public static class Vector2Extension {
    public static Vector2 Rotate(this Vector2 v, float degrees) {
        return Quaternion.Euler(0, 0, degrees) * v;
    }

    public static Vector2 Abs(this Vector2 v) {
        return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
    }
 }