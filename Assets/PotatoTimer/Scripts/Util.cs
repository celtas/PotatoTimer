using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class Util {
    public static void InvokeSafe(this UnityEvent action) {
        if (action != null)
            action.Invoke();
    }
    
    public static void InvokeSafe(this Action action) {
        if (action != null)
            action.Invoke();
    }
    
    public static void InvokeSafe<T>(this Action<T,T,T> action, T arg1, T arg2, T arg3) {
        if (action != null)
            action.Invoke(arg1,arg2,arg3);
    }

    // targetに近い値を返す
    public static float NearestValue(this IEnumerable<float> list, float target) {
        var min = list.Min( c => Mathf.Abs( c - target ) );
        var o = list.First(c => Mathf.Abs(c - target) == min);
        return o;
    }
    
    // 絶対座標のy値が最も近いRectTransformを返す
    public static RectTransform NearestY(this IEnumerable<RectTransform> list, float positionY) {
        float diff = list.Min( c => Math.Abs(c.position.y - positionY));
        return list.First(c => Math.Abs(c.position.y - positionY) == diff);
    }

    // 自身を含めない子要素のコンポーネントを取得
    public static T[] GetComponentsInChildrenWithoutSelf<T>(this GameObject self) where T : Component {
        return self.GetComponentsInChildren<T>().Where(c => self != c.gameObject).ToArray();
    }
}