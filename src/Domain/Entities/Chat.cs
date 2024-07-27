using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class Chat
{
    public int ChatId { get; set; }
    public string CreatedBy { get; set; } = null!;
    public string TargetUserId { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public AspNetUser UserNavigation = null!;
    public AspNetUser TargetUserNavigation = null!;
    public virtual ICollection<ChatLine> ChatLines { get; set; } = new List<ChatLine>();

}
