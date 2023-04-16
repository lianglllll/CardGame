using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//卡牌的状态
public enum CardState
{
    Library, Deck
}


/// <summary>
/// 给ui加点料
/// </summary>
public class ZoomUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{

    public float zoomSize;//缩放大小
    private PlayerData playerData;
    private DeckManager deckManager;
    public CardState State { get; set; }

    private void Start()
    {
        playerData = GameObject.Find("DataManager").GetComponent<PlayerData>();
        deckManager = GameObject.Find("DeckManager").GetComponent<DeckManager>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        //获取当前卡牌的id
        int id = GetComponent<DisplayCard>().Card.id;



        //卡组中的卡牌
        if (State == CardState.Deck)
        {//数量--/如果为0就去掉

            //获取当前这种牌在卡组中的数量
            int number = playerData.playerDeck[id];

            if (number > 1)
            {//--即可
                deckManager.UpdataDeckCardCount(id, -1);
            }
            else
            {//数量为0了，这个牌从卡组中移除
                
                deckManager.removeCardById(id);
            }

        }//仓库中的卡牌
        else if(State == CardState.Library)
        {//添加到卡组中,如果有了就++

            if (playerData.playerDeck.ContainsKey(id))
            {
                if (playerData.playerCards[id]> playerData.playerDeck[id])
                {
                    deckManager.UpdataDeckCardCount(id, 1);
                }
                else
                {
                    //卡牌已经达到上限
                }
            }
            else
            {
                //创建一个卡牌到卡组中
                deckManager.addDeckCard(id);
            }
        }

        //保存到文件？

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = new Vector3(zoomSize, zoomSize, 1.0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        transform.localScale = Vector3.one;
    }


}
