/// <summary>
/// 卡牌基类
/// </summary>
public class BaseCard 
{

    //卡牌基本属性
    public int id;
    public string cardName;

    /*
     构造函数
     */
    public BaseCard(int id,string cardName) {
        this.id = id;
        this.cardName = cardName; 
    }

    
}
