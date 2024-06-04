using System;
using System.Collections.Generic;

namespace FitLog.Domain.Entities;

public partial class ChatLine
{
    public int ChatLineId { get; set; }

    public int? ChatId { get; set; }

    public string? ChatLineText { get; set; }

    public string? LinkUrl { get; set; }

    public string? AttachmentPath { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Chat? Chat { get; set; }
}
