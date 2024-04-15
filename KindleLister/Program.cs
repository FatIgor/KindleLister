using System.Text;
using KindleLister;
using Newtonsoft.Json;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;


// original list is in XML format, converted to JSON
// found in C:\Users\<user>\AppData\Local\Amazon\Kindle\Cache\KindleSyncMetadataCache.xml
// I used https://codebeautify.org/xmltojson to convert the XML to JSON
// I then saved the JSON to a file
var json = File.ReadAllText("e:\\riderprojects\\kindlelister\\kindlelister\\kindle_list.json");

if (string.IsNullOrEmpty(json))
{
    Console.WriteLine("No JSON file found");
    return;
}

json = json.Replace("author\": [", "authorArray\": [");

// some books have no publication date, so I set them to 1900-01-01
json = json.Replace("publication_date\": \"\"", "publication_date\": \"1900-01-01T00:00:00+0000\"");

var kindleList = JsonConvert.DeserializeObject<Root>(json, new JsonSerializerSettings { Error = HandleDeserializationError });

var typeList = new Dictionary<string, int>();

foreach (var item in kindleList.response.add_update_list.meta_data)
{
    item.origins ??= new Origins { origin = new Origin { type = "Unknown" } };
    if (item.origins.origin.type == "")
    {
        item.origins.origin.type = "Document";
    }

    if (typeList.ContainsKey(item.origins.origin.type))
    {
        typeList[item.origins.origin.type]++;
    }
    else
    {
        typeList.Add(item.origins.origin.type, 1);
    }
}

// strip out unwanted, in my case Comixology
var finalList = kindleList.response.add_update_list.meta_data.Where(item => item.origins.origin.type != "Comixology").ToList();

var csv = new StringBuilder();
foreach (var item in finalList)
{
    var authors = GetAuthorsString(item);
    if (authors.Contains((','))) authors = "\"" + authors + "\"";
    if (item.title.Contains(',')) item.title = "\"" + item.title + "\"";
    csv.AppendLine($"{item.title},{authors},{item.publication_date},{item.origins.origin.type}");
}

File.WriteAllText("e:\\riderprojects\\kindlelister\\kindlelister\\kindle_list.csv", csv.ToString());

Console.WriteLine($"List has {kindleList.response.add_update_list.meta_data.Count} books");
Console.WriteLine($"Final list has {finalList.Count} books");
foreach (var type in typeList)
{
    Console.WriteLine($"Type: {type.Key} Count: {type.Value}");
}

void HandleDeserializationError(object? sender, ErrorEventArgs errorArgs)
{
    Console.WriteLine(errorArgs.ErrorContext.Error.Message);
    errorArgs.ErrorContext.Handled = true;
}

string GetAuthorsString(MetaData item)
{
    return item.authors.authorArray == null ? ProcessName(item.authors.author) : string.Join(", ", item.authors.authorArray.Select(ProcessName));
}

// called by GetAuthorsString
// I want all names uniform, so any Last,First will be converted to First Last
string ProcessName(string name)
{
    if (!name.Contains(',')) return name;
    var parts = name.Split(",");
    return $"{parts[1].Trim()} {parts[0].Trim()}";
}