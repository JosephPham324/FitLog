using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace FitLog.Domain.Entities;

public partial class AspNetUserLogin : IdentityUserLogin<string>
{
}
