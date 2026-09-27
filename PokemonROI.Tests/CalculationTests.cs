using PokemonROI.Services;
namespace PokemonROI.Tests;
public class CalculationTests
{
 [Fact] public void FutureValue(){Assert.Equal(161.05m,Math.Round(RoiCalculator.FutureValue(100,10,5),2));}
 [Fact] public void Fees(){Assert.Equal(82m,RoiCalculator.NetProceeds(100,10,5,3,0));}
 [Fact] public void Roi(){Assert.Equal(25m,RoiCalculator.Roi(25,100));}
 [Fact] public void Cagr(){Assert.Equal(10m,Math.Round(RoiCalculator.Cagr(100,161.051,5),0));}
}
