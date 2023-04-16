

/// <summary>
/// 怪物卡牌
/// </summary>
public class MonsterCard:BaseCard 
{

    //独有属性
    public int attack;
    public int hp;
    public int maxHp;

    /*
     构造函数
     */
    public MonsterCard(int id,string cardName,int attack,int hp,int maxHp) : base(id,cardName)
    {
        this.attack = attack;
        this.hp = hp;
        this.maxHp = maxHp;
    }

}
