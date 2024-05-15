using UnityEngine;
using System;

public class MergeSort : MonoBehaviour
{
    int swapCount = 0;

    void Start()
    {
        // ���ĵ��� ���� �迭 ����
        int[] array = new int[] { 12, 3, 17, 6, 22, 9, 14, 19 };

        // ���� �� �迭 ���
        Debug.Log("���� �� �迭: " + string.Join(", ", array));

        // ���� ���� ����
        MergeSort1(array, 0, array.Length - 1);

        // ���� �� �迭 ���
        Debug.Log("���� �� �迭: " + string.Join(", ", array));

        Debug.Log(swapCount);
    }

    public void MergeSort1(int[] array, int beginIndex, int endIndex)
    {
        if (beginIndex < endIndex)
        {
            int midIndex = (beginIndex + endIndex) / 2;
            MergeSort1(array, beginIndex, midIndex);
            MergeSort1(array, midIndex + 1, endIndex);
            Merge(array, beginIndex, midIndex, endIndex);
        }
    }

    public void Merge(int[] array, int beginIndex, int midIndex, int endIndex)
    {
        int[] lowHalf = new int[midIndex - beginIndex + 1];
        int[] highHalf = new int[endIndex - midIndex];

        int k = beginIndex;
        int i = 0;
        int j = 0;

        for (i = 0; k <= midIndex; i++, k++)
        {
            lowHalf[i] = array[k];
        }
        for (j = 0; k <= endIndex; j++, k++)
        {
            highHalf[j] = array[k];
        }

        k = beginIndex;
        i = 0;
        j = 0;

        while (i < lowHalf.Length && j < highHalf.Length)
        {
            if (lowHalf[i] < highHalf[j])
            {
                array[k] = lowHalf[i];
                i++;
                swapCount++;
            }
            else
            {
                array[k] = highHalf[j];
                j++;
                swapCount++;
            }

            k++;
        }
        while (i < lowHalf.Length)
        {
            array[k] = lowHalf[i];
            i++; k++;
        }
        while (j < highHalf.Length)
        {
            array[k] = highHalf[j];
            j++; k++;
        }
    }
}
