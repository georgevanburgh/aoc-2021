using System.Drawing;

var ranges = File.ReadLines("in.txt")
    .Select(line => line.Split("->", StringSplitOptions.TrimEntries))
    .Select(part => (start: ParsePoint(part[0]), end: ParsePoint(part[1])))
    .ToList();

var points1 = ranges.SelectMany(range => EnumeratePoints(range.start, range.end, considerDiagonals: false));
Console.WriteLine($"Part 1: {CountDangerousPoints(points1)}");

var points2 = ranges.SelectMany(range => EnumeratePoints(range.start, range.end, considerDiagonals: true));
Console.WriteLine($"Part 2: {CountDangerousPoints(points2)}");

int CountDangerousPoints(IEnumerable<Point> points) => points
        .GroupBy(point => point)
        .Count(group => group.Count() >= 2);

IEnumerable<Point> EnumeratePoints(Point start, Point end, bool considerDiagonals = false)
{
    int xdiff = end.X - start.X, ydiff = end.Y - start.Y;
    
    // 3 cases:
    // only one dimension has a diff
    // both dimensions have the same (absolute) diff, and we're considering diagonals
    // both dimensions have different diffs
    bool atMostOneDelta = (Math.Abs(xdiff) == 0 || Math.Abs(ydiff) == 0);
    bool exactDiagonal = (Math.Abs(xdiff) == Math.Abs(ydiff));
    bool canEnumerate = atMostOneDelta || (considerDiagonals && exactDiagonal);

    if (!canEnumerate)
        yield break;

    Size diff = new Size(Step(xdiff), Step(ydiff));
    var current = start;
    while (current != end)
    {
        yield return current;
        current = Point.Add(current, diff);
    }

    // Ranges are inclusive - yield the end point as well
    yield return current;
}

int Step(int diff) => Math.Sign(diff);

Point ParsePoint(string raw)
{
    var parts = raw.Split(',').Select(int.Parse).ToArray();
    return new Point(parts[0], parts[1]);
}
