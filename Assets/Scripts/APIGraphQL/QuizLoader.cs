
using System;
using System.Net.Http;
using EduRunner;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace AsyncQuizLoader
{
    public class ListQuizzes
    {
        public Quiz[] items;
    }
    public class ListQuizzesResponse
    {
        public ListQuizzes listQuizs;
    }

    public class GetQuizResponse
    {
        public Quiz getQuiz;
    }
    public class QuizLoader
    {

        public GraphQLHttpClient client;

        private GraphQLRequest ListQuizzes = new GraphQLRequest
        {
            Query = @"
                query ListQuizs(
                    $filter: ModelQuizFilterInput
                    $limit: Int
                    $nextToken: String
                  ) {
                    listQuizs(filter: $filter, limit: $limit, nextToken: $nextToken) {
                      items {
                        id
                        name
                        subject
                        description
                        questions {
                          questionText
                          answer
                          difficulty
                          points
                          id
                        }
                        ordered
                        ownerId
                        visibility
                        questionsPerQuiz
                      }
                      nextToken
                    }
                  }
             "
        };

        private GraphQLRequest GetQuiz(string id) => new GraphQLRequest
        {
            Query = @"
                query GetQuiz($id: ID!) {
                    getQuiz(id: $id) {
                      id
                      name
                      subject
                      description
                      questions {
                        questionText
                        answer
                        difficulty
                        points
                        id
                      }
                      ordered
                      ownerId
                      visibility
                      questionsPerQuiz
                    }
                }",
            OperationName = "GetQuiz",
            Variables = new
            {
                id = id
            }
        };

        public async System.Threading.Tasks.Task<Quiz[]> GetAllQuizzes()
        {
            GraphQLResponse<ListQuizzesResponse> graphQLResponse = await this.client.SendQueryAsync<ListQuizzesResponse>(this.ListQuizzes);
            return graphQLResponse.Data.listQuizs.items;
        }

        public async System.Threading.Tasks.Task<Quiz> GetQuizById(string id)
        {
            GraphQLResponse<GetQuizResponse> graphQLResponse = await this.client.SendQueryAsync<GetQuizResponse>(this.GetQuiz(id));
            return graphQLResponse.Data.getQuiz;
        }
        public QuizLoader()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("x-api-key", "da2-uw3aob4xl5c2lhz3zhbjtibmsi");

            var options = new GraphQLHttpClientOptions();
            options.EndPoint = new Uri("https://vrbvlnshabb2nl55jwduzkb62e.appsync-api.us-west-2.amazonaws.com/graphql");

            client = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(), httpClient);

        }
    }
}