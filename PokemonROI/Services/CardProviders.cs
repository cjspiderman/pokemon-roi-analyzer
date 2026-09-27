using System.Net.Http.Json;
using System.Text.Json;
using PokemonROI.Models;
namespace PokemonROI.Services;
public interface ICardProvider { string Name{get;} Task<IReadOnlyList<Card>> SearchAsync(string query,CancellationToken token=default); }
public sealed class PokemonTcgApiProvider : ICardProvider
{
 public string Name=>"Pokémon TCG API"; private readonly HttpClient http=new(){BaseAddress=new Uri("https://api.pokemontcg.io/v2/")};
 public async Task<IReadOnlyList<Card>> SearchAsync(string query,CancellationToken token=default){if(string.IsNullOrWhiteSpace(query))return [];try{var url="cards?q="+Uri.EscapeDataString($"name:{query} OR set.name:{query} OR number:{query}")+"&pageSize=50";using var response=await http.GetAsync(url,token);response.EnsureSuccessStatusCode();using var stream=await response.Content.ReadAsStreamAsync(token);using var doc=await JsonDocument.ParseAsync(stream,cancellationToken:token);var result=new List<Card>();foreach(var x in doc.RootElement.GetProperty("data").EnumerateArray()){decimal? price=null;if(x.TryGetProperty("tcgplayer",out var t)&&t.TryGetProperty("prices",out var p)){foreach(var v in p.EnumerateObject()){if(v.Value.TryGetProperty("market",out var m)&&m.ValueKind==JsonValueKind.Number){price=m.GetDecimal();break;}}}result.Add(new Card{Id=x.GetProperty("id").GetString()??"",Name=x.GetProperty("name").GetString()??"",SetName=x.GetProperty("set").GetProperty("name").GetString()??"",Number=x.GetProperty("number").GetString()??"",Rarity=x.TryGetProperty("rarity",out var rr)?rr.GetString()??"":"",ImageUrl=x.GetProperty("images").GetProperty("small").GetString()??"",MarketPrice=price,UpdatedAt=DateTime.UtcNow});}return result;}catch{return [];}}
}
