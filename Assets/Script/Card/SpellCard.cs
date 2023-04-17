/// <summary>
/// ħ������
/// </summary>
public class SpellCard : BaseCard
{

    //��������
    public string effect;


    /*
     ���캯��
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