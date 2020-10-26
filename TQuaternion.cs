using System;

namespace Lab5 // Имя укажи
{
    public class TQuaternion
    {
        public double[] Vec;

        // Конструктор по 4 значениям 
        public TQuaternion(double a, double b, double c, double d)
        {
            Vec = new double[4];
            Vec[0] = a;
            Vec[1] = b;
            Vec[2] = c;
            Vec[3] = d;
        }

        // Конструктор по углу и вектору вращения
        public TQuaternion(double phi, TVector V)
        {
            if (V.Vector.Length != 3)
            {
                return;
            }
            Vec = new double[4];
            Vec[0] = Math.Cos(phi * (Math.PI) / 360);
            for (int i = 1; i < 4; i++)
            {
                Vec[i] = Math.Sin(phi * (Math.PI) / 360) * V.Vector[i - 1];
            }
        }

        // Сопряженный кватернион
        public TQuaternion Conjugate()
        {
            TQuaternion Result = new TQuaternion(Vec[0], -Vec[1], -Vec[2], -Vec[3]);
            return Result;
        }

        // Сумма кватернионов
        public TQuaternion Sum(TQuaternion Q)
        {
            TQuaternion Result = new TQuaternion(Vec[0] + Q.Vec[0], Vec[1] + Q.Vec[1], Vec[2] + Q.Vec[0], Vec[3] + Q.Vec[0]);
            return Result;
        }

        // Норма кватерниона
        public double Norm()
        {
            double result = Math.Sqrt((Vec[0]) * (Vec[0]) + (Vec[1]) * (Vec[1]) + (Vec[2]) * (Vec[2]) + (Vec[3]) * (Vec[3]));
            return result;
        }

        // Нормализация кватерниона
        public TQuaternion Normalization()
        {
            TQuaternion result = new TQuaternion(Vec[0], Vec[1], Vec[2], this.Vec[3]);
            double n = this.Norm();
            for (int i = 0; i < 4; i++)
            {
                result.Vec[i] = result.Vec[i] / n;
            }
            return result;
        }

        // Обратный кватернион
        public TQuaternion Reverse()
        {
            TQuaternion result = this.Conjugate();
            double n = Norm() * Norm();
            for (int i = 0; i < 4; i++)
            {
                result.Vec[i] = result.Vec[i] / n;
            }
            return result;
        }

        // Умножение на кватернион
        public TQuaternion Mult(TQuaternion Q)
        {
            double a, b, c, d;
            a = Vec[0] * Q.Vec[0] - Vec[1] * Q.Vec[1] - Vec[2] * Q.Vec[2] - Vec[3] * Q.Vec[3];
            b = Vec[0] * Q.Vec[1] + Vec[1] * Q.Vec[0] + Vec[2] * Q.Vec[3] - Vec[3] * Q.Vec[2];
            c = Vec[0] * Q.Vec[2] + Vec[2] * Q.Vec[0] - Vec[1] * Q.Vec[3] + Vec[3] * Q.Vec[1];
            d = Vec[0] * Q.Vec[3] + Vec[3] * Q.Vec[0] + Vec[1] * Q.Vec[2] - Vec[2] * Q.Vec[1];
            TQuaternion result = new TQuaternion(a, b, c, d);
            return result;
        }

        // Округление
        public void Round(int n)
        {
            for (int i = 0; i < 4; i++)
            {
                Vec[i] = Math.Round(Vec[i], n);
            }
        }
    }
}