// See https://aka.ms/new-console-template for more information
Console.WriteLine("Please enter an input Number of Steps value: ");
string inputString1 = Console.ReadLine();
double inputNumSteps = Convert.ToDouble(inputString1);

Console.WriteLine("Please enter an input Number of Simulations value: ");
string inputString2 = Console.ReadLine();
double inputNumSimulations = Convert.ToDouble(inputString2);

bool IsCall = true;
double option_price = HW5.MonteCarloSimulation.EuropeanOptionPrice(100, 0.05, 0.2, 1, inputNumSteps, 105, inputNumSimulations, IsCall);
Console.WriteLine((IsCall ? "Call" : "Put") + " Option Price is: " + option_price);

double[] greeks = HW5.MonteCarloSimulation.Greeks(100, 0.05, 0.2, 1, inputNumSteps, 105, inputNumSimulations, IsCall);
Console.WriteLine("Delta is: " + greeks[0]);
Console.WriteLine("gamma is: " + greeks[1]);
Console.WriteLine("vega is: " + greeks[2]);
Console.WriteLine("theta is: " + greeks[3]);
Console.WriteLine("rho is: " + greeks[4]);

double StandardError = HW5.MonteCarloSimulation.StandardError(100, 0.05, 0.2, 1, inputNumSteps, 105, inputNumSimulations, IsCall);
Console.WriteLine("Standard Error is: " + StandardError);