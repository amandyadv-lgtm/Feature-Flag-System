using FeatureFlag.AdminAPI.Data;
using FeatureFlag.AdminAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlag.AdminAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeatureFlagsController: ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IHubContext<FlagHub> _hubContext;

        public FeatureFlagsController(AppDbContext appDbContext, IHubContext<FlagHub> hubContext)
        {
            this._context = appDbContext;
            this._hubContext = hubContext;
        }

        // 1. GET: Fetch all flags (Used by SDK on startup)
        [HttpGet]
        public async Task<IActionResult> GetFlags()
        {
            return Ok(_context.FeatureFlags.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateFlag(Models.FeatureFlag flag)
        {
            var existingFlag = await _context.FeatureFlags.FirstOrDefaultAsync(currentflag => currentflag.Key == flag.Key);

            if (existingFlag == null)
            {
                _context.FeatureFlags.Add(flag);
            }
            else
            {
                existingFlag.IsEnabled  = flag.IsEnabled;
                existingFlag.RolloutPercentage = flag.RolloutPercentage;
                existingFlag.TargetedUsers = flag.TargetedUsers;
            }

            await _context.SaveChangesAsync();

            // After saving to DB, we broadcast the update to all connected SDK clients instantly.
            // We transform the DB model to the SDK model format if they differ, 
            // but here we send the raw object for simplicity.
            await _hubContext.Clients.All.SendAsync("RecieveFlagUpdate", flag);

            return Ok(flag);

        }



    }
}
