using System.Collections;

var input = File.ReadLines("in.txt");

var bitValues = input.Select(s => s.Select(c => c == '1').ToArray());
var length = bitValues.First().Count();

// Part 1

// Slice through each column, and take the most common bit
var part1 = Enumerable.Range(0, length)
    .Select(i => bitValues.Select(a => a[i]).GroupBy(b => b).MaxBy(g => g.Count()).Key).Reverse().ToArray();

var bitArray = new BitArray(part1);
var gamma = ToInteger(bitArray);
var epsilon = ToInteger(bitArray.Not());

Console.WriteLine($"Part 1: {gamma} * {epsilon} = {gamma * epsilon}");

// Part 2

int GetRating(IEnumerable<IEnumerable<bool>> values, bool findMostCommon)
{
    var candidates = bitValues.ToList();
    for (int i = 0; i < length; i++)
    {
        var bits = candidates.Select(bv => bv[i]);
        var mostCommon = findMostCommon ? MostCommonBit(bits) : !MostCommonBit(bits);
        candidates.RemoveAll(c => c[i] != mostCommon);

        if (candidates.Count == 1)
        {
            return ToInteger(new BitArray(candidates.Single().Reverse().ToArray()));
        }
    }

    throw new Exception("No single rating found");
}

var ox = GetRating(bitValues, true);
var co2 = GetRating(bitValues, false);

Console.WriteLine($"Part 2: {ox} * {co2} = {ox * co2}");

bool MostCommonBit(IEnumerable<bool> array)
{
    var t = array.Count(b => b);
    var f = array.Count(b => !b);

    return t >= f ? true : false;
}

int ToInteger(BitArray array)
{
    var resultArray = new int[1];
    array.CopyTo(resultArray, 0);
    return resultArray[0];
}