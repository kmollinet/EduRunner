// Notes on this file: 
// QuestionSet.EnsureData(); // Make sure this gets called before trying to access data, one call should be enough so if you call it in start() you shouldn't need to call it again
// QuestionSet.GetAvailableQuizzes(); // use this to get the Id and name of all available quizzes
// QuestionSet.GetById("b273fab4-6ee1-46f9-91ca-2251c7c4788a"); // use this to get a specific quiz by ID (id may come from the above example)
// QuestionSet.Get(); // this just gets the selected quiz (defaults to 1st in list)
// // so, an example of how this could work with the quiz selection menu, inside an onclick function that takes the Id as a parameter
// string id = "b273fab4-6ee1-46f9-91ca-2251c7c4788a"; //this val should come from the onclick function somehow
// QuestionSet.SetById(id); // doesn't return anything, just sets the static QuestionSet to the quiez with the passed id
//                          // so then in another file, (ie playermotor) you can access the selected quiz like this
// Debug.Log(QuestionSet.Get().Id); // quiz matches quiz with id used in above function



using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using AsyncQuizLoader;
using System.Threading.Tasks;
public partial class QuestionSet
{
    private List<int> usedQuestionIndices = new List<int>();
    private static QuizLoader quizLoader = new QuizLoader();
    private static string defaultQuiz = "{\"description\":\"50 states & their capitals\",\"id\":\"b04b2166-9615-45fb-b24f-072f20876f2e\",\"name\":\"50 States & Capitals\",\"ordered\":false,\"ownerId\":\"tylergilland@gmail.com\",\"questions\":[{\"answer\":\"Montgomery\",\"difficulty\":2,\"id\":\"a9ea57f0-7ac8-4ea3-a644-31b8b138b995\",\"points\":2,\"questionText\":\"What is the capital of Alabama?\"},{\"answer\":\"Juneau\",\"difficulty\":2,\"id\":\"5e7b39c9-173d-4e44-bede-8d5532f5be1c\",\"points\":2,\"questionText\":\"What is the capital of Alaska?\"},{\"answer\":\"Phoenix\",\"difficulty\":2,\"id\":\"f5a55cd1-5da9-480a-96d5-c5a8347683e4\",\"points\":2,\"questionText\":\"What is the capital of Arizona?\"},{\"answer\":\"Little Rock\",\"difficulty\":2,\"id\":\"e671609b-be89-400d-a874-e5b97e1c021a\",\"points\":2,\"questionText\":\"What is the capital of Arkansas?\"},{\"answer\":\"Sacramento\",\"difficulty\":2,\"id\":\"059ffa50-5b0a-4699-a227-ad2b11d61bab\",\"points\":2,\"questionText\":\"What is the capital of California?\"},{\"answer\":\"Denver\",\"difficulty\":2,\"id\":\"35835538-aa8e-4a33-913d-9a836b876213\",\"points\":2,\"questionText\":\"What is the capital of Colorado?\"},{\"answer\":\"Hartford\",\"difficulty\":2,\"id\":\"c6859f17-02f0-40e4-aa2c-5babcef727b3\",\"points\":2,\"questionText\":\"What is the capital of Connecticut?\"},{\"answer\":\"Dover\",\"difficulty\":2,\"id\":\"7bed0305-d7bd-44a2-9610-70c84642acf6\",\"points\":2,\"questionText\":\"What is the capital of Delaware?\"},{\"answer\":\"Tallahassee\",\"difficulty\":2,\"id\":\"71b8e3e0-d538-4dd7-ab50-bbfe7ec8cd2c\",\"points\":2,\"questionText\":\"What is the capital of Florida?\"},{\"answer\":\"Atlanta\",\"difficulty\":2,\"id\":\"173b45de-a3be-40aa-b6a7-7a732ef1a552\",\"points\":2,\"questionText\":\"What is the capital of Georgia?\"},{\"answer\":\"Honolulu\",\"difficulty\":2,\"id\":\"3d9ea320-04ab-4344-9ee5-797d49ed6358\",\"points\":2,\"questionText\":\"What is the capital of Hawaii?\"},{\"answer\":\"Boise\",\"difficulty\":2,\"id\":\"480e690b-532a-4cfa-925e-e7a81993316d\",\"points\":2,\"questionText\":\"What is the capital of Idaho?\"},{\"answer\":\"Springfield\",\"difficulty\":2,\"id\":\"1a19afda-a39b-440b-9143-3072ecab0b94\",\"points\":2,\"questionText\":\"What is the capital of Illinois?\"},{\"answer\":\"Indianapolis\",\"difficulty\":2,\"id\":\"4fd7cd54-1ebb-45f8-ad09-f64fd3c6d644\",\"points\":2,\"questionText\":\"What is the capital of Indiana?\"},{\"answer\":\"Des Moines\",\"difficulty\":2,\"id\":\"908c7b7d-4bd0-42b1-9b6f-aa709158e0e9\",\"points\":2,\"questionText\":\"What is the capital of Iowa?\"},{\"answer\":\"Topeka\",\"difficulty\":2,\"id\":\"b4e97277-38e3-470d-a08d-d40a500bc05e\",\"points\":2,\"questionText\":\"What is the capital of Kansas?\"},{\"answer\":\"Frankfort\",\"difficulty\":2,\"id\":\"d5ed4ea7-45e8-4d7f-95a2-237b88ca7001\",\"points\":2,\"questionText\":\"What is the capital of Kentucky?\"},{\"answer\":\"Baton Rouge\",\"difficulty\":2,\"id\":\"d23708c6-422a-4c42-a054-ffc2d5c0343a\",\"points\":2,\"questionText\":\"What is the capital of Louisiana?\"},{\"answer\":\"Augusta\",\"difficulty\":2,\"id\":\"928f9e81-7c15-4aea-acf0-9095547ab49a\",\"points\":2,\"questionText\":\"What is the capital of Maine?\"},{\"answer\":\"Annapolis\",\"difficulty\":2,\"id\":\"5da98313-7544-4131-9d8d-a69b09a12e34\",\"points\":2,\"questionText\":\"What is the capital of Maryland?\"},{\"answer\":\"Boston\",\"difficulty\":2,\"id\":\"cdb4d24c-a214-456f-a58d-45e7672fb016\",\"points\":2,\"questionText\":\"What is the capital of Massachusetts?\"},{\"answer\":\"Lansing\",\"difficulty\":2,\"id\":\"cbe9e0d9-7bc4-43f1-81e3-2c35772f01bb\",\"points\":2,\"questionText\":\"What is the capital of Michigan?\"},{\"answer\":\"St. Paul\",\"difficulty\":2,\"id\":\"afacbbec-f7c5-45c5-8650-1aa46f62e9c9\",\"points\":2,\"questionText\":\"What is the capital of Minnesota?\"},{\"answer\":\"Jackson\",\"difficulty\":2,\"id\":\"d58dbafd-7b4d-42d7-90cc-92c9c507e391\",\"points\":2,\"questionText\":\"What is the capital of Mississippi?\"},{\"answer\":\"Jefferson City\",\"difficulty\":2,\"id\":\"dbb6243a-27d5-4d98-b504-bfc07b65e0d5\",\"points\":2,\"questionText\":\"What is the capital of Missouri?\"},{\"answer\":\"Helena\",\"difficulty\":2,\"id\":\"935b3c6b-d9ec-4472-ba7f-6da26389e17e\",\"points\":2,\"questionText\":\"What is the capital of Montana?\"},{\"answer\":\"Lincoln\",\"difficulty\":2,\"id\":\"2a8e90cc-67fe-487c-9530-ea4495dd8a93\",\"points\":2,\"questionText\":\"What is the capital of Nebraska?\"},{\"answer\":\"Carson City\",\"difficulty\":2,\"id\":\"d618b96e-f43f-4562-8a3f-4adf85f9ff05\",\"points\":2,\"questionText\":\"What is the capital of Nevada?\"},{\"answer\":\"Hampshire\",\"difficulty\":2,\"id\":\"2d00a9ff-d73f-4478-a104-df645eb0c28d\",\"points\":2,\"questionText\":\"What is the capital of New Hampshire?\"},{\"answer\":\"Trenton\",\"difficulty\":2,\"id\":\"fd6ac173-691d-423f-8ef9-9f8c9a976f54\",\"points\":2,\"questionText\":\"What is the capital of New Jersey?\"},{\"answer\":\"Santa Fe\",\"difficulty\":2,\"id\":\"c1d1d87d-8788-4551-9064-deac57a68c0b\",\"points\":2,\"questionText\":\"What is the capital of New Mexico?\"},{\"answer\":\"Albany\",\"difficulty\":2,\"id\":\"8b6ed11a-e388-4712-954a-851d40cd7d90\",\"points\":2,\"questionText\":\"What is the capital of New York?\"},{\"answer\":\"Raleigh\",\"difficulty\":2,\"id\":\"51529915-7b31-41f0-a981-6d501334631a\",\"points\":2,\"questionText\":\"What is the capital of North Carolina?\"},{\"answer\":\"Bismarck\",\"difficulty\":2,\"id\":\"fd8c3d10-3ace-439f-b827-00e3a2458454\",\"points\":2,\"questionText\":\"What is the capital of North Dakota?\"},{\"answer\":\"Columbus\",\"difficulty\":2,\"id\":\"88a3e0b2-aed7-47dd-9045-3ee651b834bb\",\"points\":2,\"questionText\":\"What is the capital of Ohio?\"},{\"answer\":\"Oklahoma City\",\"difficulty\":2,\"id\":\"9b0268f2-52c2-4bbe-97ad-16fa968a3eb9\",\"points\":2,\"questionText\":\"What is the capital of Oklahoma?\"},{\"answer\":\"Salem\",\"difficulty\":2,\"id\":\"23282698-7642-4889-ab39-d0a04a6d00e4\",\"points\":2,\"questionText\":\"What is the capital of Oregon?\"},{\"answer\":\"Harrisburg\",\"difficulty\":2,\"id\":\"0407f210-ab3d-4cee-9db4-02a5c41d28f5\",\"points\":2,\"questionText\":\"What is the capital of Pennsylvania?\"},{\"answer\":\"Providence\",\"difficulty\":2,\"id\":\"9da1fdcf-80f3-456e-b6f0-efb5a4aad713\",\"points\":2,\"questionText\":\"What is the capital of Rhode Island?\"},{\"answer\":\"Columbia\",\"difficulty\":2,\"id\":\"e1bc4516-7857-45da-a0b1-b540b20dca54\",\"points\":2,\"questionText\":\"What is the capital of South Carolina?\"},{\"answer\":\"Pierre\",\"difficulty\":2,\"id\":\"a1fe1b87-2005-48da-91a7-a9f3d491ffd2\",\"points\":2,\"questionText\":\"What is the capital of South Dakota?\"},{\"answer\":\"Nashville\",\"difficulty\":2,\"id\":\"4be8d5f3-9b0e-45cf-80c2-14939241bfd6\",\"points\":2,\"questionText\":\"What is the capital of Tennessee?\"},{\"answer\":\"Austin\",\"difficulty\":2,\"id\":\"c0bdd24b-b7be-4186-82af-b0e036ae523c\",\"points\":2,\"questionText\":\"What is the capital of Texas?\"},{\"answer\":\"Salt Lake City\",\"difficulty\":2,\"id\":\"4ea8e84c-5ce2-4c13-a1e5-744d1a83a99a\",\"points\":2,\"questionText\":\"What is the capital of Utah?\"},{\"answer\":\"Montpelier\",\"difficulty\":2,\"id\":\"da42e128-0a8a-4eba-9d49-20d4f755cd10\",\"points\":2,\"questionText\":\"What is the capital of Vermont?\"},{\"answer\":\"Richmond\",\"difficulty\":2,\"id\":\"7ffc3f44-a1f7-4708-94ad-a99c120865ae\",\"points\":2,\"questionText\":\"What is the capital of Virginia?\"},{\"answer\":\"Olympia\",\"difficulty\":2,\"id\":\"3d838f40-5347-4378-84be-f50a9e93e1f1\",\"points\":2,\"questionText\":\"What is the capital of Washington?\"},{\"answer\":\"Charleston\",\"difficulty\":2,\"id\":\"cc765fb4-90fa-4136-aae1-dbbd92372a2b\",\"points\":2,\"questionText\":\"What is the capital of West Virginia?\"},{\"answer\":\"Madison\",\"difficulty\":2,\"id\":\"3a0294fe-f833-4f89-b4e3-ecb17b02dc96\",\"points\":2,\"questionText\":\"What is the capital of Wisconsin?\"},{\"answer\":\"Cheyenne\",\"difficulty\":2,\"id\":\"a17344f8-9548-4398-a238-8c7682260aed\",\"points\":2,\"questionText\":\"What is the capital of Wyoming?\"}],\"questionsPerQuiz\":15,\"subject\":\"geography\",\"updatedAt\":\"2020-03-19T02:29:01.813Z\",\"visibility\":\"PUBLIC\"}";

    private static QuestionSet qs = QuestionSet.FromJson (QuestionSet.defaultQuiz); 

    public static bool initialized = false;
    public int CurrentQuestionIndex { get; set; }
    [JsonProperty("Id")]
    public string Id { get; set; }
    [JsonProperty("questions")]
    public List<QuestionDocument> Questions { get; set; }
    [JsonProperty ("questionsPerQuiz")]
    public double QuestionsPerQuiz { get; set; }


    public void Next()
    {
        Debug.Log("Beginning of Next() method in QuestionSet.cs file");
        if (CurrentQuestionIndex + 1 >= Questions.Count)
        {
            CurrentQuestionIndex = 0;
        }
        else
        {
            CurrentQuestionIndex = CurrentQuestionIndex + 1;
        }
    }
    public int SetQuestion()
    {
        Debug.Log("Beginning of SetQuestion() method in QuestionSet.cs file");
        int randomAnsIndex1 = -1;
        int randomAnsIndex2 = -1;
        int randomAnsIndex3 = -1;
        int randomPositionIndex = -1;
        randomAnsIndex1 = UnityEngine.Random.Range(0, Questions.Count);
        while (usedQuestionIndices.Contains(randomAnsIndex1))
        { // make sure this question hasn't been used before
            randomAnsIndex1 = UnityEngine.Random.Range(0, Questions.Count);
        }
        randomAnsIndex2 = UnityEngine.Random.Range(0, Questions.Count);
        randomAnsIndex3 = UnityEngine.Random.Range(0, Questions.Count);
        while (randomAnsIndex2 == randomAnsIndex1 || randomAnsIndex2 == randomAnsIndex3)
        {  //Get a new number if you got the exact same one as the answer
            randomAnsIndex2 = UnityEngine.Random.Range(0, Questions.Count);
        }
        while (randomAnsIndex3 == randomAnsIndex2 || randomAnsIndex3 == randomAnsIndex1)
        {
            randomAnsIndex3 = UnityEngine.Random.Range(0, Questions.Count);
        }
        GameObject scroll = GameObject.FindWithTag("scroll");
        if (scroll)
        {
            Text scrollText = scroll.GetComponentInChildren<Text>();
            scrollText.text = Questions[randomAnsIndex1].questionText;
        }
        usedQuestionIndices.Add(randomAnsIndex1); // Add the random index to the usedQuestionsIndices array to avoid repeating questions for kids during the same game
        randomPositionIndex = UnityEngine.Random.Range(1, 4); // pick a random position for the answer to be. Where 1 <= number < 4
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
        return randomPositionIndex;
    }
    private void applyTextToBanners(int rand1, int rand2, int rand3)
    {
        Debug.Log("Beginning of applyTextToBanners() method in QuestionSet.cs file");
        GameObject answer01 = GameObject.FindWithTag("answer01");
        if (answer01)
        {
            Text answer01Text = answer01.GetComponentInChildren<Text>();
            answer01Text.text = Questions[rand1].answer;
        }
        GameObject answer02 = GameObject.FindWithTag("answer02");
        if (answer02)
        {
            Text answer02Text = answer02.GetComponentInChildren<Text>();
            answer02Text.text = Questions[rand2].answer;
        }
        GameObject answer03 = GameObject.FindWithTag("answer03");
        if (answer03)
        {
            Text answer03Text = answer03.GetComponentInChildren<Text>();
            answer03Text.text = Questions[rand3].answer;
        }
    }

    void Start()
    {
        try {Debug.Log(defaultQuiz);}
        catch (Exception e) {Debug.Log(e);}
        try {Debug.Log(qs);}
        catch (Exception e) {Debug.Log(e);}
        Debug.Log("Beginning of Start() method in QuestionSet.cs file");
    }
    public void Update()
    {
    }
    public QuestionDocument CurrentQuestion
    {
        get => Questions[CurrentQuestionIndex];
    }
    public static async void Init()
    {
        Debug.Log("Beginning of Init() method in QuestionSet.cs file");
        EduRunner.Quiz[] quizzes = await QuestionSet.quizLoader.GetAllQuizzes();
        try {Debug.Log(quizzes);}
        catch (Exception e) {Debug.Log(e);}
        QuestionSet.quizzes = quizzes.ToDictionary(q => q.Id);
        try {Debug.Log(QuestionSet.quizzes);}
        catch (Exception e) {Debug.Log(e);}
        QuestionSet.qs = QuestionSet.FromJson(JsonConvert.SerializeObject(quizzes[0]));
        try {Debug.Log(QuestionSet.qs);}
        catch (Exception e) {Debug.Log(e);}
        QuestionSet.initialized = true;
        try {Debug.Log(QuestionSet.initialized);}
        catch (Exception e) {Debug.Log(e);}
        Debug.Log("end of init() method in QuestionSet.cs file. Should have just logged a bunch of things");
    }
    public static Dictionary<string, EduRunner.Quiz> quizzes;
    public static void EnsureData()
    {
        Debug.Log("Beginning of EnsureData() method in QuestionSet.cs file");
        if (initialized == false)
        {
            QuestionSet.Init();
            Debug.Log("Fetching Quizlist");
            // while (QuestionSet.initialized == false)
            // {
            //     Debug.Log("while");
            // }
            return;
        }
        else
        {
            return;
        }
    }
    public static QuestionSet Get()
    {
        Debug.Log("Beginning of Get() method in QuestionSet.cs file");
        if (QuestionSet.qs == null)
        {
            QuestionSet.qs = QuestionSet.FromJson(JsonConvert.SerializeObject(QuestionSet.quizzes.First()));
        }
        try {Debug.Log(QuestionSet.qs);}
        catch (Exception e) {Debug.Log(e);}
        return QuestionSet.qs;
    }
    public static QuestionSet GetById(string key)
    {
        Debug.Log("Beginning of GetById() method in QuestionSet.cs file");
        EduRunner.Quiz quiz;
        QuestionSet.quizzes.TryGetValue(key, out quiz);
        QuestionSet.qs = QuestionSet.FromJson(JsonConvert.SerializeObject(quiz));
        return QuestionSet.qs;
    }
    public static void SetById (string key) {
        Debug.Log("Beginning of SetById() method in QuestionSet.cs file");
        EduRunner.Quiz quiz;
        QuestionSet.quizzes.TryGetValue (key, out quiz);
        QuestionSet.qs = QuestionSet.FromJson (JsonConvert.SerializeObject (quiz));
        return;
    }
    public static void SetByIndex (int index) {
        Debug.Log("Beginning of SetByIndex() method in QuestionSet.cs file");
        QuestionSet qs;
        string id = QuestionSet.quizzes.Select (q => q).ToArray () [index].Key;
        qs = QuestionSet.GetById (id);
        QuestionSet.qs = QuestionSet.FromJson (JsonConvert.SerializeObject (qs));
        return;
    }
    public static QuizInfo[] GetAvailableQuizzes()
    {
        Debug.Log("Beginning of GetAvailableQuizzes() method in QuestionSet.cs file");
        return QuestionSet.quizzes.Select(q => new QuizInfo(q.Key, q.Value.Name)).ToArray();
    }
}
public class QuizInfo
{
    public string Id;
    public string name;
    public QuizInfo(string Id, string name)
    {
        this.Id = Id;
        this.name = name;
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
        Debug.Log("Beginning of second Init() method in QuestionSet.cs file");
        string json = "";
        if (File.Exists(Application.streamingAssetsPath + "/" + fileName))
        {
            json = File.ReadAllText(Application.streamingAssetsPath + "/" + fileName);
        }
        QuestionSet qs = FromJson(json);
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