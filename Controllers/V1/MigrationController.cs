using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace electro_shop_backend.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class MigrationController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly IWebHostEnvironment _env;

        public MigrationController(Cloudinary cloudinary, IWebHostEnvironment env)
        {
            _cloudinary = cloudinary;
            _env = env;
        }
        [HttpPost("migrate-sql-script")]
        public async Task<IActionResult> MigrateSqlScript(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string uploadsDir = Path.Combine(_env.ContentRootPath, "wwwroot", "images");
            var regex = new Regex(@"https://localhost:7169/images/([a-zA-Z0-9\-\._]+)", RegexOptions.IgnoreCase);

            string outputContent;

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var writer = new StringWriter())
            {
                while (!reader.EndOfStream)
                {
                    string line = await reader.ReadLineAsync() ?? "";
                    string newLine = line;

                    var matches = regex.Matches(line);
                    foreach (Match match in matches)
                    {
                        string oldUrl = match.Value;
                        string fileName = match.Groups[1].Value;
                        string filePath = Path.Combine(uploadsDir, fileName);

                        if (!System.IO.File.Exists(filePath))
                        {
                            Console.WriteLine($"⚠️ File not found: {filePath}");
                            continue;
                        }

                        Console.WriteLine($"Uploading {fileName}...");
                        var uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(filePath),
                            Folder = "electroshop/images"
                        };
                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                        if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            string newUrl = uploadResult.SecureUrl.ToString();
                            newLine = newLine.Replace(oldUrl, newUrl);
                            Console.WriteLine($"✅ Replaced {oldUrl} => {newUrl}");
                        }
                    }

                    await writer.WriteLineAsync(newLine);
                }

                outputContent = writer.ToString();
            }

            var outputBytes = System.Text.Encoding.UTF8.GetBytes(outputContent);
            return File(outputBytes, "application/sql", "script_new.sql");
        }

        [HttpPost("compare-sql-scripts")]
        public async Task<IActionResult> CompareSqlScripts(IFormFile oldFile, IFormFile newFile)
        {
            if (oldFile == null || newFile == null)
                return BadRequest("Please upload both old and new SQL files.");

            List<string> oldLines = new();
            List<string> newLines = new();

            using (var reader = new StreamReader(oldFile.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                    oldLines.Add(await reader.ReadLineAsync() ?? "");
            }

            using (var reader = new StreamReader(newFile.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                    newLines.Add(await reader.ReadLineAsync() ?? "");
            }

            var diffs = new List<object>();
            int maxLen = Math.Max(oldLines.Count, newLines.Count);

            for (int i = 0; i < maxLen; i++)
            {
                string oldLine = i < oldLines.Count ? oldLines[i] : "";
                string newLine = i < newLines.Count ? newLines[i] : "";

                if (!oldLine.Equals(newLine, StringComparison.Ordinal))
                {
                    diffs.Add(new
                    {
                        LineNumber = i + 1,
                        OldLine = oldLine,
                        NewLine = newLine
                    });
                }
            }

            return Ok(new
            {
                Differences = diffs,
                TotalDifferences = diffs.Count
            });
        }

    }
}
