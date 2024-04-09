using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;

namespace Survey_converter.Services;

internal sealed class FilesService : IFilesService
{
    private readonly Window _target;

    public FilesService(Window target)
    {
        _target = target;
    }

    public async Task<IStorageFolder?> GetFolderAsync()
    {
        IReadOnlyList<IStorageFolder> folder;
        if (_target.StorageProvider.CanPickFolder)
        {
            folder = await _target.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                Title = Languages.Resources.FileDialogText,
                AllowMultiple = false
            });

            return folder.Count >= 1 ? folder[0] : null;
        }

        return null;
    }
}