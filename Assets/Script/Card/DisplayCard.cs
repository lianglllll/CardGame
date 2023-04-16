using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCard : MonoBehaviour
{

    //����Ҫչʾ�Ŀ���
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

    //Cardģ�������textUI
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
     չʾ����
     */
    public void  ShowCard()
    {

        if(card is MonsterCard)
        {//���￨
            var mcard = card as MonsterCard;
            carName.text = mcard.cardName;
            attack.text = mcard.attack.ToString();
            hp.text = mcard.hp.ToString();
            effect.text = "";
        }
        else if (card is SpellCard)
        {//ħ����
            var scard = card as SpellCard;
            carName.text = scard.cardName;
            effect.text = scard.effect;
        }
    }


}
