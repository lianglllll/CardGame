using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


//���Ƶ�״̬
public enum CardState
{
    Library, Deck
}


/// <summary>
/// ��ui�ӵ���
/// </summary>
public class ZoomUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{

    public float zoomSize;//���Ŵ�С
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
        //��ȡ��ǰ���Ƶ�id
        int id = GetComponent<DisplayCard>().Card.id;



        //�����еĿ���
        if (State == CardState.Deck)
        {//����--/���Ϊ0��ȥ��

            //��ȡ��ǰ�������ڿ����е�����
            int number = playerData.playerDeck[id];

            if (number > 1)
            {//--����
                deckManager.UpdataDeckCardCount(id, -1);
            }
            else
            {//����Ϊ0�ˣ�����ƴӿ������Ƴ�
                
                deckManager.removeCardById(id);
            }

        }//�ֿ��еĿ���
        else if(State == CardState.Library)
        {//��ӵ�������,������˾�++

            if (playerData.playerDeck.ContainsKey(id))
            {
                if (playerData.playerCards[id]> playerData.playerDeck[id])
                {
                    deckManager.UpdataDeckCardCount(id, 1);
                }
                else
                {
                    //�����Ѿ��ﵽ����
                }
            }
            else
            {
                //����һ�����Ƶ�������
                deckManager.addDeckCard(id);
            }
        }

        //���浽�ļ���

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
