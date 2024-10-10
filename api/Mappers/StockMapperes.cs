using api.Dtos.Stock;
using api.Models;

namespace api.Mappers;

public static class StockMapperes
{
    public static StockDto ToStockDto(this Stock stockModel)
    {
        return new StockDto
        {
            Id = stockModel.Id,
            Symbol = stockModel.Symbol,
            CompanyName = stockModel.CompanyName,
            Purchase = stockModel.Purchase,
            LastDiv = stockModel.LastDiv,
            Industry = stockModel.Industry,
            MarketCap = stockModel.MarketCap
        };
    }

    public static Stock ToStockFromCreateDTO(this CreateStockRequestDto requestDto)
    {
        return new Stock
        {
            Symbol = requestDto.Symbol,
            CompanyName = requestDto.CompanyName,
            Purchase = requestDto.Purchase,
            LastDiv = requestDto.LastDiv,
            Industry = requestDto.Industry,
            MarketCap = requestDto.MarketCap
        };
    }
}