using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW5
{
    class MonteCarloSimulation
    {
        private static Random random = new Random();
        public static double boxMuller()
        {
            //Console.WriteLine("Running Box Muller method.");
            double return_value;
            double x1, x2;
            {
                x1 = random.NextDouble();  //generate uniform (0,1] random values
                x2 = random.NextDouble();
            }
            return_value = System.Math.Sqrt(-2 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);
            return return_value;
        }
        public static double EuropeanOptionPrice(
            double S0, // initial price
            double r, // risk-free interest rate
            double sigma, // volatility
            double T, // time to maturity
            double NumSteps, // number of steps
            double K, // strike price
            double NumSimulation,
            bool IsCall
        )
        {
            double dt = T / NumSteps;
            double SumPayoff =0;

            for (int i = 0; i < NumSimulation; i++)
            {
                ArrayList boxMullerList = new ArrayList();
                ArrayList StList = new ArrayList();
                for (int j = 0; j < NumSteps; j++)
                {
                    double z = boxMuller();
                    boxMullerList.Add(z);
                }
                double cumsum = 0;
                for (int j = 0; j < NumSteps; j++)
                {
                    double exp = (r - 0.5 * sigma * sigma) * dt + sigma * Math.Sqrt(dt) * ((double)boxMullerList[j]);
                    cumsum += exp;
                    double St = S0 * Math.Exp(cumsum);
                    StList.Add(St);
                }
                double lastElement = (double)StList[StList.Count - 1];
                double Payoff = IsCall ? (lastElement - K) : (K - lastElement);
                SumPayoff += Payoff > 0 ? Payoff : 0;
            }
            double OptionPrice = Math.Exp(-r * T) * (SumPayoff / NumSimulation);
            //Console.WriteLine((IsCall ? "Call" : "Put") + " Option Price is: " + OptionPrice);
            return OptionPrice;
        }

        public static double[] Greeks(double S0,double r,double sigma, double T,double NumSteps, double K, double NumSimulation,bool IsCall)
        {
            double delta = (EuropeanOptionPrice(S0+2, r, sigma, T, NumSteps, K, NumSimulation, IsCall)- EuropeanOptionPrice(S0-2, r, sigma, T, NumSteps, K, NumSimulation, IsCall))/(2*2);
            double gamma = (EuropeanOptionPrice(S0 + 2, r, sigma, T, NumSteps, K, NumSimulation, IsCall) - 2*EuropeanOptionPrice(S0, r, sigma, T, NumSteps, K, NumSimulation, IsCall) + EuropeanOptionPrice(S0 - 2, r, sigma, T, NumSteps, K, NumSimulation, IsCall)) / (2 * 2);
            double vega = (EuropeanOptionPrice(S0, r, sigma + 0.01, T, NumSteps, K, NumSimulation, IsCall) - EuropeanOptionPrice(S0, r, sigma - 0.01, T, NumSteps, K, NumSimulation, IsCall)) / (2 * 0.01);
            double theta = (EuropeanOptionPrice(S0, r, sigma, T + T / NumSteps, NumSteps, K, NumSimulation, IsCall) - EuropeanOptionPrice(S0, r, sigma, T, NumSteps, K, NumSimulation, IsCall)) / (T / NumSteps);
            double rho = (EuropeanOptionPrice(S0, r + 0.02, sigma, T, NumSteps, K, NumSimulation, IsCall) - EuropeanOptionPrice(S0, r - 0.02, sigma, T, NumSteps, K, NumSimulation, IsCall)) / (2 * 0.02);
            double[] return_greeks = new double[5] {delta,gamma,vega,theta,rho};
            return return_greeks;
        }

        public static double StandardError(double S0, double r, double sigma, double T, double NumSteps, double K, double NumSimulation, bool IsCall)
        {
            double dt = T / NumSteps;

            double sum_squareDifference = 0;
            for (int i = 0; i < NumSimulation; i++)
            {
                ArrayList boxMullerList = new ArrayList();
                ArrayList StList = new ArrayList();
                for (int j = 0; j < NumSteps; j++)
                {
                    double z = boxMuller();
                    boxMullerList.Add(z);
                }
                double cumsum = 0;
                for (int j = 0; j < NumSteps; j++)
                {
                    double exp = (r - 0.5 * sigma * sigma) * dt + sigma * Math.Sqrt(dt) * ((double)boxMullerList[j]);
                    cumsum += exp;
                    double St = S0 * Math.Exp(cumsum);
                    StList.Add(St);
                }
                double lastElement = (double)StList[StList.Count - 1];
                double Payoff = IsCall ? (lastElement - K) : (K - lastElement);
                double EachOptionPrice = Math.Exp(-r * T) * Payoff;
                double squareDifference = (EachOptionPrice - EuropeanOptionPrice(S0, r - 0.0002, sigma, T, NumSteps, K, NumSimulation, IsCall)) * (EachOptionPrice - EuropeanOptionPrice(S0, r - 0.0002, sigma, T, NumSteps, K, NumSimulation, IsCall));
                sum_squareDifference += squareDifference;
            }

            double SD = Math.Sqrt((1/(NumSimulation-1))*sum_squareDifference);
            double SE = SD / Math.Sqrt(NumSimulation);
            return SE;
        }
    }



   
}
