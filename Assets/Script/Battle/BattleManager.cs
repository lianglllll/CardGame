using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameStatus
{
    Start,MyDraw,MyAction,EnemyDraw,EnemyAction
}


public class BattleManager : Singelton<BattleManager>
{


    public PlayerData myData;
    public PlayerData enemyData;//玩家数据

    public GameObject cardPrefab;//手牌区卡牌的prefab

    public Transform myHand;
    public Transform enemyHand;//手牌区,用手牌区的grid

    public List<int> myDeckList = new List<int>();
    public List<int> enemyDeckList = new List<int>();//卡组中卡牌信息list，用于洗牌之类的操作。//存储的是卡的id

    public GameObject[] myBlocks;
    public GameObject[] enemyBlocks;//召唤时卡牌所在的格子

    public GameObject myHead;
    public GameObject enemyHead;//头像

    public GameStatus gameStatus = GameStatus.Start;//游戏状态

    public UnityEvent StatusChangeEvent = new UnityEvent();//游戏状态改变事件，通知订阅者

    public int[] summonCountMax = new int[2];//最大的召唤次数   0--my   1--enemy
    private int[] summonCounter = new int[2];//也就是说每回合可以召唤两次                                         


    //召唤相关的辅助变量
    private GameObject waitingMonsterCard;  //等待召唤的怪兽卡  
    private int waitingPlayer;//等待的玩家
    public GameObject summonarrowPrefab;//指示箭头的prefab

    private GameObject arrow;//场地中的箭头
    public GameObject canvas;
    public GameObject attackArrowPrefab;

    //攻击相关的辅助变量
    private GameObject attackingMonster;
    private int attackingPlayer;//等待的玩家

    GameObject[] targetBlocks; 


    private void Start()
    {

        GStart();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //销毁箭头，中止召唤请求,中断攻击请求
            DestroyArrow();
            CloseBlocks();
            waitingMonsterCard = null;
            waitingPlayer = -1;
            attackingMonster = null;
            attackingPlayer = -1;
        }
    }

    /*
     游戏开始的一些设定
     */
    public void GStart()
    {
        //读取卡组
        ReadDeck();
        //卡组洗牌
        ShuffleTheCards(0);
        ShuffleTheCards(1);
        //玩家抽卡，敌人抽卡
        DrawCard(0, 5);
        DrawCard(1, 5);

        gameStatus = GameStatus.MyDraw;
        StatusChangeEvent?.Invoke();

        //访问次数
        summonCounter[1] = summonCountMax[1];
        summonCounter[0] = summonCountMax[0];

    }

    /*
     读取卡组
     */
    public void ReadDeck()
    {
        //自己的
        foreach (int id  in myData.playerDeck.Keys)
        {
            for (int i = 0; i < myData.playerDeck[id]; i++)
            {
                myDeckList.Add(id);
            }
        }

        //敌人的
        foreach (int id in enemyData.playerDeck.Keys)
        {
            for (int i = 0; i < enemyData.playerDeck[id]; i++)
            {
                enemyDeckList.Add(id);
            }
        }

    }

    /*
     洗牌
     */
    public void ShuffleTheCards(int flag) {

        List<int> shuffletDeck = new List<int>();
        if (flag == 0)
        {
            shuffletDeck = myDeckList;
        }
        else if(flag == 1)
        {
            shuffletDeck = enemyDeckList;
        }

        System.Random rng = new System.Random();
        for (int i = 0; i < shuffletDeck.Count; i++)
        {
            int rad = Random.Range(0, shuffletDeck.Count);
            int value = shuffletDeck[i];
            shuffletDeck[i] = shuffletDeck[rad];
            shuffletDeck[rad] = value;
        }

    }

    /*
     抽卡,0 is my   1 is enemy 
     */
    public void DrawCard(int player,int number)
    {

        Transform hand;//放到哪个手牌区
        List<int> drawCards = new List<int>();

        if(player == 0)
        {
            hand = myHand;
            drawCards = myDeckList;
        }
        else 
        {
            hand = enemyHand;
            drawCards = enemyDeckList;
        }



        if (number > drawCards.Count)
        {
            Debug.Log("卡组已空");
            return;
        }


        for (int i = 0; i < number; i++)
        {
            //获取链表中第一个id,并且移除第一个元素
            int id = drawCards[0];
            drawCards.RemoveAt(0);
            //实例化
            GameObject newCard = Instantiate(cardPrefab, hand);
            BaseCard card=myData.GetCardById(id);//data

            BaseCard tempCard = null;
            if(card is MonsterCard)
            {
                tempCard = new MonsterCard(card as MonsterCard);
            }
            else if(card is SpellCard)
            {
                tempCard = new SpellCard(card as SpellCard);
            }


            newCard.GetComponent<DisplayCard>().Card = tempCard;
            newCard.GetComponent<BattleCard>().playerID = player;

        }

    }

    /*
     回合结束,切换状态
     */
    public void TurnEnd()
    {
        if (arrow != null)
        {
            return;
        }


        if(gameStatus == GameStatus.MyAction)
        {
            gameStatus = GameStatus.EnemyDraw;
            summonCounter[0] = summonCountMax[0];
            //重置攻击次数
            ResetAttackNumber(1);

        }
        else if(gameStatus == GameStatus.EnemyAction)
        {
            gameStatus = GameStatus.MyDraw;
            summonCounter[1] = summonCountMax[1];
            ResetAttackNumber(0);
        }

    }

    /*
     触发抽卡事件
     */
    public void OnMyDraw()
    {
        if (gameStatus == GameStatus.MyDraw)
        {
            DrawCard(0, 1);
            gameStatus = GameStatus.MyAction;
            StatusChangeEvent?.Invoke();
        }

    }
    public void OnEnemyDraw()
    {
        if(gameStatus == GameStatus.EnemyDraw) {

            DrawCard(1, 1);
            gameStatus = GameStatus.EnemyAction;
            StatusChangeEvent?.Invoke();
        }
     
    }

    /*
     触发回合结束事件
     */
    public void OnTurnEnd()
    {
        TurnEnd();
        StatusChangeEvent?.Invoke();
    }

    #region 召唤
    /*
     召唤卡牌请求
     */
    public void SummonRequest(int _player,GameObject _mosterCard)
    {

        //判断是否属于本阶段需要做的事情
        GameStatus status = _player == 0 ? GameStatus.MyAction : GameStatus.EnemyAction;
        if (gameStatus != status )
        {
            return;
        }

        if (arrow != null)
        {
            return;
        }


        bool hasEmptyBlock = false;

        //确定玩家的召唤格子
        targetBlocks = _player == 0 ? myBlocks : enemyBlocks;

        //判断召唤次数是否充足
        if (summonCounter[_player] > 0)
        {
            //判断格子是否为空
            foreach(var block in targetBlocks)
            {
                if (block.GetComponent<Block>().card == null)
                {
                    //等待召唤显示
                    block.GetComponent<Block>().summonBlock.SetActive(true);
                    hasEmptyBlock = true;
                }
            }
        }

        if (hasEmptyBlock)
        {
            waitingMonsterCard = _mosterCard;
            waitingPlayer = _player;
            CreatArrow(_mosterCard.transform, summonarrowPrefab);
        }

    }

    /*
     召唤确认（召唤到哪一个格子上面）
     */
    public void SummonComfirm(Transform _block)
    {
        if (waitingMonsterCard == null) return;//提前中止了

        Summon(waitingPlayer, waitingMonsterCard, _block);


        //将一些临时变量全部初始化等待下一次使用
        waitingMonsterCard = null;
        //召唤次数减少
        summonCounter[waitingPlayer]--;
        //等待的玩家标志置为-1
        int flag = waitingPlayer == 0 ? 0 : 1;
        waitingPlayer = -1;

        //关闭格子的可放置的标志
        CloseBlocks();
        DestroyArrow();
    }

    /*
     召唤
     */
    public void Summon(int _player, GameObject _mosterCard, Transform _block)
    {
        //将卡牌实例移动到block下面
        _mosterCard.transform.SetParent(_block);
        _mosterCard.transform.localPosition = Vector3.zero;//位置
        _mosterCard.GetComponent<BattleCard>().state = BattleCardState.inBlock;//状态
        _block.GetComponent<Block>().card = _mosterCard;
        //重置攻击次数
        _mosterCard.GetComponent<BattleCard>().ResetAttackCount();
    }


    #endregion

    #region 箭头
    /*
     箭头创建
     */
    public void CreatArrow(Transform _startPoint,GameObject _prefab)
    {
        if (arrow != null) return;
        arrow = GameObject.Instantiate(_prefab);
        arrow.transform.SetParent(canvas.transform);
        arrow.GetComponent<Arrow>().SetStartPoint(new Vector2(_startPoint.position.x,_startPoint.position.y));
    }

    /*
     箭头销毁
     */
    public void DestroyArrow()
    {
        if (arrow != null)
        {
            Destroy(arrow);
            arrow = null;
        }
    }
    #endregion

    /*
     格子可选关闭
     */
    public void CloseBlocks( )
    {
        if (targetBlocks == null) return;

            foreach (var block in targetBlocks)
            {
                block.GetComponent<Block>().summonBlock.SetActive(false);//关闭等待召唤显示
                block.GetComponent<Block>().attackBlock.SetActive(false);//关闭等待召唤显示
            }

        targetBlocks = null;
    }

    #region 攻击
    /*
     * 攻击请求
     */
    public void AttackRequest(int _player,GameObject _mosterCard)
    {
        //判断是否属于本阶段需要做的事情
        GameStatus status = _player == 0 ? GameStatus.MyAction : GameStatus.EnemyAction;
        if (gameStatus != status)
        {
            return;
        }

        bool hasMosterBlock = false;

        //确定要攻击对象的格子序列
        targetBlocks = _player == 0 ? enemyBlocks : myBlocks;


        //判断格子序列里面有没有卡牌对象
        foreach (var block in targetBlocks)
        {
            if (block.GetComponent<Block>().card != null)
            {
                //等待攻击显示
                block.GetComponent<Block>().attackBlock.SetActive(true);
                //将可攻击目标的脚本打开
                block.GetComponent<Block>().card.GetComponent<AttackTarget>().attackable = true;     
                hasMosterBlock = true;
            }
        }
        
        if (hasMosterBlock)
        {
            attackingMonster = _mosterCard;
            attackingPlayer = _player;
            CreatArrow(_mosterCard.transform, attackArrowPrefab);
        }
    }

    /*
     攻击确认，这个是打怪物的
     */
    public void AttackComfirm(GameObject _target)
    {

        if (attackingMonster == null) return;//提前中止了

        Attack(attackingMonster, _target);

        //将可攻击目标的脚本关闭

        foreach (GameObject block in targetBlocks)
        {
            if (block.GetComponent<Block>().card != null)
            {
                block.GetComponent<Block>().card.GetComponent<AttackTarget>().attackable = false;
            }
        }
        attackingMonster = null;
        attackingPlayer = -1;
        CloseBlocks();
        DestroyArrow();

    }


    /*
     攻击
     */
    public void Attack(GameObject _attacker, GameObject _target)
    {
        //获取攻击者的攻击力
        MonsterCard monsterCard = _attacker.GetComponent<DisplayCard>().Card as MonsterCard;
        //触发被攻击者
        _target.GetComponent<AttackTarget>().ApplyDamage(monsterCard.attack);
        //减少攻击者的攻击次数
        _attacker.GetComponent<BattleCard>().CostAttackCount();


    }

    /*
    攻击确认,这个是打玩家的
    */
    public void AttackComfirm()
    {

    }
    #endregion

    /*
     本回合结束，将我们这边场上的卡牌次数全部重置
     */
    public void ResetAttackNumber(int i)
    {
        if (i == 0)
        {
            foreach (GameObject block in myBlocks)
            {
                if (block.GetComponent<Block>().card != null)
                {
                    block.GetComponent<Block>().card.GetComponent<BattleCard>().ResetAttackCount();
                }
            }
        }
        else
        {
            foreach (GameObject block in enemyBlocks)
            {
                if (block.GetComponent<Block>().card != null)
                {
                    block.GetComponent<Block>().card.GetComponent<BattleCard>().ResetAttackCount();
                }
            }
        }
    }

}
