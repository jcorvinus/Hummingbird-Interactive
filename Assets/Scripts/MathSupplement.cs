using UnityEngine;
using System.Collections;

public static class MathSupplement
{
    /// <summary>
    /// Flips a 0-1 value. Will always return positive.
    /// </summary>
    /// <param name="tValue">input value</param>
    /// <returns></returns>
    public static float UnitReciprocal(float tValue)
    {
        return Mathf.Abs(((tValue * 100) - 100) * 0.01f);
    }

    public static float Coserp(float low, float high, float t)
    {
        return Mathf.Lerp(low, high, (1f - Mathf.Cos(t * Mathf.PI * 0.5f)));
    }

    /// <summary>
    /// lerp with an exponential tvalue
    /// </summary>
    public static float Exerp(float from, float to, float t)
    {
        return Mathf.Lerp(from, to, t * t);
    }

    public static Vector3 Exerp(Vector3 to, Vector3 from, float tValue)
    {
        return new Vector3(Exerp(to.x, from.x, tValue),
            Exerp(to.y, from.y, tValue),
            Exerp(to.z, from.z, tValue));
    }
}
