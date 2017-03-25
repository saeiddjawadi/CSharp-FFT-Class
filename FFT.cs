namespace Ociloscope
{
    static class statistics
    {
       //dim is (256 or 512 or 1024 or 2048 .......)
       public static string FFT(Point[] FirstInput, int dim)
        {
            Complex[,] input = new Complex[dim + 1, dim + 1];
            for (int i = 0; i < dim; i++)
            {
                input[0, i] = new Complex(FirstInput[i].Y, 0);
            }
            double[,] input2 = new double[4, 10];
            int CountX;
            for (CountX = 0; CountX < dim; CountX++)
            {
                //if (FirstInput[CountX] == 0)
                //    break;
            }
            int size = CountX, SizeX = CountX / 2, SizeY = 1, SizeZ = CountX;
            double l = Math.Log(size, 2);
            double p = Math.Ceiling(l);
            double N = Math.Pow(2, p);
            double N2 = N / 2;
            Complex yy = -Math.PI * Complex.Sqrt(-1) / N2;
            Complex ww = Complex.Exp(yy);

            Complex[] JJ = new Complex[(int)N2 - 1];
            for (int i = 0; i < JJ.Length; i++)
            {
                JJ[i] = new Complex(i, 0);
            }
            Complex[,] W = new Complex[size, size];

            for (int i = 0; i < N2 - 1; i++)
            {
                W[0, i] = Complex.Pow(ww, JJ[i]);
            }
            Complex[,] u = new Complex[size, size];
            Complex[,] v = new Complex[size, size];
            Complex[,] t = new Complex[size, size];
            Complex[,] S = new Complex[size, size];
            Complex[,] U = new Complex[size, size];

            for (int i = 0; i < p; i++)
            {
                for (int n = 0; n < dim; n++)
                {
                    for (int m = 0; m < dim; m++)
                    {
                        u[n, m] = new Complex(0, 0);
                    }
                }
                // *********u=Y(:,1:N2);***********
                for (int n = 0; n < SizeY; n++)
                {
                    for (int m = 0; m < SizeX; m++)
                    {
                        u[n, m] = input[n, m];
                    }
                }
                for (int n = 0; n < dim; n++)
                {
                    for (int m = 0; m < dim; m++)
                    {
                        v[n, m] = new Complex(0, 0);
                    }
                }
                // ******************************** 
                // *********v=Y(:,N2+1:N);*********
                for (int n = 0; n < SizeY; n++)
                {
                    for (int m = SizeX; m < SizeX * 2; m++)
                    {
                        v[n, m - SizeX] = input[n, m];
                    }
                }
                // ********************************
                // *********t=u+v;*****************
                for (int n = 0; n < dim; n++)
                {
                    for (int m = 0; m < dim; m++)
                    {
                        t[n, m] = new Complex(0, 0);
                    }
                }
                for (int n = 0; n < SizeY; n++)
                {
                    for (int m = 0; m < SizeX; m++)
                    {
                        t[n, m] = u[n, m] + v[n, m];
                    }
                }
                // ********************************
                // *********S=W.*(u-v);************
                for (int n = 0; n < dim; n++)
                {
                    for (int m = 0; m < dim; m++)
                    {
                        S[n, m] = new Complex(0, 0);
                    }
                }
                for (int n = 0; n < SizeY; n++)
                {
                    for (int m = 0; m < SizeX; m++)
                    {
                        S[n, m] = W[n, m] * (u[n, m] - v[n, m]);
                    }
                }
                // ********************************
                // *********Y=[t ; S];*************
                for (int n = 0; n < SizeY; n++)
                {
                    for (int m = 0; m < SizeX * 2; m++)
                    {
                        input[n, m] = new Complex(0, 0);
                    }
                }
                for (int n = 0; n < SizeY; n++)
                {
                    for (int m = 0; m < SizeX; m++)
                    {
                        input[n, m] = t[n, m];
                    }
                }
                int b = 0;
                for (int n = SizeY; n < SizeY * 2; n++)
                {
                    for (int m = 0; m < SizeX; m++)
                    {
                        input[n, m] = S[b, m];
                    }
                    b++;
                }
                // ********************************
                // *********U=W(:,1:2:N2);*********
                b = 0;
                for (int n = 0; n < dim; n++)
                {
                    for (int m = 0; m < dim; m++)
                    {
                        U[n, m] = new Complex(0, 0);
                    }
                }
                for (int n = 0; n < SizeY; n++)
                {
                    for (int m = 0; m < SizeX; m++)
                    {
                        if (m % 2 == 0)
                            U[n, m / 2] = W[n, m];
                    }
                }
                // *******************************
                // *********W=[U ;U];*************
                for (int n = 0; n < dim; n++)
                {
                    for (int m = 0; m < dim; m++)
                    {
                        W[n, m] = new Complex(0, 0);
                    }
                }
                for (int n = 0; n < SizeY; n++)
                {
                    for (int m = 0; m < SizeX; m++)
                    {
                        W[n, m] = U[n, m];
                    }
                }
                b = 0;
                for (int n = SizeY; n < SizeY * 2; n++)
                {
                    for (int m = 0; m < SizeX; m++)
                    {
                        W[n, m] = U[b, m];
                    }
                    b++;
                }
                // ********************************
                // *********N=N2;*****N2=N2/2;*****
                SizeZ = SizeX;
                SizeX /= 2;
                SizeY *= 2;
            }
            // ********************************
            //+++++++++++++++++++++++++++++++++
            // ********************************
            // *********u = Y(:, 1);***********
            for (int n = 0; n < SizeY; n++)
            {
                for (int m = 0; m < 1; m++)
                {
                    u[n, m] = input[n, m];
                }
            }
            // ********************************
            // *********v = Y(:, 2);***********
            for (int n = 0; n < SizeY; n++)
            {
                for (int m = 1; m < 2; m++)
                {
                    v[n, m] = input[n, m];
                }
            }
            // ********************************
            // *********Y =[u + v; u - v];*****
            Complex[] Final = new Complex[513];
            for (int n = 0; n < SizeY; n++)
            {
                Final[n] = input[n, 0];
            }

            for (int n = 0; n < SizeY; n++)
            {
                Final[n] = Complex.Abs(Final[n] / size);
            }


            return "get maximum value of Final array. that is signal frequency";

        }
    }
}
