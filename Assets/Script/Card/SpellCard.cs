/// <summary>
/// 魔法卡牌
/// </summary>
public class SpellCard : BaseCard
{

    //独有属性
    public string effect;


    /*
     构造函数
     */
    public SpellCard(int id, string cardName,string effect) : base(id, cardName)
    {
        this.effect = effect;
    }

    public SpellCard(SpellCard card) : base(card.id, card.cardName)
    {
        this.effect = card.effect;
    }

}