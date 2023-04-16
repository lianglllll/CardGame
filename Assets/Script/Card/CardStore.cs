using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 卡牌商城/也可用作卡池
/// </summary>
public class CardStore : MonoBehaviour
{
    private TextAsset cardData;
    //private List<BaseCard> cardList = new List<BaseCard>();//todo
    private Dictionary<int, BaseCard> cardList = new Dictionary<int, BaseCard>();
    private List<int> keys;//用于保存字典的key，用于随机获取键值用



    /*
     加载卡池文件,又玩家数据那边进行调用加载
     */
    public void LoadCardData()
    {
        //加载文件
        cardData = Resources.Load<TextAsset>("Config/CardData");

        if (cardData == null)
        {
            Debug.Log("CarDStore中textAsset为空！");
            return;
        }

        //Text文件根据一行进行分割
        string[] dataRow = cardData.text.Split('\n');
        //注意去除我们的注释信息
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');

            if (rowArray[0] == "#") {
                //跳过
                continue;
            }else if (rowArray[0] == "monster")
            {   //怪物卡
                int id = int.Parse(rowArray[1]);
                string cardName = rowArray[2];
                int attack = int.Parse(rowArray[3]);
                int hp = int.Parse(rowArray[4]);
                MonsterCard mcard = new MonsterCard(id, cardName, attack, hp, hp);
                cardList.Add(id, mcard);
            }
            else if (rowArray[0] == "spell")
            {   //魔法卡
                int id = int.Parse(rowArray[1]);
                string cardName = rowArray[2];
                string affect = rowArray[3];
                SpellCard scard = new SpellCard(id, cardName, affect);
                cardList.Add(id,scard);
            }
        }

        //保存key
        keys = new List<int>(cardList.Keys);

    }

    /*
     从卡池中随机获取一张卡牌（卡池数量不变）（商店用）
     */
    public BaseCard RandomCard()
    {
        //利用随机值
        BaseCard card = cardList[Random.Range(0, keys.Count)];
        return card;
        
    }


    /*
     通过key获取字典中的card类
     */
    public BaseCard GetCardById(int id)
    {
        if (cardList.ContainsKey(id))
        {
            return cardList[id];
        }
        Debug.Log("CardStore.GetCardById检测到不存在的id:"+id);
        return null;
    }

}
