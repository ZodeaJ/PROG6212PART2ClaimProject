using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG6212POE.Data;
using PROG6212POE.Models;

namespace PROG6212POE.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        //shows all submitted claims for coordinator to review
        public async Task<IActionResult> ReviewClaims()
        {
            var claims = await _context.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Submitted)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

            return View(claims);
        }

        //Coordinator forwards the claim to manager with feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForwardClaim(int id, string coordinatorMessage)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Forwarded;

            _context.Feedbacks.Add(new Feedback
            {
                ClaimId = id,
                Role = "Coordinator",
                Message = string.IsNullOrWhiteSpace(coordinatorMessage) ? "Forwarded to manager" : coordinatorMessage,
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ReviewClaims));
        }

        //Coordinator rejects claim with feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectByCoordinator(int id, string coordinatorMessage)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;

            _context.Feedbacks.Add(new Feedback
            {
                ClaimId = id,
                Role = "Coordinator",
                Message = string.IsNullOrWhiteSpace(coordinatorMessage) ? "Rejected by coordinator" : coordinatorMessage,
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ReviewClaims));
        }

        //show all forwarded claims for manager to verify
        public async Task<IActionResult> VerifyClaims()
        {
            var claims = await _context.Claims
                .Include(c => c.Lecturer)
                .Where(c => c.Status == ClaimStatus.Forwarded)
                .OrderBy(c => c.CreatedAt)
                .ToListAsync();

            return View(claims);
        }

        //manager approves forwarded claim with feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveClaim(int id, string managerMessage)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Approved;

            _context.Feedbacks.Add(new Feedback
            {
                ClaimId = id,
                Role = "Manager",
                Message = string.IsNullOrWhiteSpace(managerMessage) ? "Approved by manager" : managerMessage,
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(VerifyClaims));
        }

        //manager rejects forwarded claim with feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectByManager(int id, string managerMessage)
        {
            var claim = await _context.Claims.FindAsync(id);
            if (claim == null) return NotFound();

            claim.Status = ClaimStatus.Rejected;

            _context.Feedbacks.Add(new Feedback
            {
                ClaimId = id,
                Role = "Manager",
                Message = string.IsNullOrWhiteSpace(managerMessage) ? "Rejected by manager" : managerMessage,
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(VerifyClaims));
        }
    }
}
