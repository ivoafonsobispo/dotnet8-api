using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await context.Stock.ToListAsync();
        var stockDto = stocks.Select(s => s.ToStockDTO());

        return Ok(stockDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Stock? stock = await context.Stock.FindAsync(id);

        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDTO());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();
        await context.AddAsync(stockModel);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO stockUpdateDto)
    {
        var stockModel = await context.Stock.FirstOrDefaultAsync(stock => stock.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        stockModel.Symbol = stockUpdateDto.Symbol;
        stockModel.CompanyName = stockUpdateDto.CompanyName;
        stockModel.Purchase = stockUpdateDto.Purchase;
        stockModel.Industry = stockUpdateDto.Industry;
        stockModel.LastDiv = stockUpdateDto.LastDiv;
        stockModel.MarketCap = stockUpdateDto.MarketCap;

        await context.SaveChangesAsync();

        return Ok(stockModel.ToStockDTO());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await context.Stock.FirstOrDefaultAsync(stock => stock.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        context.Stock.Remove(stockModel);
        await context.SaveChangesAsync();

        return NoContent();
    }
}