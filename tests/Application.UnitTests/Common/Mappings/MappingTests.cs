using System.Reflection;
using System.Runtime.CompilerServices;
using AutoMapper;
using FitLog.Application.CoachProfiles.Queries.GetCoachProfileDetails;
using FitLog.Application.Common.Interfaces;
using FitLog.Application.Common.Models;
using FitLog.Application.Equipments.Queries.GetEquipmentsList;
using FitLog.Application.Exercises.Queries.GetExerciseDetails;
using FitLog.Application.Exercises.Queries.GetExercises;
using FitLog.Application.MuscleGroups.Commands.UpdateMuscleGroup;
using FitLog.Application.MuscleGroups.Queries.GetMuscleGroupDetails;
using FitLog.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using FitLog.Application.TodoLists.Queries.GetTodos;
using FitLog.Application.TrainingSurveys.Commands;
using FitLog.Application.Users.Commands.Regiser;
using FitLog.Application.Users.Queries.GetCoachesListWithPagination;
using FitLog.Application.Users.Queries.GetUserDetails;
using FitLog.Application.Users.Queries.Login;
using FitLog.Application.WorkoutLogs.Queries.GetWorkoutLogsWithPagination;
using FitLog.Domain.Entities;
using NUnit.Framework;

namespace FitLog.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config => 
            config.AddMaps(Assembly.GetAssembly(typeof(IApplicationDbContext))));

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(TodoList), typeof(TodoListDto))]
    [TestCase(typeof(TodoItem), typeof(TodoItemDto))]
    [TestCase(typeof(TodoList), typeof(LookupDto))]
    [TestCase(typeof(TodoItem), typeof(LookupDto))]
    [TestCase(typeof(TodoItem), typeof(TodoItemBriefDto))]
    [TestCase(typeof(Domain.Entities.Profile), typeof(CoachProfileDetailsDto))]
    [TestCase(typeof(Equipment), typeof(EquipmentDetailsDTO))]
    [TestCase(typeof(Equipment), typeof(EquipmentDTO))]
    [TestCase(typeof(Exercise), typeof(ExerciseDetailsDTO))]
    [TestCase(typeof(Exercise), typeof(ExerciseDTO))]
    //[TestCase(typeof(MuscleGroup), typeof(UpdateMuscleGroupDTO))]
    [TestCase(typeof(MuscleGroup), typeof(MuscleGroupDTO))]
    [TestCase(typeof(SurveyAnswer), typeof(TrainingSurveyDTO))]
    //[TestCase(typeof(CoachSummaryDTO), typeof(CoachSummaryDTO))]
    [TestCase(typeof(Certification), typeof(CertificationDTO))]
    [TestCase(typeof(CoachingService), typeof(CoachingServiceDTO))]
    [TestCase(typeof(Program), typeof(ProgramDTO))]
    [TestCase(typeof(AspNetUser), typeof(UserProfileDTO))]
    //[TestCase(typeof(UserDTO), typeof(UserDTO))]
    //[TestCase(typeof(LoginResultDTO), typeof(LoginResultDTO))]
    [TestCase(typeof(ExerciseLog), typeof(ExerciseLogDTO))]
    [TestCase(typeof(WorkoutLog), typeof(WorkoutLogDTO))]

    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return RuntimeHelpers.GetUninitializedObject(type);
    }
}
