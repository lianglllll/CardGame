

/// <summary>
/// ���￨��
/// </summary>
public class MonsterCard:BaseCard 
{

    //��������
    public int attack;
    public int hp;
    public int maxHp;

    /*
     ���캯��
     */
    public MonsterCard(int id,string cardName,int attack,int hp,int maxHp) : base(id,cardName)
    {
        this.attack = attack;
        this.hp = hp;
        this.maxHp = maxHp;
    }

}
