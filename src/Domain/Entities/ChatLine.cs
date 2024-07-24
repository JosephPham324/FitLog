using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class ChatLine
{
    public int ChatLineId { get; set; }
    public string CreatedBy { get; set; } = null!;

    public int? ChatId { get; set; }

    public string? ChatLineText { get; set; }

    public string? LinkUrl { get; set; }

    public string? AttachmentPath { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Chat? Chat { get; set; }
    public virtual AspNetUser? CreatedByNavigation { get; set; }
}
