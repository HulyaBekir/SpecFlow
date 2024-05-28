// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


var httpClient = new HttpClient();
var response = await httpClient.GetAsync("https://gorest.co.in/public/v2/users");