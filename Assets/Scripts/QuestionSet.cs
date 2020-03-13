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
    [JsonProperty("questions")]
    public List<QuestionDocument> Questions { get; set; }
    public int CurrentQuestionIndex { get; set; }

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

    public void SetQuestion()
    {
        int randomAnsIndex1 = -1;
        int randomAnsIndex2 = -1;
        int randomAnsIndex3 = -1;
        int randomPositionIndex = -1;


        randomAnsIndex1 = UnityEngine.Random.Range(0, Questions.Count - 1);
        randomAnsIndex2 = UnityEngine.Random.Range(0, Questions.Count - 1);
        randomAnsIndex3 = UnityEngine.Random.Range(0, Questions.Count - 1);

        while(randomAnsIndex2 == randomAnsIndex1){  //Get a new number if you got the exact same one as the answer
            randomAnsIndex2 = UnityEngine.Random.Range(0, Questions.Count - 1);
        }
        while(randomAnsIndex3 == randomAnsIndex2 && randomAnsIndex3 == randomAnsIndex1){
            randomAnsIndex3 = UnityEngine.Random.Range(0, Questions.Count - 1);
        }


        GameObject scroll = GameObject.FindWithTag ("scroll");
        if(scroll){
            Text scrollText = scroll.GetComponentInChildren<Text>();
            scrollText.text = Questions[randomAnsIndex1].questionText;
        }

        randomPositionIndex = UnityEngine.Random.Range(1, 4); // pick a random position for the answer to be. Where 1 <= number < 4
        Debug.Log(randomPositionIndex);
        switch (randomPositionIndex)
        {
            case 1:
                applyTextToBanners(randomAnsIndex1, randomAnsIndex3, randomAnsIndex2);
                break;
            case 2:
                applyTextToBanners(randomAnsIndex2, randomAnsIndex1, randomAnsIndex3);
                break;
            default:
                applyTextToBanners(randomAnsIndex3, randomAnsIndex2, randomAnsIndex1);
                break;
        }
    }

    private void applyTextToBanners(int rand1, int rand2, int rand3){
        GameObject answer01 = GameObject.FindWithTag ("answer01");
        if(answer01){
            Text answer01Text = answer01.GetComponentInChildren<Text>();
            answer01Text.text = Questions[rand1].answer;
        }
        GameObject answer02 = GameObject.FindWithTag ("answer02");
        if(answer02){
            Text answer02Text = answer02.GetComponentInChildren<Text>();
            answer02Text.text = Questions[rand2].answer;
        }
        GameObject answer03 = GameObject.FindWithTag ("answer03");
        if(answer03){
            Text answer03Text = answer03.GetComponentInChildren<Text>();
            answer03Text.text = Questions[rand3].answer;
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
    [JsonProperty("questionText")]
    public string questionText { get; set; }

    [JsonProperty("answer")]
    public string answer { get; set; }
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
