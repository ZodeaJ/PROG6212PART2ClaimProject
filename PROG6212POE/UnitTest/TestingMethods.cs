using Xunit;
using Moq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using PROG6212POE.Models;

namespace UnitTest
{
    public class TestingMethods
    {
        [Fact]
        public void Amount_Calculates_Correctly()
        {
            var claim = new Claim { HoursWorked = 10, HourlyRate = 100 };
            Assert.Equal(1000, claim.Amount);
        }

        [Fact]
        public void Description_Is_Required()
        {
            var claim = new Claim
            {
                LecturerId = 1,
                Month = "October",
                HoursWorked = 5,
                HourlyRate = 100,
                Description = null
            };

            var results = new List<ValidationResult>();
            Validator.TryValidateObject(claim, new ValidationContext(claim), results, true);
            Assert.Contains(results, r => r.ErrorMessage.Contains("required", System.StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task File_Upload_Validation_Works()
        {
            var mockEnv = new Mock<IWebHostEnvironment>();
            mockEnv.Setup(e => e.WebRootPath).Returns("testroot");
            var storage = new FileStorage(mockEnv.Object);

            var badFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("test")), 0, 4, "file", "bad.exe");
            await Assert.ThrowsAsync<InvalidOperationException>(() => storage.SaveFile(badFile));

            var largeFile = new FormFile(new MemoryStream(new byte[101 * 1024 * 1024]), 0, 101 * 1024 * 1024, "file", "large.pdf");
            await Assert.ThrowsAsync<InvalidOperationException>(() => storage.SaveFile(largeFile));

            Directory.CreateDirectory(Path.Combine("testroot", "uploads"));
            var goodFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("ok")), 0, 2, "file", "valid.pdf");
            var saved = await storage.SaveFile(goodFile);
            Assert.True(File.Exists(Path.Combine("testroot", "uploads", saved)));
            File.Delete(Path.Combine("testroot", "uploads", saved));
        }
    }
}