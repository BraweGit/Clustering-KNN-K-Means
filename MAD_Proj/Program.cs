using System;
using System.Globalization;
using System.IO;
using System.Linq;
using static MAD_Proj.Methods;

namespace MAD_Proj
{
    class Program
    {
        static void Main(string[] args)
        {
            var print = true;
            var paramsToIgnore = new int[] { };

            int counterLines = 0;
            string line;
            var path = "network.csv";
            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(path);
            var linesCount = File.ReadLines(path).Count();

            string[][] data = new string[linesCount][];

            while ((line = file.ReadLine()) != null)
            {
                var split = line.Split(';');
                data[counterLines] = new string[NumOfParams];

                var counter = 0;
                for (int i = 0; i < NumOfParamsRaw; i++)
                {
                    if (paramsToIgnore.Any(x => x == i))
                        continue;
                    
                    data[counterLines][counter] = split[i];
                    counter++;
                }

                counterLines++;
            }

            var dataNumeric = new double[data.GetLength(0)][];

            for (int i = 0; i < data.GetLength(0); i++)
            {
                dataNumeric[i] = new double[NumOfParams];
                for (int j = 0; j < dataNumeric[i].Length; j++)
                {
                    if (data[i][j] == NotRecommend)
                        data[i][j] = "0";

                    if (data[i][j] == Recommend)
                        data[i][j] = "1";

                    var test = data[i][j];
                    dataNumeric[i][j] = float.Parse(data[i][j], CultureInfo.InvariantCulture.NumberFormat);
                }
                
                //dataNumeric[i][0] = float.Parse(data[i][0], CultureInfo.InvariantCulture.NumberFormat);
                //dataNumeric[i][1] = float.Parse(data[i][1], CultureInfo.InvariantCulture.NumberFormat);
                //dataNumeric[i][2] = float.Parse(data[i][2], CultureInfo.InvariantCulture.NumberFormat);
                //dataNumeric[i][3] = float.Parse(data[i][3], CultureInfo.InvariantCulture.NumberFormat);
            }

            //ShowData(dataNumeric, 1, true, true);

            // KNN

            //KNN knn = new KNN();

            //knn.LoadData("irisData.txt", KNN.DataType.TRAININGDATA);
            //knn.LoadData("testData.txt", KNN.DataType.TESTDATA);
            //knn.Classify(5);

            // K-Means

            int numClusters = 3;
            if(print)
                Console.WriteLine("\nNumber of clusters: " + numClusters);

            int numberOfKMeans = 5;
            int[][] clustering = new int[numberOfKMeans][];

            for (int i = 0; i < numberOfKMeans; i++)
            {
                clustering[i] = new int[data.GetLength(0)];
                clustering[i] = Cluster(dataNumeric, numClusters);
            }
            //int[] clustering = Cluster(dataNumeric, numClusters); // this is it

            //Console.WriteLine("Final clustering in internal form:\n");
            //ShowVector(clustering, true);

            if (print)
            {
                Console.WriteLine("Data in each cluster:\n");
                ShowClustered(dataNumeric, clustering[0], numClusters, 1);
            }
            

            // Statistic parameters

            var result = "";

            var average = GetAverage(data);
            var mean = GetMean(data);
            var dispersion = GetDispersion(average, data);
            var distance = GetDistance(data, average, NumOfParams);
            var cosineSim = GetCosineSimilarity(data, average, NumOfParams);
            var standardDeviation = dispersion;
            //var normalDistrib = GetRate(data, mean, standardDeviation);


            for (int i = 0; i < NumOfParams; i++)
            {
                standardDeviation[i] = Math.Sqrt(standardDeviation[i]);
            }



            result += "Average review is: ";
            if (print)
            {
                Console.WriteLine("Average review is: ");
                PrintResult(average);
            }
            
            result += PrintResultToFile(average);

            result += Environment.NewLine + "Mean review is: ";
            if (print)
            {
                Console.WriteLine("Mean review is: ");
                PrintResult(mean);
            }
            
            result += PrintResultToFile(mean);

            result += Environment.NewLine + "Dispersion is: ";
            if (print)
            {
                Console.WriteLine("Dispersion is: ");
                PrintResult(dispersion);
            }
            
            result += PrintResultToFile(dispersion);

            result += Environment.NewLine + "Standard deviation is: ";
            if (print)
            {
                Console.WriteLine("Standard deviation is: ");
                PrintResult(standardDeviation);
            }
            
            result += PrintResultToFile(standardDeviation);

            
            result += System.Environment.NewLine +
                "Distribution is: " + System.Environment.NewLine + GetRate(data, mean, standardDeviation);
            File.WriteAllText("Summary.txt", result);
            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
