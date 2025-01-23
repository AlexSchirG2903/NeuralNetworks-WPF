using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace NeuralNetworks_Lab1
{
    class nn3S
    {
        // Gewichts-Matrizen
        double[,] wih, who; // Input-to-Hidden und Hidden-to-Output Gewichtungen

        // Anzahl der Neuronen in den Schichten
        int inodes, hnodes, onodes;

        int hnodes_count;

        // Eingabe-, Hidden- und Output-Werte
        double[] hidden_inputs;    // Eingabewerte der Hidden Layer
        double[] hidden_outputs;   // Ausgabewerte der Hidden Layer
        double[] final_inputs;     // Eingabewerte der Output Layer
        double[] final_outputs;    // Ausgabewerte der Output Layer

        // Fehlerwerte
        double[] hidden_errors;    // Fehler der Hidden Layer
        double[] output_errors;    // Fehler der Output Layer

        private Dictionary<string, double[]> hidden_inputs_dict;
        private Dictionary<string, double[]> hidden_outputs_dict;
        private Dictionary<string, double[]> hidden_errors_dict;
        private Dictionary<string, double[,]> weightMatrices_betweenLayer;

        // Lernrate
        double learningRate;

        // Instanz der nnMath-Klasse für mathematische Operationen
        nnMath nnMathO = new nnMath();

        // Properties für den Zugriff von außen
        public double[] Hidden_inputs { get { return hidden_inputs; } }
        public double[] Hidden_outputs { get { return hidden_outputs; } }
        public double[] Final_inputs { get { return final_inputs; } }
        public double[] Final_outputs { get { return final_outputs; } }

        public double[] Hidden_errors { get { return hidden_errors; } }
        public double[] Output_errors { get { return output_errors; } }
        public double[,] WIH { get { return wih; } }
        public double[,] WHO { get { return who; } }

        // Konstruktor der Klasse
        public nn3S(int inodes, int hnodes, int onodes, double learningRate)
        {
            this.inodes = inodes;
            this.hnodes = hnodes;
            this.onodes = onodes;
            this.learningRate = learningRate;

            // Initialisierung der Gewichtsmatrizen und Arrays
            hidden_inputs = new double[hnodes];
            hidden_outputs = new double[hnodes];
            final_inputs = new double[onodes];
            final_outputs = new double[onodes];
            hidden_errors = new double[hnodes];
            output_errors = new double[onodes];

            createWeightMatrizes();
        }



        // Initialisierung der Gewichtsmatrizen
        private void createWeightMatrizes()
        {
            wih = new double[hnodes, inodes];
            who = new double[onodes, hnodes];

            // Eine einzige Instanz von Random erzeugen
            Random random = new Random();

            // Gewichte für Input-Hidden-Schicht initialisieren
            for (int j = 0; j < hnodes; j++)
            {
                for (int i = 0; i < inodes; i++)
                {
                    wih[j, i] = random.NextDouble() * 2.0 - 1.0; // Werte im Bereich [-1.0, 1.0]
                }
            }

            // Gewichte für Hidden-Output-Schicht initialisieren
            for (int j = 0; j < onodes; j++)
            {
                for (int i = 0; i < hnodes; i++)
                {
                    who[j, i] = random.NextDouble() * 2.0 - 1.0; // Werte im Bereich [-1.0, 1.0]
                }
            }
        }

        // Berechnung der Vorwärtsausbreitung
        public void queryNN(double[] inputs)
        {
            // Berechnung der Eingabewerte der Hidden Layer
            hidden_inputs = nnMathO.matrixMult(wih, inodes, inputs);

            // Berechnung der Ausgabewerte der Hidden Layer
            hidden_outputs = nnMathO.activationFunction(hidden_inputs);

            // Berechnung der Eingabewerte der Output Layer
            final_inputs = nnMathO.matrixMult(who, hnodes, hidden_outputs);

            // Berechnung der Ausgabewerte der Output Layer
            final_outputs = nnMathO.activationFunction(final_inputs);
        }

        // Trainingsmethode
        public void train(double[] inputs, double[] targets, double learningratetemp)


        {
            learningRate = learningratetemp;

            // 1. Forward Propagation
            queryNN(inputs);

            // 2. Fehler im Output Layer berechnen
            output_errors = nnMathO.CalculateOutputErrors(targets, final_outputs);

            // 3. Fehler im Hidden Layer berechnen
            hidden_errors = nnMathO.CalculateHiddenError(who, output_errors);

            // 4. Gradienten für die Ausgabeschicht berechnen
            double[] outputGradients = new double[onodes];
            for (int i = 0; i < onodes; i++)
            {
                outputGradients[i] = output_errors[i] * final_outputs[i] * (1 - final_outputs[i]);
            }

            // 5. Gradienten für die versteckte Schicht berechnen
            double[] hiddenGradients = new double[hnodes];
            for (int i = 0; i < hnodes; i++)
            {
                hiddenGradients[i] = hidden_errors[i] * hidden_outputs[i] * (1 - hidden_outputs[i]);
            }

            // 6. Gewichtsanpassung für Hidden-to-Output
            for (int i = 0; i < hnodes; i++)
            {
                for (int j = 0; j < onodes; j++)
                {
                    who[j, i] += learningRate * outputGradients[j] * hidden_outputs[i];
                }
            }

            // 7. Gewichtsanpassung für Input-to-Hidden
            for (int i = 0; i < inodes; i++)
            {
                for (int j = 0; j < hnodes; j++)
                {
                    wih[j, i] += learningRate * hiddenGradients[j] * inputs[i];
                }
            }
        }
        public void setWIHMatrix(int i, int j, double value)
        {
            if (j < 0 || j >= wih.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(i), "Index i liegt außerhalb des gültigen Bereichs.");

            if (i < 0 || i >= wih.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(j), "Index j liegt außerhalb des gültigen Bereichs.");

            wih[j, i] = value;
        }

        public void setWHOMatrix(int i, int j, double value)
        {
            if (j < 0 || j >= who.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(j), "Index i liegt außerhalb des gültigen Bereichs.");

            if (i < 0 || i >= who.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(i), "Index j liegt außerhalb des gültigen Bereichs.");

            who[j, i] = value;
        }



        //// ##########################################################################
        ///Wenn ich mehr Hidden_layer habe 
        /// ##########################################################################
        public nn3S(int inodes, int hnodes, int onodes, double learningRate, int hnodes_count)
        {

            this.inodes = inodes;
            this.hnodes = hnodes;
            this.onodes = onodes;
            this.learningRate = learningRate;
            this.hnodes_count = hnodes_count;

            hidden_inputs_dict = new Dictionary<string, double[]>();
            hidden_outputs_dict = new Dictionary<string, double[]>();
            hidden_errors_dict = new Dictionary<string, double[]>();


            for (int i = 0; i < hnodes_count; i++)
            {
                hidden_inputs_dict[$"hidden_{i + 1}_inputs"] = new double[hnodes];
                hidden_outputs_dict[$"hidden_{i + 1}_outputs"] = new double[hnodes];
                hidden_errors_dict[$"hidden_{i + 1}_error"] = new double[hnodes];

            }

            // Initialisierung der Gewichtsmatrizen und Arrays

            final_inputs = new double[onodes];
            final_outputs = new double[onodes];
            output_errors = new double[onodes];
            createWeightMatrizes_moreLayer();

        }




        private void createWeightMatrizes_moreLayer()
        {
            wih = new double[hnodes, inodes];
            who = new double[onodes, hnodes];

            weightMatrices_betweenLayer = new Dictionary<string, double[,]>();



            // Eine einzige Instanz von Random erzeugen
            Random random = new Random();

            // Gewichte für Input-Hidden-Schicht initialisieren
            for (int j = 0; j < hnodes; j++)
            {
                for (int i = 0; i < inodes; i++)
                {
                    wih[j, i] = random.NextDouble() * 2.0 - 1.0; // Werte im Bereich [-1.0, 1.0]
                }
            }


            for (int i = 1; i < hnodes_count; i++)
            {
                // Schlüssel für die aktuelle Gewichtsmatrix
                string key = $"Hidden{i}-Hidden{i + 1}";

                // Gewichtsmatrix initialisieren
                double[,] weights = new double[hnodes, hnodes];

                // Werte der Gewichtsmatrix füllen
                for (int j = 0; j < hnodes; j++)
                {
                    for (int k = 0; k < hnodes; k++)
                    {
                        weights[j, k] = random.NextDouble() * 2.0 - 1.0; // Werte im Bereich [-1.0, 1.0]
                    }
                }
                weightMatrices_betweenLayer[key] = weights;
            }

            // Gewichte für Hidden-Output-Schicht initialisieren
            for (int j = 0; j < onodes; j++)
            {
                for (int i = 0; i < hnodes; i++)
                {
                    who[j, i] = random.NextDouble() * 2.0 - 1.0; // Werte im Bereich [-1.0, 1.0]
                }
            }
        }

        // Berechnung der Vorwärtsausbreitung
        public void queryNN_moreLayer(double[] inputs)
        {
            // Berechnung der Eingabewerte der Hidden Layer
            hidden_inputs = nnMathO.matrixMult(wih, inodes, inputs);

            // Berechnung der Ausgabewerte der Hidden Layer
            hidden_outputs = nnMathO.activationFunction(hidden_inputs);





            for (int i = 1; i < hnodes_count; i++)
            {
                // Schlüssel für die aktuelle Gewichtsmatrix und Hidden Layer
                string key = $"Hidden{i}-Hidden{i + 1}";
                string currentInputKey = $"hidden_{i + 1}_inputs";
                string currentOutputKey = $"hidden_{i + 1}_outputs";

                if (i + 1 == 2)
                {
                    hidden_inputs_dict[currentInputKey] = nnMathO.matrixMult(weightMatrices_betweenLayer[key], hidden_outputs.Length, hidden_outputs);
                    hidden_outputs_dict[currentOutputKey] = nnMathO.activationFunction(hidden_inputs_dict[currentInputKey]);

                }
                else
                {
                    // Berechnung der Eingabewerte der aktuellen Hidden Layer
                    hidden_inputs_dict[currentInputKey] = nnMathO.matrixMult(weightMatrices_betweenLayer[key], hidden_outputs_dict[$"hidden_{i}_outputs"].Length, hidden_outputs_dict[$"hidden_{i}_outputs"]);

                    // Berechnung der Ausgabewerte der aktuellen Hidden Layer
                    hidden_outputs_dict[currentOutputKey] = nnMathO.activationFunction(hidden_inputs_dict[currentInputKey]);
                }



            }




            // Berechnung der Eingabewerte der Output Layer
            final_inputs = nnMathO.matrixMult(who, hidden_outputs_dict[$"hidden_{hnodes_count}_outputs"].Length, hidden_outputs_dict[$"hidden_{hnodes_count}_outputs"]);
            // Berechnung der Ausgabewerte der Output Layer
            final_outputs = nnMathO.activationFunction(final_inputs);
        }



        // Trainingsmethode
        public void train_moreLayer(double[] inputs, double[] targets, double learningratetemp)


        {
            learningRate = learningratetemp;

            // 1. Forward Propagation
            queryNN_moreLayer(inputs);

            // 2. Fehler im Output Layer berechnen
            output_errors = nnMathO.CalculateOutputErrors(targets, final_outputs);

            // 3. Backward Propagation für den Output Layer
            double[] outputGradients = new double[onodes];
            for (int i = 0; i < onodes; i++)
            {
                outputGradients[i] = output_errors[i] * final_outputs[i] * (1 - final_outputs[i]);
            }

            // 4. Fehlerberechnung für den letzten Hidden Layer
            string lastHiddenOutputKey = $"hidden_{hnodes_count}_outputs";
            string lastHiddenErrorKey = $"hidden_{hnodes_count}_error";

            hidden_errors_dict[lastHiddenErrorKey] = nnMathO.CalculateHiddenError(who, output_errors);

            // 5. Gradientenberechnung für den letzten Hidden Layer
            double[] lastHiddenGradients = new double[hnodes];
            for (int i = 0; i < hnodes; i++)
            {
                lastHiddenGradients[i] = hidden_errors_dict[lastHiddenErrorKey][i] *
                                          hidden_outputs_dict[lastHiddenOutputKey][i] *
                                          (1 - hidden_outputs_dict[lastHiddenOutputKey][i]);
            }

            // 6. Gewichtsanpassung für Hidden-to-Output
            for (int i = 0; i < hnodes; i++)
            {
                for (int j = 0; j < onodes; j++)
                {
                    who[j, i] += learningRate * outputGradients[j] * hidden_outputs_dict[lastHiddenOutputKey][i];
                }
            }

            // 7. Dynamische Backward Propagation für die Hidden Layers
            for (int i = hnodes_count; i > 1; i--)
            {
                string currentHiddenErrorKey = $"hidden_{i}_error";
                string previousHiddenErrorKey = $"hidden_{i - 1}_error";
                string currentHiddenOutputKey = $"hidden_{i}_outputs";
                string previousHiddenOutputKey = $"hidden_{i - 1}_outputs";
                string weightMatrixKey = $"Hidden{i - 1}-Hidden{i}";

                // Fehlerberechnung für die vorherige Hidden Layer
                hidden_errors_dict[previousHiddenErrorKey] = nnMathO.CalculateHiddenError(weightMatrices_betweenLayer[weightMatrixKey],
                                                                                          hidden_errors_dict[currentHiddenErrorKey]);

                // Gradientenberechnung für die aktuelle Hidden Layer
                double[] hiddenGradients = new double[hnodes];
                for (int j = 0; j < hnodes; j++)
                {
                    hiddenGradients[j] = hidden_errors_dict[previousHiddenErrorKey][j] *
                                         hidden_outputs_dict[previousHiddenOutputKey][j] *
                                         (1 - hidden_outputs_dict[previousHiddenOutputKey][j]);
                }

                // Gewichtsanpassung für die Verbindung zwischen den Hidden Layers
                for (int j = 0; j < hnodes; j++)
                {
                    for (int k = 0; k < hnodes; k++)
                    {
                        weightMatrices_betweenLayer[weightMatrixKey][j, k] +=
                            learningRate * hiddenGradients[j] * hidden_outputs_dict[previousHiddenOutputKey][k];
                    }
                }
            }

            // 8. Backward Propagation für Input-to-Hidden Layer
            string firstHiddenErrorKey = "hidden_1_error";
            hidden_errors_dict[firstHiddenErrorKey] = nnMathO.CalculateHiddenError(wih, hidden_errors_dict["hidden_2_error"]);

            double[] firstHiddenGradients = new double[hnodes];
            for (int i = 0; i < hnodes; i++)
            {
                firstHiddenGradients[i] = hidden_errors_dict[firstHiddenErrorKey][i] *
                                          hidden_outputs_dict["hidden_1_outputs"][i] *
                                          (1 - hidden_outputs_dict["hidden_1_outputs"][i]);
            }

            // Gewichtsanpassung für Input-to-Hidden
            for (int i = 0; i < inodes; i++)
            {
                for (int j = 0; j < hnodes; j++)
                {
                    wih[j, i] += learningRate * firstHiddenGradients[j] * inputs[i];
                }
            }
        }
    }
}
