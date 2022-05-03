using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class SaveSerial : MonoBehaviour
{
    //Data that needs to be saved:
    //Player body parts
    int[] playerPartsSave;
    //Player stickers
    int[] playerStickerSave;
    //Tutorial state
    bool[] tutorialStateSave;
    //Battle state
    BattleState battleStateSave;
    State stateSave;
    //What round of combat it is
    int currentRoundSave;
    //Number of moves
    int movesSave;
    //What level is the player on
    int numberOfBattlesSave;
    //The playersHand
    Deck playerHandSave;


    void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
                     + "/MySaveData.dat");
        SaveData data = new SaveData();
        data.playerPartsSave = playerPartsSave;
        data.playerStickerSave = playerStickerSave;
        data.tutorialStateSave = tutorialStateSave;
        data.battleStateSave = battleStateSave;
        data.stateSave = stateSave;
        data.currentRoundSave = currentRoundSave;
        data.movesSave = movesSave;
        data.numberOfBattlesSave = numberOfBattlesSave;
        data.playerHandSave = playerHandSave;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }

    void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath
                   + "/MySaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open(Application.persistentDataPath
                       + "/MySaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            playerPartsSave = data.playerPartsSave;
            playerStickerSave = data.playerStickerSave;
            tutorialStateSave = data.tutorialStateSave;
            battleStateSave = data.battleStateSave;
            stateSave = data.stateSave;
            currentRoundSave = data.currentRoundSave;
            movesSave = data.movesSave;
            numberOfBattlesSave = data.numberOfBattlesSave;
            playerHandSave = data.playerHandSave;
            Debug.Log("Game data loaded!");
        }
        else
            Debug.LogError("There is no save data!");
    }

    void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
                      + "/MySaveData.dat"))
        {
            File.Delete(Application.persistentDataPath
                              + "/MySaveData.dat");
            playerPartsSave = new int[0];
            playerStickerSave = new int[0];
            tutorialStateSave = new bool[0];
            battleStateSave = BattleState.START;
            stateSave = State.EnemySelection;
            currentRoundSave = 0;
            movesSave = 0;
            numberOfBattlesSave = 0;
            playerHandSave = null;
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save data to delete.");
    }
}


[Serializable]
class SaveData 
{
    public int[] playerPartsSave;
    public int[] playerStickerSave;
    public bool[] tutorialStateSave;
    public BattleState battleStateSave;
    public State stateSave;
    public int currentRoundSave;
    public int movesSave;
    public int numberOfBattlesSave;
    public Deck playerHandSave;
}