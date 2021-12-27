var displays = File.ReadLines("in.txt").Select(Display.Parse).ToList();

var outputsWithUniqueCount = displays.Sum(display => display.NumberOfOutputsWithDistinctCount);

Console.WriteLine($"Part 1: {outputsWithUniqueCount}");
Console.WriteLine($"Part 2: {displays.Sum(display => display.ReadDigits())}");

record Display
{
    private List<string> Patterns { get; init; }
    private List<string> Outputs { get; init; }
    private static readonly int[] _uniqueLengths = new[] { 2, 3, 4, 7 };

    public static Display Parse(string line) => new Display(line);

    private Display(string line)
    {
        var parts = line.Split('|', StringSplitOptions.TrimEntries);
        Patterns = parts[0].Split(' ').ToList();
        Outputs = parts[1].Split(' ').ToList();
    }

    public int NumberOfOutputsWithDistinctCount
    {
        get => Outputs.Count(output => _uniqueLengths.Contains(output.Length));
    }

    public int ReadDigits()
    {
        return ReadDigit(Outputs[0]) * 1000 +
               ReadDigit(Outputs[1]) * 100 +
               ReadDigit(Outputs[2]) * 10 +
               ReadDigit(Outputs[3]) * 1;
    }

    private int ReadDigit(string given)
    {
        var patternForOne = PatternWithLength(2);
        var patternForFour = PatternWithLength(4);

        return given switch
        {
            { Length: 2 } => 1,
            { Length: 3 } => 7,
            { Length: 4 } => 4,
            { Length: 7 } => 8,
            { Length: 5 } pattern when NumberOfCommonDigits(pattern, patternForOne) == 2 => 3,
            { Length: 5 } pattern when NumberOfCommonDigits(pattern, patternForFour) == 3 => 5,
            { Length: 5 } pattern when NumberOfCommonDigits(pattern, patternForFour) == 2 => 2,
            { Length: 6 } pattern when NumberOfCommonDigits(pattern, patternForOne) < 2 => 6,
            { Length: 6 } pattern when NumberOfCommonDigits(pattern, patternForFour) == 4=> 9,
            { Length: 6 } => 0,
            _ => throw new Exception($"Cannot decipher pattern {given}")
        };
    }

    private static int NumberOfCommonDigits(string left, string right) => left.Count(c => right.Contains(c));

    private string PatternWithLength(int length) => Patterns.Single(p => p.Length == length);
}