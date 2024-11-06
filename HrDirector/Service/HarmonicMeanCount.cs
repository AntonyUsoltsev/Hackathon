namespace HrDirector.Service;

public class HarmonicMeanCount
{
    public static double CountHarmonicMean(List<int> numbers)
    {
        return numbers.Count / numbers.Sum(number => 1d / number);
    }
}