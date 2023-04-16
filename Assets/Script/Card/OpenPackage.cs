using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 开包
/// </summary>
public class OpenPackage : MonoBehaviour
{
    //卡牌模型
    public GameObject CardPrefab;
    //（商店）
    private CardStore cardStore;
    //一个卡包里面有多少张卡牌
    private int cardNumberInPackage = 5;
    //卡组，用于展示card实例
    public GameObject cardPool;
    //临时存储Card实例化，用于进行销毁
    List<GameObject> cards = new List<GameObject>();

    //此时需要保存抽卡数据的玩家
    public PlayerData playerData;


    void Start()
    {
        cardStore = GetComponent<CardStore>();

    }

    /*
     开卡包
     */
    public void OnClickOpen()
    {

        //扣钱
        if (playerData.coins < 2)
        {
            return;
        }
        else
        {
            playerData.coins -= 2;
        }


        //保证当前卡组没有卡牌
        ClearPool();
        //实例化卡牌
        for (int i = 0; i < cardNumberInPackage; i++)
        {
            //实例化模型,同时还可以挂载到cardPool物体下
            GameObject newCard = GameObject.Instantiate(CardPrefab, cardPool.transform);
            //初始化,从卡池中随机找了张给他
            newCard.GetComponent<DisplayCard>().Card = cardStore.RandomCard();
            cards.Add(newCard);
        }
        SaveCardDataToPlayer();//将进行保存到playerdata中


    }

    /*
     清空卡组，方便下一次开卡包
     */
    public void ClearPool()
    {
        foreach (var card in cards)
        {
            Destroy(card);
        }
        cards.Clear();
    }


    /*
     保存数据到playerData当中
     */
    public void SaveCardDataToPlayer()
    {
        foreach (var card in cards)
        {
            int id = card.GetComponent<DisplayCard>().Card.id;
            if (playerData.playerCards.ContainsKey(id))
            {
                playerData.playerCards[id] += 1;
            }
            else
            {
                playerData.playerCards[id] = 1;
            }
        }

        playerData.SavePlayerData();//保存到文件中
    }

}
