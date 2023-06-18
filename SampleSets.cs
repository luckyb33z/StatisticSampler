using System;
using System.Collections.Generic;

namespace SampleSets
{
    using Util;

    public static class SampleUtil
    {
        /// <summary>
        /// From a given 2D array (like an array of strata with samples in each strata), flatten into a single array.
        /// </summary>
        public static int[] FlattenSample(ref int[][] sampleToFlatten)
        {
            int numStrata = sampleToFlatten.Length;
            int samplesPerStrata = sampleToFlatten[0].Length;
            int sampleLength = numStrata * samplesPerStrata;
            int[] allSamples = new int[sampleLength];

            int sampleIndex = 0;
            for (int strata = 0; strata < numStrata; strata++)
            {
                for (int sample = 0; sample < samplesPerStrata; sample++)
                {   
                    allSamples[sampleIndex++] = sampleToFlatten[strata][sample];
                }
            }

            return allSamples;
        }
    }

    abstract public class Sample<T>
    {
        private T _sampleStorage;

        public T SampleStorage
        {
            get {return _sampleStorage;}
            protected set {_sampleStorage = value;}
        }

        public int NumSamples {get; protected set;}

        public Sample(int numSamples) {NumSamples = numSamples;}

        abstract public void BuildSamples(ref int[][] samplesToRead);
        abstract public void ReportSamples();
    }

    public class StratifiedSample: Sample<int[]>
    {

        public StratifiedSample(int numSamples): base(numSamples) {}

        public override void BuildSamples(ref int[][] stratums)
        {
            int numStratums = stratums.Length;
            int entriesPerStratum = stratums[0].Length;

            int[] samples = new int [numStratums * NumSamples];

            int sampleIndex = 0;
            for (int stratum = 0; stratum < stratums.Length; stratum++)
            {
                for (int sample = 0; sample < NumSamples; sample++)
                {
                    int randomSample = Util.Rand.Next(entriesPerStratum);
                    samples[sampleIndex++] = randomSample;
                }
            }

            SampleStorage = samples;
        }

        public override void ReportSamples()
        {
            try
            {
                string stratifiedSample = Util.GetSamples(SampleStorage);
                Console.WriteLine($"Stratified sample: {stratifiedSample}");
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Cannot report stratified sample; initial data failed to load.");
            }
        }

    }

    public class ClusterSample: Sample<int[][]>
    {
        public ClusterSample(int numSamples): base(numSamples) {}

        public override void BuildSamples(ref int[][] stratums)
        {
            int numStratums = stratums.Length;

            int[][] clusters = new int [NumSamples][];

            int clusterGrabbed = -1;
            for (int cluster = 0; cluster < NumSamples; cluster++)
            {
                int columnIndex = Util.Rand.Next(numStratums);
                while (columnIndex == clusterGrabbed)
                {
                    columnIndex = Util.Rand.Next(numStratums);
                }
                clusters[cluster] = stratums[columnIndex];
                clusterGrabbed = columnIndex;
            }

            SampleStorage = clusters;
        }

        public override void ReportSamples()
        {
            try
            {
                foreach (var cluster in SampleStorage)
                {
                    Console.WriteLine($"Cluster sample: {Util.GetSamples(cluster)}");
                }
            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("Cannot report cluster sample; initial data failed to load.");
            }

        }
    }

    public class SimpleRandomSample: Sample<int[]>
    {
        public SimpleRandomSample(int numSamples): base(numSamples) {}

        public override void BuildSamples(ref int[][] samplesToRead)
        {
            int[] allSamples = SampleUtil.FlattenSample(ref samplesToRead);

            List<int> samplesTaken = new List<int>();
            int[] samplesToReturn = new int[NumSamples];

            for (int i = 0; i < NumSamples; i++)
            {
                int randomIndex = -1;
                
                do
                {
                    randomIndex = Util.Rand.Next(allSamples.Length);
                } while (samplesTaken.Contains(randomIndex));

                samplesTaken.Add(randomIndex);
                samplesToReturn[i] = allSamples[randomIndex];
            }

            SampleStorage = samplesToReturn;
                
        }

        public override void ReportSamples()
        {
            Console.WriteLine($"Simple Random Sample: {Util.GetSamples(SampleStorage)}");
        }
    }

    public class SystematicSample: Sample<int[]>
    {
        private int _startValue = 0;
        private int _increment = 1;

        public SystematicSample(int numSamples, int startValue = 0, int increment = 1): base(numSamples)
        {
            _startValue = startValue;
            _increment = increment;
        }

        public override void BuildSamples(ref int[][] samplesToRead)
        {
            int[] allSamples = SampleUtil.FlattenSample(ref samplesToRead);

            int indexToGet = _startValue;
            int[] systematicSamples = new int[NumSamples];
            for (int i = 0; i < NumSamples; i++)
            {
                if (indexToGet >= allSamples.Length)
                {
                    // Check if we will get a meaningful index on the next wraparound.
                    // If the modulo is zero, then we'll just be repeating numbers...
                    // If not, we're ok.
                    if (allSamples.Length % _increment == 0)
                    {
                        indexToGet = Util.Rand.Next(allSamples.Length);
                    }
                    else
                    {
                        indexToGet -= allSamples.Length;
                    }
                }

                systematicSamples[i] = allSamples[indexToGet];

                indexToGet += _increment;
            }

            SampleStorage = systematicSamples;
        }

        public override void ReportSamples()
        {
            Console.WriteLine($"Systematic Sample (Start: {_startValue}, Increment: {_increment}): {Util.GetSamples(SampleStorage)}");
        }
    }
}