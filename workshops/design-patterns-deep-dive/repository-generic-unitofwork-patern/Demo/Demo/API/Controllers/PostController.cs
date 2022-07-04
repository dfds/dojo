using Demo.Domain.Entities;
using Demo.Domain.Repositories;
using Infrastructure.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.API.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PostController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // GET: api/<PostController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // this is just an example to show you how to call repositories using unitOfWork
            var result = await _unitOfWork.Posts.GetAll();
            return result != null ? Ok(result) : StatusCode(500);
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            // this is just an example to show you how to call repositories using unitOfWork
            var result = _unitOfWork.Posts.GetById(id).Result;
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateNewPost([FromBody] Post post)
        {
            // this is just an example to show you how to call repositories using unitOfWork
            _unitOfWork.Posts.Insert(post);
            _unitOfWork.Save();
            return Created("", post.Title);
        }

    }
}
