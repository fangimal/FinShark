﻿using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IStockRepository _stockRepository;
    private readonly IPortfolioRepository _portfolioRepository;

    public PortfolioController(
        UserManager<AppUser> userManager, 
        IStockRepository stockRepository,
        IPortfolioRepository portfolioRepository)
    {
        _userManager = userManager;
        _stockRepository = stockRepository;
        _portfolioRepository = portfolioRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUserPortfolio()
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
        return Ok(userPortfolio);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var stock = await _stockRepository.GetBySymbolAsync(symbol);
        
        if (stock == null) 
            return BadRequest("Stock not found");
        
        var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);

        if (userPortfolio.Any(x => x.Symbol.ToLower() == symbol.ToLower())) 
            return BadRequest("Cannot add same to portfolio");

        var portfolioModel = new Portfolio
        {
            StockId = stock.Id,
            AppUserId = appUser.Id
        };
        
        await _portfolioRepository.CreateAsync(portfolioModel);

        if (portfolioModel == null)
        {
            return StatusCode(500, "Could not create");
        }
        else
        {
            return Created();
        }
    }
}