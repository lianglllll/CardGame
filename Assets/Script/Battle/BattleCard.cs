using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



//����״̬
public enum BattleCardState
{
    inHand,inBlock
}



public class BattleCard : MonoBehaviour, IPointerDownHandler
{

    public int playerID;
    public BattleCardState state = BattleCardState.inHand;

    /*
        ����¼�
     */
    public void OnPointerDown(PointerEventData eventData)
    {

        //�������Ƶ����ʱ�򣬷����ٻ�����
        //ֻ���ǹ��￨�����������вſ���
        if(GetComponent<DisplayCard>().Card is MonsterCard && state == BattleCardState.inHand)
        {
            BattleManager.Instance.SummonRequest(playerID, gameObject);
        }



        //���ڳ��ϵ����ʱ�򣬷��𹥻�����



    }
}
