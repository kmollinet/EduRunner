using System;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine.UI;


public partial class QuestionSet
{
    [JsonProperty("Questions")]
    public List<QuestionDocument> Questions { get; set; }
    public int CurrentQuestionIndex { get; set; }


    // public Transform answer1Transform { get; set; }
    // public Transform answer2Transform { get; set; }
    // public Transform answer3Transform { get; set; }


    public void Next()
    {
        if (CurrentQuestionIndex + 1 >= Questions.Count)
        {
            CurrentQuestionIndex = 0;
        }
        else
        {
            CurrentQuestionIndex = CurrentQuestionIndex + 1;
        }
    }

        public void SetQuestion(string quesText, string ans1Text, string ans2Text, string ans3Text)
    {
        GameObject scroll = GameObject.FindWithTag ("scroll");
        if(scroll){
            Text scrollText = scroll.GetComponentInChildren<Text>();
            scrollText.text = quesText;
        }
        GameObject answer01 = GameObject.FindWithTag ("answer01");
        if(answer01){
            Text answer01Text = answer01.GetComponentInChildren<Text>();
            answer01Text.text = ans1Text;
        }
        GameObject answer02 = GameObject.FindWithTag ("answer02");
        if(answer02){
            Text answer02Text = answer02.GetComponentInChildren<Text>();
            answer02Text.text = ans2Text;
        }
        GameObject answer03 = GameObject.FindWithTag ("answer03");
        if(answer03){
            Text answer03Text = answer03.GetComponentInChildren<Text>();
            answer03Text.text = ans3Text;
        }

    }

    public void Update(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        // answer1Transform.position = v1;
        // answer2Transform.position = v2;
        // answer3Transform.position = v3;

    }

    public QuestionDocument CurrentQuestion
    {
        get => Questions[CurrentQuestionIndex];
    }


}

public partial class QuestionDocument
{
    [JsonProperty("question")]
    public string QuestionText { get; set; }

    [JsonProperty("answer")]
    public string AnswerText { get; set; }

    [JsonProperty("incorrectAnswers")]
    public List<string> IncorrectAnswers { get; set; }
}

public enum IncorrectAnswer { Apple, Book, Cat };

public partial class QuestionSet
{
    public static QuestionSet FromJson(string json) => JsonConvert.DeserializeObject<QuestionSet>(json, global::Converter.Settings);
    public static QuestionSet Init(string fileName)
    {
        string json = "";
        if (File.Exists(Application.streamingAssetsPath + "/" + fileName))
        {
            json = File.ReadAllText(Application.streamingAssetsPath + "/" + fileName);
        }
        QuestionSet qs = FromJson(json);

        // qs.answer2Transform = GameObject.FindGameObjectWithTag("answer2").transform;
        // qs.answer1Transform = GameObject.FindGameObjectWithTag("answer1").transform;
        // qs.answer3Transform = GameObject.FindGameObjectWithTag("answer3").transform;

        qs.CurrentQuestionIndex = 0;

        return qs;
    }
}

public static class Serialize
{
    public static string ToJson(this QuestionSet self) => JsonConvert.SerializeObject(self, global::Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
}
