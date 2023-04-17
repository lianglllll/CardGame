using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AttackTarget : MonoBehaviour, IPointerClickHandler
{


    public bool attackable;
    DisplayCard display;//玩家和怪物的区别在于有无这个类

    public void OnPointerClick(PointerEventData eventData)
    {
        if (attackable)
        {
            if (display == null)
            {
                BattleManager.Instance.AttackComfirm();
            }
            else
            {
                BattleManager.Instance.AttackComfirm(gameObject);
            }

        }
    }

    /*
     造成伤害
     */
    public void ApplyDamage(int _damage)
    {
        MonsterCard card = GetComponent<DisplayCard>().Card as MonsterCard;
        card.hp -= _damage;
        if (card.hp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {//更新血量ui
            GetComponent<DisplayCard>().ShowCard();
        }


    }


    private void Start()
    {
        display = GetComponent<DisplayCard>();
    }
}
