

/// <summary>
/// ���￨��
/// </summary>
public class MonsterCard:BaseCard 
{

    //��������
    public int attack;
    public int hp;
    public int maxHp;
    public int attackCount;

    /*
     ���캯��
     */
    public MonsterCard(int id,string cardName,int attack,int hp,int maxHp) : base(id,cardName)
    {
        this.attack = attack;
        this.hp = hp;
        this.maxHp = maxHp;
        attackCount = 1;
    }


    public MonsterCard(MonsterCard card):base(card.id, card.cardName)
    {
        this.attack = card.attack;
        this.hp = card.maxHp;
        this.maxHp = card.maxHp;
        attackCount = 1;
    }

}
