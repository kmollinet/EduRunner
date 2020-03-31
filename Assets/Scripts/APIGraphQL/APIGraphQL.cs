using System;

namespace GraphQL {
    public static class APIGraphQL {
        private const string ApiURL = "https://vrbvlnshabb2nl55jwduzkb62e.appsync-api.us-west-2.amazonaws.com/graphql";
        public static string Token = "da2-uw3aob4xl5c2lhz3zhbjtibmsi";

        public static bool LoggedIn {
            get { return !Token.Equals(""); } //todo: improve loggedin verification
        } 

        private static readonly GraphQLClient API = new GraphQLClient(ApiURL);

        public static void Query (string query, object variables = null, Action<GraphQLResponse> callback = null) {
            API.Query(query, variables, callback, Token);
        }
    }
}