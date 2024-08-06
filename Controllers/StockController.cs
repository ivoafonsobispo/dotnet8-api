using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        var stocks = context.Stock.ToList().Select(s => s.ToStockDTO());

        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
        Stock? stock = context.Stock.Find(id);

        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDTO());
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockRequestDTO stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();
        context.Add(stockModel);
        context.SaveChanges();
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDTO());
    }
}