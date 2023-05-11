using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static Vector3 AxisAlign(this Vector3 inputVector) {
        Vector3 alignedVector = Vector3.zero;
        float xAbs = Mathf.Abs(inputVector.x), yAbs = Mathf.Abs(inputVector.y), zAbs = Mathf.Abs(inputVector.z);
        if (xAbs >= yAbs && xAbs >= zAbs) 
            alignedVector.x = Mathf.Sign(inputVector.x);
        else if (yAbs >= xAbs && yAbs >= zAbs)
            alignedVector.y = Mathf.Sign(inputVector.y);
        else
            alignedVector.z = Mathf.Sign(inputVector.z);
        return alignedVector;
    }
    
    public static Vector3 DirectionToIgnoreY(this Transform tmyPos, Transform tposition)
    {
        Vector3 myPos = tmyPos.position;
        myPos.y = 0;
        var position = tposition.position;
        position.y = 0;
        return myPos.DirectionTo(position);
    }
    
    public static Vector3 DirectionTo(this Transform transform, Transform other)
    {
        return Vector3.Normalize(other.position - transform.position);
    }
    public static float DistanceTo(this Vector3 original, Vector3 other)
    {
        return Vector3.Distance(other, original);
    }
    public static Vector3 DirectionTo(this Vector3 myPos, Vector3 position)
    {
        return Vector3.Normalize(position - myPos);
    }
    
    
    public static T GetRandom<T> (this List<T> list)
    {
        if (list.Count == 0) return default;
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

}
