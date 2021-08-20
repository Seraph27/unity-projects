using System.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class EnumerableHelper
{
    public static T RandomElement<T>(this List<T> l) {
        var r = UnityEngine.Random.Range(0, l.Count);
        return l[r];
    }

    
}
