using System;
using System.Threading.Tasks;
using UnityEngine;

public static class BubbleSort
{
    public static void BubbleSortAlgorithm(float[] array)
    {
        int n = array.Length;
        bool swapped;

        for (int i = 0; i < n - 1; i++)
        {
            swapped = false;
            for (int j = 0; j < n - i - 1; j++)
            {
                if (array[j] > array[j + 1])
                {
                    // Swap the elements
                    float temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;

                    swapped = true;
                }
            }

            if (!swapped)
                break;
        }
    }

    public static async Task BubbleSortAlgorithmAsync(float[] array)
    {
        await Task.Run(() => BubbleSortAlgorithm(array));
    }
}
