using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GraphNoise : MonoBehaviour {

	float t = 0f;
    float inc = 0.01f;

    float t2 = 0f;
    float inc2 = 0.005f;

    float t3 = 0f;
    float inc3 = 0.001f;

    float timeStamp = 0f;
    float increment = 0.01f;

    float randomSeed;

    float MapTo (float min, float max, float oldMin, float oldMax, float value)
    {
        return Mathf.Lerp(min, max, Mathf.InverseLerp(oldMin, oldMax, value));
    }

    float OctavesTotal ()
    {
        t += inc;
        float n = Mathf.PerlinNoise(t, 1);
        Grapher.Log(n, "Perlin1", Color.yellow);

        t2 += inc2;
        float n2 = Mathf.PerlinNoise(t2, 200);
        Grapher.Log(n2, "Perlin2", Color.red);

        t3 += inc3;
        float n3 = Mathf.PerlinNoise(t3, -200);
        Grapher.Log(n3, "Perlin3", Color.blue);

        float total = (n + n2 + n3) / 3f;

        total = MapTo(0f, 150f, 0f, 1f, total);

        return total;
    }

    /// <summary>
	/// 
	/// </summary>
	/// <param name="time">the location at which to read the perlin value</param>
	/// <param name="octaves">the number of perlin noises being added together</param>
	/// <param name="persistence">the amount each succesive octave has influence on the total outcome</param>
	/// <param name="seed"></param>
	/// <returns></returns>
    float fBM (float time, int octaves, float persistence, float seed)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;

        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise (time * frequency, seed) * amplitude;
            maxValue += amplitude;

            // increase amplitude and
            // frequency each iteration
            amplitude *= persistence;
            frequency *= 2f;
        }

        return total / maxValue;
    }

    void Start ()
    {
        randomSeed = Random.Range(-5000f, 5000f);
    }

    void Update ()
	{
        timeStamp += increment;

        float motion = fBM(timeStamp, 3, 0.1f, randomSeed);

        Grapher.Log(motion, "fBM", Color.blue);

        Grapher.Log(OctavesTotal(), "Octaves", Color.green);
    }
}
