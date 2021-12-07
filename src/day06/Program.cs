var startingStates = File.ReadLines("in.txt").First().Split(',').Select(int.Parse).ToArray();

Console.WriteLine($"Part 1: {RunSimulation(startingStates, 80)}");
Console.WriteLine($"Part 2: {RunSimulation(startingStates, 256)}");

long RunSimulation(int[] givenStates, int days)
{
    var states = givenStates
        .GroupBy(g => g)
        .ToDictionary(g => g.Key, g => (long)g.Count());

    foreach (var day in Enumerable.Range(0, days))
    {
        var toAdd = states.GetValueOrDefault(0, 0);

        states[0] = states.GetValueOrDefault(1, 0);
        states[1] = states.GetValueOrDefault(2, 0);
        states[2] = states.GetValueOrDefault(3, 0);
        states[3] = states.GetValueOrDefault(4, 0);
        states[4] = states.GetValueOrDefault(5, 0);
        states[5] = states.GetValueOrDefault(6, 0);
        states[6] = states.GetValueOrDefault(7, 0) + toAdd;
        states[7] = states.GetValueOrDefault(8, 0);

        states[8] = toAdd;
    }

    return states.Sum(kvp => kvp.Value);
}