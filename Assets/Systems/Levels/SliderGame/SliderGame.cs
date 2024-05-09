using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderGame : MonoBehaviour
{
    public Animator anim;
    public int FamilyPts { get; set; }
    public int HealthPts { get; set; }
    public int MoneyPts { get; set; }
    public int PrestigePts { get; set; }

    //Questions
    [TextArea]
    public string Text;
    public bool Interpret;
    [SerializeField] bool extractJSON;

    [System.Serializable]
    public class question
    {
        [TextArea()] public string Title;
        [System.Serializable]
        public class answer
        {
            [TextArea()]
            public string Reply;
            [Space]
            public int FamilyPts;
            public int HealthPts;
            public int MoneyPts;
            public int PrestigePts;
        }
        [SerializeField] public answer ReplyA;
        [SerializeField] public answer ReplyB;
        [SerializeField] public answer ReplyC;
    }
    [Space(15)] public List<question> Questions = new List<question>();
    public int currentQuestionIndex { get; set; }

    //UI
    public TextMeshProUGUI LABEL_QUESTION;
    [Space]
    public TextMeshProUGUI LABEL_REPLYA;
    public TextMeshProUGUI LABEL_REPLYB;
    public TextMeshProUGUI LABEL_REPLYC;
    [Space]
    public SliderGame_Tube FamilyTube;
    public SliderGame_Tube HealthTube;
    public SliderGame_Tube MoneyTube;
    public SliderGame_Tube PrestigeTube;
    [Space]
    public GameObject GameOver;
    public TextMeshProUGUI FinalScore;
    public SkinnedMeshRenderer StatusCircle;
    public TextMeshProUGUI HealthScoreLabel;
    public TextMeshProUGUI FamilyScoreLabel;
    public TextMeshProUGUI MoneyScoreLabel;
    public TextMeshProUGUI PrestigeScoreLabel;

    public void UpdateLabels()
    {
        //↑↓-
        //💕➕💲🥇


        if (currentQuestionIndex >= Questions.Count)
        {
            OnGameOver();
        }
        else
        {

            string q1 = "";
            string q2 = "";
            string q3 = "";
            string question = (currentQuestionIndex + 1) + "/" + Questions.Count + " - ";

            /*
            if (Questions[currentQuestionIndex].ReplyA.FamilyPts > 0) { q1 += "<color=green>🏡</color> "; } else if (Questions[currentQuestionIndex].ReplyA.FamilyPts < 0) { q1 += "<color=red>🏡</color> "; } else { q1 += "<color=#50505050>🏡</color> "; }
            if (Questions[currentQuestionIndex].ReplyA.HealthPts > 0) { q1 += "<color=green>❤</color> "; } else if (Questions[currentQuestionIndex].ReplyA.HealthPts < 0) { q1 += "<color=red>❤</color> "; } else { q1 += "<color=#50505050>❤</color> "; }
            if (Questions[currentQuestionIndex].ReplyA.MoneyPts > 0) { q1 += "<color=green>💲</color> "; } else if (Questions[currentQuestionIndex].ReplyA.MoneyPts < 0) { q1 += "<color=red>💲</color> "; } else { q1 += "<color=#50505050>💲</color> "; }
            if (Questions[currentQuestionIndex].ReplyA.PrestigePts > 0) { q1 += "<color=green>🥇</color> "; } else if (Questions[currentQuestionIndex].ReplyA.PrestigePts < 0) { q1 += "<color=red>🥇</color> "; } else { q1 += "<color=#50505050>🥇</color> "; }

            if (Questions[currentQuestionIndex].ReplyB.FamilyPts > 0) { q2 += "<color=green>🏡</color> "; } else if (Questions[currentQuestionIndex].ReplyB.FamilyPts < 0) { q2 += "<color=red>🏡</color> "; } else { q2 += "<color=#50505050>🏡</color> "; }
            if (Questions[currentQuestionIndex].ReplyB.HealthPts > 0) { q2 += "<color=green>❤</color> "; } else if (Questions[currentQuestionIndex].ReplyB.HealthPts < 0) { q2 += "<color=red>❤</color> "; } else { q2 += "<color=#50505050>❤</color> "; }
            if (Questions[currentQuestionIndex].ReplyB.MoneyPts > 0) { q2 += "<color=green>💲</color> "; } else if (Questions[currentQuestionIndex].ReplyB.MoneyPts < 0) { q2 += "<color=red>💲</color> "; } else { q2 += "<color=#50505050>💲</color> "; }
            if (Questions[currentQuestionIndex].ReplyB.PrestigePts > 0) { q2 += "<color=green>🥇</color> "; } else if (Questions[currentQuestionIndex].ReplyB.PrestigePts < 0) { q2 += "<color=red>🥇</color> "; } else { q2 += "<color=#50505050>🥇</color> "; }

            if (Questions[currentQuestionIndex].ReplyC.FamilyPts > 0) { q3 += "<color=green>🏡</color> "; } else if (Questions[currentQuestionIndex].ReplyC.FamilyPts < 0) { q3 += "<color=red>🏡</color> "; } else { q3 += "<color=#50505050>🏡</color> "; }
            if (Questions[currentQuestionIndex].ReplyC.HealthPts > 0) { q3 += "<color=green>❤</color> "; } else if (Questions[currentQuestionIndex].ReplyC.HealthPts < 0) { q3 += "<color=red>❤</color> "; } else { q3 += "<color=#50505050>❤</color> "; }
            if (Questions[currentQuestionIndex].ReplyC.MoneyPts > 0) { q3 += "<color=green>💲</color> "; } else if (Questions[currentQuestionIndex].ReplyC.MoneyPts < 0) { q3 += "<color=red>💲</color> "; } else { q3 += "<color=#50505050>💲</color> "; }
            if (Questions[currentQuestionIndex].ReplyC.PrestigePts > 0) { q3 += "<color=green>🥇</color> "; } else if (Questions[currentQuestionIndex].ReplyC.PrestigePts < 0) { q3 += "<color=red>🥇</color> "; } else { q3 += "<color=#50505050>🥇</color> "; }
            */

            LABEL_QUESTION.text = question + Questions[currentQuestionIndex].Title;
            LABEL_REPLYA.text = $"{Questions[currentQuestionIndex].ReplyA.Reply}";
            LABEL_REPLYB.text = $"{Questions[currentQuestionIndex].ReplyB.Reply}";
            LABEL_REPLYC.text = $"{Questions[currentQuestionIndex].ReplyC.Reply}";
        }

        LABEL_REPLYA.GetComponentInParent<Button>().gameObject.SetActive(!string.IsNullOrEmpty(Questions[currentQuestionIndex].ReplyA.Reply));
        LABEL_REPLYB.GetComponentInParent<Button>().gameObject.SetActive(!string.IsNullOrEmpty(Questions[currentQuestionIndex].ReplyB.Reply));
        LABEL_REPLYC.GetComponentInParent<Button>().gameObject.SetActive(!string.IsNullOrEmpty(Questions[currentQuestionIndex].ReplyC.Reply));

        FinalScore.text = "Saúde: " + HealthPts + "\n";
        FinalScore.text += "Família: " + FamilyPts + "\n";
        FinalScore.text += "Dinheiro: " + MoneyPts + "\n";
        FinalScore.text += "Prestígio: " + PrestigePts;

        HealthScoreLabel.text = "Saúde: " + HealthPts;
        FamilyScoreLabel.text = "Família: " + FamilyPts;
        MoneyScoreLabel.text = "Dinheiro: " + MoneyPts;
        PrestigeScoreLabel.text = "Prestígio: " + PrestigePts;

        StatusCircle.SetBlendShapeWeight(1, HealthPts);
        StatusCircle.SetBlendShapeWeight(2, FamilyPts);
        StatusCircle.SetBlendShapeWeight(3, MoneyPts);
        StatusCircle.SetBlendShapeWeight(4, PrestigePts);
    }

    public void OnGameOver()
    {
        FamilyTube.SetValue(0);
        HealthTube.SetValue(0);
        MoneyTube.SetValue(0);
        PrestigeTube.SetValue(0);
        anim.SetBool("GameOver", true);
        GameOver.gameObject.SetActive(true);
    }

    //Gamplay Mechanics
    public void SelectQuestion(int index)
    {
        currentQuestionIndex = index;
        UpdateLabels();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">0 = a, 1 = b, 2 = c</param>
    public void SelectReply(int index)
    {
        Debug.Log($"[SliderGame] Answer selected {index}");
        question q = Questions[currentQuestionIndex];
        question.answer answer = null;

        if (index == 0) { answer = q.ReplyA; }
        if (index == 1) { answer = q.ReplyB; }
        if (index == 2) { answer = q.ReplyC; }

        Debug.Log($"[SliderGame] Reply selected {index}, {JsonUtility.ToJson(answer)}");

        FamilyPts = Mathf.Clamp(FamilyPts + answer.FamilyPts, 0, 100);
        HealthPts = Mathf.Clamp(HealthPts + answer.HealthPts, 0, 100);
        MoneyPts = Mathf.Clamp(MoneyPts + answer.MoneyPts, 0, 100);
        PrestigePts = Mathf.Clamp(PrestigePts + answer.PrestigePts, 0, 100);


        FamilyTube.SetValue((FamilyPts / 100.0f));
        HealthTube.SetValue(HealthPts / 100.0f);
        MoneyTube.SetValue(MoneyPts / 100.0f);
        PrestigeTube.SetValue(PrestigePts / 100.0f);

        currentQuestionIndex += 1;
        //currentQuestionIndex = (int)Mathf.Repeat(currentQuestionIndex, Questions.Count);
        SelectQuestion(currentQuestionIndex);
    }
    public void ShuffleQuestions()
    {
        Questions.Shuffle();
    }
    public void Play()
    {
        gameObject.SetActive(true);
        GameOver.gameObject.SetActive(false);
        anim.SetBool("GameOver", false);
        CareOnLevel.main.anim.SetBool("Sliders", true);
        ShuffleQuestions();
        SelectQuestion(0);
        UpdateLabels();
    }

    public void Stop()
    {
        FamilyPts = 0;
        HealthPts = 0;
        MoneyPts = 0;
        PrestigePts = 0;


        GameOver.gameObject.SetActive(false);
        anim.SetBool("GameOver", false);
        CareOnLevel.main.anim.SetBool("Sliders", false);
        gameObject.SetActive(false);
    }


    //JSON Interpreter
    SliderGame tmp;
    public void ExtractJSON()
    {
        Text = JsonUtility.ToJson(this, true);
    }

    //Mono
    private void OnEnable()
    {
        Play();
    }
    private void OnValidate()
    {
        if (Interpret)
        {
            SliderGame g = new GameObject("tmp").AddComponent<SliderGame>();
            JsonUtility.FromJsonOverwrite(Text, g);
            tmp = g;
            Questions = g.Questions;
            DestroyImmediate(tmp.gameObject);
            Interpret = false;
        }

        if (extractJSON)
        {
            ExtractJSON();
            extractJSON = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (tmp == null)
            return;
        DestroyImmediate(tmp.gameObject);
    }
}
