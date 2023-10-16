using HoneyLovely.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace HoneyLovely.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MemberDetailsController : ControllerBase
    {
        private readonly ILogger<MembersController> _logger;
        private readonly IMemberDetailService _memberDetailService;

        public MemberDetailsController(IMemberDetailService memberDetailService, ILogger<MembersController> logger)
        {
            _memberDetailService = memberDetailService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public Task<MemberDetail> GetByIdAsync(Guid id)
        {
            return _memberDetailService.GetByIdAsync(id);
        }

        [HttpPost]
        public Task<int> CreateAsync([FromBody] MemberDetail detail)
        {
            return _memberDetailService.CreateAsync(detail);
        }
    }
}