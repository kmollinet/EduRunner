using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphQL;

public class APIClient : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private class Credentials
    {
        // public string email;
        // public string password;
        public string token;

    }

    // public InputField Email;
    // public InputField Password;

    private Credentials GetCredentials => new Credentials
    {
        // email = Email.text,
        // password = Password.text
        // token = "da2-uw3aob4xl5c2lhz3zhbjtibmsi";
    };

    string query =
      @"query ($input: String!) {
        token: Login(credentials: $input)
    }";

    string mutation =
      @"mutation ($input: String!) {
        token: Register(credentials: $input)
    }";

    void Login()
    {
        APIGraphQL.Query(query, new { input = GetCredentials }, callback);
    }

    void Register()
    {
        APIGraphQL.Query(mutation, new { input = GetCredentials }, callback);
    }

    private void callback(GraphQLResponse response)
    {
        string token = response.Get<string>("token");
    }
  }

