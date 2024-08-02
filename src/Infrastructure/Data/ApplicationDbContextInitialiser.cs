using System.Runtime.InteropServices;
using FitLog.Domain.Constants;
using FitLog.Domain.Entities;
using FitLog.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FitLog.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AspNetUser> _userManager;
    private readonly RoleManager<AspNetRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<AspNetUser> userManager, RoleManager<AspNetRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new AspNetRole(Roles.Administrator);
        administratorRole.Id = Guid.NewGuid().ToString();

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        var memberRole = new AspNetRole(Roles.Member);
        memberRole.Id = Guid.NewGuid().ToString();

        if (_roleManager.Roles.All(r => r.Name != memberRole.Name))
        {
            await _roleManager.CreateAsync(memberRole);
        }

        var coachRole = new AspNetRole(Roles.Coach);
        coachRole.Id = Guid.NewGuid().ToString();

        if (_roleManager.Roles.All(r => r.Name != coachRole.Name))
        {
            await _roleManager.CreateAsync(coachRole);
        }

        // Default users
        var administrator = new AspNetUser { Id = Guid.NewGuid().ToString(), UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            //PASSWORD: Administrator1!
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }

        if (!_context.Equipment.Any())
        {
            _context.Equipment.AddRange(
                new Equipment { EquipmentName = "Dumbbell", ImageUrl = null },
                new Equipment { EquipmentName = "Barbell", ImageUrl = null },
                new Equipment { EquipmentName = "Kettlebell", ImageUrl = null }
            );

            await _context.SaveChangesAsync();
        }

        // Seed Muscle Groups if necessary
        if (!_context.MuscleGroups.Any())
        {
            _context.MuscleGroups.AddRange(
                new MuscleGroup { MuscleGroupName = "Chest", ImageUrl = null },
                new MuscleGroup { MuscleGroupName = "Back", ImageUrl = null },
                new MuscleGroup { MuscleGroupName = "Legs", ImageUrl = null }
            );

            await _context.SaveChangesAsync();
        }

        // Seed Exercises if necessary
        if (!_context.Exercises.Any())
        {
            // Create MuscleGroups if they do not exist.
            var muscleGroups = new List<MuscleGroup>
            {
                new MuscleGroup { MuscleGroupName = "Chest" },  // Assume ID 1
                new MuscleGroup { MuscleGroupName = "Back" },   // Assume ID 2
                new MuscleGroup { MuscleGroupName = "Legs" }    // Assume ID 3
            };

            foreach (var mg in muscleGroups)
            {
                if (!_context.MuscleGroups.Any(x => x.MuscleGroupName == mg.MuscleGroupName))
                {
                    _context.MuscleGroups.Add(mg);
                }
            }
            await _context.SaveChangesAsync();

            // Equipment assumed similar to muscle groups.
            var equipmentList = new List<Equipment>
            {
                new Equipment { EquipmentName = "Barbell" }, // Assume ID 2
                new Equipment { EquipmentName = "None" }     // Assume null handled
            };

            foreach (var eq in equipmentList)
            {
                if (!_context.Equipment.Any(e => e.EquipmentName == eq.EquipmentName))
                {
                    _context.Equipment.Add(eq);
                }
            }
            await _context.SaveChangesAsync();

            // Adding exercises with MuscleGroup associations
            _context.Exercises.AddRange(
                new Exercise
                {
                    ExerciseName = "Bench Press",
                    EquipmentId = _context.Equipment.FirstOrDefault(e => e.EquipmentName == "Barbell")?.EquipmentId,
                    Type = ExerciseTypes.WeightResistance,
                    ExerciseMuscleGroups = new List<ExerciseMuscleGroup>
                    {
                            new ExerciseMuscleGroup { MuscleGroup = _context.MuscleGroups.FirstOrDefault(mg=>mg.MuscleGroupName=="Chesst") ?? new MuscleGroup() }
                    }
                },
                new Exercise
                {
                    ExerciseName = "Pull-Up",
                    EquipmentId = null, // No equipment specified
                    Type = ExerciseTypes.Calisthenics,
                    ExerciseMuscleGroups = new List<ExerciseMuscleGroup>
                    {
                            new ExerciseMuscleGroup { MuscleGroup = _context.MuscleGroups.FirstOrDefault(mg => mg.MuscleGroupName == "Back") ?? new MuscleGroup()}
                    }
                },
                new Exercise
                {
                    ExerciseName = "Squat",
                    EquipmentId = _context.Equipment.FirstOrDefault(e => e.EquipmentName == "Barbell")?.EquipmentId,
                    Type = ExerciseTypes.WeightResistance,
                    ExerciseMuscleGroups = new List<ExerciseMuscleGroup>
                    {
                            new ExerciseMuscleGroup { MuscleGroup = _context.MuscleGroups.FirstOrDefault(mg => mg.MuscleGroupName == "Legs") ?? new MuscleGroup()}
                    }
                }
            );

            await _context.SaveChangesAsync();
        }

    }
}
