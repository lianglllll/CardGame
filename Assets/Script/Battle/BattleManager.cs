using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameStatus
{
    Start,MyDraw,MyAction,EnemyDraw,EnemyAction
}


public class BattleManager : Singelton<BattleManager>
{

    public GameObject cardPrefab;//手牌区卡牌的prefab

    public PlayerData myData;
    public PlayerData enemyData;//玩家数据

    public Transform myHand;
    public Transform enemyHand;//手牌区,用手牌区的grid

    public List<int> myDeckList = new List<int>();
    public List<int> enemyDeckList = new List<int>();//卡组中卡牌信息list，用于洗牌之类的操作。//存储的是卡的id

    public GameObject[] myBlocks;
    public GameObject[] enemyBlocks;//游戏中卡牌登场放置的格子

    public GameObject myHead;
    public GameObject enemyHead;//头像

    public GameStatus gameStatus = GameStatus.Start;//游戏状态
    public UnityEvent StatusChangeEvent = new UnityEvent();//游戏状态改变事件，通知订阅者



    private void Start()
    {

        GStart();
    }



    /*
     游戏开始的一些设定
     */
    public void GStart()
    {
        //读取卡组
        ReadDeck();
        //卡组洗牌
        ShuffleTheCards(0);
        ShuffleTheCards(1);
        //玩家抽卡，敌人抽卡
        DrawCard(0, 5);
        DrawCard(1, 5);
        gameStatus = GameStatus.MyDraw;
        StatusChangeEvent?.Invoke();

    }

    /*
     读取卡组
     */
    public void ReadDeck()
    {
        //自己的
        foreach (int id  in myData.playerDeck.Keys)
        {
            for (int i = 0; i < myData.playerDeck[id]; i++)
            {
                myDeckList.Add(id);
            }
        }

        //敌人的
        foreach (int id in enemyData.playerDeck.Keys)
        {
            for (int i = 0; i < enemyData.playerDeck[id]; i++)
            {
                enemyDeckList.Add(id);
            }
        }

    }

    /*
     洗牌
     */
    public void ShuffleTheCards(int flag) {

        List<int> shuffletDeck = new List<int>();
        if (flag == 0)
        {
            shuffletDeck = myDeckList;
        }
        else if(flag == 1)
        {
            shuffletDeck = enemyDeckList;
        }

        System.Random rng = new System.Random();
        for (int i = 0; i < shuffletDeck.Count; i++)
        {
            int rad = Random.Range(0, shuffletDeck.Count);
            int value = shuffletDeck[i];
            shuffletDeck[i] = shuffletDeck[rad];
            shuffletDeck[rad] = value;
        }

    }

    /*
     抽卡,0 is my   1 is enemy 
     */
    public void DrawCard(int player,int number)
    {

        Transform hand;//放到哪个手牌区
        List<int> drawCards = new List<int>();

        if(player == 0)
        {
            hand = myHand;
            drawCards = myDeckList;
        }
        else 
        {
            hand = enemyHand;
            drawCards = enemyDeckList;
        }



        if (number > drawCards.Count)
        {
            Debug.Log("卡组已空");
            return;
        }


        for (int i = 0; i < number; i++)
        {
            //获取链表中第一个id,并且移除第一个元素
            int id = drawCards[0];
            drawCards.RemoveAt(0);
            //实例化
            GameObject newCard = Instantiate(cardPrefab, hand);
            newCard.GetComponent<DisplayCard>().Card = myData.GetCardById(id);//data
        }

    }



    /*
     回合结束,切换状态
     */
    public void TurnEnd()
    {
        if(gameStatus == GameStatus.MyAction)
        {
            gameStatus = GameStatus.EnemyDraw;

        }else if(gameStatus == GameStatus.EnemyAction)
        {
            gameStatus = GameStatus.MyDraw;
        }

    }


    /*
     触发抽卡事件
     */
    public void OnMyDraw()
    {
        if (gameStatus == GameStatus.MyDraw)
        {
            DrawCard(0, 1);
            gameStatus = GameStatus.MyAction;
            StatusChangeEvent?.Invoke();
        }

    }
    public void OnEnemyDraw()
    {
        if(gameStatus == GameStatus.EnemyDraw) {

            DrawCard(1, 1);
            gameStatus = GameStatus.EnemyAction;
            StatusChangeEvent?.Invoke();
        }
     
    }


    /*
     触发回合结束事件
     */
    public void OnTurnEnd()
    {
        TurnEnd();
        StatusChangeEvent?.Invoke();
    }




}
