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
    public PlayerData enemyData;//�������

    public GameObject cardPrefab;//���������Ƶ�prefab

    public Transform myHand;
    public Transform enemyHand;//������,����������grid

    public List<int> myDeckList = new List<int>();
    public List<int> enemyDeckList = new List<int>();//�����п�����Ϣlist������ϴ��֮��Ĳ�����//�洢���ǿ���id

    public GameObject[] myBlocks;
    public GameObject[] enemyBlocks;//�ٻ�ʱ�������ڵĸ���

    public GameObject myHead;
    public GameObject enemyHead;//ͷ��

    public GameStatus gameStatus = GameStatus.Start;//��Ϸ״̬

    public UnityEvent StatusChangeEvent = new UnityEvent();//��Ϸ״̬�ı��¼���֪ͨ������

    public int[] summonCountMax = new int[2];//�����ٻ�����   0--my   1--enemy
    private int[] summonCounter = new int[2];//Ҳ����˵ÿ�غϿ����ٻ�����                                         


    //�ٻ���صĸ�������
    private GameObject waitingMonsterCard;  //�ȴ��ٻ��Ĺ��޿�  
    private int waitingPlayer;//�ȴ������
    public GameObject summonarrowPrefab;//ָʾ��ͷ��prefab

    private GameObject arrow;//�����еļ�ͷ
    public GameObject canvas;
    public GameObject attackArrowPrefab;

    //������صĸ�������
    private GameObject attackingMonster;
    private int attackingPlayer;//�ȴ������

    GameObject[] targetBlocks; 


    private void Start()
    {

        GStart();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //���ټ�ͷ����ֹ�ٻ�����,�жϹ�������
            DestroyArrow();
            CloseBlocks();
            waitingMonsterCard = null;
            waitingPlayer = -1;
            attackingMonster = null;
            attackingPlayer = -1;
        }
    }

    /*
     ��Ϸ��ʼ��һЩ�趨
     */
    public void GStart()
    {
        //��ȡ����
        ReadDeck();
        //����ϴ��
        ShuffleTheCards(0);
        ShuffleTheCards(1);
        //��ҳ鿨�����˳鿨
        DrawCard(0, 5);
        DrawCard(1, 5);

        gameStatus = GameStatus.MyDraw;
        StatusChangeEvent?.Invoke();

        //���ʴ���
        summonCounter[1] = summonCountMax[1];
        summonCounter[0] = summonCountMax[0];

    }

    /*
     ��ȡ����
     */
    public void ReadDeck()
    {
        //�Լ���
        foreach (int id  in myData.playerDeck.Keys)
        {
            for (int i = 0; i < myData.playerDeck[id]; i++)
            {
                myDeckList.Add(id);
            }
        }

        //���˵�
        foreach (int id in enemyData.playerDeck.Keys)
        {
            for (int i = 0; i < enemyData.playerDeck[id]; i++)
            {
                enemyDeckList.Add(id);
            }
        }

    }

    /*
     ϴ��
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
     �鿨,0 is my   1 is enemy 
     */
    public void DrawCard(int player,int number)
    {

        Transform hand;//�ŵ��ĸ�������
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
            Debug.Log("�����ѿ�");
            return;
        }


        for (int i = 0; i < number; i++)
        {
            //��ȡ�����е�һ��id,�����Ƴ���һ��Ԫ��
            int id = drawCards[0];
            drawCards.RemoveAt(0);
            //ʵ����
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
     �غϽ���,�л�״̬
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
            //���ù�������
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
     �����鿨�¼�
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
     �����غϽ����¼�
     */
    public void OnTurnEnd()
    {
        TurnEnd();
        StatusChangeEvent?.Invoke();
    }

    #region �ٻ�
    /*
     �ٻ���������
     */
    public void SummonRequest(int _player,GameObject _mosterCard)
    {

        //�ж��Ƿ����ڱ��׶���Ҫ��������
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

        //ȷ����ҵ��ٻ�����
        targetBlocks = _player == 0 ? myBlocks : enemyBlocks;

        //�ж��ٻ������Ƿ����
        if (summonCounter[_player] > 0)
        {
            //�жϸ����Ƿ�Ϊ��
            foreach(var block in targetBlocks)
            {
                if (block.GetComponent<Block>().card == null)
                {
                    //�ȴ��ٻ���ʾ
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
     �ٻ�ȷ�ϣ��ٻ�����һ���������棩
     */
    public void SummonComfirm(Transform _block)
    {
        if (waitingMonsterCard == null) return;//��ǰ��ֹ��

        Summon(waitingPlayer, waitingMonsterCard, _block);


        //��һЩ��ʱ����ȫ����ʼ���ȴ���һ��ʹ��
        waitingMonsterCard = null;
        //�ٻ���������
        summonCounter[waitingPlayer]--;
        //�ȴ�����ұ�־��Ϊ-1
        int flag = waitingPlayer == 0 ? 0 : 1;
        waitingPlayer = -1;

        //�رո��ӵĿɷ��õı�־
        CloseBlocks();
        DestroyArrow();
    }

    /*
     �ٻ�
     */
    public void Summon(int _player, GameObject _mosterCard, Transform _block)
    {
        //������ʵ���ƶ���block����
        _mosterCard.transform.SetParent(_block);
        _mosterCard.transform.localPosition = Vector3.zero;//λ��
        _mosterCard.GetComponent<BattleCard>().state = BattleCardState.inBlock;//״̬
        _block.GetComponent<Block>().card = _mosterCard;
        //���ù�������
        _mosterCard.GetComponent<BattleCard>().ResetAttackCount();
    }


    #endregion

    #region ��ͷ
    /*
     ��ͷ����
     */
    public void CreatArrow(Transform _startPoint,GameObject _prefab)
    {
        if (arrow != null) return;
        arrow = GameObject.Instantiate(_prefab);
        arrow.transform.SetParent(canvas.transform);
        arrow.GetComponent<Arrow>().SetStartPoint(new Vector2(_startPoint.position.x,_startPoint.position.y));
    }

    /*
     ��ͷ����
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
     ���ӿ�ѡ�ر�
     */
    public void CloseBlocks( )
    {
        if (targetBlocks == null) return;

            foreach (var block in targetBlocks)
            {
                block.GetComponent<Block>().summonBlock.SetActive(false);//�رյȴ��ٻ���ʾ
                block.GetComponent<Block>().attackBlock.SetActive(false);//�رյȴ��ٻ���ʾ
            }

        targetBlocks = null;
    }

    #region ����
    /*
     * ��������
     */
    public void AttackRequest(int _player,GameObject _mosterCard)
    {
        //�ж��Ƿ����ڱ��׶���Ҫ��������
        GameStatus status = _player == 0 ? GameStatus.MyAction : GameStatus.EnemyAction;
        if (gameStatus != status)
        {
            return;
        }

        bool hasMosterBlock = false;

        //ȷ��Ҫ��������ĸ�������
        targetBlocks = _player == 0 ? enemyBlocks : myBlocks;


        //�жϸ�������������û�п��ƶ���
        foreach (var block in targetBlocks)
        {
            if (block.GetComponent<Block>().card != null)
            {
                //�ȴ�������ʾ
                block.GetComponent<Block>().attackBlock.SetActive(true);
                //���ɹ���Ŀ��Ľű���
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
     ����ȷ�ϣ�����Ǵ�����
     */
    public void AttackComfirm(GameObject _target)
    {

        if (attackingMonster == null) return;//��ǰ��ֹ��

        Attack(attackingMonster, _target);

        //���ɹ���Ŀ��Ľű��ر�

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
     ����
     */
    public void Attack(GameObject _attacker, GameObject _target)
    {
        //��ȡ�����ߵĹ�����
        MonsterCard monsterCard = _attacker.GetComponent<DisplayCard>().Card as MonsterCard;
        //������������
        _target.GetComponent<AttackTarget>().ApplyDamage(monsterCard.attack);
        //���ٹ����ߵĹ�������
        _attacker.GetComponent<BattleCard>().CostAttackCount();


    }

    /*
    ����ȷ��,����Ǵ���ҵ�
    */
    public void AttackComfirm()
    {

    }
    #endregion

    /*
     ���غϽ�������������߳��ϵĿ��ƴ���ȫ������
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
