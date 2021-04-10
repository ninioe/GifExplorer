using GifExplorer.DTOs;
using GifExplorer.Models;
using GifExplorer.Repository.iRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GifExplorer.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GifController : ControllerBase
    {
        IGifRepository _repo;
        public GifController(IGifRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Trending()
        {
            try
            {
                List<Gif> list = await _repo.GetTrending();
                return Ok(list);
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            ModelState.AddModelError("", "Something went wronge");
            return StatusCode(500, ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            try
            {
                List<Gif> list = await _repo.Search(term);
                return Ok(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ModelState.AddModelError("", "Something went wronge");
            return StatusCode(500, ModelState);
        }
    }
}
