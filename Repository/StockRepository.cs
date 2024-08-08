using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace api.Repository;

public class StockRepository(ApplicationDbContext context) : IStockRepository
{
    public async Task<List<Stock>> GetAllAsync()
    {
        return await context.Stock.Include(c => c.Comments).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await context.Stock.Include(c=> c.Comments).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stock> CreateAsync(Stock stock)
    {
        await context.Stock.AddAsync(stock);
        await context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockUpdateDto)
    {
        var existingStock = await context.Stock.FirstOrDefaultAsync(stock => stock.Id == id);
        if (existingStock == null)
        {
            return null;
        }
        
        existingStock.Symbol = stockUpdateDto.Symbol;
        existingStock.CompanyName = stockUpdateDto.CompanyName;
        existingStock.Purchase = stockUpdateDto.Purchase;
        existingStock.Industry = stockUpdateDto.Industry;
        existingStock.LastDiv = stockUpdateDto.LastDiv;
        existingStock.MarketCap = stockUpdateDto.MarketCap;

        await context.SaveChangesAsync();

        return existingStock;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await context.Stock.FirstOrDefaultAsync(stock => stock.Id == id);
        if (stockModel == null)
        {
            return null;
        }

        context.Stock.Remove(stockModel);
        await context.SaveChangesAsync();
        
        return stockModel;
    }
}