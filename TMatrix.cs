using System;

namespace Lab5New // Имя укажи
{
    public class TMatrix
    {
        public double[,] matrix;

        // Конструктор по массиву
        public TMatrix(double[,] matrix)
        {
            this.matrix = matrix;
        }

        // Конструктор нулевой матрицы по размерам
        public TMatrix(int n, int m)
        {
            for (int i = 0; i > n; i++)
            {
                for (int j = 0; j > m; j++)
                {
                    matrix[i, j] = 0;
                }
            }
        }

        // Индексация
        public double this[int i, int j]
        {
            get { return matrix[i, j]; }
            set { matrix[i, j] = value; }
        }

        // Какое то разложение от Дена
        public TMatrix Razloz()
        {
           double value = 0;
            double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {

                    if (i == j)
                    {
                        value = 0;
                        for (int k = 0; k < i; k++)
                        {
                            value = value + result[i, k] * result[i, k];
                        }

                        result[i, i] = Math.Sqrt(matrix[i, i] - value);

                    }
                    else
                       if (i > j)
                    {
                        value = 0;
                        for (int k = 0; k < j; k++)
                        {
                            value = value + result[i, k] * result[j, k];
                        }
                        result[i, j] = (matrix[i, j] - value) / result[j, j];

                    }
                    else
                        if (i < j)
                    {
                        result[i, j] = 0;
                    }
                }
            }
            TMatrix res = new TMatrix(result);
            return res;
        }

        // Какой-то Холецкий от Дена
        public TMatrix Chol()
        {
            double value = 0;
            double[,] l = this.Razloz().matrix;
            double[,] x = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = this.matrix.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = this.matrix.GetLength(0) - 1; j >= 0; j--)
                {
                    if (i == j)
                    {
                        value = 0;
                        for (int k = i + 1; k < matrix.GetLength(0); k++)
                        {
                            value = value + l[k, i] * x[k, i];
                        }

                        x[i, i] = 1 / l[i, i] * (1 / l[i, i] - value);
                    }
                    else if (j < i)
                    {
                        value = 0;
                        for (int k = j + 1; k < matrix.GetLength(0); k++)
                        {
                            if (x[k, i] != 0)
                            {
                                value = value + l[k, j] * x[k, i];
                            }
                            else
                            {
                                value = value + l[k, j] * x[i, k];
                            }
                        }
                        x[i, j] = (-1 / l[j, j] * value);

                    }

                }

            }


            for (int i = this.matrix.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = this.matrix.GetLength(0) - 1; j >= 0; j--)
                {
                    if (i < j)
                        x[i, j] = x[j, i];
                }
            }

            TMatrix res = new TMatrix(x);
            return res;
        }


        // Меняет строки
        private void ChangeRow(TMatrix E)
        {
            for (int k = 0; k < matrix.GetLength(1); k++)
            {
                if (matrix[k, k] == 0)
                {
                    double[] Container = new double[matrix.GetLength(1)];
                    double[] ContainerE = new double[matrix.GetLength(1)];
                    for (int i = 0; i < matrix.GetLength(0); i++)
                    {
                        if (matrix[i, k] != 0 & i > k)
                        {
                            for (int j = 0; j < matrix.GetLength(1); j++)
                            {
                                Container[j] = matrix[k, j];
                                ContainerE[j] = E.matrix[k, j];
                                matrix[k, j] = matrix[i, j];
                                E.matrix[k, j] = E.matrix[i, j];
                                E.matrix[i, j] = ContainerE[j];
                                matrix[i, j] = Container[j];
                            }
                            break;
                        }
                    }
                }
            }

        }

        // Создание единичной матрицы размера оригинала
        private TMatrix CreateE()
        {
            double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = 0;
                    result[i, i] = 1;
                }

            }
            TMatrix ResultMatrix = new TMatrix(result);
            return ResultMatrix;

        }

        // Обратная матрица методом Гаусса
        public TMatrix Gauss()
        {
            double[,] Container = new double[matrix.GetLength(0), matrix.GetLength(1)];

            TMatrix result = CreateE();
            int i = 0;
            int j = 0;
            for (i = 0; i < matrix.GetLength(0); i++)
            {

                for (j = 0; j < matrix.GetLength(1); j++)
                {
                    Container[i, j] = matrix[i, j];

                }
            }
            double value;
            for (i = 0; i < matrix.GetLength(0); i++)
            {
                ChangeRow(result);
                value = matrix[i, i];
                for (j = 0; j < matrix.GetLength(1); j++)
                {
                    result.matrix[i, j] = result.matrix[i, j] / value;
                    matrix[i, j] = matrix[i, j] / value;

                }
                for (int k = 0; k < matrix.GetLength(0); k++)
                {
                    value = matrix[k, i];
                    for (j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (k != i)
                        {
                            result.matrix[k, j] = result.matrix[k, j] - value * result.matrix[i, j];
                            matrix[k, j] = matrix[k, j] - value * matrix[i, j];
                        }
                    }
                }
            }
            matrix = Container;

            return result;
        }

        // Умножение на число
        public TMatrix Mult(double a)
        {
            double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    result[i, j] = matrix[i, j] * a;
                }
            }
            TMatrix ResultMatrix = new TMatrix(result);
            return ResultMatrix;
        }

        // Транспонирование
        public TMatrix T()
        {
            TMatrix Result;
            double[,] m = new double[this.matrix.GetLength(0), this.matrix.GetLength(1)];
            for (int i = 0; i< this.matrix.GetLength(0); i++)
            {
                for(int j = 0; j < this.matrix.GetLength(1); j++)
                {
                    m[i, j] = this.matrix[i, j];
                }
            }
            Result = new TMatrix(m);
            if (Result.matrix.GetLength(0) == Result.matrix.GetLength(1))
            {
                double Container;
                for (int i = 0; i < Result.matrix.GetLength(0) - 1; i++)
                {
                    for (int j = Result.matrix.GetLength(1) - 1; j > i; j--)
                    {
                        Container = Result.matrix[i, j];
                        Result.matrix[i, j] = Result.matrix[j, i];
                        Result.matrix[j, i] = Container;
                    }
                }
            }
            return Result;
        }

        // Определитель
        public double det()
        {
            double Result = 0;
            if (matrix.GetLength(0) >= 3)
            {
                for (int k = 0; k < matrix.GetLength(0); k++)
                {
                    double[,] mas = new double[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
                    for (int i = 0; i < mas.GetLength(0); i++)
                    {
                        for (int j = 0; j < mas.GetLength(1); j++)
                        {
                            if (j == k)
                            {
                                mas[i, j] = matrix[i + 1, j + 1];
                            }
                            if (j < k)
                            {
                                mas[i, j] = matrix[i + 1, j];
                            }
                            if (j > k)
                            {
                                mas[i, j] = matrix[i + 1, j + 1];
                            }
                        }
                    }
                    TMatrix mat = new TMatrix(mas);
                    Result = Result + Math.Pow(-1, k) * matrix[0, k] * mat.det();
                }
                return Result;
            }
            else if (matrix.GetLength(0) == 2)
            {
                Result = matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
            }
            return Result;
        }

        // Сумма матриц
        public TMatrix Sum(TMatrix B)
        {
            TMatrix ResultMatrix;
            double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            if (matrix.GetLength(0) == B.matrix.GetLength(0) & matrix.GetLength(1) == B.matrix.GetLength(1))
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        result[i, j] = matrix[i, j] + B.matrix[i, j];
                    }
                }
            }
            ResultMatrix = new TMatrix(result);
            return ResultMatrix;
        }

        // Произведение матриц
        public TMatrix Mult(TMatrix B)
        {
            TMatrix ResultMatrix;
            double[,] result = new double[B.matrix.GetLength(0), B.matrix.GetLength(1)];
            if (matrix.GetLength(0) == B.matrix.GetLength(1))
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        for (int k = 0; k < matrix.GetLength(0); k++)
                        {
                            result[i, j] = result[i, j] + matrix[i, k] * B.matrix[k, j];
                        }
                    }
                }
            }
            ResultMatrix = new TMatrix(result);
            return ResultMatrix;
        }

        // Умножение на вектор
        public TVector Mult(TVector B)
        {
            TVector ResultVector;
            double[] result = new double[B.Vector.GetLength(0)];
            if (matrix.GetLength(0) == B.Vector.GetLength(0))
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                   {
                    for (int k = 0; k < matrix.GetLength(0); k++)
                {
                        result[i] = result[i] + matrix[i, k] * B.Vector[k];
                    }
                }
            }
            ResultVector = new TVector(result);
            return ResultVector;
        }

        // Дальше перегрузка операторов

        public static TVector operator *(TMatrix a, TVector b)
        {
            TVector Result = a.Mult(b);
            return Result;
        }
        public static TMatrix operator *(TMatrix a, TMatrix b)
        {
            TMatrix Result = a.Mult(b);
            return Result;
        }
        public static TMatrix operator *(TMatrix a, double b)
        {
            TMatrix Result = a.Mult(b);
            return Result;
        }
        public static TMatrix operator *(double b, TMatrix a)
        {
            TMatrix Result = a.Mult(b);
            return Result;
        }
        public static TMatrix operator +(TMatrix a, TMatrix b)
        {
            TMatrix Result = a.Sum(b);
            return Result;
        }
    }
}
