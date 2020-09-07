using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    static int maxHeight = 150;
    static float smooth = 0.01f;
    static int octaves = 4;
    static float persistance = 0.5f;

    public static int GenerateStoneHeight(float x, float z)
    {
        float height = Map(0, maxHeight-20, 0, 1, fBM(x * smooth *1.5f, z * smooth*1.5f, octaves, persistance));
        return (int)height;
    }

    public static int GenerateHeight(float x, float z)
    {
        float height = Map(0, maxHeight, 0, 1, fBM(x * smooth, z * smooth, octaves, persistance));
        return (int) height;
    }

    static float Map(float newMin, float newMax, float origMin, float originMax, float value)
    {
        return Mathf.Lerp(newMin, newMax, Mathf.InverseLerp(origMin, originMax, value));
    }

    public static float fBM3D(float x, float y, float z, float sm, int oct, float pers)
    {
        float XY = fBM(x * sm, y * sm, oct, pers);
        float YZ = fBM(y * sm, z * sm, oct, pers);
        float XZ = fBM(x * sm, z * sm, oct, pers);


        float YX = fBM(y * sm, x * sm, oct, pers);
        float ZY = fBM(z * sm, y * sm, oct, pers);
        float ZX = fBM(z * sm, x * sm, oct, pers);

        return (XY + YZ + XZ + YX + ZY + ZX) / 6.0f;
    }

    static float fBM(float x, float z, int oct, float pers)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;
        for (int i = 0; i < oct; i++)
        {
            total += Mathf.PerlinNoise(x * frequency, z * frequency) * amplitude;

            maxValue += amplitude;

            amplitude *= pers;
            frequency *= 2;
        }

        return total / maxValue;
    }
}
