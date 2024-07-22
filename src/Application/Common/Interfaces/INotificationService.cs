using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Application.Common.Interfaces;
public interface INotificationService
{
    Task SendNotificationAsync(string user, string message);
}
