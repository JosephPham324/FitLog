using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using static FitLog.Domain.Entities.ProgramEnrollment;

namespace FitLog.Infrastructure.Data.Converters;
public class WorkoutsProgressConverter : ValueConverter<Dictionary<int, WorkoutProgress>, string>
{
    public WorkoutsProgressConverter() : base(
        v => JsonConvert.SerializeObject(v),
        v => JsonConvert.DeserializeObject<Dictionary<int, WorkoutProgress>>(v) ?? new Dictionary<int, WorkoutProgress>()
    )
    {
    }
}
