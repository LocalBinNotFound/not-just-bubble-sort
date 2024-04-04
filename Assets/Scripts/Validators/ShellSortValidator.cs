using UnityEngine;
using TMPro;

public class ShellSortValidator : MonoBehaviour, ISwapValidator
{
    int currentGap = 0;

    public bool IsValidSwap(GameObject[] nodes, int index1, int index2)
    {
        var swapPair = GetNextSwap(nodes);

        if ((index1 == swapPair.Item1 && index2 == swapPair.Item2) ||
          (index1 == swapPair.Item2 && index2 == swapPair.Item1))
        {
            return true;
        }
        return false;
    }

    public void SetNumbersToBeSorted(int[] numbersToBeSorted)
    { }

    public (int, int) GetNextSwap(GameObject[] nodes)
    {
        if (currentGap == 0) {
            currentGap = nodes.Length / 2;
        }
        for (int gap = nodes.Length / 2; gap > 0; gap /= 2)
        {
            if (gap != currentGap)
            {
                continue;  //in case of repeating the previous gap
            }
            for (int i = gap; i < nodes.Length; i++)
            {
                int j = i;
                while (j - gap >= 0 && int.Parse(nodes[j].GetComponentInChildren<TextMeshPro>().text) <
                    int.Parse(nodes[j - gap].GetComponentInChildren<TextMeshPro>().text))
                {
                    return (j, j - gap);
                }
            }
            currentGap = gap / 2;
        }
        return (-1, -1);
    }
}
