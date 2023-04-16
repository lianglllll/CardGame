using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// �����̳�/Ҳ����������
/// </summary>
public class CardStore : MonoBehaviour
{
    private TextAsset cardData;
    //private List<BaseCard> cardList = new List<BaseCard>();//todo
    private Dictionary<int, BaseCard> cardList = new Dictionary<int, BaseCard>();
    private List<int> keys;//���ڱ����ֵ��key�����������ȡ��ֵ��



    /*
     ���ؿ����ļ�,����������Ǳ߽��е��ü���
     */
    public void LoadCardData()
    {
        //�����ļ�
        cardData = Resources.Load<TextAsset>("Config/CardData");

        if (cardData == null)
        {
            Debug.Log("CarDStore��textAssetΪ�գ�");
            return;
        }

        //Text�ļ�����һ�н��зָ�
        string[] dataRow = cardData.text.Split('\n');
        //ע��ȥ�����ǵ�ע����Ϣ
        foreach (var row in dataRow)
        {
            string[] rowArray = row.Split(',');

            if (rowArray[0] == "#") {
                //����
                continue;
            }else if (rowArray[0] == "monster")
            {   //���￨
                int id = int.Parse(rowArray[1]);
                string cardName = rowArray[2];
                int attack = int.Parse(rowArray[3]);
                int hp = int.Parse(rowArray[4]);
                MonsterCard mcard = new MonsterCard(id, cardName, attack, hp, hp);
                cardList.Add(id, mcard);
            }
            else if (rowArray[0] == "spell")
            {   //ħ����
                int id = int.Parse(rowArray[1]);
                string cardName = rowArray[2];
                string affect = rowArray[3];
                SpellCard scard = new SpellCard(id, cardName, affect);
                cardList.Add(id,scard);
            }
        }

        //����key
        keys = new List<int>(cardList.Keys);

    }

    /*
     �ӿ����������ȡһ�ſ��ƣ������������䣩���̵��ã�
     */
    public BaseCard RandomCard()
    {
        //�������ֵ
        BaseCard card = cardList[Random.Range(0, keys.Count)];
        return card;
        
    }


    /*
     ͨ��key��ȡ�ֵ��е�card��
     */
    public BaseCard GetCardById(int id)
    {
        if (cardList.ContainsKey(id))
        {
            return cardList[id];
        }
        Debug.Log("CardStore.GetCardById��⵽�����ڵ�id:"+id);
        return null;
    }

}
