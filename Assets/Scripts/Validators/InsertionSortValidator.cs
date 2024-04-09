using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSortValidator : MonoBehaviour, ISwapValidator
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
        for (int i = 1; i < numbersToBeSorted.Length; i++)
        {
            for (int j = i - 1; j >= 0 && numbersToBeSorted[j] > numbersToBeSorted[j + 1]; j--)
            {
                swapOrders.Add((j, j + 1));
                (numbersToBeSorted[j], numbersToBeSorted[j + 1]) = (numbersToBeSorted[j + 1], numbersToBeSorted[j]);
            }
        }
    }

    public (int, int) GetNextSwap(GameObject[] nodes) {
        return curIdx < swapOrders.Count ? swapOrders[curIdx] : (-1, -1);
    }
}
