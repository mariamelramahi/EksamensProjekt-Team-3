using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                foreach (var file in files)
                {
                    if (Path.GetExtension(file).Equals(".XLSX", StringComparison.OrdinalIgnoreCase))
                    {
                        FileDropped?.Invoke(file); // Call ViewModel when file is dropped
                        break;
                    }
                    else
                    {
                        MessageBox.Show("Kun filformat (.xlsx) kan bruges.");
                    }
                }
            }
            e.Handled = true;
        }
    }
}
