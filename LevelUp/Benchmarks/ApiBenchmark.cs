using BenchmarkDotNet.Attributes;

namespace Benchmarks;

public class ApiBenchmark
{
    private readonly HttpClient _httpClient = new();
    private const string allSimple = "https://localhost:44359/Products/allSimple";
    private const string allAsSplitQuery = "https://localhost:44359/Products/allAsSplitQuery";
    private const string allAsNoTracking = "https://localhost:44359/Products/allAsNoTracking";
    private const string allAsSplitAsNoTracking = "https://localhost:44359/Products/allAsSplitAsNoTracking";

    [Benchmark]
    public async Task AllSimple()
    {
        await _httpClient.GetAsync(allSimple);
    }
    
    [Benchmark]
    public async Task AllAsSplitQuery()
    {
        await _httpClient.GetAsync(allAsSplitQuery);
    }  
    
    [Benchmark]
    public async Task AllAsNoTracking()
    {
        await _httpClient.GetAsync(allAsNoTracking);
    }
    
    [Benchmark]
    public async Task AllAsSplitAsNoTracking()
    {
        await _httpClient.GetAsync(allAsSplitAsNoTracking);
    }
    

}