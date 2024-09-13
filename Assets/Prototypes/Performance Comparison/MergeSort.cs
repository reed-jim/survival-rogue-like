using System;
using System.Threading.Tasks;
using UnityEngine;

public static class MergeSort
{
    public static async Task MergeSortAlgorithmAsync(float[] array, int left, int right)
    {
        await Task.Run(() => MergeSortAlgorithm(array, left, right));
    }

    public static void MergeSortAlgorithm(float[] array, int left, int right)
    {
        if (left < right)
        {
            int middle = left + (right - left) / 2;

            // Recursively sort the two halves
            MergeSortAlgorithm(array, left, middle);
            MergeSortAlgorithm(array, middle + 1, right);

            // Merge the sorted halves
            Merge(array, left, middle, right);
        }
    }

    // Method to merge two sorted halves
    private static void Merge(float[] array, int left, int middle, int right)
    {
        int n1 = middle - left + 1;
        int n2 = right - middle;

        // Create temporary arrays
        float[] leftArray = new float[n1];
        float[] rightArray = new float[n2];

        // Copy data to temporary arrays
        Array.Copy(array, left, leftArray, 0, n1);
        Array.Copy(array, middle + 1, rightArray, 0, n2);

        // Merge the temporary arrays back into the original array
        int i = 0, j = 0, k = left;
        while (i < n1 && j < n2)
        {
            if (leftArray[i] <= rightArray[j])
            {
                array[k] = leftArray[i];
                i++;
            }
            else
            {
                array[k] = rightArray[j];
                j++;
            }
            k++;
        }

        // Copy the remaining elements of leftArray, if any
        while (i < n1)
        {
            array[k] = leftArray[i];
            i++;
            k++;
        }

        // Copy the remaining elements of rightArray, if any
        while (j < n2)
        {
            array[k] = rightArray[j];
            j++;
            k++;
        }
    }
}
