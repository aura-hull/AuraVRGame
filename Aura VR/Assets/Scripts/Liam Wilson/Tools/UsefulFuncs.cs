using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UsefulFuncs
{
    public static string NeatTime(float seconds)
    {
        return NeatTime((int)seconds);
    }

    public static string NeatTime(int seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);

        if (seconds < 3600) return time.ToString(@"mm\:ss");
        else return time.ToString(@"hh\:mm\:ss");
    }

    public static double CosineSimilarity(Vector3 V1, Vector3 V2)
    {
        return ((V1.x * V2.x) + (V1.y * V2.y) + (V1.z * V2.z))
               / (Math.Sqrt(Math.Pow(V1.x, 2) + Math.Pow(V1.y, 2) + Math.Pow(V1.z, 2))
                  * Math.Sqrt(Math.Pow(V2.x, 2) + Math.Pow(V2.y, 2) + Math.Pow(V2.z, 2)));
    }
}
