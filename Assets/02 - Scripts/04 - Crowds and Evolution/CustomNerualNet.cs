using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNerualNet : SimpleNeuralNet
{
    public CustomNerualNet(SimpleNeuralNet other): base(other)
    {

    }
    public CustomNerualNet(CustomNerualNet other) : base(other)
    {

    }

    public CustomNerualNet(int[] structure) : base(structure)
    {

    }

    public void mutate()
    {
        float pro = UnityEngine.Random.value; // mutate probability
        float max = (2.0f * 1 - 1.0f) * 10.0f;
        float min = (2.0f * 0 - 1.0f) * 10.0f;
        foreach (float[,] weights in allWeights)
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    float rand = UnityEngine.Random.value;
                    if (rand < pro)
                    {
                        // just make a little bit change based on previous weights
                        weights[i, j] += UnityEngine.Random.Range(-1f, 1f);
                        weights[i, j] = Mathf.Clamp(weights[i, j], min, max);
                    }
                }
            }
        }
    }
}
