namespace EduRunner
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Quiz
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("ordered")]
        public bool Ordered { get; set; }

        [JsonProperty("ownerId")]
        public string OwnerId { get; set; }

        [JsonProperty("questions")]
        public List<Question> Questions { get; set; }

        [JsonProperty("questionsPerQuiz")]
        public double? QuestionsPerQuiz { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("visibility")]
        public QuizVisibilityEnumEnum Visibility { get; set; }
    }

    public partial class Question
    {
        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("points")]
        public double Points { get; set; }

        [JsonProperty("questionText")]
        public string QuestionText { get; set; }
    }

    public partial class QuestionInput
    {
        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("difficulty")]
        public double Difficulty { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("points")]
        public double Points { get; set; }

        [JsonProperty("questionText")]
        public string QuestionText { get; set; }
    }

    public enum QuizVisibilityEnumEnum { Private, Public };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                QuizVisibilityEnumEnumConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class QuizVisibilityEnumEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(QuizVisibilityEnumEnum) || t == typeof(QuizVisibilityEnumEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "PRIVATE":
                    return QuizVisibilityEnumEnum.Private;
                case "PUBLIC":
                    return QuizVisibilityEnumEnum.Public;
            }
            throw new Exception("Cannot unmarshal type QuizVisibilityEnumEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (QuizVisibilityEnumEnum)untypedValue;
            switch (value)
            {
                case QuizVisibilityEnumEnum.Private:
                    serializer.Serialize(writer, "PRIVATE");
                    return;
                case QuizVisibilityEnumEnum.Public:
                    serializer.Serialize(writer, "PUBLIC");
                    return;
            }
            throw new Exception("Cannot marshal type QuizVisibilityEnumEnum");
        }

        public static readonly QuizVisibilityEnumEnumConverter Singleton = new QuizVisibilityEnumEnumConverter();
    }
}
