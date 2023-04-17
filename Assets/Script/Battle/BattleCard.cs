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

    public int playerID;//���ƹ�����һ�����
    public BattleCardState state = BattleCardState.inHand;
    private int attackCountMax;
    public int attackCount;

    private void Start()
    {
        if(GetComponent<DisplayCard>().Card is MonsterCard)
        {
            attackCountMax = (GetComponent<DisplayCard>().Card as MonsterCard).attackCount;
        }
    }

    /*
        ����¼�
     */
    public void OnPointerDown(PointerEventData eventData)
    {
        if(GetComponent<DisplayCard>().Card is MonsterCard)
        {
            //�������Ƶ����ʱ�򣬷����ٻ�����
            //ֻ���ǹ��￨�����������вſ���
            if (state == BattleCardState.inHand)
            {
                BattleManager.Instance.SummonRequest(playerID, gameObject);

            }else if (state == BattleCardState.inBlock&&attackCount>0)//���ڳ��ϵ����ʱ�򣬷��𹥻�����
            {
                BattleManager.Instance.AttackRequest(playerID, gameObject);
            }
        }
    }

    /*
     ���ù�������
     */
    public void ResetAttackCount()
    {
        attackCount = attackCountMax;
    }

    /*
     ���Ĺ�������
     */
    public void CostAttackCount()
    {
        attackCount--;
    }
}
