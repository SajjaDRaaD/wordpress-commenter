using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Models.User;
using ClientApp.Models.User;
using ClientApp.Services.User;
using AutoMapper;
using AppDTOs;
using ClientApp.Services.ConfigurationService;
using Hangfire;
using System.Text.Json;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;

namespace ClientApp.Controllers
{
    [Authorize]
    public class UserPanelController : Controller
    {

        private readonly UWebsitesService _uWebsitesService;
        private readonly UCommentGroupService _uCommentGroupService;
        private readonly UCommentsService _uCommentsService;
        private readonly UReviewService _uReviewService;
        private readonly USendCommentService _uSendCommentService;
        private readonly SendCommentConfigurationService _sendCommentConfigurationService;
        private readonly UReportsService _uReportsService;
        private readonly UDashboardService _uDashboardService;
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IMapper _mapper;


        public UserPanelController(UWebsitesService uWebsitesService, UCommentGroupService uCommentGroupService, UCommentsService uCommentsService, UReviewService uReviewService, IMapper mapper, USendCommentService uSendCommentService, SendCommentConfigurationService sendCommentConfigurationService, UReportsService uReportsService, IBackgroundJobClient backgroundJobClient, UDashboardService uDashboardService)
        {
            _uWebsitesService = uWebsitesService;
            _uCommentGroupService = uCommentGroupService;
            _uCommentsService = uCommentsService;
            _uReviewService = uReviewService;
            _mapper = mapper;
            _uSendCommentService = uSendCommentService;
            _sendCommentConfigurationService = sendCommentConfigurationService;
            _uReportsService = uReportsService;
            _backgroundJobClient = backgroundJobClient;
            _uDashboardService = uDashboardService;
        }
        [Route("/")]
        public async Task<IActionResult> Dashboard()
        {
            var vm = await _uDashboardService.GetDashboardCounts();
            return View(vm);
        }

        #region Website Actions
        [HttpGet("/dashboard/websites")]
        public async Task<IActionResult> Websites()
        {
            var websites = await _uWebsitesService.GetAllWebsites();
            var mappedWebsites = _mapper.Map<List<WebsiteDTO>>(websites);
            var websitesVM = new WebsitesVM { WebsitesList = mappedWebsites };
            return View(websitesVM);
        }

        [HttpPost("/dashboard/websites")]
        public async Task<IActionResult> Websites(WebsitesVM websitesVm)
        {
            var mappedWebsite = _mapper.Map<Websites>(websitesVm.Websites);
            var result = await _uWebsitesService.AddWebsite(mappedWebsite);
            TempData["status"] = result.Status;
            TempData["message"] = result.Message;
            return RedirectToAction("Websites");
        }

        [HttpPost]
        public async Task<JsonResult> GetWebsite(int Id)
        {
            var website = await _uWebsitesService.GetWebsite(Id);
            return (website == null ? this.Json("Website Not Found") : this.Json(website));
        }

        [HttpPost]
        public async Task<IActionResult> EditWebsite( WebsiteDTO website)
        {
            var mappedWebsite = _mapper.Map<Websites>(website);
            var result = await _uWebsitesService.EditWebsite(mappedWebsite);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Websites(int id)
        {
            var result = await _uWebsitesService.DeleteWebsite(id);
            return Ok(result);
        }

        #endregion

        #region CommentGroups Actions
        [HttpGet("/dashboard/groups")]
        public async Task<IActionResult> CommentGroups()
        {
            var commentGroups = await _uCommentGroupService.GetAllCommentGroups();
            var mappedCommentGroups = _mapper.Map<List<CommentGroupDTO>>(commentGroups);
            var commentGroupsVM = new CommentGroupVM() {
                CommentGroupsList = mappedCommentGroups
            };
            return View(commentGroupsVM);
        }

        [HttpPost("/dashboard/groups")]
        public async Task<IActionResult> CommentGroups(CommentGroupDTO commentGroup)
        {
            if (!ModelState.IsValid) return BadRequest("Enter required fields");

            var mappedCommentGroup = _mapper.Map<CommentGroup>(commentGroup);
            await _uCommentGroupService.AddCommentGroup(mappedCommentGroup);
            TempData["message"] = "success";
            return RedirectToAction("CommentGroups");

        }

        [HttpPost]
        public async Task<JsonResult> GetCommentGroup(int Id)
        {
            var commentGroup = await _uCommentGroupService.GetCommentGroup(Id);
            return (commentGroup == null ? this.Json("Comment Group Not Found") : this.Json(commentGroup));
        }

        [HttpPost]
        public async Task<IActionResult> EditCommentGroup(CommentGroupDTO commentGroup)
        {
            if (!ModelState.IsValid) return BadRequest("Enter required fields");

            var mappedCommentGroup = _mapper.Map<CommentGroup>(commentGroup);
            await _uCommentGroupService.EditCommentGroup(mappedCommentGroup);
            return Ok("Edited Succesfully ...");
        }
        [HttpDelete]
        public async Task<IActionResult> CommentGroups(int id)
        {
            var result = await _uCommentGroupService.DeleteCommentGroup(id);
            return Ok(result);
        }

        #endregion

        #region Comments Actions

        [Route("dashboard/comments")]
        public async Task<IActionResult> Comments()
        {
            var comments = await _uCommentsService.GetAllComments();
            var mapeedComments = _mapper.Map<List<CommentDTO>>(comments);

            var reviews = await _uReviewService.GetAllReview();
            var mapeedReviews = _mapper.Map<List<ReviewDTO>>(reviews);

            var groups = await _uCommentGroupService.GetAllCommentGroups();
            var mappedGroups = _mapper.Map<List<CommentGroupDTO>>(groups);

            ReviewVm vm = new ReviewVm { Groups = mappedGroups, Comments = mapeedComments, Reviews = mapeedReviews };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(ReviewDTO review)
        {
            if (!ModelState.IsValid) return BadRequest("Enter required fields");

            var mappedReview = _mapper.Map<Review>(review);
            await _uReviewService.AddReview(mappedReview);
            TempData["message"] = "success";
            return RedirectToAction("Comments");
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentDTO comment)
        {
            if (!ModelState.IsValid) return BadRequest("Enter required fields");

            var mappedComment = _mapper.Map<Comment>(comment);
            await _uCommentsService.AddComment(mappedComment);
            TempData["message"] = "success";
            return RedirectToAction("Comments");
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteReview(int id)
        {
            await _uReviewService.DeleteReview(id);
            return Json("Deleted Succesfully ...");
        }

        [HttpDelete]
        public async Task<JsonResult> DeleteComment(int id)
        {
            await _uCommentsService.DeleteComment(id);
            return Json("Deleted Succesfully ...");
        }

        [HttpPost]
        public async Task<JsonResult> GetReview(int id)
        {
            var review = await _uReviewService.GetReview(id);
            return this.Json(review);
        }

        public async Task<JsonResult> GetComment(int id)
        {
            var comment = await _uCommentsService.GetComment(id);
            return this.Json(comment);
        }


        #endregion

        #region Reports Actions

        [Route("dashboard/reports")]
        public async Task<IActionResult> Reports()
        {
            var data = await _uReportsService.GetAllSendRequests();
            var listOfConfigs = new List<ConfigurationReportVm>();

            foreach (var config in data)
            {
                var tempConfig = JsonSerializer.Deserialize<SendCommentConfigurationDetailsDTO>(config.Configuration);
                listOfConfigs.Add(new ConfigurationReportVm
                {
                    Id = config.Id,
                    config = tempConfig,
                    Status = tempConfig.Ids.Count == 0 ? "Finished" : "Running"
                });
            }

            var vm = new ReportsVM
            {
                SendRequests = listOfConfigs
            };

            return View(vm);
        }

        #endregion

        #region SendComment

        [HttpGet("/dashboard/send")]
        public async Task<IActionResult> SendComment()
        {
            var websitelst = await _uWebsitesService.GetAllWebsites();
            var mappedWebsites = _mapper.Map<List<WebsiteDTO>>(websitelst);

            var grouplst = await _uCommentGroupService.GetAllCommentGroups();
            var mappedgroups = _mapper.Map<List<CommentGroupDTO>>(grouplst);

            var initialData = new SendCommentVm { WebsitesList = mappedWebsites, CommentGroupList = mappedgroups };
            return View(initialData);
        }

        [HttpPost]
        public async Task<IActionResult> SendSingleComment(IFormCollection data)
        {
            var result = await _uSendCommentService.CreateSendRecurringJob(data);
            return RedirectToAction("Reports");
        }

        [HttpPost]
        public async Task<List<CategoryDTO>> GetWordpressCategories(int websiteId, string commentType)
        {
            var categories = await _uSendCommentService.GetWebsiteCategories(websiteId, commentType);
            return categories;
        }

        [HttpPost]
        public async Task<List<int>> GetWordpressPostsIds(int websiteId, string commentType, int catId)
        {
            var ids = await _uSendCommentService.GetWordpressPostsIds(websiteId, commentType, catId);
            return ids;
        }

        #endregion

        #region Account Actions

        public IActionResult Account()
        {
            return View();
        }

        #endregion


    }
}