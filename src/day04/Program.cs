var moves = File.ReadLines("in.txt")
    .First()
    .Split(',', StringSplitOptions.TrimEntries)
    .Select(int.Parse)
    .ToList();

var rawBoards = File.ReadAllText("in.txt")
    .Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
    .Skip(1)
    .Select(raw => new Board(raw))
    .ToList();

bool foundWinning = false, foundLosing = false;
foreach (int move in moves)
{
    // Play the next move on all the boards
    rawBoards.ForEach(board => board.Play(move));

    // Check for first winning board
    var winningBoard = rawBoards.FirstOrDefault(board => board.HasCompletedRowOrColumn());
    if (winningBoard != null && !foundWinning)
    {
        Console.WriteLine($"Part 1: {winningBoard.Sum()} * {move} = {winningBoard.Sum() * move}");
        foundWinning = true;
    }

    // Check for last losing board
    var losingBoards = rawBoards.Where(board => !board.HasCompletedRowOrColumn());
    if (losingBoards.Count() == 1 && !foundLosing)
    {
        var losingBoard = losingBoards.Single();

        // Play the losing board until it completes
        var finishingMove = moves.First(move => 
        {
            losingBoard.Play(move);
            return losingBoard.HasCompletedRowOrColumn();
        });

        Console.WriteLine($"Part 2: {losingBoard.Sum()} * {finishingMove} = {losingBoard.Sum() * finishingMove}");
        foundLosing = true;
    }

    if (foundLosing && foundWinning)
        break;
}

record Board
{
    private readonly Tile[][] _board;
    private readonly int width;
    private readonly int height;

    public Board(string text)
    {
        var lines = text
            .Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray();
        height = lines.Length;
        width = lines[0].Length;
        _board = new Tile[height][];

        for (int i = 0; i < width; i++)
            _board[i] = new Tile[height];

        for (int x = 0; x < height; x++)
        for (int y = 0; y < width; y++)
            _board[x][y] = new Tile { Value = lines[x][y] };
    }

    public void Play(int value)
    {
        foreach (var tile in EnumerateTiles())
            if (tile.Value == value) tile.Chosen = true;
    }

    public bool HasCompletedRowOrColumn()
    {
        if (_board.Any(row => row.All(tile => tile.Chosen)))
            return true;

        if (Enumerable.Range(0, width).Any(i => _board.Select(row => row[i]).All(tile => tile.Chosen)))
            return true;

        return false;
    }

    public int Sum()
    {
        return EnumerateTiles()
            .Where(tile => !tile.Chosen)
            .Sum(tile => tile.Value);
    }

    private IEnumerable<Tile> EnumerateTiles()
    {
        for (int x = 0; x < height; x++)
        for (int y = 0; y < width; y++)
            yield return _board[x][y];

        yield break;
    }
}

record Tile
{
    public int Value { get; init; }
    public bool Chosen { get; set; }
}