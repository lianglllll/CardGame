using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 更新仓库信息，卡组信息//应该是要控制仓库这个ui
/// </summary>
public class DeckManager : MonoBehaviour
{
    //两个ui面板
    public GameObject libraryPanel;
    public GameObject deckPanel;


    //不同卡牌的预制体
    public GameObject libraryCardPrefab;
    public GameObject deckCardPrefab;


    //获取外界的物体
    public GameObject playerdataManager;


    //需要用到的脚本
    private PlayerData playerData;
    private CardStore cardStore;

    //保存卡组中卡牌的实例
    public Dictionary<int, GameObject> deckCardGameObject = new Dictionary<int, GameObject>();


    private void Start()
    {
        playerData = playerdataManager.GetComponent<PlayerData>();
        cardStore = playerdataManager.GetComponent<CardStore>();
        UpdataLibrary();
        UpdataDeck();
    }

    /*
     更新仓库的卡牌信息
     */
    public void UpdataLibrary()
    {
        //这里遍历playerdata中的字典，然后生成到library中
        foreach (var value in playerData.playerCards.Keys)
        {
            GameObject newCard = Instantiate(libraryCardPrefab, libraryPanel.transform.Find("LibraryGrid").GetComponent<Transform>());
            newCard.GetComponent<CardCount>().counter.text = playerData.playerCards[value].ToString();//number
            newCard.GetComponent<DisplayCard>().Card = cardStore.GetCardById(value);//data
            newCard.GetComponent<ZoomUI>().State = CardState.Library;
        }

    }


    /*
     更新卡组的信息
     */
    public void UpdataDeck()
    {
        //清空
        

        //这里遍历playerdata中的字典，然后生成到library中
        foreach (var value in playerData.playerDeck.Keys)
        {
            GameObject newCard = Instantiate(deckCardPrefab, deckPanel.transform.Find("DeckGrid").GetComponent<Transform>());
            newCard.GetComponent<CardCount>().counter.text = playerData.playerDeck[value].ToString();//number
            newCard.GetComponent<DisplayCard>().Card = cardStore.GetCardById(value);//data
            newCard.GetComponent<ZoomUI>().State = CardState.Deck;
            deckCardGameObject.Add(value, newCard);

        }
    }


    /*
     移除卡组中的实例
     */
    public void removeCardById(int id)
    {
        playerData.playerDeck.Remove(id);
        Destroy(deckCardGameObject[id]);
        deckCardGameObject.Remove(id);
    }


    /*
       增加卡组中的实例
     */
    public void addDeckCard(int id)
    {
        //给字典中添加一个
        playerData.playerDeck[id] = 1;
        //实例化一张卡牌
        GameObject newCard = Instantiate(deckCardPrefab, deckPanel.transform.Find("DeckGrid").GetComponent<Transform>());
        newCard.GetComponent<CardCount>().counter.text = playerData.playerCards[id].ToString();//number
        newCard.GetComponent<DisplayCard>().Card = cardStore.GetCardById(id);//data
        newCard.GetComponent<ZoomUI>().State = CardState.Deck;
        deckCardGameObject.Add(id, newCard);
        newCard.GetComponent<CardCount>().counter.text = "1";


    }


    /*
     修改卡组实例中的数量
     */
    public void UpdataDeckCardCount(int id,int number)
    {

        //修改卡组字典里面的记录
        int count = playerData.playerDeck[id] += number;
        deckCardGameObject[id].GetComponent<CardCount>().counter.text = count.ToString();
    }

}
