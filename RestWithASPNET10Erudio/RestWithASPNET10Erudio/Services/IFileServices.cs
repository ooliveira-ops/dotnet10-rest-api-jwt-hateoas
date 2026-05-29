using RestWithASPNET10Erudio.Data.DTO.V1;
using RestWithASPNET10Erudio.DATA.DTO.V1;

namespace RestWithASPNET10Erudio.Services
{
	public interface IFileServices
	{
		byte[] GetFile(string fileName);
		Task<FileDetailDTO> SaveFileToDisk(IFormFile file);
		Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files);
	}
}
