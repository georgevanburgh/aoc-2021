var input = File.ReadLines("in.txt");
var results = input.Select(line => ParseString(line));

Console.WriteLine($"Part 1: {results.Sum(result => result.CorruptionScore)}");

var autoCompleteScores = results.Where(res => res.AutoCompleteScore > 0);
var numberOfAutocompleteScores = autoCompleteScores.Count();
var scoreToTake = (int)Math.Floor(numberOfAutocompleteScores / 2f);
var scores = autoCompleteScores.OrderByDescending(score => score.AutoCompleteScore);
var middleScore = scores.Skip(scoreToTake).First();

Console.WriteLine($"Part 2: {middleScore.AutoCompleteScore}");

Result ParseString(string line)
{
    var seen = new Stack<char>();
    var matcher = new Dictionary<char, char>
    {
        {'(', ')'},
        {'[', ']'},
        {'<', '>'},
        {'{', '}'},
    };

    foreach (var ch in line)
    {
        if (matcher.Keys.Contains(ch))
        {
            seen.Push(ch);
        }
        else if (matcher[seen.Pop()] != ch)
        {
            return new Result { CorruptChar = ch };
        }
    }

    if (seen.Any())
    {
        return new Result
        {
            MissingChars = seen.Select(ch => matcher[ch]).ToList()
        };
    }

    return new Result();
}

record Result
{
    public char? CorruptChar { get; init; }
    public List<char> MissingChars { get; init; } = new List<char>();
    public int CorruptionScore => CorruptChar switch
    {
        null => 0,
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => 0
    };

    public long AutoCompleteScore => MissingChars.Aggregate(0L, (total, next) => total = (total * 5L) + AutoCompleteCharScore(next));

    private long AutoCompleteCharScore(char c) => c switch
    {
        ')' => 1L,
        ']' => 2L,
        '}' => 3L,
        '>' => 4L,
        _ => 0L
    };
}