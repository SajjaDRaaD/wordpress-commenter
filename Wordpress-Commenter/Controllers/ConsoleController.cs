using AppDTOs;
using AutoMapper;
using ClientApp.Services.User;
using DataAccess.Models.User;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConsoleController : ControllerBase
    {
        private readonly UCommentGroupService _UCommentGroupService;
        private readonly UReviewService _UReviewService;
        private readonly IMapper _mapper;

        public ConsoleController(UCommentGroupService UCommentGroupService, UCommentsService uCommentService, UReviewService uReviewService, IMapper mapper)
        {
            _UCommentGroupService = UCommentGroupService;
            _UReviewService = uReviewService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<CommentGroup>> GetCategories()
        {
            List<CommentGroup> groups = await _UCommentGroupService.GetAllCommentGroups();
            return groups;
        }

        public async Task<int> GetReviewCount(int groupId) 
        {
            var reviews = await _UReviewService.GetReviewsByGroupId(groupId);
            return reviews.Count;
        }

        [HttpPost]
        public async Task<IActionResult> FlushGroup([FromBody] int groupId)
        {
            if (groupId == 0)
            {
                return Ok("Group Id Is 0");
            }

            await _UReviewService.DeleteAllByGroup(groupId);
            return Ok("All Reviews Deleted Succesfuly ...");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitComments(List<ReviewDTO> reviews)
        {
            if (!ModelState.IsValid) return BadRequest("Review fields is not correct");

            var mappedReviews = _mapper.Map<List<Review>>(reviews);

            try
            {
                await _UReviewService.AddBulkAsync(mappedReviews);
            }
            catch (Exception err)
            {

                return Ok(err);
            }

            return Ok($"{reviews.Count} Reviews Succesfully Added ...");
        }
    }
}
