namespace SimplifiedSlotMachine;

public class ReelsSpin
{
    private readonly int symbolsPerReel;
    private readonly int noOfReels;

    public ReelsSpin(int symbolsPerReel, int noOfReels)
    {
        this.symbolsPerReel = symbolsPerReel;
        this.noOfReels = noOfReels;
        Cells = new List<List<string>>();
    }

    public List<List<string>> Cells { get; }

    public List<List<string>> PopulateCells( Dictionary<string,(int lowerBound, int upperBound)> probabilityBounds)
    {
        var random = new Random();
        for (int i = 0; i < symbolsPerReel; i++)
        {
            Cells.Add(new List<string>());
            for (int j = 0; j < noOfReels; j++)
            {
                var number = random.Next(0, 100);
                var symbol = probabilityBounds.Single(pair => Utils.IsWithinBounds(number, pair.Value)).Key;
                Cells[i].Add(symbol);
            }
        }

        return Cells;
    }


}
