using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace Survey_converter.Services;

public interface IFilesService
{
    //public Task<IStorageFile?> OpenFileAsync();
    //public Task<IStorageFile?> SaveFileAsync();
    public Task<IStorageFolder?> GetFolderAsync();
}