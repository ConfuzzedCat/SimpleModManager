// ReSharper disable InconsistentNaming

using System.Data;

namespace SimpleModManager.Api;

public class ModInfoApi
{
    public ModInfoApi()
    {
        version = string.Empty;
        author = string.Empty;
        name = string.Empty;
        summary = string.Empty;
        description = string.Empty;
        picture_url = string.Empty;
        domain_name = string.Empty;
        uploaded_by = string.Empty;
        uploaded_users_profile_url = string.Empty;
        status = string.Empty;
        user = User.Empty;
        endorsement = Endorsement.Empty;
    }

    public static ModInfoApi Empty =>
        new();

    public string name { get; set; }
    public string summary { get; set; }
    public string description { get; set; }
    public string picture_url { get; set; }
    public int mod_downloads { get; set; }
    public int mod_unique_downloads { get; set; }
    public long uid { get; set; }
    public int mod_id { get; set; }
    public int game_id { get; set; }
    public bool allow_rating { get; set; }
    public string domain_name { get; set; }
    public int category_id { get; set; }
    public string version { get; set; }
    public int endorsement_count { get; set; }
    public int created_timestamp { get; set; }
    public DateTime created_time { get; set; }
    public int updated_timestamp { get; set; }
    public DateTime updated_time { get; set; }
    public string author { get; set; }
    public string uploaded_by { get; set; }
    public string uploaded_users_profile_url { get; set; }
    public bool contains_adult_content { get; set; }
    public string status { get; set; }
    public bool available { get; set; }
    public User user { get; set; }
    public Endorsement endorsement { get; set; }

    public class User
    {
        public static User Empty => new();

        public User()
        {
            name = string.Empty;
        }
        
        public int member_id { get; set; }
        public int member_group_id { get; set; }
        public string name { get; set; }
    }

    public class Endorsement
    {
        public Endorsement()
        {
            endorse_status = string.Empty;
            version = string.Empty;
        }
        
        public static Endorsement Empty => new();
        public string endorse_status { get; set; }
        public int timestamp { get; set; }
        public string version { get; set; }
    }
}