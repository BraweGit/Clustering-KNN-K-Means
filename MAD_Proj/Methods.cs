using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAD_Proj
{
    public static class Methods
    {

        public static int NumOfParams = 2;
        public static int NumOfParamsRaw = 2;
        public static string Recommend = "Recommend";
        public static string NotRecommend = "Not Recommend";

        public static double GetNormalDistib(double x, double mean, double stddev, double var)
        {
            double one_over_2pi = (float)(1.0 /
                    (stddev * Math.Sqrt(2 * Math.PI)));

            return (double)(one_over_2pi *
                Math.Exp(-(x - mean) * (x - mean) / (2 * var)));
        }

        public static string GetRate(string[][] data, double[] average, double[] deviation, double min = 0, double max = Double.MaxValue)
        {
            double[][] columns = new double[NumOfParams][];

            var result = "";

            for(int k = 0; k < NumOfParams; k++)
            {
                columns[k] = new double[data.GetLength(0)];
            }

            //columns[0] = new double[data.GetLength(0)];
            //columns[1] = new double[data.GetLength(0)];
            //columns[2] = new double[data.GetLength(0)];
            //columns[3] = new double[data.GetLength(0)];

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for(int j = 0; j < NumOfParams; j++)
                {
                    if ((min <= float.Parse(data[i][j], CultureInfo.InvariantCulture.NumberFormat)) && (max >= float.Parse(data[i][j], CultureInfo.InvariantCulture.NumberFormat)))
                        columns[j][i] = float.Parse(data[i][j], CultureInfo.InvariantCulture.NumberFormat);
                }
                //if ((min <= float.Parse(data[i][0], CultureInfo.InvariantCulture.NumberFormat)) && (max >= float.Parse(data[i][0], CultureInfo.InvariantCulture.NumberFormat)))
                //    columns[0][i] = float.Parse(data[i][0], CultureInfo.InvariantCulture.NumberFormat);
                //if ((min <= float.Parse(data[i][1], CultureInfo.InvariantCulture.NumberFormat)) && (max >= float.Parse(data[i][1], CultureInfo.InvariantCulture.NumberFormat)))
                //    columns[1][i] = float.Parse(data[i][1], CultureInfo.InvariantCulture.NumberFormat);
                //if ((min <= float.Parse(data[i][2], CultureInfo.InvariantCulture.NumberFormat)) && (max >= float.Parse(data[i][2], CultureInfo.InvariantCulture.NumberFormat)))
                //    columns[2][i] = float.Parse(data[i][2], CultureInfo.InvariantCulture.NumberFormat);
                //if ((min <= float.Parse(data[i][3], CultureInfo.InvariantCulture.NumberFormat)) && (max >= float.Parse(data[i][3], CultureInfo.InvariantCulture.NumberFormat)))
                //    columns[3][i] = float.Parse(data[i][3], CultureInfo.InvariantCulture.NumberFormat);
            }

            //var groups = columns[0].GroupBy(v => v);
            //foreach (var group in groups)
            //{
            //    result += "" + group.Key + ";" + GetNormalDistib(group.Key, average[0], deviation[0], Math.Pow(deviation[0], 2)) + ";" + group.Count() + System.Environment.NewLine;
            //}

            int clmnCounter = 1;
            foreach(var c in columns)
            {
                result += Environment.NewLine + "Distribution of " + clmnCounter + ". attribute is: " + Environment.NewLine;
                var temp = c.GroupBy(v => v);
                foreach (var t in temp)
                    result += "" + t.Key + ";" + t.Count() + System.Environment.NewLine;

                result += Environment.NewLine;
                clmnCounter++;
            }

            //var groups1 = columns[1].GroupBy(v => v);
            //foreach (var group in groups1)
            //    result += "" + group.Key + ";" + group.Count() + System.Environment.NewLine;

            //var groups2 = columns[2].GroupBy(v => v);
            //foreach (var group in groups2)
            //    result += "" + group.Key + ";" + group.Count() + System.Environment.NewLine;

            //var groups3 = columns[3].GroupBy(v => v);
            //foreach (var group in groups3)
            //    result += "" + group.Key + ";" + group.Count() + System.Environment.NewLine;


            //File.AppendAllText("ValueCount.txt", result);
            return result;
            //return columns;
        }

        public static double[] GetCosineSimilarity(string[][] data, double[] average, int len)
        {
            double[] result = new double[data.GetLength(0)];
            double sim;

            double dot = 0.0d;
            double mag1 = 0.0d;
            double mag2 = 0.0d;

            for (int i = 0; i < data.GetLength(0); i++)
            {
                sim = 0.0d;

                for (int j = 0; j < len; j++)
                {
                    var tmp = float.Parse(data[i][j], CultureInfo.InvariantCulture.NumberFormat);
                    dot += tmp * average[j];
                    mag1 += Math.Pow(tmp, 2);
                    mag2 += Math.Pow(average[j], 2);
                }

                result[i] = dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
            }

            return result;
        }

        public static double GetDistance(string[] data, double[] average, int len)
        {
            double distance = 0.0d;
            for (int j = 0; j < len; j++)
            {
                var tmp = float.Parse(data[j], CultureInfo.InvariantCulture.NumberFormat);
                distance += Math.Pow((tmp - average[j]), 2);
            }
            distance = Math.Sqrt(distance);

            return distance;
        }

        public static double[] GetDistance(string[][] data, double[] average, int len)
        {
            double distance;
            double[] result = new double[data.GetLength(0)];

            for (var i = 0; i < data.GetLength(0); i++)
            {
                distance = 0.0d;
                for (int j = 0; j < len; j++)
                {
                    var tmp = float.Parse(data[i][j], CultureInfo.InvariantCulture.NumberFormat);
                    distance += Math.Pow((tmp - average[j]), 2);
                }
                result[i] = Math.Sqrt(distance);
            }
            return result;
        }

        public static double[] GetMean(string[][] data)
        {
            double[] mean = new double[NumOfParams];
            double[][] sorted = new double[NumOfParams][];

            for (int i = 0; i < NumOfParams; i++)
            {
                sorted[i] = new double[data.GetLength(0)];

                for (int j = 0; j < data.GetLength(0); j++)
                {
                    sorted[i][j] = float.Parse(data[j][i], CultureInfo.InvariantCulture.NumberFormat);
                }

                sorted[i] = sorted[i].OrderBy(c => c).ToArray();
            }

            if (data.GetLength(0) % 2 == 0)
            {
                for (int i = 0; i < NumOfParams; i++)
                {
                    mean[i] = (sorted[i][data.GetLength(0) / 2] + sorted[i][data.GetLength(0) / 2]) / 2;
                }
            }
            else
            {
                for (int i = 0; i < NumOfParams; i++)
                {
                    mean[i] = sorted[i][(data.GetLength(0) / 2) + 1];
                }
            }

            return mean;
        }


        public static double[] GetAverage(string[][] data)
        {
            double[] sum = new double[NumOfParams];
            double tempSum = 0.0d;
            for (int j = 0; j < data.GetLength(0); j++)
            {
                for (int i = 0; i < NumOfParams; i++)
                {
                    var tmp = float.Parse(data[j][i], CultureInfo.InvariantCulture.NumberFormat);
                    sum[i] += tmp;
                }
            }

            for (int i = 0; i < sum.Length; i++)
            {
                sum[i] = sum[i] / data.GetLength(0);
                //Console.WriteLine(sum[i]);
            }

            return sum;
        }

        public static double[] GetDispersion(double[] average, string[][] data)
        {
            double[] result = new double[NumOfParams];

            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < data.GetLength(0); j++)
                {
                    var x = average[i];
                    result[i] += Math.Pow(float.Parse(data[j][i], CultureInfo.InvariantCulture.NumberFormat) - average[i], 2);
                }

                result[i] = (1d / data.GetLength(0)) * result[i];
                //Console.WriteLine(result[i]);
            }


            return result;
        }

        public static void K_Means(int k, string[][] data)
        {
            // if clusters changed
            var changed = false;
            var centroids = GetCentroids(data, k);
            var clusters = new double[k][];

            // initialize clusters
            for (int i = 0; i < k; i++)
            {
                clusters[i] = new double[data.GetLength(0)];
            }

            // do while clusters changing
            while (!changed)
            {
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    // choose best cluster
                    var distance = Double.MaxValue;
                    var bestCluster = 0;
                    for (int j = 0; i < k; j++)
                    {
                        if (GetDistance(data[i], clusters[j], NumOfParams) < distance)
                        {
                            bestCluster = j;
                            var x = data[i];
                            distance = GetDistance(data[i], clusters[j], NumOfParams);
                        }
                    }

                }

            }
        }

        public static double[][] GetCentroids(string[][] data, int k)
        {
            double[][] result = new double[k][];
            var r = new Random();
            for (int i = 0; i < k; i++)
            {
                result[i] = new double[NumOfParams];
                int index = r.Next(data.GetLength(0));
                result[i][0] = float.Parse(data[index][0], CultureInfo.InvariantCulture.NumberFormat);
                result[i][1] = float.Parse(data[index][0], CultureInfo.InvariantCulture.NumberFormat);
                result[i][2] = float.Parse(data[index][0], CultureInfo.InvariantCulture.NumberFormat);
                result[i][3] = float.Parse(data[index][0], CultureInfo.InvariantCulture.NumberFormat);
            }


            return result;
        }


        // ============================================================================

        public static int[] Cluster(double[][] rawData, int numClusters)
        {
            // k-means clustering
            // index of return is tuple ID, cell is cluster ID
            // ex: [2 1 0 0 2 2] means tuple 0 is cluster 2, tuple 1 is cluster 1, tuple 2 is cluster 0, tuple 3 is cluster 0, etc.
            // an alternative clustering DS to save space is to use the .NET BitArray class
            double[][] data = Normalized(rawData); // so large values don't dominate

            bool changed = true; // was there a change in at least one cluster assignment?
            bool success = true; // were all means able to be computed? (no zero-count clusters)

            // init clustering[] to get things started
            // an alternative is to initialize means to randomly selected tuples
            // then the processing loop is
            // loop
            //    update clustering
            //    update means
            // end loop
            int[] clustering = InitClustering(data.Length, numClusters, 0); // semi-random initialization
            double[][] means = Allocate(numClusters, data[0].Length); // small convenience

            int maxCount = data.Length * 10; // sanity check
            int ct = 0;
            while (changed == true && success == true && ct < maxCount)
            {
                ++ct; // k-means typically converges very quickly
                success = UpdateMeans(data, clustering, means); // compute new cluster means if possible. no effect if fail
                changed = UpdateClustering(data, clustering, means); // (re)assign tuples to clusters. no effect if fail
            }
            // consider adding means[][] as an out parameter - the final means could be computed
            // the final means are useful in some scenarios (e.g., discretization and RBF centroids)
            // and even though you can compute final means from final clustering, in some cases it
            // makes sense to return the means (at the expense of some method signature uglinesss)
            //
            // another alternative is to return, as an out parameter, some measure of cluster goodness
            // such as the average distance between cluster means, or the average distance between tuples in 
            // a cluster, or a weighted combination of both
            return clustering;
        }

        private static double[][] Normalized(double[][] rawData)
        {
            // normalize raw data by computing (x - mean) / stddev
            // primary alternative is min-max:
            // v' = (v - min) / (max - min)

            // make a copy of input data
            double[][] result = new double[rawData.Length][];
            for (int i = 0; i < rawData.Length; ++i)
            {
                result[i] = new double[rawData[i].Length];
                Array.Copy(rawData[i], result[i], rawData[i].Length);
            }

            for (int j = 0; j < result[0].Length; ++j) // each col
            {
                double colSum = 0.0;
                for (int i = 0; i < result.Length; ++i)
                    colSum += result[i][j];
                double mean = colSum / result.Length;
                double sum = 0.0;
                for (int i = 0; i < result.Length; ++i)
                    sum += (result[i][j] - mean) * (result[i][j] - mean);
                double sd = sum / result.Length;
                for (int i = 0; i < result.Length; ++i)
                    result[i][j] = (result[i][j] - mean) / sd;
            }
            return result;
        }

        private static int[] InitClustering(int numTuples, int numClusters, int randomSeed)
        {
            Random random = new Random(randomSeed);
            int[] clustering = new int[numTuples];
            for (int i = 0; i < numClusters; ++i) // make sure each cluster has at least one tuple
                clustering[i] = i;
            for (int i = numClusters; i < clustering.Length; ++i)
                clustering[i] = random.Next(0, numClusters); // other assignments random
            return clustering;
        }

        private static double[][] Allocate(int numClusters, int numColumns)
        {
            // convenience matrix allocator for Cluster()
            double[][] result = new double[numClusters][];
            for (int k = 0; k < numClusters; ++k)
                result[k] = new double[numColumns];
            return result;
        }

        private static bool UpdateMeans(double[][] data, int[] clustering, double[][] means)
        {
            // returns false if there is a cluster that has no tuples assigned to it
            // parameter means[][] is really a ref parameter

            // check existing cluster counts
            // can omit this check if InitClustering and UpdateClustering
            // both guarantee at least one tuple in each cluster (usually true)
            int numClusters = means.Length;
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = clustering[i];
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < numClusters; ++k)
                if (clusterCounts[k] == 0)
                    return false; // bad clustering. no change to means[][]

            // update, zero-out means so it can be used as scratch matrix 
            for (int k = 0; k < means.Length; ++k)
                for (int j = 0; j < means[k].Length; ++j)
                    means[k][j] = 0.0;

            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = clustering[i];
                for (int j = 0; j < data[i].Length; ++j)
                    means[cluster][j] += data[i][j]; // accumulate sum
            }

            for (int k = 0; k < means.Length; ++k)
                for (int j = 0; j < means[k].Length; ++j)
                    means[k][j] /= clusterCounts[k]; // danger of div by 0
            return true;
        }

        private static bool UpdateClustering(double[][] data, int[] clustering, double[][] means)
        {
            // (re)assign each tuple to a cluster (closest mean)
            // returns false if no tuple assignments change OR
            // if the reassignment would result in a clustering where
            // one or more clusters have no tuples.

            int numClusters = means.Length;
            bool changed = false;

            int[] newClustering = new int[clustering.Length]; // proposed result
            Array.Copy(clustering, newClustering, clustering.Length);

            double[] distances = new double[numClusters]; // distances from curr tuple to each mean

            for (int i = 0; i < data.Length; ++i) // walk thru each tuple
            {
                for (int k = 0; k < numClusters; ++k)
                {
                    distances[k] = Distance(data[i], means[k]); // compute distances from curr tuple to all k means
                }


                int newClusterID = MinIndex(distances); // find closest mean ID
                if (newClusterID != newClustering[i])
                {
                    changed = true;
                    newClustering[i] = newClusterID; // update
                }
            }

            /*
            var sumDistances = 0d;
            for(int k = 0; k < numClusters; k++)
            {
                sumDistances += distances[k] * distances[k];
            }*/

            //Console.WriteLine("Clustering quality is: {0}", sumDistances);
            if (changed == false)
                return false; // no change so bail and don't update clustering[][]

            // check proposed clustering[] cluster counts
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < data.Length; ++i)
            {
                int cluster = newClustering[i];
                ++clusterCounts[cluster];
            }

            for (int k = 0; k < numClusters; ++k)
                if (clusterCounts[k] == 0)
                    return false; // bad clustering. no change to clustering[][]

            Array.Copy(newClustering, clustering, newClustering.Length); // update
            return true; // good clustering and at least one change
        }

        private static double Distance(double[] tuple, double[] mean)
        {
            // Euclidean distance between two vectors for UpdateClustering()
            // consider alternatives such as Manhattan distance
            double sumSquaredDiffs = 0.0;
            for (int j = 0; j < tuple.Length; ++j)
                sumSquaredDiffs += Math.Pow((tuple[j] - mean[j]), 2);

            return Math.Sqrt(sumSquaredDiffs);
        }

        private static int MinIndex(double[] distances)
        {
            // index of smallest value in array
            // helper for UpdateClustering()
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < smallDist)
                {
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }

        // ============================================================================

        // misc display helpers for demo

        public static void ShowData(double[][] data, int decimals, bool indices, bool newLine)
        {
            for (int i = 0; i < data.Length; ++i)
            {
                if (indices) Console.Write(i.ToString().PadLeft(3) + " ");
                for (int j = 0; j < data[i].Length; ++j)
                {
                    if (data[i][j] >= 0.0) Console.Write(" ");
                    Console.Write(data[i][j].ToString("F" + decimals) + " ");
                }
                Console.WriteLine("");
            }
            if (newLine) Console.WriteLine("");
        } // ShowData

        public static void ShowVector(int[] vector, bool newLine)
        {
            for (int i = 0; i < vector.Length; ++i)
                Console.Write(vector[i] + " ");
            if (newLine) Console.WriteLine("\n");
        }

        public static void ShowClustered(double[][] data, int[] clustering, int numClusters, int decimals)
        {
            for (int k = 0; k < numClusters; ++k)
            {
                Console.WriteLine("Cluster {0}", k + 1);
                Console.WriteLine("=======================");
                for (int i = 0; i < data.Length; ++i)
                {
                    int clusterID = clustering[i];
                    if (clusterID != k) continue;
                    Console.Write(i.ToString().PadLeft(3) + " ");
                    for (int j = 0; j < data[i].Length; ++j)
                    {
                        if (data[i][j] >= 0.0) Console.Write(" ");
                        Console.Write(data[i][j].ToString("F" + decimals) + " ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("=======================");
            } // k
        }

        public static void PrintResult(double[] toPrint)
        {
            for (int i = 0; i < NumOfParams; i++)
            {
                if (i == 0)
                    Console.Write(toPrint[i]);
                Console.Write("; " + toPrint[i]);
            }

            Console.WriteLine("\n====================================================================================\n\n");
        }

        public static string PrintResultToFile(double[] toPrint)
        {
            var result = "";
            for (int i = 0; i < NumOfParams; i++)
            {
                if (i == 0)
                    result += toPrint[i];
                result += "; " + toPrint[i];
            }

            return result += Environment.NewLine + "====================================================================================\n\n";
        }
    }
}
