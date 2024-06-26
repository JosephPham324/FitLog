using FluentAssertions;
using NUnit.Framework;
using System;

namespace FitLog.Application.UnitTests.Common.Extensions.Tests
{
    [TestFixture]
    public class DateTimeExtensionsTests
    {
        [TestCase("2024-06-26", DayOfWeek.Monday, "2024-06-24")]
        [TestCase("2024-06-26", DayOfWeek.Sunday, "2024-06-23")]
        [TestCase("2024-06-26", DayOfWeek.Wednesday, "2024-06-26")]
        [TestCase("2024-06-26", DayOfWeek.Friday, "2024-06-21")]
        public void StartOfWeek_ShouldReturnCorrectStartDate(string date, DayOfWeek startOfWeek, string expectedStartDate)
        {
            // Arrange
            var dt = DateTime.Parse(date);
            var expectedDate = DateTime.Parse(expectedStartDate);

            // Act
            var result = dt.StartOfWeek(startOfWeek);

            // Assert
            result.Should().Be(expectedDate);
        }

        [Test]
        public void StartOfWeek_WhenDayOfWeekIsSame_ShouldReturnSameDate()
        {
            // Arrange
            var dt = new DateTime(2024, 6, 26); // Wednesday
            var startOfWeek = DayOfWeek.Wednesday;

            // Act
            var result = dt.StartOfWeek(startOfWeek);

            // Assert
            result.Should().Be(dt.Date);
        }

        [Test]
        public void StartOfWeek_WhenDateIsStartOfWeek_ShouldReturnSameDate()
        {
            // Arrange
            var dt = new DateTime(2024, 6, 24); // Monday
            var startOfWeek = DayOfWeek.Monday;

            // Act
            var result = dt.StartOfWeek(startOfWeek);

            // Assert
            result.Should().Be(dt.Date);
        }
    }
}

public static class DateTimeExtensions
{
    public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }
}
