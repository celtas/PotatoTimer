﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public static class Util {
    public static void InvokeSafe(this UnityEvent action) {
        if (action != null)
            action.Invoke();
    }

    // targetに近い値を返す
    public static float NearestValue(this IEnumerable<float> list, float target) {
        var min = list.Min( c => Mathf.Abs( c - target ) );
        var o = list.First(c => Mathf.Abs(c - target) == min);
        return o;
    }
    
    // targetに絶対値が近いRectTransformを返す
    public static RectTransform NearestAbs(this IEnumerable<RectTransform> list, float target) {
        float diff = list.Min( c => Math.Abs(-c.anchoredPosition.y - target));
        return list.First(c => Math.Abs(-c.anchoredPosition.y - target) == diff);
    }

    // 自身を含めない子要素のコンポーネントを取得
    public static T[] GetComponentsInChildrenWithoutSelf<T>(this GameObject self) where T : Component {
        return self.GetComponentsInChildren<T>().Where(c => self != c.gameObject).ToArray();
    }
}