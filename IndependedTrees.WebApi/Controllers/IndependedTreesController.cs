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

        [HttpPost("api.user.tree.getAll")]
        public async Task<IResult> GetAllTrees()
        {
            return await this._independedTreesService.GetTreesAsync()
                .ContinueWith(task => _mapper.Map<IEnumerable<TreeNodeApiModel>>(task.Result))
                .ContinueWith(task => Results.Json(task.Result));
        }

        [HttpPost("api.user.tree.get")]
        public async Task<IResult> GetTree([Required] string treeName)
        {
            return await this._independedTreesService.GetTreeAsync(treeName)
                .ContinueWith(task => _mapper.Map<TreeNodeApiModel>(task.Result))
                .ContinueWith(task => Results.Json(task.Result));
        }

        [HttpPost("api.user.tree.create")]
        public async Task<IResult> CreateTree([Required] string treeName)
        {
            return await this._independedTreesService.CreateTreeAsync(treeName)
                .ContinueWith(task => task.IsFaulted ? 
                    throw task.Exception.InnerException : Results.Created());
        }

        [HttpPost("api.user.tree.node.create")]
        public async Task<IResult> Create([Required] string treeName,
            [Required] int parentNodeId,
            [Required] string nodeName)
        {
            return await this._independedTreesService.CreateTreeNodeAsync(treeName, parentNodeId, nodeName)
                .ContinueWith(task => task.IsFaulted ? 
                    throw task.Exception.InnerException : Results.Created());
        }

        [HttpPost("api.user.tree.node.rename")]
        public async Task<IResult> Rename([Required] string treeName,
            [Required] int nodeId,
            [Required] string newNodeName)
        {
            return await this._independedTreesService.RenameTreeNodeAsync(treeName, nodeId, newNodeName)
                .ContinueWith(task => task.IsFaulted ? 
                    throw task.Exception.InnerException : Results.Ok());
        }

        [HttpPost("api.user.tree.node.delete")]
        public async Task<IResult> Delete([Required] string treeName, [Required] int nodeId)
        {
            return await this._independedTreesService.DeleteTreeNodeAsync(treeName, nodeId)
                .ContinueWith(task => task.IsFaulted ? 
                    throw task.Exception.InnerException : Results.Ok());
        }
    }
}
