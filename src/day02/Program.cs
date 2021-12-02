int x = 0, y = 0, aim = 0;
var commands = File.ReadLines("in.txt").Select(Instruction.Parse).ToList();

// Part 1
foreach (var command in commands)
{
    x += command.Direction switch
    {
        Direction.FORWARD => command.Distance,
        _ => 0
    };

    y += command.Direction switch
    {
        Direction.UP => -command.Distance,
        Direction.DOWN => +command.Distance,
        _ => 0
    };
}

Console.WriteLine($"Part 1: {x * y}");

// Part 2
x = 0; y = 0; aim = 0;
foreach (var command in commands)
{
    switch (command.Direction)
    {
        case Direction.DOWN:
            aim += command.Distance;
            break;
        case Direction.UP:
            aim -= command.Distance;
            break;
        case Direction.FORWARD:
            x += command.Distance;
            y += aim * command.Distance;
            break;
    }
}

Console.WriteLine($"Part 2: {x * y}");

record Instruction
{
    public Direction Direction { get; init; }
    public int Distance { get; init; }

    public static Instruction Parse(string input)
    {
        var parts = input.Split();

        return new Instruction
        {
            Direction = Enum.TryParse(parts[0].ToUpper(), out Direction parsed)
                ? parsed :
                throw new ArgumentException($"Invalid direction: {parts[0]}", nameof(input)),

            Distance = int.Parse(parts[1])
        };
    }
}

enum Direction
{
    FORWARD,
    UP,
    DOWN
}