using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPos;
    public Vector3 playerRot;

    public List<int> invenArrayNum = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNum = new List<int>();
}


public class SaveNLoad : MonoBehaviour
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;
    private string SAVE_FILENAME = "/SaveFile.txt";

    private PlayerController playerController;
    private Inventory inventory;

    private void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + "/Saves/";
    
        if(!Directory.Exists(SAVE_DATA_DIRECTORY))
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY);
        }
    }

    public void SaveData()
    {
        playerController = FindObjectOfType<PlayerController>();
        inventory = FindObjectOfType<Inventory>();

        saveData.playerPos = playerController.transform.position;
        saveData.playerRot = playerController.transform.eulerAngles;

        ItemSlot[] slots = inventory.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                saveData.invenArrayNum.Add(i);
                saveData.invenItemName.Add(slots[i].item.itemName);
                saveData.invenItemNum.Add(slots[i].itemCount);
            }
        }

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if(File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            playerController = FindObjectOfType<PlayerController>();
            inventory = FindObjectOfType<Inventory>();

            playerController.transform.position = saveData.playerPos;
            playerController.transform.eulerAngles = saveData.playerRot;

            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                inventory.LoadToInven(saveData.invenArrayNum[i], saveData.invenItemName[i], saveData.invenItemNum[i]);
            }

            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
        }
        
    }
}
