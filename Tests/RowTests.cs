using System;
using System.Collections.Generic;
using SimplifiedSlotMachine;
using Xunit;

namespace Tests;

public class UnitTest1
{
   readonly Dictionary<string, int> probabilities = new Dictionary<string, int>
    {
        ["A"] = 45,
        ["B"] = 35,
        ["P"] = 15,
        ["*"] = 5,
    };

    readonly Dictionary<string, decimal> symbols = new Dictionary<string, decimal>
    {
        ["A"] = 0.4m,
        ["B"] = 0.6m,
        ["P"] = 0.8m,
        ["*"] = 0,

    };

    [Fact]
    public void RowCoefficientsAreAddedCorrectly()
    {
        var testRow1 = new List<string>{"B", "B", "B"};
        var testRow2 = new List<string>{"A", "A", "A"};
        var testRow3 = new List<string>{"A", "*", "A"};
        var testRow4 = new List<string>{"*", "P", "P"};

        var result1 = Utils.AddCoefficients(symbols, testRow1);
        var result2 = Utils.AddCoefficients(symbols, testRow2);
        var result3 = Utils.AddCoefficients(symbols, testRow3);
        var result4 = Utils.AddCoefficients(symbols, testRow4);

        Assert.Equal(1.8m, result1);
        Assert.Equal(1.2m, result2);
        Assert.Equal(0.8m, result3);
        Assert.Equal(1.60m, result4);
    }

    [Fact]
    public void WinningRowsAreIdentifiedCorrectly()
    {
        var testRow1 = new List<string>{"B", "A", "A"};
        var testRow2 = new List<string>{"A", "A", "A"};
        var testRow3 = new List<string>{"A", "*", "B"};
        var testRow4 = new List<string>{"*", "P", "P"};

        var result1 = Utils.IsWinningRow(testRow1);
        var result2 = Utils.IsWinningRow(testRow2);
        var result3 = Utils.IsWinningRow(testRow3);
        var result4 = Utils.IsWinningRow(testRow4);

        Assert.False(result1);
        Assert.True(result2);
        Assert.False(result3);
        Assert.True(result4);
    }

    [Fact]
    public void ProbabilityBoundsAreGeneratedCorrectly()
    {


        var expected = new Dictionary<string, (int lowerBound, int  upperBound)>
        {
            ["A"] = (0, 45),
            ["B"] = (45, 80),
            ["P"] = (80, 95),
            ["*"] = (95, 100)
        };

        var result = Utils.GenerateProbabilityBounds(probabilities);

        foreach (KeyValuePair<string,(int lowerBound, int  upperBound)> pair in expected)
        {
            Assert.Equal(pair.Value, result[pair.Key]);
        }
    }

    [Fact]
    public void CheckProbabilityDistribution()
    {
        var symbolBounds = Utils.GenerateProbabilityBounds(probabilities);
        var random = new Random();
        const int iterations = 1_000_000;

        var results =  new Dictionary<string, int >
        {
            ["A"] = 0,
            ["B"] = 0,
            ["P"] = 0,
            ["*"] = 0
        };

        foreach (var tuple in symbolBounds)
        {
            for (int i = 0; i < iterations; i++)
            {
                var num = random.Next(0, 100);
                var match = Utils.IsWithinBounds(num, tuple.Value);
                if (match)
                {
                    results[tuple.Key]++;
                }
            }
        }

        Assert.Equal(0.45d, Math.Round(results["A"] / (double)iterations, 2));
        Assert.Equal(0.35d, Math.Round(results["B"] / (double)iterations, 2));
        Assert.Equal(0.15d, Math.Round(results["P"] / (double)iterations, 2));
        Assert.Equal(0.05d, Math.Round(results["*"] / (double)iterations, 2));

    }
}
