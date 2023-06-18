using System;
using SampleSets;

namespace SampleExamples
{
    using Util;

    class StatisticSampler
    {
        static int[][] stratums;

        static void Main(string[] args)
        {

            int numStratums = 6;
            
            BuildMainData(out stratums, numStratums);

            int numStrataSamples = 3;
            SampleSets.StratifiedSample stratifiedSample = new StratifiedSample(numStrataSamples);
            stratifiedSample.BuildSamples(ref stratums);
            stratifiedSample.ReportSamples();

            int numClusterSamples = 2;
            SampleSets.ClusterSample clusterSample = new ClusterSample(numClusterSamples);
            clusterSample.BuildSamples(ref stratums);
            clusterSample.ReportSamples();

            int numSimpleRandomSamples = 15;
            SampleSets.SimpleRandomSample simpleRandomSample = new SimpleRandomSample(numSimpleRandomSamples);
            simpleRandomSample.BuildSamples(ref stratums);
            simpleRandomSample.ReportSamples();

            int numSystematicSamples = 12;
            int samplesLength = stratums.Length * stratums[0].Length;
            int startValue = Util.Rand.Next(samplesLength);
            int increment = 10;
            SampleSets.SystematicSample systematicSample = new SystematicSample(numSystematicSamples, startValue, increment);
            systematicSample.BuildSamples(ref stratums);
            systematicSample.ReportSamples();
            
        }

        static void BuildMainData(out int[][] stratums, int numStratums)
        {
            stratums = new int[numStratums][];

            // Modify this later if I want to manually put in data

            stratums[0] = new int [] { 5, 10, 9, 9, 7, 9, 7, 8, 9, 8 };
            stratums[1] = new int [] { 7, 5, 10, 10, 8, 9, 7 ,8, 7, 8 };
            stratums[2] = new int [] { 10, 9, 8, 10, 9, 9, 10, 9, 8, 10 };
            stratums[3] = new int [] { 9, 8, 6, 9, 5, 10, 9, 10, 7, 9 };
            stratums[4] = new int [] { 8, 7, 7, 8, 7, 8, 8, 8, 7, 8 };
            stratums[5] = new int [] { 3, 6, 9, 9, 4, 7, 8, 8, 8, 7 };
        }

        static void ReportSimpleRandomSamples(int numSimpleRandomSamples)
        {
            throw new NotImplementedException();
        }

    }
}
