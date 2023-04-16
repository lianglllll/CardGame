using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour
{

    //导入要展示的卡牌
    private  BaseCard card;
    public BaseCard Card
    {
        get
        {
            return card;
        }
        set
        {
            card = value;
            ShowCard();
        }
    }

    //Card模型里面的textUI
    public Image cardBackGroud;
    public Text carName;
    public Text effect;
    public Text attack;
    public Text hp;


    private void Start()
    {
        ShowCard();
    }


    /*
     展示卡牌
     */
    public void  ShowCard()
    {

        if(card is MonsterCard)
        {//怪物卡
            var mcard = card as MonsterCard;
            carName.text = mcard.cardName;
            attack.text = mcard.attack.ToString();
            hp.text = mcard.hp.ToString();
            effect.text = "";
        }
        else if (card is SpellCard)
        {//魔法卡
            var scard = card as SpellCard;
            carName.text = scard.cardName;
            effect.text = scard.effect;
        }
    }


}
