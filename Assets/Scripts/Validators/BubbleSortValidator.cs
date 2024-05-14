using UnityEngine;
using TMPro;
using static NodeController;
using System;
using System.Linq;

public class BubbleSortValidator : MonoBehaviour, ISwapValidator {
    private int lastSwapIndex = -1;
    private int sortedBoundary;
    private int[] numbersToBeSorted;

    void Start() {
        sortedBoundary = numbersToBeSorted.Length-1;
    }

    public bool IsValidSwap(GameObject[] nodes, int index1, int index2) {
        if (index1 != index2 - 1) return false;

        if (index1 >= sortedBoundary || index2 > sortedBoundary) return false;

        int checkStartIndex = lastSwapIndex >= 0 ? lastSwapIndex : 0;
        UpdateSortedBoundary();

        for (int i = checkStartIndex; i < sortedBoundary; i++) {
            int currentValue = int.Parse(nodes[i].GetComponentInChildren<TextMeshPro>().text);
            int nextValue = int.Parse(nodes[i + 1].GetComponentInChildren<TextMeshPro>().text);

            if (currentValue > nextValue) {
                if (i == index1 && (i + 1) == index2) {
                    Swap(numbersToBeSorted, index1, index2);
                    lastSwapIndex = i + 1;
                    UpdateSortedBoundary();
                    if (lastSwapIndex == sortedBoundary) {
                        sortedBoundary--;
                        lastSwapIndex = -1;
                    }
                    return true;
                } else {
                    return false;
                }
            }
        }
        return false;
    }

    public void SetNumbersToBeSorted(int[] numbersToBeSorted)
    {
        this.numbersToBeSorted = numbersToBeSorted;
    }

    private void Swap(int[] array, int index1, int index2) {
        int temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }

    private void UpdateSortedBoundary() {
        bool boundaryUpdated = false;
        while (sortedBoundary > 0) {
            int[] subArray = new int[sortedBoundary + 1];
            Array.Copy(numbersToBeSorted, 0, subArray, 0, sortedBoundary + 1);

            int curMax = subArray.Max();
            if (curMax == numbersToBeSorted[sortedBoundary]) {
                sortedBoundary--;
                boundaryUpdated = true;
            } else {
                break;
            }
        }
        if (boundaryUpdated) {
            lastSwapIndex = -1;
        }
    }

    public (int, int) GetNextSwap(GameObject[] nodes) {
        int tempLastSwapIndex = lastSwapIndex;
        int tempSortedBoundary = sortedBoundary;

        while (true) {
            int start = tempLastSwapIndex >= 0 ? tempLastSwapIndex : 0;

            for (int i = start; i < tempSortedBoundary; i++) {
                int currentValue = int.Parse(nodes[i].GetComponentInChildren<TextMeshPro>().text);
                int nextValue = int.Parse(nodes[i + 1].GetComponentInChildren<TextMeshPro>().text);

                if (currentValue > nextValue) {
                    return (i, i + 1);
                }
            }

            if (start > 0) {
                tempSortedBoundary--;
                tempLastSwapIndex = -1;

                if (tempSortedBoundary <= 0) {
                    break;
                }
            } else {
                break;
            }
        }

        return (-1, -1);
    }



}
