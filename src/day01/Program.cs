using MoreLinq;

// Part 1
var input = File.ReadLines("ex1.txt").Select(int.Parse);
var part1 = input.Pairwise((first, second) => second > first ? 1 : 0).Sum();
Console.WriteLine($"Part 1: {part1}");

// Part 2

var part2 = input.Window(3).Pairwise((first, second) => second.Sum() > first.Sum() ? 1 : 0).Sum();
Console.WriteLine($"Part 2: {part2}");