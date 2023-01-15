using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HashChecker.Windows;

/// <summary>
/// Interaction logic for FileInput.xaml
/// </summary>
public partial class FileInput : UserControl
{
    public static readonly DependencyProperty dp_File = DependencyProperty.Register("File", typeof(FileInfo), typeof(FileInput));
    public static readonly RoutedEvent re_FileSelected = EventManager.RegisterRoutedEvent("FileSelected", RoutingStrategy.Bubble, typeof(FileSelectedEventHandler), typeof(FileInput));


    private static readonly string ds = System.IO.Path.DirectorySeparatorChar.ToString();

    public FileInfo File
    {
        get => (FileInfo)GetValue(dp_File);
        set => SetValue(dp_File, value);
    }

    public event FileSelectedEventHandler FileSelected
    {
        add => AddHandler(re_FileSelected, value);
        remove => RemoveHandler(re_FileSelected, value);
    }

    public FileInput()
    {
        InitializeComponent();
        ResizeRadius();
        FileSelected += UpdateFile;
    }

    private void FileInput_Drop(object sender, DragEventArgs e)
    {
        try
        {
            string f = ((string[])e.Data.GetData(DataFormats.FileDrop)).FirstOrDefault("");

            if (System.IO.File.Exists(f))
            {
                File = new(f);
                RaiseEvent(new FileSelectedEventArgs(re_FileSelected, File));
                no_file.Visibility = Visibility.Hidden;
                file_path.Visibility = Visibility.Visible;
            }
        }
        catch
        {

        }
    }

    private void FileInput_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        OpenFileDialog();
    }

    private void UpdateFile(object sender, FileSelectedEventArgs e)
    {
        n_file.Text = e.File.Name;
        n_parent.Text = ds + e.File.Directory?.Name + ds ?? "";
        n_dir.Text = e.File.DirectoryName?[..e.File.DirectoryName.LastIndexOf(ds)] ?? "";

        root.ToolTip = $"Selected File: {e.File.FullName}\n"  + "Double-click or Drag &amp; drop to reselect a file";

        open_item.ToolTip = $"Click here to open file '{e.File.Name}'";
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (File is null) OpenFileDialog();
        else Process.Start("explorer", File.FullName);
    }

    private void OpenFileDialog()
    {
        OpenFileDialog fd = new()
        {
            AddExtension = true,
            Multiselect = false
        };

        fd.FileOk += (f_sender, f_e) =>
        {
            File = new(fd.FileName);
            RaiseEvent(new FileSelectedEventArgs(re_FileSelected, File)); ;
            no_file.Visibility = Visibility.Hidden;
            file_path.Visibility = Visibility.Visible;
        };

        fd.ShowDialog();
    }

    private void ResizeRadius()
    {
        double r = root.ActualHeight / 2 ;
        bg.RadiusX = r;
        bg.RadiusY = r;
    }

    private void FileInput_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        ResizeRadius();
    }
}

public class FileSelectedEventArgs : RoutedEventArgs
{
    public FileInfo File { get; }

    public FileSelectedEventArgs(RoutedEvent re, FileInfo file): base(re)
    {
        File = file;
    }

    public FileStream OpenFileForRead()
    {
        return File.OpenRead();
    }

    public FileStream OpenFileForWrite()
    {
        return File.OpenWrite();
    }
}

public delegate void FileSelectedEventHandler(object sender, FileSelectedEventArgs e);
