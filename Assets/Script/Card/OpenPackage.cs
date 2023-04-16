using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ����
/// </summary>
public class OpenPackage : MonoBehaviour
{
    //����ģ��
    public GameObject CardPrefab;
    //���̵꣩
    private CardStore cardStore;
    //һ�����������ж����ſ���
    private int cardNumberInPackage = 5;
    //���飬����չʾcardʵ��
    public GameObject cardPool;
    //��ʱ�洢Cardʵ���������ڽ�������
    List<GameObject> cards = new List<GameObject>();

    //��ʱ��Ҫ����鿨���ݵ����
    public PlayerData playerData;


    void Start()
    {
        cardStore = GetComponent<CardStore>();

    }

    /*
     ������
     */
    public void OnClickOpen()
    {

        //��Ǯ
        if (playerData.coins < 2)
        {
            return;
        }
        else
        {
            playerData.coins -= 2;
        }


        //��֤��ǰ����û�п���
        ClearPool();
        //ʵ��������
        for (int i = 0; i < cardNumberInPackage; i++)
        {
            //ʵ����ģ��,ͬʱ�����Թ��ص�cardPool������
            GameObject newCard = GameObject.Instantiate(CardPrefab, cardPool.transform);
            //��ʼ��,�ӿ�������������Ÿ���
            newCard.GetComponent<DisplayCard>().Card = cardStore.RandomCard();
            cards.Add(newCard);
        }
        SaveCardDataToPlayer();//�����б��浽playerdata��


    }

    /*
     ��տ��飬������һ�ο�����
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
     �������ݵ�playerData����
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

        playerData.SavePlayerData();//���浽�ļ���
    }

}
