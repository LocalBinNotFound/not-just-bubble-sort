using UnityEngine;
using TMPro;

public class SelectionSortValidator : MonoBehaviour, ISwapValidator {
    public bool IsValidSwap(GameObject[] nodes, int index1, int index2, int startIndex) {
    int minIndex = 0;

    while (startIndex < nodes.Length - 1) {
        minIndex = startIndex;
        for (int i = startIndex; i < nodes.Length; i++) {
            if (int.Parse(nodes[i].GetComponentInChildren<TextMeshPro>().text) <
                int.Parse(nodes[minIndex].GetComponentInChildren<TextMeshPro>().text))
            {
                minIndex = i;
            }
        }

        if (minIndex != startIndex) {
            break;
        }
        startIndex++;
    }

    if ((index1 == startIndex && index2 == minIndex) ||
        (index1 == minIndex && index2 == startIndex)) {
        return true;
        }
    return false;
    }

    public void SetNumbersToBeSorted(int[] numbersToBeSorted)
    {}

    public (int, int) GetNextSwap(GameObject[] nodes) {
        return (-1, -1);
    }
}