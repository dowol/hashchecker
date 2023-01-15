using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Label = System.Windows.Controls.Label;

namespace HashChecker.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataObject.AddCopyingHandler(description, (sender, e) =>
        {
            if (e.IsDragDrop) e.CancelCommand();
        });

        foreach (Label label in new[] { hash_md5, hash_sha1, hash_sha256, hash_sha384, hash_sha512 })
            label.Content = string.Concat(Enumerable.Repeat('#', 256));
    }

    private void FileInput_FileSelected(object sender, FileSelectedEventArgs e)
    {
        hash_md5.Content    = ToTextBlock(MD5   .HashData(e.File.OpenRead()));
        hash_sha1.Content   = ToTextBlock(SHA1  .HashData(e.File.OpenRead()));
        hash_sha256.Content = ToTextBlock(SHA256.HashData(e.File.OpenRead()));
        hash_sha384.Content = ToTextBlock(SHA384.HashData(e.File.OpenRead()));
        hash_sha512.Content = ToTextBlock(SHA512.HashData(e.File.OpenRead()));

        TextBlock ToTextBlock(byte[] hashValue)
        {
            string hs = Convert.ToHexString(hashValue).ToUpperInvariant();
            return new TextBlock()
            {
                FontFamily = new("Noto Mono, D2Coding, Consolas, monospace"),
                TextWrapping = TextWrapping.NoWrap,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Text = hs,
                ToolTip = hs
            };
        }
    }

    private void HashValue_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            Label label = (Label)sender;
            Clipboard.SetText(((TextBlock)label.Content).Text);
            string hashName = label.Name.ToUpperInvariant().Replace("HASH_", "");
            MessageBox.Show(this, $"{hashName} value of the file \"{fileInput.File.Name}\" successfully copied to your clipboard.", "HashChecker", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (InvalidCastException)
        {
            MessageBox.Show(this, "No hash value to copy is computed yet.", "HashChecker", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }


}
