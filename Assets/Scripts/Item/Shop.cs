﻿using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    public GameObject buyButton;
    public TextMeshProUGUI itemName, itemPrice;
    public TextMeshProUGUI livesCount, hintsCount, autoCompleteCount;
    public Animation notEnough;
    public AudioSource backgroundAudioSource;
    public AudioClip shopBGM;
    public AudioClip defaultBGM;

    public Transform items;
    private List<GameObject> gameItems;
    private int itemIndex = 0;
    private ItemManager itemManager;
    private NodeController nodeController;

    void Start()
    {
        itemManager = FindObjectOfType<ItemManager>();
        nodeController = FindObjectOfType<NodeController>();
        LoadItems();
    }

    public void Previous()
    {
        if (itemIndex > 0)
        {
            itemIndex--;
            LoadItems();
        }
    }

    public void Next()
    {
        if (itemIndex < gameItems.Count - 1)
        {
            itemIndex++;
            LoadItems();
        }
    }

    public void Buy()
    {
        Item item = gameItems[itemIndex].GetComponent<Item>();
        if (Wallet.GetAmount() >= item.price)
        {
            Wallet.SetAmount(Wallet.GetAmount() - item.price);
            UpdateItemCount(item);
        }
        else
        {
            notEnough.Play("Not-Enough-In");
        }
    }

    private void LoadItems()
    {
        gameItems = new List<GameObject>();

        foreach(Transform item in items)
        {
            gameItems.Add(item.gameObject);
        }

        UpdateDisplayedItem();
    }

    private void UpdateDisplayedItem()
    {
        for(int i = 0; i < gameItems.Count; i++)
        {
            gameItems[i].SetActive(i == itemIndex);
        }

        Item currentItem = gameItems[itemIndex].GetComponent<Item>();

        itemName.text = gameItems[itemIndex].name;
        itemPrice.text = currentItem.price.ToString();
    }


    private void UpdateItemCount(Item item)
    {
        switch (item.gameObject.name)
        {
            case "Life":
                itemManager.AddItem("Life", 1);
                nodeController.UpdateLifeCountUI();
                break;
            case "Hint":
                itemManager.AddItem("Hint", 1);
                nodeController.UpdateHintCountUI();
                break;
            case "Auto Complete":
                itemManager.AddItem("Auto Complete", 1);
                nodeController.UpdateCompleteCountUI();
                break;
            default:
                Debug.LogError("Unknown item name");
                break;
        }
    }

    public void PlayShopBGM()
    {
        backgroundAudioSource.clip = shopBGM;
        backgroundAudioSource.Play();
    }

    public void PlayDefaultBGM()
    {
        backgroundAudioSource.clip = defaultBGM;
        backgroundAudioSource.Play();
    }

}