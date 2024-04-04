using UnityEngine;

public class ItemManager : MonoBehaviour {
    private int hintCount = 15;
    private int autoCompleteCount = 2;
    private int lifeCount = 5;
    public int HintCount => hintCount;
    public int AutoCompleteCount => autoCompleteCount;
    public int LifeCount => lifeCount;

    private NodeController nodeController;

    void Start() {
        nodeController = FindObjectOfType<NodeController>();
    }

    public bool ConsumeLife() {
        if (lifeCount > 0) {
            lifeCount--;
            return lifeCount > 0;
        }
        return false;
    }

    public bool ConsumeHint() {
        if (hintCount > 0) {
            hintCount--;
            return true;
        }
        return false;
    }

    public bool ConsumeAutoComplete() {
        if (autoCompleteCount > 0) {
            autoCompleteCount--;
            return true;
        }
        return false;
    }

    public void AddItem(string itemName, int count) {
        switch (itemName) {
            case "Hint":
                hintCount += count;
                break;
            case "Auto Complete":
                autoCompleteCount += count;
                break;
            case "Life":
                lifeCount += count;
                break;
            default:
                Debug.LogError("Unknown item name");
                break;
        }
    }
}
