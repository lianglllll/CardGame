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

    public GameObject cardPrefab;//���������Ƶ�prefab

    public PlayerData myData;
    public PlayerData enemyData;//�������

    public Transform myHand;
    public Transform enemyHand;//������,����������grid

    public List<int> myDeckList = new List<int>();
    public List<int> enemyDeckList = new List<int>();//�����п�����Ϣlist������ϴ��֮��Ĳ�����//�洢���ǿ���id

    public GameObject[] myBlocks;
    public GameObject[] enemyBlocks;//��Ϸ�п��Ƶǳ����õĸ���

    public GameObject myHead;
    public GameObject enemyHead;//ͷ��

    public GameStatus gameStatus = GameStatus.Start;//��Ϸ״̬
    public UnityEvent StatusChangeEvent = new UnityEvent();//��Ϸ״̬�ı��¼���֪ͨ������



    private void Start()
    {

        GStart();
    }



    /*
     ��Ϸ��ʼ��һЩ�趨
     */
    public void GStart()
    {
        //��ȡ����
        ReadDeck();
        //����ϴ��
        ShuffleTheCards(0);
        ShuffleTheCards(1);
        //��ҳ鿨�����˳鿨
        DrawCard(0, 5);
        DrawCard(1, 5);
        gameStatus = GameStatus.MyDraw;
        StatusChangeEvent?.Invoke();

    }

    /*
     ��ȡ����
     */
    public void ReadDeck()
    {
        //�Լ���
        foreach (int id  in myData.playerDeck.Keys)
        {
            for (int i = 0; i < myData.playerDeck[id]; i++)
            {
                myDeckList.Add(id);
            }
        }

        //���˵�
        foreach (int id in enemyData.playerDeck.Keys)
        {
            for (int i = 0; i < enemyData.playerDeck[id]; i++)
            {
                enemyDeckList.Add(id);
            }
        }

    }

    /*
     ϴ��
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
     �鿨,0 is my   1 is enemy 
     */
    public void DrawCard(int player,int number)
    {

        Transform hand;//�ŵ��ĸ�������
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
            Debug.Log("�����ѿ�");
            return;
        }


        for (int i = 0; i < number; i++)
        {
            //��ȡ�����е�һ��id,�����Ƴ���һ��Ԫ��
            int id = drawCards[0];
            drawCards.RemoveAt(0);
            //ʵ����
            GameObject newCard = Instantiate(cardPrefab, hand);
            newCard.GetComponent<DisplayCard>().Card = myData.GetCardById(id);//data
        }

    }



    /*
     �غϽ���,�л�״̬
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
     �����鿨�¼�
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
     �����غϽ����¼�
     */
    public void OnTurnEnd()
    {
        TurnEnd();
        StatusChangeEvent?.Invoke();
    }




}
