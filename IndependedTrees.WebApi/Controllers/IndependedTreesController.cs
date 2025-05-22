using AutoMapper;
using IndependedTrees.BLL.Services.Trees;
using IndependedTrees.WebApi.Models.IndependedTrees;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IndependedTrees.WebApi.Controllers
{
    [ApiController]
    public class IndependedTreesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IIndependedTreesService _independedTreesService;

        public IndependedTreesController(IMapper mapper,
            IIndependedTreesService independedTreesService)
        {
            this._mapper = mapper;
            this._independedTreesService = independedTreesService;
        }

        [HttpPost("api.user.tree.get")]
        public async Task<IResult> Get([Required]string treeName)
        {
            return await this._independedTreesService.GetTreeAsync(treeName)
                .ContinueWith(task => _mapper.Map<TreeNodeApiModel>(task.Result))
                .ContinueWith(task => Results.Json(task.Result));
        }

        [HttpPost("api.user.tree.node.create")]
        public async Task<IResult> Create([Required] string treeName,
            [Required] int parentNodeId,
            [Required] string nodeName)
        {
            return await this._independedTreesService.CreateTreeNodeAsync(treeName, parentNodeId, nodeName)
                .ContinueWith(task => task.IsFaulted? throw task.Exception.InnerException : Results.Created());
        }
        

        // GET api/<IndependedTreesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<IndependedTreesController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<IndependedTreesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<IndependedTreesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
