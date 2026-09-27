using System.Globalization;
using System.IO;
using System.Text;
using PokemonROI.Models;
namespace PokemonROI.Services;
public static class RoiCalculator
{ public static decimal FutureValue(decimal value,decimal annualRate,int years)=>value*(decimal)Math.Pow((double)(1+annualRate/100),years); public static decimal NetProceeds(decimal gross,decimal marketplace,decimal processing,decimal shipping,decimal other)=>gross-gross*(marketplace+processing)/100-shipping-other; public static decimal Roi(decimal profit,decimal cost)=>cost==0?0:profit/cost*100; public static decimal Cagr(decimal start,decimal end,int years)=>start<=0||end<=0||years<=0?0:(decimal)(Math.Pow((double)(end/start),1.0/years)-1)*100; }
public static class CsvService { public static void Export(IEnumerable<PortfolioItem> items,string path){using var w=new StreamWriter(path,false,Encoding.UTF8);w.WriteLine("Card,Set,Grade,Quantity,Purchase Price,Purchase Date,Current Price,Current Value,Profit,ROI");foreach(var x in items)w.WriteLine(string.Join(',',Q(x.CardName),Q(x.SetName),Q(x.Grade),x.Quantity,x.PurchasePrice.ToString(CultureInfo.InvariantCulture),x.PurchaseDate.ToString("yyyy-MM-dd"),x.CurrentPrice?.ToString(CultureInfo.InvariantCulture)??"",x.CurrentValue.ToString(CultureInfo.InvariantCulture),x.Profit.ToString(CultureInfo.InvariantCulture),x.Roi.ToString("0.##",CultureInfo.InvariantCulture)));}static string Q(string s)=>"\""+s.Replace("\"","\"\"")+"\"";}
}
