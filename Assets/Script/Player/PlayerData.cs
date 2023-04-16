using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


/// <summary>
/// ������ҵ���Ϣ���������������̵�ϵͳ
/// </summary>
public class PlayerData : MonoBehaviour
{
    public TextAsset playerDataFile;//��Ϣ�ļ�


    public int coins;//���
    public Dictionary<int, int> playerCards = new Dictionary<int, int>();//����ȫ������key��card_id,value��number
    public Dictionary<int, int> playerDeck= new Dictionary<int, int>();//���еĿ�����key��id,value������


    public CardStore cardStore;//��Ҫ�ǵ�������Ŀ�����Ϣ



    private void Awake()
    {
        cardStore.LoadCardData();
        LoadPlayerData();
    }

    /*
     ������ҵ�����
     */
    public void LoadPlayerData()
    {
        

        if (playerDataFile == null)
        {
            Debug.Log("PlayerData��textAssetΪ�գ�");
            return;
        }

        //Text�ļ�����һ�н��зָ�
        string[] dataRow = playerDataFile.text.Split('\n');
        //ע��ȥ�����ǵ�ע����Ϣ������ÿһ��
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');

             if (rowArray[0] == "coins")
            {   //���
                coins = int.Parse(rowArray[1]);
            }
            else if (rowArray[0] == "card")
            {   //����
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
     ������ҵ�����
     */
    public void SavePlayerData()
    {

        //��������·��
        string path = Application.dataPath + "/Resources/Config/PlayerData.txt";
        List<string> datas = new List<string>();
        datas.Add("coins," + coins.ToString());

        //�������п���
        foreach ( int id in playerCards.Keys)
        {
            datas.Add("card," + id.ToString() + "," + playerCards[id].ToString());
        }
        //���濨��
        foreach (int id in playerDeck.Keys)
        {
            datas.Add("deck," + id.ToString() + "," + playerDeck[id].ToString());
        }

        //�ļ���д
        // ����һ���µ��ļ���д��һЩ����
        File.WriteAllLines(path, datas);

        Debug.Log("PlayerData already save");

    }

    /*
     ����id��ȡcard��Ϣ
     */
    public BaseCard GetCardById(int id)
    {
        return cardStore.GetCardById(id);
    }

}
