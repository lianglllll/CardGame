using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���²ֿ���Ϣ��������Ϣ//Ӧ����Ҫ���Ʋֿ����ui
/// </summary>
public class DeckManager : MonoBehaviour
{
    //����ui���
    public GameObject libraryPanel;
    public GameObject deckPanel;


    //��ͬ���Ƶ�Ԥ����
    public GameObject libraryCardPrefab;
    public GameObject deckCardPrefab;


    //��ȡ��������
    public GameObject playerdataManager;


    //��Ҫ�õ��Ľű�
    private PlayerData playerData;
    private CardStore cardStore;

    //���濨���п��Ƶ�ʵ��
    public Dictionary<int, GameObject> deckCardGameObject = new Dictionary<int, GameObject>();


    private void Start()
    {
        playerData = playerdataManager.GetComponent<PlayerData>();
        cardStore = playerdataManager.GetComponent<CardStore>();
        UpdataLibrary();
        UpdataDeck();
    }

    /*
     ���²ֿ�Ŀ�����Ϣ
     */
    public void UpdataLibrary()
    {
        //�������playerdata�е��ֵ䣬Ȼ�����ɵ�library��
        foreach (var value in playerData.playerCards.Keys)
        {
            GameObject newCard = Instantiate(libraryCardPrefab, libraryPanel.transform.Find("LibraryGrid").GetComponent<Transform>());
            newCard.GetComponent<CardCount>().counter.text = playerData.playerCards[value].ToString();//number
            newCard.GetComponent<DisplayCard>().Card = cardStore.GetCardById(value);//data
            newCard.GetComponent<ZoomUI>().State = CardState.Library;
        }

    }


    /*
     ���¿������Ϣ
     */
    public void UpdataDeck()
    {
        //���
        

        //�������playerdata�е��ֵ䣬Ȼ�����ɵ�library��
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
     �Ƴ������е�ʵ��
     */
    public void removeCardById(int id)
    {
        playerData.playerDeck.Remove(id);
        Destroy(deckCardGameObject[id]);
        deckCardGameObject.Remove(id);
    }


    /*
       ���ӿ����е�ʵ��
     */
    public void addDeckCard(int id)
    {
        //���ֵ������һ��
        playerData.playerDeck[id] = 1;
        //ʵ����һ�ſ���
        GameObject newCard = Instantiate(deckCardPrefab, deckPanel.transform.Find("DeckGrid").GetComponent<Transform>());
        newCard.GetComponent<CardCount>().counter.text = playerData.playerCards[id].ToString();//number
        newCard.GetComponent<DisplayCard>().Card = cardStore.GetCardById(id);//data
        newCard.GetComponent<ZoomUI>().State = CardState.Deck;
        deckCardGameObject.Add(id, newCard);
        newCard.GetComponent<CardCount>().counter.text = "1";


    }


    /*
     �޸Ŀ���ʵ���е�����
     */
    public void UpdataDeckCardCount(int id,int number)
    {

        //�޸Ŀ����ֵ�����ļ�¼
        int count = playerData.playerDeck[id] += number;
        deckCardGameObject[id].GetComponent<CardCount>().counter.text = count.ToString();
    }

}
