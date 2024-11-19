using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Judgment : MonoBehaviour
{
    // �ֻ��� ���� �ʵ�
    [SerializeField] Sprite[] diceSides; // �ֻ��� �� �̹��� �迭
    [SerializeField] Image diceImage1; // ù ��° �ֻ��� �̹��� UI
    [SerializeField] Image diceImage2; // �� ���� �ֻ��� �̹��� UI
    private bool isRolling = false; // �ֻ����� �������� ������ ����

    public const int STATUS_X_MAX = 6; //����Xũ��
    public const int STATUS_Y_MAX = 9; //����Yũ��
    private int dice = 0; //���� �ֻ�����
    private int judgment_value; //��������ġ
    private JudgeResult judgeResult; //���� ���

    public JudgeResult result { get { return judgeResult; } } //���� ������� �ܺο� ����

    // ���� ��� ������
    public enum JudgeResult
    {
        Pumble,
        Fail,
        Success,
        Special
    }

    // ���� �̸��� ǥ�󿡼��� ��ġ�� ǥ���ϴ� ����ü
    public struct Status
    {
        public string name;
        public int x;
        public int y;

        public Status(string name, int x, int y) : this()
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }
    };

    //��� ������ ������ �迭
    private List<Status> statuses = new List<Status>();
    //Ȱ��ȭ �� ����
    private List<Status> availableStatuses = new List<Status>();
    //��Ȱ��ȭ �� ����
    private List<Status> disavaiableStatuses = new List<Status>();
    //���� ����
    private List<Status> havingStatuses = new List<Status>();

    public string LastJudgeStatName { get; private set; }

    // UI ��� �ʵ�
    [SerializeField] Text resultText; // ���� ����� UI ��� �ʵ�
    [SerializeField] Text judgmentText; // ���� (���� ���) UI ���

    // Start �޼���
    void Start()
    {
        // �ֻ��� �̹��� �ʱ�ȭ
        diceImage1 = diceImage1.GetComponent<Image>();
        diceImage2 = diceImage2.GetComponent<Image>();

        // ���ȸ�� ��ġ�� ���� ���Ͽ��� �о�鿩 ����Ʈ�� ����
        StreamReader sr = new StreamReader("Assets/Scripts/Judgment/stats.txt");
        while (sr.Peek() >= 0)
        {
            string line = sr.ReadLine();
            string[] lineSplit = line.Split(',');

            Status stat = new Status();
            stat.name = lineSplit[0];
            stat.x = int.Parse(lineSplit[1]);
            stat.y = int.Parse(lineSplit[2]);

            statuses.Add(stat);
        }
        sr.Close();

        // �׽�Ʈ �ڵ�
        havingStatuses.Add(SearchStatByName("���"));
        havingStatuses.Add(SearchStatByName("����"));
        havingStatuses.Add(SearchStatByName("���Լ�"));
        availableStatuses = havingStatuses.ToList();
    }


    //���� ���ȿ� �ش� ��ǥ�� ���� �߰�
    public void AddStat(int x, int y)
    {
        Status stat = GetStatus(x, y);
        havingStatuses.Add(stat);
    }

    //������ �̸����� ã�� ��ȯ
    public Status SearchStatByName(string name)
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            Status compare = statuses[i];
            if (compare.name == name)
            {
                return compare;
            }
        }

        return new Status("null", 0, 0);
    }
    // x, y ��ǥ�� ���� ã��
    public Status GetStatus(int x, int y)
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            Status stat = statuses[i];
            if (stat.x == x && stat.y == y)
            {
                return stat;
            }
        }

        return new Status("null", 0, 0);
    }

    // x, y ��ǥ�� ������ ã�� �̸� ��ȯ
    public string GetStatusName(int x, int y)
    {
        for (int i = 0; i < statuses.Count; i++)
        {
            Status stat = statuses[i];
            if (stat.x == x && stat.y == y)
            {
                return stat.name;
            }
        }

        return "null";
    }


    //����ġ ���, name: ������ ���� �̸� judgement_value�� ����
    void GetJudgeNum(string name)
    {
        int judgeNum = 5;
        Status judge_status = SearchStatByName(name);

        //���� ���ȿ� ���� ���
        if (availableStatuses.Contains(judge_status))
        {
            Debug.Log("GetJudgeNum: 5, Having Status");
            judgment_value = judgeNum;
            return;
        }
        //���� ��� ����ġ ���
        else
        {
            Status minDistStatus = statuses[0];
            int minDist = 999;

            //ǥ�ּ� ���� ����� ������ �Ÿ��� ���� �⺻ġ�� ���Ѵ�..
            for (int i = 0; i < availableStatuses.Count; i++)
            {
                Status cmp = (Status)availableStatuses[i];
                int dist = Mathf.Abs(cmp.x - judge_status.x) + Mathf.Abs(cmp.y - judge_status.y);
                if (dist < minDist)
                {
                    minDist = dist;
                }
            }

            Debug.Log(string.Format("GetJudgeNum: +{0} not Having Status", minDist));

            judgment_value = judgeNum + minDist;
        }
    }


    // �ֻ��� ������ �޼��� (�� ���� �ֻ���)
    public void RollDice()
    {
        if (!isRolling)
        {
            isRolling = true;
            StartCoroutine(RollTheDice());
        }
    }

    //ȭ�鿡 �ֻ����� �������� ����Ʈ�� ǥ���Ѵ�.
    private IEnumerator RollTheDice()
    {   
        int randomDiceSide1 = 0;
        int randomDiceSide2 = 0;

        // �ֻ����� 20�� ���� �������� ����
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide1 = Random.Range(0, 6);
            randomDiceSide2 = Random.Range(0, 6);
            diceImage1.sprite = diceSides[randomDiceSide1]; // ù ��° �ֻ��� ��
            diceImage2.sprite = diceSides[randomDiceSide2]; // �� ��° �ֻ��� ��
            yield return new WaitForSeconds(0.06f);
        }

        // �� �ֻ��� �� �ջ��Ͽ� ��� ó��
        dice = randomDiceSide1 + randomDiceSide2 + 2; // +2�� �ֻ��� ���� 1���� �����ϹǷ� ����
        resultText.text = "�ֻ��� ��: " + dice.ToString();

        isRolling = false;
        Judge("���");    
    }

    // UI ��ư�� Ŭ�� �� ȣ���� �޼���
    public void OnRollDiceButtonClicked()
    {
        RollDice(); // �ֻ��� ������ �޼��带 ȣ��
    }

    // ���� ���ȿ� ���� �ٽ� ����
    public void ReJudge()
    {
        RollDice();
    }


    // Ư�� ���ȿ� ���� ���� ����� ����ϰ� ����� judgeResult�� ����
    void Judge(string name)
    {
        LastJudgeStatName = name;
        GetJudgeNum(name); //judgment_value �� ����

        if (dice <= 2)
        {
            judgmentText.text = "�ߺ�!!!";
            judgeResult = JudgeResult.Pumble;
            return;
        }
        else if (dice >= 12)
        {
            judgmentText.text = "�����!!!";
            judgeResult = JudgeResult.Special;
            return;
        }
        else if (dice >= judgment_value)
        {
            judgmentText.text = "����!!!!";
            judgeResult = JudgeResult.Success;
            return;
        }

        judgmentText.text = "����!!!";
        judgeResult = JudgeResult.Fail;
        return;
    }

    //ü�� ���� ���� ���� ��Ȱ��ȭ
    public void DisableStat(HealthStat stat)
    {
        for (int i = availableStatuses.Count - 1; i >= 0; i--)
        {
            if (availableStatuses[i].x == (int)stat)
            {
                Debug.Log("����:" + availableStatuses[i].name);
                disavaiableStatuses.Add(availableStatuses[i]);
                availableStatuses.RemoveAt(i);
            }
        }
    }
    //���� ȸ��
    public void EnableStat(HealthStat stat)
    {
        for (int i = disavaiableStatuses.Count - 1; i >= 0; i--)
        {
            if (disavaiableStatuses[i].x == (int)stat)
            {
                Debug.Log("����:" + disavaiableStatuses[i].name);
                availableStatuses.Add(disavaiableStatuses[i]);
                disavaiableStatuses.RemoveAt(i);
            }
        }
    }

    //Ȱ��ȭ�� ������ ���� �������� �ʱ�ȭ �Ѵ�.
    public void ResetStat()
    {
        availableStatuses = havingStatuses.ToList();
    }


    // ��ü ���� ���
    //���� ���� ���
    public void PrintStat()
    {
        Debug.Log("��ü ����:");
        for (int i = 0; i < havingStatuses.Count; i++)
        {
            Debug.Log(havingStatuses[i].name);
        }

        Debug.Log("������ ����");
        for (int i = 0; i < availableStatuses.Count; i++)
        {
            Debug.Log(availableStatuses[i].name);
        }
    }

    // Update �޼���
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Judge("���");
            Judge("���������");
            Judge("����Ȱ��");
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            PrintStat();
        }
    }
}
