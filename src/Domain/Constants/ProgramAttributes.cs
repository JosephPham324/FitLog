using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitLog.Domain.Constants;
public abstract class ProgramAttributes
{
    public static readonly List<string> ExperienceLevels =
    [
        "Novice",
        "Beginner",
        "Intermediate",
        "Advanced"
    ];

    public static readonly Dictionary<string, string> GymTypes = new Dictionary<string, string>()
    {
        { "Home", "Contain minimal equipment like dumbbells, barbell, power rack, pull-up bar" },
        {"Full gym", "Most standard exercise selections are available" },
        {"Calisthenics", "Pull-up bar, dip bar, gymnastic rings" }
    };

    public static readonly List<string> Goals =
    [
        "Bodybuilding",
        "Powerlifting",
        "Powerbuilding",
        "Calisthenics skills",
        "Strongman",
        "Street lifting"
    ];
}
