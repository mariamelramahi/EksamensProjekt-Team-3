﻿using System.IO;
using System.Windows;

namespace EksamensProjekt.Services
{
    public class DragAndDropService
    {
        public Action<string>? FileDropped { get; set; } // Callback to ViewModel when file is dropped 

        public void HandleDragOver(object sender, DragEventArgs e)
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop)
                ? DragDropEffects.Copy
                : DragDropEffects.None;
            e.Handled = true;
        }

        public void HandleDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                ValidateFileFormat(files);
            }
            e.Handled = true;
        }


        public void ValidateFileFormat(string[] files)
        {
            foreach (var file in files)
            {
                var extension = Path.GetExtension(file);
                if (extension.Equals(".XLSX", StringComparison.OrdinalIgnoreCase) ||
                    extension.Equals(".XLS", StringComparison.OrdinalIgnoreCase) ||
                    extension.Equals(".CSV", StringComparison.OrdinalIgnoreCase))
                {
                    FileDropped?.Invoke(file); // Call ViewModel when file is dropped
                    break;
                }
                else
                {
                    throw new FormatException($"Unsupported file format: {extension}");
                }
            }
        }



    }
}
