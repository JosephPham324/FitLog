using FitLog.Domain.Constants;
using FluentAssertions;
using NUnit.Framework;

namespace FitLog.Domain.UnitTests.Constants;
public class ExerciseTypeTests
{
    
    [Test]
    public void WeightResistanceShouldReturnCorrectValue()
    {
        ExerciseTypes.WeightResistance.Should().Be("WeightResistance");
    }
    [Test]
    public void CalisthenicsShouldReturnCorrectValue()
    {
        ExerciseTypes.Calisthenics.Should().Be("Calisthenics");
    }
    [Test]
    public void PlyometricsShouldReturnCorrectValue()
    {
        ExerciseTypes.Plyometrics.Should().Be("Plyometrics");
    }
    [Test]
    public void LissCardioShouldReturnCorrectValue()
    {
        ExerciseTypes.LissCardio.Should().Be("LissCardio");
    }
    [Test]
    public void HitCardioShouldReturnCorrectValue()
    {
        ExerciseTypes.HitCardio.Should().Be("LissCardio");
    }
    [Test]
    public void HitCardioShouldNotBeEqualToWeightResistance()
    {
        ExerciseTypes.HitCardio.Should().NotBe(ExerciseTypes.WeightResistance);
    }
    [Test]
    public void HitCardioShouldNotBeEqualToCalisthenics()
    {
        ExerciseTypes.HitCardio.Should().NotBe(ExerciseTypes.Calisthenics);
    }
    [Test]
    public void HitCardioShouldNotBeEqualToPlyometrics()
    {
        ExerciseTypes.HitCardio.Should().NotBe(ExerciseTypes.Plyometrics);
    }
    [Test]
    public void HitCardioShouldNotBeEqualToLissCardio()
    {
        ExerciseTypes.HitCardio.Should().NotBe(ExerciseTypes.LissCardio);
    }
    [Test]
    public void LissCardioShouldNotBeEqualToWeightResistance()
    {
        ExerciseTypes.LissCardio.Should().NotBe(ExerciseTypes.WeightResistance);
    }
    [Test]
    public void LissCardioShouldNotBeEqualToCalisthenics()
    {
        ExerciseTypes.LissCardio.Should().NotBe(ExerciseTypes.Calisthenics);
    }
    [Test]
    public void LissCardioShouldNotBeEqualToPlyometrics()
    {
        ExerciseTypes.LissCardio.Should().NotBe(ExerciseTypes.Plyometrics);
    }
    [Test]
    public void PlyometricsShouldNotBeEqualToWeightResistance()
    {
        ExerciseTypes.Plyometrics.Should().NotBe(ExerciseTypes.WeightResistance);
    }
    [Test]
    public void PlyometricsShouldNotBeEqualToCalisthenics()
    {
        ExerciseTypes.Plyometrics.Should().NotBe(ExerciseTypes.Calisthenics);
    }
    [Test]
    public void CalisthenicsShouldNotBeEqualToWeightResistance()
    {
        ExerciseTypes.Calisthenics.Should().NotBe(ExerciseTypes.WeightResistance);
    }
}
