using Microsoft.AspNetCore.Http;

namespace RestWithASPNET10Erudio.DATA.DTO.V1
{
	public class FileUploadDTO
	{
		public IFormFile File { get; set; }
	}

	public class MultipleFilesUploadDTO
	{
		public IList<IFormFile> Files { get; set; }
	}
}