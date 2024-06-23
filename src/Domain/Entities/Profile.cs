using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FitLog.Domain.Entities;

public partial class Profile
{
    public int ProfileId { get; set; }

    public string? UserId { get; set; }

    [JsonPropertyName("overview")]
    public string? Bio { get; set; }

    public string? ProfilePicture { get; set; }

    public virtual AspNetUser? User { get; set; }

    public List<string>? MajorAchievements { get; set; }

    public List<string>? GalleryImageLinks { get; set; }

    //Turns list into a JSON string for storage in the database
    public string GalleryImageLinksJson
    {
        get => JsonSerializer.Serialize(GalleryImageLinks);
        set => GalleryImageLinks = string.IsNullOrEmpty(value) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(value);
    }
}
