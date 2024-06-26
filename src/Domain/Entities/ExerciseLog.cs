using System;
using System.Collections.Generic;
using System.Text.Json;

namespace FitLog.Domain.Entities;

public partial class ExerciseLog
{
    public int ExerciseLogId { get; set; }

    public int? WorkoutLogId { get; set; }

    public int? ExerciseId { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime LastModified { get; set; }

    public int? OrderInSession { get; set; }

    public int? OrderInSuperset { get; set; }

    public string? Note { get; set; }

    public int? NumberOfSets { get; set; }
    
    public List<int>? WeightsUsedValue { get; set; }
    public List<int>? NumberOfRepsValue { get; set; }

    public string? WeightsUsed
    {
        get => JsonSerializer.Serialize(WeightsUsedValue);
        set => WeightsUsedValue = string.IsNullOrEmpty(value) ? new List<int>() : JsonSerializer.Deserialize<List<int>>(value);
    }

    public string? NumberOfReps
    {
        get => JsonSerializer.Serialize(NumberOfRepsValue);
        set => NumberOfRepsValue = string.IsNullOrEmpty(value) ? new List<int>() : JsonSerializer.Deserialize<List<int>>(value);
    }

    public List<string>? FootageURLsList { get; set; }

    public string? FootageUrls
    {
        get => JsonSerializer.Serialize(FootageURLsList);
        set => FootageURLsList = string.IsNullOrEmpty(value) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(value);
    }
    public virtual Exercise? Exercise { get; set; }

    public virtual WorkoutLog? WorkoutLog { get; set; }
}
