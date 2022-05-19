using SimplifiedSlotMachine;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;


IConfiguration config = new ConfigurationBuilder().AddJsonFile(@$"{Directory.GetCurrentDirectory()}/appsettings.json").Build();

using IHost host = Host.CreateDefaultBuilder(args).Build();


Console.WriteLine("Please deposit money you would like to play with:");

var gameInProgress = false;
var depositTaken = false;
decimal balance = 0m;
while (!depositTaken)
{
    if (decimal.TryParse(Console.ReadLine(), out var deposit))
    {
        if (deposit <= 0)
            continue;

        balance = deposit;
        depositTaken = true;
    }
}

var symbolProbabilityBounds = Utils.GenerateProbabilityBounds(Utils.Probabilities);

while (balance > 0m)
{
    var stakeEntered = false;
    var stake = 0m;

    while (!stakeEntered)
    {
        Console.WriteLine("Enter stake amount:");
        if (decimal.TryParse(Console.ReadLine(), out var enteredStake))
        {
            if (enteredStake <= 0)
                continue;
            if (balance - enteredStake < 0)
            {
                Console.WriteLine($"You dont have enough balance, maximum you can stake is {balance}");
                continue;
            }
            stake = enteredStake;
            stakeEntered = true;
        }
    }

    balance -= stake;

    decimal winAmount = 0;
    Console.WriteLine(Environment.NewLine);

    var spin = new ReelsSpin(4, 3).PopulateCells(symbolProbabilityBounds);
    var winnings = spin.Where(r => Utils.IsWinningRow(r)).Select(r => Utils.AddCoefficients(Utils.Coefficients, r))
        .Sum() * stake;

    foreach (var row in spin)
    {
        Console.WriteLine(string.Join("", row));
    }
    Console.WriteLine(Environment.NewLine);
    //spin
    //show cells
    // calculate winnigs



    balance += winnings;
    Console.WriteLine($"You have won: {winnings}");
    Console.WriteLine($"Current balance is: {balance}");
}

host.RunAsync();





