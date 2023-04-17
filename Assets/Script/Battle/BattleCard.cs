using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



//卡的状态
public enum BattleCardState
{
    inHand,inBlock
}

public class BattleCard : MonoBehaviour, IPointerDownHandler
{

    public int playerID;//卡牌归属哪一个玩家
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
        点击事件
     */
    public void OnPointerDown(PointerEventData eventData)
    {
        if(GetComponent<DisplayCard>().Card is MonsterCard)
        {
            //当在手牌点击的时候，发起召唤请求
            //只有是怪物卡并且在手牌中才可以
            if (state == BattleCardState.inHand)
            {
                BattleManager.Instance.SummonRequest(playerID, gameObject);

            }else if (state == BattleCardState.inBlock&&attackCount>0)//当在场上点击的时候，发起攻击请求
            {
                BattleManager.Instance.AttackRequest(playerID, gameObject);
            }
        }
    }

    /*
     重置攻击次数
     */
    public void ResetAttackCount()
    {
        attackCount = attackCountMax;
    }

    /*
     消耗攻击次数
     */
    public void CostAttackCount()
    {
        attackCount--;
    }
}
