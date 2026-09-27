using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using PokemonROI.Models;
using PokemonROI.Services;
namespace PokemonROI.ViewModels;
public sealed class MainViewModel : Notify
{
 public ObservableCollection<Card> Cards{get;}=[]; public ObservableCollection<PortfolioItem> Portfolio{get;}=[]; public ICardProvider Provider{get;}=new PokemonTcgApiProvider(); private readonly AppDatabase db; private string search=""; public string SearchText{get=>search;set=>Set(ref search,value);} private string status="Ready"; public string Status{get=>status;set=>Set(ref status,value);} public decimal TotalInvested=>Portfolio.Sum(x=>x.Invested);public decimal TotalValue=>Portfolio.Sum(x=>x.CurrentValue);public decimal Profit=>TotalValue-TotalInvested;public decimal Roi=>RoiCalculator.Roi(Profit,TotalInvested); public decimal Projection5=>RoiCalculator.FutureValue(TotalValue,8,5); public Card? SelectedCard{get;set;} public PortfolioItem? SelectedPortfolio{get;set;}
 public ICommand SearchCommand{get;} public ICommand AddCommand{get;} public ICommand DeleteCommand{get;} public ICommand ExportCommand{get;} public ICommand RefreshCommand{get;}
 public MainViewModel(AppDatabase database){db=database;foreach(var x in db.GetPortfolio())Portfolio.Add(x);SearchCommand=new AsyncCommand(Search);AddCommand=new RelayCommand(_=>AddSelected());DeleteCommand=new RelayCommand(_=>DeleteSelected());ExportCommand=new RelayCommand(_=>Export());RefreshCommand=new AsyncCommand(_=>Search());Portfolio.CollectionChanged+=(s,e)=>{On(nameof(TotalInvested));On(nameof(TotalValue));On(nameof(Profit));On(nameof(Roi));On(nameof(Projection5));};}
 async Task Search(){if(string.IsNullOrWhiteSpace(SearchText)){Status="Enter a card name, set, or number.";return;}Status="Searching…";Cards.Clear();var found=await Provider.SearchAsync(SearchText);foreach(var c in found)Cards.Add(c);Status=found.Count==0?"No live results. Check connection or try another search.":$"{found.Count} result(s) • Source: {Provider.Name} • {DateTime.Now:t}";}
 void AddSelected(){if(SelectedCard is null){Status="Select a card first.";return;}var x=new PortfolioItem{CardId=SelectedCard.Id,CardName=SelectedCard.Name,SetName=SelectedCard.SetName,CurrentPrice=SelectedCard.MarketPrice};db.Save(x);Portfolio.Insert(0,x);Status="Added to portfolio. Enter your purchase details below.";}
 void DeleteSelected(){if(SelectedPortfolio is null)return;db.Delete(SelectedPortfolio.Id);Portfolio.Remove(SelectedPortfolio);Status="Portfolio item removed.";}
 void Export(){var d=new SaveFileDialog{Filter="CSV files|*.csv",FileName="pokemon-roi-portfolio.csv"};if(d.ShowDialog()==true){CsvService.Export(Portfolio,d.FileName);Status="Portfolio exported.";}}
 void On(string n)=>PropertyChanged?.Invoke(this,new(n)); public new event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
}
public sealed class RelayCommand(Action<object?> action):ICommand{public event EventHandler? CanExecuteChanged;public bool CanExecute(object? p)=>true;public void Execute(object? p)=>action(p);}
public sealed class AsyncCommand(Func<Task> action):ICommand{public event EventHandler? CanExecuteChanged;public bool CanExecute(object? p)=>true;public async void Execute(object? p)=>await action();}
