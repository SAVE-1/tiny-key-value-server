// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
string result = "123";
string responseString = $"{{'code': 200, 'endpoint': 'get', 'result': '{result}'}}";

Console.WriteLine(responseString);
