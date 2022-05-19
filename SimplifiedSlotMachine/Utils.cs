namespace SimplifiedSlotMachine;

public static class Utils
{
    public static readonly Dictionary<string, int> Probabilities = new()
    {
        ["A"] = 45,
        ["B"] = 35,
        ["P"] = 15,
        ["*"] = 5,
    };

    public static readonly Dictionary<string, decimal> Coefficients = new Dictionary<string, decimal>
    {
        ["A"] = 0.4m,
        ["B"] = 0.6m,
        ["P"] = 0.8m,
        ["*"] = 0
    };

    public static decimal AddCoefficients(Dictionary<string, decimal> coefficients, List<string> row) =>
        row.Select(s => coefficients[s]).Sum();

    public static bool IsWinningRow(List<string> row)
    {
        var distinctSymbols = row.Distinct().ToList();
        var numberOfDistinctSymbols = distinctSymbols.Count();

        if (numberOfDistinctSymbols == 1) return true;

        if (numberOfDistinctSymbols == 2 && distinctSymbols.Contains("*")) return true;

        return false;
    }

    public static bool IsWithinBounds(int num, (int lower, int upper) bounds) => bounds.lower <= num && num < bounds.upper;

    public static Dictionary<string, (int lowerBound, int upperBound)> GenerateProbabilityBounds(
        Dictionary<string, int> probabilities)
    {
        var bounds = new Dictionary<string, (int lowerBound , int upperBound)>();
        var currentProbability = 0;
        foreach (KeyValuePair<string,int> probability in probabilities)
        {
            (int lower, int upper) symbolBounds = (0, 0);
            if (currentProbability == 0)
            {
                symbolBounds.upper = probability.Value;
                currentProbability += probability.Value;
            }
            else
            {
                symbolBounds.lower = currentProbability;
                symbolBounds.upper += currentProbability + probability.Value;
                currentProbability += probability.Value;
            }
            bounds[probability.Key] = symbolBounds;
        }

        return bounds;
    }
}
