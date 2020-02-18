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


    public Transform answer1Transform { get; set; }
    public Transform answer2Transform { get; set; }
    public Transform answer3Transform { get; set; }


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

    public void Update(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        answer1Transform.position = v1;
        answer2Transform.position = v2;
        answer3Transform.position = v3;

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

        qs.answer2Transform = GameObject.FindGameObjectWithTag("answer2").transform;
        qs.answer1Transform = GameObject.FindGameObjectWithTag("answer1").transform;
        qs.answer3Transform = GameObject.FindGameObjectWithTag("answer3").transform;

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
