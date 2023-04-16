using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class Block : MonoBehaviour,IPointerDownHandler
{

    public GameObject card;//当前格子上的卡牌
    public GameObject summonBlock;//通过开启和取消这个子物体来标识这个block可以用来放置

    public void OnPointerDown(PointerEventData eventData)
    {
        if (summonBlock.activeInHierarchy)
        {   
            BattleManager.Instance.SummonComfirm(transform);
        }
    }
}
