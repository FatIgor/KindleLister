namespace KindleLister;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class AddUpdateList
{
    public List<MetaData> meta_data { get; set; }
}

public class Authors
{
    public string author { get; set; }
    public string[] authorArray { get; set; }
}

public class CacheMetadata
{
    public int version { get; set; }
}

public class MetaData
{
    public string ASIN { get; set; }
    public string title { get; set; }
    public Authors authors { get; set; }
    public object publishers { get; set; }
    public DateTime publication_date { get; set; }
    public object purchase_date { get; set; }
    public string textbook_type { get; set; }
    public string cde_contenttype { get; set; }
    public string content_type { get; set; }
    public Origins origins { get; set; }
}

public class Origin
{
    public string type { get; set; }
}

public class Origins
{
    public Origin origin { get; set; }
}

public class Response
{
    public string sync_time { get; set; }
    public CacheMetadata cache_metadata { get; set; }
    public AddUpdateList add_update_list { get; set; }
}

public class Root
{
    public Response response { get; set; }
}

