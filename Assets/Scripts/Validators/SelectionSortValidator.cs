using UnityEngine;
using TMPro;

public class SelectionSortValidator : MonoBehaviour, ISwapValidator {
    int startIndex = 0;
    int minIndex = 0;

    public bool IsValidSwap(GameObject[] nodes, int index1, int index2) {
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
        while (startIndex < nodes.Length - 1)
        {
            minIndex = startIndex;
            for (int i = startIndex; i < nodes.Length; i++)
            {
                if (int.Parse(nodes[i].GetComponentInChildren<TextMeshPro>().text) <
                    int.Parse(nodes[minIndex].GetComponentInChildren<TextMeshPro>().text))
                {
                    minIndex = i;
                }
            }

            if (minIndex != startIndex)
            {
                break;
            }
            startIndex++;
        }

        if (startIndex != -1 && minIndex != -1) {
            return (startIndex, minIndex);
        }
        return (-1, -1);
    }
}