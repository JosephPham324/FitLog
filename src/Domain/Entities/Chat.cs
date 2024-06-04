using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class Chat
{
    public int ChatId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<ChatLine> ChatLines { get; set; } = new List<ChatLine>();
}
