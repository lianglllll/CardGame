using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/// <summary>
/// 管理玩家的信息，它还关联上了商店系统
/// </summary>
public class PlayerData : MonoBehaviour
{
    public TextAsset playerDataFile;//信息文件


    public int coins;//金币
    public Dictionary<int, int> playerCards = new Dictionary<int, int>();//持有全部的牌key是card_id,value是number
    public Dictionary<int, int> playerDeck= new Dictionary<int, int>();//持有的卡组中key是id,value是数量


    public CardStore cardStore;//主要是调用里面的卡牌信息



    private void Awake()
    {
        cardStore.LoadCardData();
        LoadPlayerData();
    }

    /*
     加载玩家的数据
     */
    public void LoadPlayerData()
    {
        

        if (playerDataFile == null)
        {
            Debug.Log("PlayerData中textAsset为空！");
            return;
        }

        //Text文件根据一行进行分割
        string[] dataRow = playerDataFile.text.Split('\n');
        //注意去除我们的注释信息，遍历每一行
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');

             if (rowArray[0] == "coins")
            {   //金币
                coins = int.Parse(rowArray[1]);
            }
            else if (rowArray[0] == "card")
            {   //卡牌
                int id = int.Parse(rowArray[1]);
                int number = int.Parse(rowArray[2]);
                playerCards[id] = number;
            }
            else if (rowArray[0] == "deck")
            {
                int id = int.Parse(rowArray[1]);
                int number = int.Parse(rowArray[2]);
                playerDeck[id] = number;
            }
        }


    }

    /*
     保存玩家的数据
     */
    public void SavePlayerData()
    {

        //保存的相对路径
        string path = Application.dataPath + "/Resources/Config/PlayerData.txt";
        List<string> datas = new List<string>();
        datas.Add("coins," + coins.ToString());

        //保存所有卡牌
        foreach ( int id in playerCards.Keys)
        {
            datas.Add("card," + id.ToString() + "," + playerCards[id].ToString());
        }
        //保存卡组
        foreach (int id in playerDeck.Keys)
        {
            datas.Add("deck," + id.ToString() + "," + playerDeck[id].ToString());
        }

        //文件读写
        // 创建一个新的文件并写入一些数据
        File.WriteAllLines(path, datas);

        Debug.Log("PlayerData already save");

    }

    /*
     根据id获取card信息
     */
    public BaseCard GetCardById(int id)
    {
        return cardStore.GetCardById(id);
    }

}
