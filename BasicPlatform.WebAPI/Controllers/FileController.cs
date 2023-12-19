using System.Text.RegularExpressions;

namespace BasicPlatform.WebAPI.Controllers;

/// <summary>
/// 文件控制器
/// </summary>
[ApiController]
[Route("api/[controller]/[action]")]
public partial class FileController : ControllerBase
{
    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="file"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    public async Task<string> UploadAsync(
        [FromServices] IWebHostEnvironment environment,
        [FromForm] IFormFile file,
        [FromQuery] string dir = "upload"
    )
    {
        var fileName = file.FileName;
        // 只能上传固定格式的文件
        var ext = Path.GetExtension(fileName);
        if (string.IsNullOrEmpty(ext) || !FileExtensionRegex().IsMatch(ext))
        {
            throw new Exception(
                "只能上传固定格式的文件，如：.jpg、.png、.gif、.jpeg、.svg、.ico、.zip、.rar、.7z、.doc、.docx、.xls、.xlsx、.ppt、.pptx、.pdf、.txt、.mp3、.mp4、.avi、.rmvb、.flv");
        }

        var dirPath = Path.Combine(environment.WebRootPath, "static", dir);
        // 如果目录不存在则创建
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        var filePath = Path.Combine(dirPath, fileName);
        // 如果文件存在并且大小相同则直接返回
        if (global::System.IO.File.Exists(filePath))
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length == file.Length)
            {
                return $"/static/{dir}/{fileName}";
            }

            throw new Exception("文件名重复，请修改文件名后重试。");
        }

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
        return $"/static/{dir}/{fileName}";
    }

    #region Private Methods

    [GeneratedRegex(
        "^\\.((jpg)|(png)|(gif)|(jpeg)|(svg)|(ico)|(zip)|(rar)|(7z)|(doc)|(docx)|(xls)|(xlsx)|(ppt)|(pptx)|(pdf)|(txt)|(mp3)|(mp4)|(avi)|(rmvb)|(flv))$")]
    private static partial Regex FileExtensionRegex();

    #endregion
}