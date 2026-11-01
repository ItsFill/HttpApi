using System.Text.Json;

public static class Response
{
    public static async Task<string> GetFilm(JsonElement root, HttpClient client) =>
        await client.GetStringAsync(root.GetProperty("films").GetString());

    public static async Task<string> GetPeople(JsonElement root, HttpClient client) =>
        await client.GetStringAsync(root.GetProperty("people").GetString());

    public static async Task<string> GetPlanets(JsonElement root, HttpClient client) =>
        await client.GetStringAsync(root.GetProperty("planets").GetString());

    public static async Task<string> GetSpecies(JsonElement root, HttpClient client) =>
        await client.GetStringAsync(root.GetProperty("species").GetString());

    public static async Task<string> GetStarships(JsonElement root, HttpClient client) =>
        await client.GetStringAsync(root.GetProperty("starships").GetString());

    public static async Task<string> GetVehicles(JsonElement root, HttpClient client) =>
        await client.GetStringAsync(root.GetProperty("vehicles").GetString());

    
    public static void PrintFilms(string jsonResponse)
    {
        using JsonDocument doc = JsonDocument.Parse(jsonResponse);
        JsonElement results = doc.RootElement.GetProperty("results");

        Console.WriteLine("\nFilms List: ");
        foreach (JsonElement film in results.EnumerateArray())
        {
            string title = film.GetProperty("title").GetString();
            int episode = film.GetProperty("episode_id").GetInt32();
            Console.WriteLine($"Episode {episode}: {title}");
        }
    }
    
    public static void PrintNames(string jsonResponse, string categoryTitle)
    {
        using JsonDocument doc = JsonDocument.Parse(jsonResponse);
        JsonElement results = doc.RootElement.GetProperty("results");

        Console.WriteLine($"\n{categoryTitle}");
        foreach (JsonElement item in results.EnumerateArray())
        {
            string name = item.GetProperty("name").GetString();
            Console.WriteLine($"- {name}");
        }
    }
}

class Program
{
    static string[] commands = {"films", "people", "planets", "species", "starships", "vehicles", "exit"};
    
    static async Task Main(string[] args)
    {
        using var client = new HttpClient();

        try
        {
            string rootUrl = "https://swapi.py4e.com/api/";
            
            string rootJsonResponse = await client.GetStringAsync(rootUrl);
            
            using JsonDocument doc = JsonDocument.Parse(rootJsonResponse);
            JsonElement root = doc.RootElement;

            bool isLaunched = true;
            
            Console.WriteLine("Hello, What do you want to know about the Stars Wars?");
            string userResponse = Console.ReadLine();
            switch (userResponse.ToLower())
            {
                case "yes":
                    isLaunched = true;
                    break;
                case "no":
                    isLaunched = false;
                    break;
            }
            
            while (isLaunched)
            {
                Console.WriteLine("Commands: ");
                foreach (string command in commands)
                {
                    Console.WriteLine($"\t{command}");
                }
                Console.WriteLine();
                
                userResponse = Console.ReadLine();

                switch (userResponse.ToLower())
                {
                    case "films":
                        string filmResponse = await Response.GetFilm(root, client);
                        Response.PrintFilms(filmResponse);
                        break;
                    case "people":
                        string peopleResponse = await Response.GetPeople(root, client);
                        Response.PrintNames(peopleResponse, "People List:");
                        break;
                    case "planets":
                        string planetsResponse = await Response.GetPlanets(root, client);
                        Response.PrintNames(planetsResponse, "Planets List:");
                        break;
                    case "species":
                        string speciesResponse = await Response.GetSpecies(root, client);
                        Response.PrintNames(speciesResponse, "Species List:");
                        break;
                    case "starships":
                        string starshipsResponse = await Response.GetStarships(root, client);
                        Response.PrintNames(starshipsResponse, "Starships List:");
                        break;
                    case "vehicles":
                        string vehiclesResponse = await Response.GetVehicles(root, client);
                        Response.PrintNames(vehiclesResponse, "Vehicles List:");
                        break;
                    case "exit":
                        isLaunched = false;
                        break;
                }
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Wrong: {ex.Message}");
        }
    }
}