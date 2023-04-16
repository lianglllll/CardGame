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

    public int playerID;
    public BattleCardState state = BattleCardState.inHand;

    /*
        点击事件
     */
    public void OnPointerDown(PointerEventData eventData)
    {

        //当在手牌点击的时候，发起召唤请求
        //只有是怪物卡并且在手牌中才可以
        if(GetComponent<DisplayCard>().Card is MonsterCard && state == BattleCardState.inHand)
        {
            BattleManager.Instance.SummonRequest(playerID, gameObject);
        }



        //当在场上点击的时候，发起攻击请求



    }
}
