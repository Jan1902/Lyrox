using Newtonsoft.Json.Linq;

var programs = new Dictionary<string, Action>
{
    { "Recipe Combination", CombineRecipes },
    { "Loot Table Combination", CombineLootTables }
};

while (true)
{
    DisplayMenu();
    Console.Write("Choice: ");
    if (!int.TryParse(Console.ReadLine(), out var choice)
        || choice >= programs.Count)
    {
        Console.WriteLine("Invalid Choice");
        Console.ReadLine();
        continue;
    }

    Console.Clear();
    Console.WriteLine("## " + programs.ToArray()[choice].Key);
    Console.WriteLine();
    programs.ToArray()[choice].Value.Invoke();
}

void DisplayMenu()
{
    Console.Clear();
    Console.WriteLine("##############################################");
    Console.WriteLine("##    Lyrox Tools - Static Data Combiner    ##");
    Console.WriteLine("##############################################");
    Console.WriteLine();
    Console.WriteLine("Available Programs: ");
    for (int i = 0; i < programs.Count; i++)
        Console.WriteLine($"[{i}] {programs.ToArray()[i].Key}");
}

void CombineRecipes()
{
    Console.Write("Recipes Folder Path: ");
    var path = Console.ReadLine();
    var files = Directory.GetFiles(path);

    var recipes = new JArray();
    foreach (var file in files)
        recipes.Add(JObject.Parse(File.ReadAllText(file)));

    File.WriteAllText("recipes.json", new JObject(new JProperty("recipes", recipes)).ToString());
    Console.WriteLine("Combined JSON saved to 'recipes.json'");
    Console.ReadLine();
}

void CombineLootTables()
{
    Console.Write("Block Loot Tables Folder Path: ");
    var path = Console.ReadLine();
    var files = Directory.GetFiles(path);

    var recipes = new JArray();
    foreach (var file in files)
        recipes.Add(JObject.Parse(File.ReadAllText(file)));

    File.WriteAllText("loot_tables.json", new JObject(new JProperty("loot_tables", recipes)).ToString());
    Console.WriteLine("Combined JSON saved to 'loot_tables.json'");
    Console.ReadLine();
}
