var lines = File.ReadLines("in.txt");

var grid = lines.Select(l => l.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
var width = grid[0].Length;
var height = grid.Length;

var minimumPositions = EnumerateCoordinates().Where(pos =>
{
    var neighbours = EnumerateNeighbourValues(pos.x, pos.y);
    var localValue = grid[pos.y][pos.x];
    return neighbours.All(neighbour => neighbour > localValue);
});

Console.WriteLine($"Part 1: {minimumPositions.Sum(p => grid[p.y][p.x] + 1)}");

var biggestBasins = minimumPositions.OrderByDescending(pos => SizeOfBasinCenteredAt(pos.x, pos.y)).Take(3);
var productOfBasinSizes = biggestBasins.Aggregate(1, (int curr, (int x, int y) pos) => curr *= SizeOfBasinCenteredAt(pos.x, pos.y));

Console.WriteLine($"Part 2: {productOfBasinSizes}");

int SizeOfBasinCenteredAt(int startX, int startY)
{
    var basinCoordinates = new HashSet<(int x, int y)> { (startX, startY) };
    int lastSize = -1;
    do
    {
        lastSize = basinCoordinates.Count;
        var discovered = new List<(int x, int y)>();
        foreach (var coordinate in basinCoordinates)
            discovered.AddRange(EnumerateBasinCoordinates(coordinate.x, coordinate.y));

        foreach (var discovery in discovered) { basinCoordinates.Add(discovery); }
    } while(basinCoordinates.Count != lastSize);

    return basinCoordinates.Count;
}

IEnumerable<(int x, int y)> EnumerateBasinCoordinates(int x, int y)
{
    if (TryGetValue(x - 1, y, out int value) && value != 9) yield return (x - 1, y);
    if (TryGetValue(x + 1, y, out value) && value != 9) yield return (x + 1, y);
    if (TryGetValue(x, y - 1, out value) && value != 9) yield return (x, y - 1);
    if (TryGetValue(x, y + 1, out value) && value != 9) yield return (x, y + 1);
}

IEnumerable<int> EnumerateNeighbourValues(int x, int y)
{
    if (TryGetValue(x - 1, y, out int value)) yield return value;
    if (TryGetValue(x + 1, y, out value)) yield return value;
    if (TryGetValue(x, y - 1, out value)) yield return value;
    if (TryGetValue(x, y + 1, out value)) yield return value;
}

IEnumerable<(int x, int y)> EnumerateCoordinates()
{
    for (int y = 0; y < height; y++)
    for (int x = 0; x < width; x++)
        yield return (x, y);
}

bool TryGetValue(int x, int y, out int value)
{
    if (x < 0 || x >= width || y < 0 || y >= height)
    {
        value = -1;
        return false;
    }

    value = grid[y][x];
    return true;
}