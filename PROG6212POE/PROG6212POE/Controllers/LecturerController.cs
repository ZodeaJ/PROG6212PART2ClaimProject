using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG6212POE.Data;
using PROG6212POE.Models;

namespace PROG6212POE.Controllers
{
    public class LecturerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileStorage _fileStorage;

        public LecturerController(AppDbContext context, IFileStorage fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        [HttpGet]
        public IActionResult MakeAClaim()
        {
            var lecturers = _context.Lecturers.ToList();

            //select lecturers from list
            ViewBag.Lecturers = lecturers.Select(l => new SelectListItem
            {
                Value = l.LecturerId.ToString(),
                Text = l.Name
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAClaim(Claim claim, IFormFile SupportingDocument)
        {
            ViewBag.Lecturers = _context.Lecturers
            .Select(l => new SelectListItem
            {
                Value = l.LecturerId.ToString(),
                Text = l.Name
            }).ToList();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please recheck the form values.";
                return View(claim);
            }

            var lecturer = await _context.Lecturers.FindAsync(claim.LecturerId);
            if (lecturer == null)
            {
                TempData["ErrorMessage"] = "Lecturer does not exist.";
                return View(claim);
            }

            if (SupportingDocument == null)
            {
                ModelState.AddModelError("SupportingDocument", "Supporting document is required.");
                return View(claim);
            }

            try
            {
                claim.SupportingDocument = await _fileStorage.SaveFile(SupportingDocument);
                claim.Status = ClaimStatus.Submitted;
                claim.CreatedAt = DateTime.UtcNow;

                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Claim submitted successfully!";
                return RedirectToAction(nameof(MakeAClaim));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error submitting claim: " + ex.Message;
                return View(claim);
            }
        }

        //lecturer tracks the claims status as it goes through the process
        public async Task<IActionResult> TrackClaimStatus(int lecturerId)
        {


            var claims = await _context.Claims
        .Include(c => c.Lecturer)
        .Include(c => c.FeedbackMessages)
        .OrderByDescending(c => c.CreatedAt)
        .ToListAsync();

            return View("~/Views/Claim/TrackClaimStatus.cshtml", claims);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteClaim(int claimId)
        {
            var claim = await _context.Claims.FindAsync(claimId);
            if (claim == null) return NotFound();

            // allow deletion if the claim is rejected or approved for space
            if (claim.Status != ClaimStatus.Rejected && claim.Status != ClaimStatus.Approved)
            {
                TempData["ErrorMessage"] = "Only rejected or approved claims can be deleted.";
                return RedirectToAction(nameof(TrackClaimStatus), new { lecturerId = claim.LecturerId });
            }

            try
            {
                // delete supporting document if exists
                if (!string.IsNullOrEmpty(claim.SupportingDocument))
                {
                    await _fileStorage.DeleteFile(claim.SupportingDocument);
                }

                _context.Claims.Remove(claim);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Claim deleted successfully!";
                return RedirectToAction(nameof(TrackClaimStatus), new { lecturerId = claim.LecturerId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting claim: " + ex.Message;
                return RedirectToAction(nameof(TrackClaimStatus), new { lecturerId = claim.LecturerId });
            }
        }


        public IActionResult ClaimFeedback(int claimId)
        {
            return RedirectToAction("FeedbackForClaim", "Claim", new { claimId });
        }
    }
}