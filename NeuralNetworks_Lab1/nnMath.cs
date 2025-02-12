﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks_Lab1
{
    class nnMath
    {

        public double[] matrixMult(double[,] gewichtung, int anzahl_neuronen, double[] Eingabewerte)
        {
            double[] Eingangsergebnis = new double[gewichtung.GetLength(0)];
            if (gewichtung.GetLength(1) != Eingabewerte.Length)
                throw new ArgumentException("Matrix columns must match vector length.");


            for (int i = 0; i < gewichtung.GetLength(0); i++)             // Anzahl der reihen 
            {
                for (int j = 0; j < Eingabewerte.Length; j++) // Anzahl der spalten 
                {
                    Eingangsergebnis[i] += Eingabewerte[j] * gewichtung[i, j];

                }


            }


            return Eingangsergebnis;
        }

        public double[] activationFunction(double[] inputs)
        {
            double[] ausgabewerte_sigmoid_Funktion = new double[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                ausgabewerte_sigmoid_Funktion[i] = 1 /(1 + Math.Exp(-inputs[i]));
            }

            return ausgabewerte_sigmoid_Funktion;
        }








        // Einfache Differenz als Fehler-Funktion (Cost function)
        public double[] CalculateOutputErrors(double[] targets, double[] outputs)
        {
            double[] test = new double[targets.Length];
            double[] errors = new double[targets.Length];
            int a;

            for (int i = 0; i < targets.Length; i++)
            {
                /* test[i] = targets[i] - outputs[i];

                 if (test[i] < 0)
                 {
                     a = -1;
                 }
                 else
                 {
                     a = 1;

                 }

                 errors[i] = a * Math.Pow(targets[i] - outputs[i], 2);*/
                errors[i] = targets[i] - outputs[i]; 
            }
            return errors;

        }

        // Fehler-Funktion für den Hidden Layer
        public double[] CalculateHiddenError(double[,] weights, double[] errorOutput)
        {
            int rows = weights.GetLength(0); // Anzahl der Neuronen in der Hidden-Schicht
            int cols = weights.GetLength(1); // Anzahl der Neuronen in der Output-Schicht
            double nenner = 0;             

            

            double[] errorHidden = new double[cols];

            for (int i = 0; i < cols; i++)
            {
                errorHidden[i] = 0.0;
                for (int j = 0; j < errorOutput.Length; j++)


                {

                   /* for (int k = 0; k < rows; k++)
                    {
                        
                        nenner += weights[k, j];


                    }*/


                    errorHidden[i] += weights[j, i]  * errorOutput[j];

                  
                }
            }

            return errorHidden;

        }
    }
}
