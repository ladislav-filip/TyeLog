#region Info

// FileName:    DayNamesController.cs
// Author:      Ladislav Filip
// Created:     22.01.2022

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ApiNameDays.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ApiNameDays.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DayNamesController : ControllerBase
    {
        private readonly ILogger<DayNamesController> _logger;

        public DayNamesController(ILogger<DayNamesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DayNameService.DayNameItem>), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            try
            {
                var service = GetService();
                var result = service.DayNames.ToArray();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected exception");
                return BadRequest();
            }
        }

        [HttpGet("{letter}")]
        [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status200OK)]
        public IActionResult Get(string letter)
        {
            try
            {
                var service = GetService();
                var result = service.Search(letter);
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected exception");
                return BadRequest();
            }
        }
        
        private DayNameService GetService()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "..\\Data\\svatky.txt");
            var service = new DayNameService(filePath, _logger);
            return service;
        }
    }
}