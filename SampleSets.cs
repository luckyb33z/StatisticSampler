using System;

namespace SampleSets
{
    using Util;

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
}