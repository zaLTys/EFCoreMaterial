using BenchmarkDotNet.Attributes;

namespace Benchmarks;

public class ApiBenchmark
{
    private readonly HttpClient _httpClient = new();

    [Benchmark]
    public async Task AllSimple()
    {
        await _httpClient.GetAsync("https://localhost:44359/Products/allSimple");
    }
    
    // [Benchmark]
    // public async Task AllAsSplitQuery()
    // {
    //     await _httpClient.GetAsync("https://localhost:44359/Products/allAsSplitQuery");
    // }  
    //
    // [Benchmark]
    // public async Task AllAsNoTracking()
    // {
    //     await _httpClient.GetAsync("https://localhost:44359/Products/allAsNoTracking");
    // }
    
    [Benchmark]
    public async Task AllAsSplitAsNoTracking()
    {
        await _httpClient.GetAsync("https://localhost:44359/Products/allAsSplitAsNoTracking");
    }
    

}