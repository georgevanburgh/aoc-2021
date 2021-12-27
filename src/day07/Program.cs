var startingPositions = File.ReadLines("in.txt").First().Split(',').Select(int.Parse).ToList();
var max = startingPositions.Max();

var part1CostFunction = (int origin, int target) => Math.Abs(origin - target);
var optimumAlignment = Enumerable.Range(0, max).MinBy(p => CostToAlign(p, startingPositions, part1CostFunction));
var optimumCost = CostToAlign(optimumAlignment, startingPositions, part1CostFunction);

Console.WriteLine($"Part 1: {optimumAlignment} with cost of {optimumCost}");

var part2CostFunction = (int origin, int target) => SumToN(part1CostFunction(origin, target));
optimumAlignment = Enumerable.Range(0, max).MinBy(p => CostToAlign(p, startingPositions, part2CostFunction));
optimumCost = CostToAlign(optimumAlignment, startingPositions, part2CostFunction);

Console.WriteLine($"Part 2: {optimumAlignment} with cost of {optimumCost}");

static int CostToAlign(int alignmentPoint, List<int> positions, Func<int, int, int> cost) => positions.Sum(p => cost(p, alignmentPoint));

static int SumToN(int n) => (n * (n + 1)) / 2;
