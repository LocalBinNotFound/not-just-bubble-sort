using UnityEngine;

public class ItemManager : MonoBehaviour {
    private int hintCount;
    private int autoCompleteCount;
    private int lifeCount = 5;
    public int HintCount => hintCount;
    public int AutoCompleteCount => autoCompleteCount;
    public int LifeCount => lifeCount;

    private NodeController nodeController;


    void Start() {
        nodeController = FindObjectOfType<NodeController>();

        hintCount = PlayerPrefs.GetInt("Hints");
        autoCompleteCount = PlayerPrefs.GetInt("AutoComplete");
        nodeController.UpdateCompleteCountUI();
        nodeController.UpdateHintCountUI();
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
            PlayerPrefs.SetInt("Hints", hintCount);
            return true;
        }
        return false;
    }

    public bool ConsumeAutoComplete() {
        if (autoCompleteCount > 0) {
            autoCompleteCount--;
            PlayerPrefs.SetInt("AutoComplete", autoCompleteCount);
            return true;
        }
        return false;
    }

    public void AddItem(string itemName, int count) {
        switch (itemName) {
            case "Hint":
                hintCount += count;
                PlayerPrefs.SetInt("Hints", hintCount);
                break;
            case "Auto Complete":
                autoCompleteCount += count;
                PlayerPrefs.SetInt("AutoComplete", autoCompleteCount);
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
