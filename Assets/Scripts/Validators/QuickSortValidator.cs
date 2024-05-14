using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSortValidator : MonoBehaviour, ISwapValidator
{
    private int curIdx = 0;
    private List<(int, int)> swapOrders = new();
    public bool IsValidSwap(GameObject[] nodes, int index1, int index2)
    {
        if (curIdx < swapOrders.Count && (swapOrders[curIdx].Equals((index1, index2)) || swapOrders[curIdx].Equals((index2, index1))))
        {
            curIdx++;
            return true;
        }
        return false;
    }

    public void SetNumbersToBeSorted(int[] numbersToBeSorted)
    {
        QuickSort(numbersToBeSorted, 0, numbersToBeSorted.Length - 1);
    }

    private int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;
        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot) {
                i++;
                if (i != j)
                {
                    swapOrders.Add((i, j));
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                }
            }
        }
        if (i + 1 != high)
        {
            swapOrders.Add((i + 1, high));
            (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        }
        return i + 1;
    }

    private void QuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int p = Partition(arr, low, high);
            QuickSort(arr, low, p - 1);
            QuickSort(arr, p + 1, high);
        }
    }

    public (int, int) GetNextSwap(GameObject[] nodes) {
        return curIdx < swapOrders.Count ? swapOrders[curIdx] : (-1, -1);
    }
}
