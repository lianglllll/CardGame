using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class Block : MonoBehaviour,IPointerDownHandler
{

    public GameObject card;//��ǰ�����ϵĿ���
    public GameObject summonBlock;//ͨ��������ȡ���������������ʶ���block������������

    public void OnPointerDown(PointerEventData eventData)
    {
        if (summonBlock.activeInHierarchy)
        {   
            BattleManager.Instance.SummonComfirm(transform);
        }
    }
}
